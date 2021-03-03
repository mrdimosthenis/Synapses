import gleam/string
import gleam/float
import gleam/map.{Map}
import gleam/result
import gleam_zlists.{ZList} as zlist
import decode.{Decoder}
import gleam/jsone.{JsonValue}
import model/encoding/serialization.{
  Attribute, AttributeSerialized, ContinuousAttribute, ContinuousAttributeSerialized,
}

pub fn parse(s: String) -> Float {
  let Ok(res) =
    s
    |> string.trim
    |> float.parse
  res
}

pub fn updated(
  continuous_attribute: Attribute,
  datapoint: Map(String, String),
) -> Attribute {
  let ContinuousAttribute(key, min, max) = continuous_attribute
  let Ok(v) =
    datapoint
    |> map.get(key)
    |> result.map(parse)
  ContinuousAttribute(key, min: float.min(v, min), max: float.max(v, max))
}

pub fn encode(continuous_attribute: Attribute, value: String) -> ZList(Float) {
  let ContinuousAttribute(_, min, max) = continuous_attribute
  case min == max {
    True -> 0.5
    False -> {
      let nomin = parse(value) -. min
      let denomin = max -. min
      nomin /. denomin
    }
  }
  |> zlist.singleton
}

pub fn decode(
  continuous_attribute: Attribute,
  encoded_values: ZList(Float),
) -> String {
  let ContinuousAttribute(_, min, max) = continuous_attribute
  case min == max {
    True -> min
    False -> {
      let Ok(v) = zlist.head(encoded_values)
      let factor = max -. min
      v *. factor +. min
    }
  }
  |> float.to_string
}

pub fn serialized(continuous_attribute: Attribute) -> AttributeSerialized {
  let ContinuousAttribute(key, min, max) = continuous_attribute
  ContinuousAttributeSerialized(key, min, max)
}

pub fn deserialized(
  continuous_attribute_serialized: AttributeSerialized,
) -> Attribute {
  let ContinuousAttributeSerialized(key, min, max) =
    continuous_attribute_serialized
  ContinuousAttribute(key, min, max)
}

pub fn json_encoded(
  continuous_attribute_serialized: AttributeSerialized,
) -> JsonValue {
  let ContinuousAttributeSerialized(key, min, max) =
    continuous_attribute_serialized
  jsone.object([
    tuple("key", jsone.string(key)),
    tuple("min", jsone.float(min)),
    tuple("max", jsone.float(max)),
  ])
}

pub fn json_decoder() -> Decoder(AttributeSerialized) {
  decode.map3(
    ContinuousAttributeSerialized,
    decode.field("key", decode.string()),
    decode.field("min", decode.float()),
    decode.field("max", decode.float()),
  )
}
