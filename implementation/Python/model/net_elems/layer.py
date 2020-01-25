from itertools import count
from typing import Callable, Tuple, List
from functional import seq
from functional.pipeline import Sequence

from model import utilities
from model.net_elems import activation
from model.net_elems import neuron
from model.net_elems.neuron import NeuronSerialized

Layer = Sequence


def init(input_size: int,
         output_size: int,
         activation_f: activation.Activation,
         weight_init_f: Callable[[], Callable[[], float]]
         ) -> Layer:
    return seq \
        .range(output_size) \
        .map(lambda _:
             neuron.init(input_size,
                         activation_f,
                         weight_init_f()))


def output(input: Sequence, layer: Layer) -> Sequence:
    return layer.map(lambda x: neuron.output(input, x))


def back_propagated(learning_rate: float,
                    input: Sequence,
                    output_with_errors: Sequence,
                    layer: Layer
                    ) -> Tuple[Sequence, Layer]:
    errors_multi_with_new_neurons = output_with_errors \
        .zip(layer) \
        .map(lambda t: neuron.back_propagated(learning_rate, input, t[0], t[1]))
    (errors_multi, new_neurons) = utilities \
        .lazy_unzip(errors_multi_with_new_neurons)
    in_errors = errors_multi.fold_left(
        seq(count(0)).map(lambda _: 0.0),
        lambda acc, x: acc.zip(x).map(lambda t: t[0] + t[1])
    )
    return in_errors, new_neurons


LayerSerialized = List[NeuronSerialized]


def serialized(layer: Layer) -> LayerSerialized:
    return layer \
        .map(lambda x: neuron.serialized(x)) \
        .to_list()


def deserialized(layer_serialized: LayerSerialized) -> Layer:
    return seq(layer_serialized) \
        .map(lambda x: neuron.deserialized(x))
