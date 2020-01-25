from typing import Dict
from functional.pipeline import Sequence

from model.encoding import discrete_attribute, continuous_attribute
from model.encoding.serialization import Preprocessor, DiscreteAttribute, ContinuousAttribute, Attribute


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

