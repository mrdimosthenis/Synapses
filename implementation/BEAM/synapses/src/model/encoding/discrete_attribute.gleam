import gleam/map.{Map}
import gleam/pair
import gleam_zlists.{ZList} as zlist
import decode.{Decoder}
import model/encoding/serialization.{
  Attribute, AttributeSerialized, DiscreteAttribute, DiscreteAttributeSerialized,
  FSharpAttributeSerialized,
}

pub fn updated(
  discrete_attribute: Attribute,
  datapoint: Map(String, String),
) -> Attribute {
  let DiscreteAttribute(key, values) = discrete_attribute
  let Ok(v) = map.get(datapoint, key)
  let updated_values = case zlist.has_member(values, v) {
    True -> values
    False -> zlist.cons(values, v)
  }
  DiscreteAttribute(key, values: updated_values)
}

pub fn encode(discrete_attribute: Attribute, value: String) -> ZList(Float) {
  let DiscreteAttribute(_, values) = discrete_attribute
  values
  |> zlist.map(fn(x) {
    case x == value {
      True -> 1.0
      False -> 0.0
    }
  })
}

pub fn decode(
  discrete_attribute: Attribute,
  encoded_values: ZList(Float),
) -> String {
  let DiscreteAttribute(_, values) = discrete_attribute
  let Ok(tuple(hd, tl)) =
    values
    |> zlist.zip(encoded_values)
    |> zlist.uncons
  zlist.reduce(
    tl,
    hd,
    fn(x, acc) {
      let tuple(_, x_float_val) = x
      let tuple(_, acc_float_val) = acc
      case x_float_val >. acc_float_val {
        True -> x
        False -> acc
      }
    },
  )
  |> pair.first
}

pub fn serialized(discrete_attribute: Attribute) -> AttributeSerialized {
  let DiscreteAttribute(key, values) = discrete_attribute
  DiscreteAttributeSerialized(key, zlist.to_list(values))
}

pub fn deserialized(
  discrete_attribute_serialized: AttributeSerialized,
) -> Attribute {
  let DiscreteAttributeSerialized(key, values) = discrete_attribute_serialized
  DiscreteAttribute(key, zlist.of_list(values))
}

pub fn json_decoder() -> Decoder(AttributeSerialized) {
  decode.map2(
    DiscreteAttributeSerialized,
    decode.field("key", decode.string()),
    decode.field("values", decode.list(decode.string())),
  )
}
