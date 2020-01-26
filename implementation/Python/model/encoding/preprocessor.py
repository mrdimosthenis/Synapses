import json
from typing import Dict
from functional import seq
from functional.pipeline import Sequence

from model import utilities
from model.encoding import discrete_attribute, continuous_attribute
from model.encoding.serialization import Preprocessor, DiscreteAttribute, ContinuousAttribute, Attribute, \
    PreprocessorSerialized, AttributeSerialized, DiscreteAttributeSerialized, ContinuousAttributeSerialized


def updated_f(datapoint: Dict[str, str],
              attr: Attribute) -> Attribute:
    if isinstance(attr, DiscreteAttribute):
        return discrete_attribute.updated(datapoint, attr)
    elif isinstance(attr, ContinuousAttribute):
        return continuous_attribute.updated(datapoint, attr)
    else:
        raise Exception('Attribute is neither Discrete nor Continuous')


def updated(datapoint: Dict[str, str],
            preprocessor: Preprocessor) -> Preprocessor:
    return preprocessor \
        .map(lambda x: updated_f(datapoint, x))


def init_f(key: str,
           is_discrete: bool,
           dataset: Sequence
           ) -> Attribute:
    if is_discrete:
        return DiscreteAttribute(
            key,
            dataset.take(1).map(lambda x: x[key])
        )
    else:
        v = continuous_attribute.parse(dataset.head()[key])
        return ContinuousAttribute(
            key,
            v,
            v
        )


def init(keys_with_flags: Sequence,
         dataset: Sequence
         ) -> Preprocessor:
    init_preprocessor = keys_with_flags \
        .map(lambda x: init_f(x[0], x[1], dataset))
    return dataset \
        .fold_left(init_preprocessor,
                   lambda acc, x: updated(x, acc))


def encode_f(datapoint: Dict[str, str],
             attr: Attribute) -> Sequence:
    if isinstance(attr, DiscreteAttribute):
        return discrete_attribute.encode(datapoint[attr.key], attr)
    elif isinstance(attr, ContinuousAttribute):
        return continuous_attribute.encode(datapoint[attr.key], attr)
    else:
        raise Exception('Attribute is neither Discrete nor Continuous')


def encode(datapoint: Dict[str, str],
           preprocessor: Preprocessor
           ) -> Sequence:
    return preprocessor \
        .flat_map(lambda x: encode_f(datapoint, x))


def decode_acc_f(acc: (Sequence, Sequence),
                 attr: Attribute
                 ) -> (Sequence, Sequence):
    (unprocessed_floats, processed_ks_vs) = acc
    if isinstance(attr, DiscreteAttribute):
        split_index = attr.values.size()
    elif isinstance(attr, ContinuousAttribute):
        split_index = 1
    else:
        raise Exception('Attribute is neither Discrete nor Continuous')
    (encoded_values, next_floats) = utilities \
        .lazy_split_at(split_index, unprocessed_floats)
    if isinstance(attr, DiscreteAttribute):
        decoded_value = discrete_attribute.decode(encoded_values, attr)
    elif isinstance(attr, ContinuousAttribute):
        decoded_value = continuous_attribute.decode(encoded_values, attr)
    else:
        raise Exception('Attribute is neither Discrete nor Continuous')
    next_ks_vs = utilities \
        .lazy_cons((attr.key, decoded_value), processed_ks_vs)
    return next_floats, next_ks_vs


def decode(encoded_datapoint: Sequence,
           preprocessor: Preprocessor
           ) -> Dict[str, str]:
    return preprocessor.fold_left(
        (encoded_datapoint, seq([])),
        lambda acc, x: decode_acc_f(acc, x))[1] \
        .to_dict()


def serialized_f(attr: Attribute
                 ) -> AttributeSerialized:
    if isinstance(attr, DiscreteAttribute):
        return discrete_attribute.serialized(attr)
    elif isinstance(attr, ContinuousAttribute):
        return continuous_attribute.serialized(attr)
    else:
        raise Exception('Attribute is neither Discrete nor Continuous')


def serialized(preprocessor: Preprocessor
               ) -> PreprocessorSerialized:
    return preprocessor \
        .map(lambda x: serialized_f(x)) \
        .to_list()


def deserialized_f(attr: AttributeSerialized
                   ) -> Attribute:
    if isinstance(attr, DiscreteAttributeSerialized):
        return discrete_attribute.deserialized(attr)
    elif isinstance(attr, ContinuousAttributeSerialized):
        return continuous_attribute.deserialized(attr)
    else:
        raise Exception('Attribute is neither Discrete nor Continuous')


def to_json(preprocessor: Preprocessor) -> str:
    return json.dumps(
        serialized(preprocessor),
        separators=(',', ':'),
        cls=utilities.EnhancedJSONEncoder
    )


def deserialized(preprocessor_serialized: PreprocessorSerialized
                 ) -> Preprocessor:
    return seq(preprocessor_serialized) \
        .map(lambda x: deserialized_f(x))


def of_json(s: str) -> Preprocessor:
    return deserialized(json.loads(s))
