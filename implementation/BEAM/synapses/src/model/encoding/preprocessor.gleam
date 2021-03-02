import gleam/map.{Map}
import gleam/pair
import gleam_zlists.{ZList} as zlist
import model/encoding/discrete_attribute
import model/encoding/continuous_attribute
import model/encoding/serialization.{
  Attribute, AttributeSerialized, ContinAttr, ContinAttrSerialized, ContinuousAttribute,
  ContinuousAttributeSerialized, DiscrAttr, DiscrAttrSerialized, DiscreteAttribute,
  DiscreteAttributeSerialized, Preprocessor, PreprocessorSerialized,
}

fn lazy_realization(preprocessor: Preprocessor) -> Preprocessor {
  serialized(preprocessor)
  preprocessor
}

fn updated_f(attr: Attribute, datapoint: Map(String, String)) -> Attribute {
  case attr {
    DiscrAttr(discr_attr) ->
      discr_attr
      |> discrete_attribute.updated(datapoint)
      |> DiscrAttr
    ContinAttr(contin_attr) ->
      contin_attr
      |> continuous_attribute.updated(datapoint)
      |> ContinAttr
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
    True ->
      DiscreteAttribute(key, zlist.singleton(str_val))
      |> DiscrAttr
    False -> {
      let v = continuous_attribute.parse(str_val)
      ContinuousAttribute(key, v, v)
      |> ContinAttr
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
  |> lazy_realization
}

// public
pub fn encode(
  preprocessor: Preprocessor,
  datapoint: Map(String, String),
) -> ZList(Float) {
  zlist.flat_map(
    preprocessor,
    fn(x) {
      case x {
        DiscrAttr(discr_attr) -> {
          let Ok(v) = map.get(datapoint, discr_attr.key)
          discrete_attribute.encode(discr_attr, v)
        }
        ContinAttr(contin_attr) -> {
          let Ok(v) = map.get(datapoint, contin_attr.key)
          continuous_attribute.encode(contin_attr, v)
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
    DiscrAttr(discr_attr) -> tuple(
      discr_attr.key,
      zlist.count(discr_attr.values),
    )
    ContinAttr(contin_attr) -> tuple(contin_attr.key, 1)
  }
  let tuple(encoded_values_ls, next_floats_zls) =
    zlist.split(unprocessed_floats, split_index)
  let encoded_values_zls = zlist.of_list(encoded_values_ls)
  let decoded_value = case attr {
    DiscrAttr(discr_attr) ->
      discrete_attribute.decode(discr_attr, encoded_values_zls)
    ContinAttr(contin_attr) ->
      continuous_attribute.decode(contin_attr, encoded_values_zls)
  }
  let next_ks_vs = zlist.cons(processed_ks_vs, tuple(key, decoded_value))
  tuple(next_floats_zls, next_ks_vs)
}

// public
pub fn decode(
  preprocessor: Preprocessor,
  encoded_datapoint: ZList(Float),
) -> Map(String, String) {
  todo
}

fn serialized(preprocessor: Preprocessor) -> PreprocessorSerialized {
  preprocessor
  |> zlist.map(fn(x) {
    case x {
      DiscrAttr(discr_attr) ->
        discr_attr
        |> discrete_attribute.serialized
        |> DiscrAttrSerialized
      ContinAttr(contin_attr) ->
        contin_attr
        |> continuous_attribute.serialized
        |> ContinAttrSerialized
    }
  })
  |> zlist.to_list
}

fn deserialized(preprocessor_serialized: PreprocessorSerialized) -> Preprocessor {
  preprocessor_serialized
  |> zlist.of_list
  |> zlist.map(fn(x) {
    case x {
      DiscrAttrSerialized(discr_attr_serialized) ->
        discr_attr_serialized
        |> discrete_attribute.deserialized
        |> DiscrAttr
      ContinAttrSerialized(contin_attr_serialized) ->
        contin_attr_serialized
        |> continuous_attribute.deserialized
        |> ContinAttr
    }
  })
}
