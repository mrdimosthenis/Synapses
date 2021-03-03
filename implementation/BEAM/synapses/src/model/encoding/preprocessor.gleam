//// public

import gleam/map.{Map}
import gleam/pair
import gleam/list
import gleam_zlists.{ZList} as zlist
import decode.{Decoder}
import gleam/jsone.{JsonValue}
import model/encoding/discrete_attribute
import model/encoding/continuous_attribute
import model/encoding/serialization.{
  Attribute, AttributeSerialized, ContinuousAttribute, ContinuousAttributeSerialized,
  DiscreteAttribute, DiscreteAttributeSerialized, FSharpAttributeSerialized, FSharpPreprocessorSerialized,
  Preprocessor, PreprocessorSerialized,
}

//fn lazy_realization(preprocessor: Preprocessor) -> Preprocessor {
//  serialized(preprocessor)
//  preprocessor
//}
fn updated_f(attr: Attribute, datapoint: Map(String, String)) -> Attribute {
  case attr {
    DiscreteAttribute(_, _) -> discrete_attribute.updated(attr, datapoint)
    ContinuousAttribute(_, _, _) ->
      continuous_attribute.updated(attr, datapoint)
  }
}

fn updated(
  preprocessor: Preprocessor,
  datapoint: Map(String, String),
) -> Preprocessor {
  zlist.map(preprocessor, fn(x) { updated_f(x, datapoint) })
}

fn init_f(
  key: String,
  is_discrete: Bool,
  dataset_head: Map(String, String),
) -> Attribute {
  let Ok(str_val) = map.get(dataset_head, key)
  case is_discrete {
    True -> DiscreteAttribute(key, zlist.singleton(str_val))
    False -> {
      let v = continuous_attribute.parse(str_val)
      ContinuousAttribute(key, v, v)
    }
  }
}

// public
pub fn init(
  keys_with_flags: ZList(tuple(String, Bool)),
  dataset: ZList(Map(String, String)),
) -> Preprocessor {
  let Ok(tuple(dataset_head, dataset_tail)) = zlist.uncons(dataset)
  let init_preprocessor =
    zlist.map(
      keys_with_flags,
      fn(t) {
        let tuple(key, is_discrete) = t
        init_f(key, is_discrete, dataset_head)
      },
    )
  dataset_tail
  |> zlist.reduce(init_preprocessor, fn(x, acc) { updated(acc, x) })
  //|> lazy_realization
}

// public
pub fn encode(
  preprocessor: Preprocessor,
  datapoint: Map(String, String),
) -> ZList(Float) {
  zlist.flat_map(
    preprocessor,
    fn(attr) {
      case attr {
        DiscreteAttribute(key, _) -> {
          let Ok(v) = map.get(datapoint, key)
          discrete_attribute.encode(attr, v)
        }
        ContinuousAttribute(key, _, _) -> {
          let Ok(v) = map.get(datapoint, key)
          continuous_attribute.encode(attr, v)
        }
      }
    },
  )
}

fn decode_acc_f(
  attr: Attribute,
  acc: tuple(ZList(Float), ZList(tuple(String, String))),
) -> tuple(ZList(Float), ZList(tuple(String, String))) {
  let tuple(unprocessed_floats, processed_ks_vs) = acc
  let tuple(key, split_index) = case attr {
    DiscreteAttribute(key, values) -> tuple(key, zlist.count(values))
    ContinuousAttribute(key, _, _) -> tuple(key, 1)
  }
  let tuple(encoded_values_ls, next_floats_zls) =
    zlist.split(unprocessed_floats, split_index)
  let encoded_values_zls = zlist.of_list(encoded_values_ls)
  let decoded_value = case attr {
    DiscreteAttribute(_, _) ->
      discrete_attribute.decode(attr, encoded_values_zls)
    ContinuousAttribute(_, _, _) ->
      continuous_attribute.decode(attr, encoded_values_zls)
  }
  let next_ks_vs = zlist.cons(processed_ks_vs, tuple(key, decoded_value))
  tuple(next_floats_zls, next_ks_vs)
}

// public
pub fn decode(
  preprocessor: Preprocessor,
  encoded_datapoint: ZList(Float),
) -> Map(String, String) {
  preprocessor
  |> zlist.reduce(
    tuple(encoded_datapoint, zlist.new()),
    fn(x, acc) { decode_acc_f(x, acc) },
  )
  |> pair.second
  |> zlist.to_list
  |> map.from_list
}
//fn serialized(preprocessor: Preprocessor) -> PreprocessorSerialized {
//  preprocessor
//  |> zlist.map(fn(x) {
//    case x {
//      DiscrAttr(discr_attr) ->
//        discr_attr
//        |> discrete_attribute.serialized
//        |> DiscrAttrSerialized
//      ContinAttr(contin_attr) ->
//        contin_attr
//        |> continuous_attribute.serialized
//        |> ContinAttrSerialized
//    }
//  })
//  |> zlist.to_list
//}
//
//fn deserialized(preprocessor_serialized: PreprocessorSerialized) -> Preprocessor {
//  preprocessor_serialized
//  |> zlist.of_list
//  |> zlist.map(fn(x) {
//    case x {
//      DiscrAttrSerialized(discr_attr_serialized) ->
//        discr_attr_serialized
//        |> discrete_attribute.deserialized
//        |> DiscrAttr
//      ContinAttrSerialized(contin_attr_serialized) ->
//        contin_attr_serialized
//        |> continuous_attribute.deserialized
//        |> ContinAttr
//    }
//  })
//}
//
//fn f_sharp_deserialized(
//  f_sharp_serialized: FSharpPreprocessorSerialized,
//) -> PreprocessorSerialized {
//  list.map(
//    f_sharp_serialized,
//    fn(x) {
//      let FSharpAttributeSerialized(case_, fields) = x
//      let Ok(field) = list.head(fields)
//      field
//    },
//  )
//}
//
//fn f_sharp_attr_json_decoder() -> Decoder(FSharpAttributeSerialized) {
//  let discror_contin_json_decoder =
//    decode.one_of([
//      discrete_attribute.discr_json_decoder(),
//      continuous_attribute.contin_json_decoder(),
//    ])
//
//  decode.map2(
//    FSharpAttributeSerialized,
//    decode.field("Case", decode.string()),
//    decode.field("Fileds", decode.list(discror_contin_json_decoder)),
//  )
//}
//
//fn f_sharp_preprocessor_json_decoder() -> Decoder(FSharpPreprocessorSerialized) {
//  decode.list(f_sharp_attr_json_decoder())
//}
//
//pub fn from_json(s: String) -> Preprocessor {
//  let Ok(dyn) = jsone.decode(s)
//  let Ok(res) = decode.decode_dynamic(dyn, f_sharp_preprocessor_json_decoder())
//  res
//  |> f_sharp_deserialized
//  |> deserialized
//  |> lazy_realization
//}
