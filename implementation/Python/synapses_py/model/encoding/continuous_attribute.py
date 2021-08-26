from typing import Dict

from functional import seq
from functional.pipeline import Sequence

from synapses_py.model.encoding.serialization import ContinuousAttribute, ContinuousAttributeSerialized


def parse(s: str) -> float:
    return float(s.strip())


def updated(datapoint: Dict[str, str],
            continuous_attribute: ContinuousAttribute
            ) -> ContinuousAttribute:
    v = parse(datapoint[continuous_attribute.key])
    return ContinuousAttribute(
        continuous_attribute.key,
        min(v, continuous_attribute.min),
        max(v, continuous_attribute.max)
    )


def encode(value: str,
           continuous_attribute: ContinuousAttribute
           ) -> Sequence:
    return seq([0.5]) if \
        (continuous_attribute.min == continuous_attribute.max) else \
        seq([(parse(value) - continuous_attribute.min) /
             (continuous_attribute.max - continuous_attribute.min)])


def decode(encoded_values: Sequence,
           continuous_attribute: ContinuousAttribute
           ) -> str:
    if continuous_attribute.min == continuous_attribute.max:
        v = continuous_attribute.min
    else:
        v = encoded_values.head() * \
            (continuous_attribute.max - continuous_attribute.min) + \
            continuous_attribute.min
    return str(v)


def serialized(continuous_attribute: ContinuousAttribute
               ) -> ContinuousAttributeSerialized:
    return ContinuousAttributeSerialized(
        continuous_attribute.key,
        continuous_attribute.min,
        continuous_attribute.max
    )


def deserialized(continuous_attribute_serialized: Dict
                 ) -> ContinuousAttribute:
    return ContinuousAttribute(
        continuous_attribute_serialized['key'],
        continuous_attribute_serialized['min'],
        continuous_attribute_serialized['max']
    )
