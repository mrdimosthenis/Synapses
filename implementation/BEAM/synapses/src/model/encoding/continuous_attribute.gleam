import gleam/string
import gleam/float
import gleam/map.{Map}
import gleam/result
import gleam_zlists.{ZList} as zlist
import model/encoding/serialization.{
  ContinuousAttribute, ContinuousAttributeSerialized,
}

fn parse(s: String) -> Float {
  let Ok(res) =
    s
    |> string.trim
    |> float.parse
  res
}

pub fn updated(
  continuous_attribute: ContinuousAttribute,
  datapoint: Map(String, String),
) -> ContinuousAttribute {
  let Ok(v) =
    datapoint
    |> map.get(continuous_attribute.key)
    |> result.map(parse)
  ContinuousAttribute(
    ..continuous_attribute,
    min: float.min(v, continuous_attribute.min),
    max: float.max(v, continuous_attribute.max),
  )
}

pub fn encode(
  continuous_attribute: ContinuousAttribute,
  value: String,
) -> ZList(Float) {
  case continuous_attribute.min == continuous_attribute.max {
    True -> 0.5
    False -> {
      let nomin = parse(value) -. continuous_attribute.min
      let denomin = continuous_attribute.max -. continuous_attribute.min
      nomin /. denomin
    }
  }
  |> zlist.singleton
}

pub fn decode(
  continuous_attribute: ContinuousAttribute,
  encoded_values: ZList(Float),
) -> String {
  case continuous_attribute.min == continuous_attribute.max {
    True -> continuous_attribute.min
    False -> {
      let Ok(v) = zlist.head(encoded_values)
      let factor = continuous_attribute.max -. continuous_attribute.min
      v *. factor +. continuous_attribute.min
    }
  }
  |> float.to_string
}

pub fn serialized(
  continuous_attribute: ContinuousAttribute,
) -> ContinuousAttributeSerialized {
  let ContinuousAttribute(key, min, max) = continuous_attribute
  ContinuousAttributeSerialized(key, min, max)
}

pub fn deserialized(
  continuous_attribute_serialized: ContinuousAttributeSerialized,
) -> ContinuousAttribute {
  let ContinuousAttributeSerialized(key, min, max) =
    continuous_attribute_serialized
  ContinuousAttribute(key, min, max)
}
