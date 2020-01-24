from typing import List, Callable, Tuple
from dataclasses import dataclass
from functional import seq
from functional.pipeline import Sequence
from fn import _

from model.mathematics import dot_product
from model.net_elems import activation


@dataclass(frozen=True)
class Neuron:
    activation_f: activation.Activation
    weights: Sequence


def init(input_size: int,
         activation_f: activation.Activation,
         weight_init_f: Callable[[], float]
         ) -> Neuron:
    weights = seq \
        .range(input_size + 1) \
        .map(lambda _: weight_init_f())
    return Neuron(activation_f, weights)


def output(input: Sequence, neuron: Neuron) -> float:
    activation_input = dot_product(
        seq([1.0]) + input,
        neuron.weights
    )
    return neuron.activation_f.f(activation_input)


def back_propagated(learning_rate: float,
                    input: Sequence,
                    output_with_error: Tuple[float, float],
                    neuron: Neuron
                    ) -> Tuple[Sequence, Neuron]:
    (output, error) = output_with_error
    output_inverse = neuron \
        .activation_f \
        .inverse(output)
    common = error * neuron.activation_f.deriv(output_inverse)
    in_errors = input.map(_ * common)
    new_weights = neuron \
        .weights \
        .zip(seq([1.0] + input)) \
        .map(lambda t: t[0] - learning_rate * common * t[1])
    new_neuron = Neuron(neuron.activation_f, new_weights)
    return in_errors, new_neuron


@dataclass(frozen=True)
class NeuronSerialized:
    activation_f: activation.ActivationSerialized
    weights: List[float]


def serialized(neuron: Neuron) -> NeuronSerialized:
    return NeuronSerialized(
        activation.serialized(neuron.activation_f),
        neuron.weights.to_list()
    )


def deserialized(neuronSerialized: NeuronSerialized) -> Neuron:
    return Neuron(
        activation.deserialized(neuronSerialized.activation_f),
        seq(neuronSerialized.weights)
    )
