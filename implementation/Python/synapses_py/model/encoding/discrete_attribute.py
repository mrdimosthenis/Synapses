from typing import Dict

from functional import seq
from functional.pipeline import Sequence

from synapses_py.model import utilities
from synapses_py.model.encoding.serialization import DiscreteAttribute, DiscreteAttributeSerialized


def updated(datapoint: Dict[str, str],
            discrete_attribute: DiscreteAttribute
            ) -> DiscreteAttribute:
    exist = discrete_attribute \
        .values \
        .exists(lambda x: x == datapoint[discrete_attribute.key])
    updated_vals = discrete_attribute.values if exist else \
        utilities.lazy_cons(
            datapoint[discrete_attribute.key],
            discrete_attribute.values
        )
    return DiscreteAttribute(discrete_attribute.key, updated_vals)


def encode(value: str,
           discrete_attribute: DiscreteAttribute
           ) -> Sequence:
    return discrete_attribute \
        .values \
        .map(lambda x: 1.0 if x == value else 0.0)


def decode(encoded_values: Sequence,
           discrete_attribute: DiscreteAttribute
           ) -> str:
    return discrete_attribute \
        .values \
        .zip(encoded_values) \
        .reduce(lambda acc, x:
                x if x[1] > acc[1] else
                acc)[0]


def serialized(discrete_attribute: DiscreteAttribute
               ) -> DiscreteAttributeSerialized:
    return DiscreteAttributeSerialized(
        discrete_attribute.key,
        discrete_attribute.values.to_list()
    )


def deserialized(discrete_attribute_serialized: Dict
                 ) -> DiscreteAttribute:
    return DiscreteAttribute(
        discrete_attribute_serialized['key'],
        seq(discrete_attribute_serialized['values'])
    )
