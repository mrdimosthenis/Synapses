from itertools import count
from typing import Callable, Tuple, List
from functional import seq
from functional.pipeline import Sequence

from synapses_py.model import utilities
from synapses_py.model.net_elems import neuron
from synapses_py.model.net_elems.activation import Activation
from synapses_py.model.net_elems.neuron import NeuronSerialized

Layer = Sequence


def init(input_size: int,
         output_size: int,
         activation_f: Activation,
         weight_init_f: Callable[[], Callable[[], float]]
         ) -> Layer:
    return seq \
        .range(output_size) \
        .map(lambda _:
             neuron.init(input_size,
                         activation_f,
                         weight_init_f()))


def output(input_val: Sequence, layer_val: Layer) -> Sequence:
    return layer_val.map(lambda x: neuron.output(input_val, x))


def back_propagated(learning_rate: float,
                    input_val: Sequence,
                    output_with_errors: Sequence,
                    layer: Layer
                    ) -> Tuple[Sequence, Layer]:
    errors_multi_with_new_neurons = output_with_errors \
        .zip(layer) \
        .map(lambda t: neuron.back_propagated(learning_rate, input_val, t[0], t[1]))
    (errors_multi, new_neurons) = utilities \
        .lazy_unzip(errors_multi_with_new_neurons)
    in_errors = errors_multi.fold_left(
        seq(count(0)).map(lambda _: 0.0),
        lambda acc, x: acc.zip(x).map(lambda t: t[0] + t[1])
    )
    return in_errors, new_neurons


LayerSerialized = List[NeuronSerialized]


def serialized(layer_val: Layer) -> LayerSerialized:
    return seq(layer_val) \
        .map(lambda x: neuron.serialized(x)) \
        .to_list()


def deserialized(layer_serialized: LayerSerialized) -> Layer:
    return seq(layer_serialized) \
        .map(lambda x: neuron.deserialized(x))
