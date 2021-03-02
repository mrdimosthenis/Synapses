import gleam/map.{Map}
import gleam/pair
import gleam_zlists.{ZList} as zlist
import decode.{Decoder}
import model/encoding/serialization.{
  AttributeSerialized, DiscrAttrSerialized, DiscreteAttribute, DiscreteAttributeSerialized,
  FSharpAttributeSerialized,
}

pub fn updated(
  discrete_attribute: DiscreteAttribute,
  datapoint: Map(String, String),
) -> DiscreteAttribute {
  let Ok(v) = map.get(datapoint, discrete_attribute.key)
  let updated_values = case zlist.has_member(discrete_attribute.values, v) {
    True -> discrete_attribute.values
    False -> zlist.cons(discrete_attribute.values, v)
  }
  DiscreteAttribute(..discrete_attribute, values: updated_values)
}

pub fn encode(
  discrete_attribute: DiscreteAttribute,
  value: String,
) -> ZList(Float) {
  discrete_attribute.values
  |> zlist.map(fn(x) {
    case x == value {
      True -> 1.0
      False -> 0.0
    }
  })
}

pub fn decode(
  discrete_attribute: DiscreteAttribute,
  encoded_values: ZList(Float),
) -> String {
  let Ok(tuple(hd, tl)) =
    discrete_attribute.values
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

pub fn serialized(
  discrete_attribute: DiscreteAttribute,
) -> DiscreteAttributeSerialized {
  let DiscreteAttribute(key, values) = discrete_attribute
  DiscreteAttributeSerialized(key, zlist.to_list(values))
}

pub fn deserialized(
  discrete_attribute_serialized: DiscreteAttributeSerialized,
) -> DiscreteAttribute {
  let DiscreteAttributeSerialized(key, values) = discrete_attribute_serialized
  DiscreteAttribute(key, zlist.of_list(values))
}

pub fn discr_json_decoder() -> Decoder(DiscreteAttributeSerialized) {
  decode.map2(
    DiscreteAttributeSerialized,
    decode.field("key", decode.string()),
    decode.field("values", decode.list(decode.string())),
  )
}

pub fn attr_json_decoder() -> Decoder(AttributeSerialized) {
  decode.element(0, decode.map(DiscrAttrSerialized, discr_json_decoder()))
}

pub fn f_sharp_attr_json_decoder() -> Decoder(FSharpAttributeSerialized) {
  decode.map2(
    FSharpAttributeSerialized,
    decode.field("Case", decode.string()),
    decode.field("Fields", decode.list(attr_json_decoder())),
  )
}
