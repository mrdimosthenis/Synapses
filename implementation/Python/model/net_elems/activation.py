import math
from typing import Callable
from dataclasses import dataclass


@dataclass(frozen=True)
class Function:
    name: str
    f: Callable[[float], float]
    deriv: Callable[[float], float]
    inverse: Callable[[float], float]


def safe_sigmoid(x: float) -> float:
    n = min(-x, 500.0)
    return 1.0 / (1.0 + math.exp(n))


sigmoid: Function = Function(
    'sigmoid',
    safe_sigmoid,
    lambda x: safe_sigmoid(x) * (1.0 - safe_sigmoid(x)),
    lambda x: math.log(x / (1.0 - x))
)

identity: Function = Function(
    'identity',
    lambda x: x,
    lambda _: 1.0,
    lambda x: x
)

tanh: Function = Function(
    'tanh',
    math.tanh,
    lambda x: 1.0 - math.tanh(x) * math.tanh(x),
    lambda x: 0.5 * math.log((1.0 + x) / (1.0 - x))
)

leakyReLU: Function = Function(
    'leakyReLU',
    lambda x: 0.01 * x if x < 0.0 else x,
    lambda x: 0.01 if x < 0.0 else 1.0,
    lambda x: x / 0.01 if x < 0.0 else x
)

ActivationSerialized = str


def serialized(activ_f: Function) -> ActivationSerialized:
    return activ_f.name


def deserialized(s: ActivationSerialized) -> Function:
    return {
        'sigmoid': sigmoid,
        'identity': identity,
        'tanh': tanh,
        'leakyReLU': leakyReLU,
    }[s]
