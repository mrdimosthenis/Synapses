from typing import List, Callable, Tuple, Dict
from dataclasses import dataclass
from functional import seq
from functional.pipeline import Sequence

from synapses_py.model import utilities
from synapses_py.model.mathematics import dot_product
from synapses_py.model.net_elems import activation


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


def output(input_val: Sequence, neuron: Neuron) -> float:
    activation_input = dot_product(
        utilities.lazy_cons(1.0, input_val),
        neuron.weights
    )
    return neuron.activation_f.f(activation_input)


def back_propagated(learning_rate: float,
                    input_val: Sequence,
                    output_with_error: Tuple[float, float],
                    neuron: Neuron
                    ) -> Tuple[Sequence, Neuron]:
    (output_val, error) = output_with_error
    output_inverse = neuron \
        .activation_f \
        .inverse(output_val)
    common = error * neuron.activation_f.deriv(output_inverse)
    in_errors = input_val.map(lambda x: x * common)
    new_weights = neuron \
        .weights \
        .zip(utilities.lazy_cons(1.0, input_val)) \
        .map(lambda t: t[0] - learning_rate * common * t[1])
    new_neuron = Neuron(neuron.activation_f, new_weights)
    return in_errors, new_neuron


@dataclass(frozen=True)
class NeuronSerialized:
    activationF: activation.ActivationSerialized
    weights: List[float]


def serialized(neuron: Neuron) -> NeuronSerialized:
    return NeuronSerialized(
        activation.serialized(neuron.activation_f),
        neuron.weights.to_list()
    )


def deserialized(neuron_serialized: Dict) -> Neuron:
    return Neuron(
        activation.deserialized(neuron_serialized['activationF']),
        seq(neuron_serialized['weights'])
    )
