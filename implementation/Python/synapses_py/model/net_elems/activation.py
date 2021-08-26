import math
from typing import Callable
from dataclasses import dataclass


@dataclass(frozen=True)
class Activation:
    name: str
    f: Callable[[float], float]
    deriv: Callable[[float], float]
    inverse: Callable[[float], float]
    min_max_in_vals: (float, float)


def restricted_input(activation: Activation, x: float) -> float:
    return max(
        min(
            x,
            activation.min_max_in_vals[1]
        ),
        activation.min_max_in_vals[0]
    )


def restricted_output(activation: Activation, y: float) -> float:
    return max(
        min(
            y,
            activation.f(activation.min_max_in_vals[1])
        ),
        activation.f(activation.min_max_in_vals[0])
    )


def sigmoid_f(x: float) -> float:
    return 1.0 / (1.0 + math.exp(-x))


sigmoid: Activation = Activation(
    'sigmoid',
    sigmoid_f,
    lambda d: sigmoid_f(d) * (1.0 - sigmoid_f(d)),
    lambda y: math.log(y / (1.0 - y)),
    (-700.0, 20.0)
)

identity: Activation = Activation(
    'identity',
    lambda x: x,
    lambda _: 1.0,
    lambda y: y,
    (-1.7976931348623157E308, 1.7976931348623157E308)
)

tanh: Activation = Activation(
    'tanh',
    math.tanh,
    lambda d: 1.0 - math.tanh(d) * math.tanh(d),
    lambda y: 0.5 * math.log((1.0 + y) / (1.0 - y)),
    (-10.0, 10.0)
)

leakyReLU: Activation = Activation(
    'leakyReLU',
    lambda x: 0.01 * x if x < 0.0 else x,
    lambda d: 0.01 if d < 0.0 else 1.0,
    lambda y: y / 0.01 if y < 0.0 else y,
    (-1.7976931348623157E308, 1.7976931348623157E308)
)

ActivationSerialized = str


def serialized(activ_f: Activation) -> ActivationSerialized:
    return activ_f.name


def deserialized(s: ActivationSerialized) -> Activation:
    return {
        'sigmoid': sigmoid,
        'identity': identity,
        'tanh': tanh,
        'leakyReLU': leakyReLU,
    }[s]
