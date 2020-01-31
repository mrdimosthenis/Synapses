import json
from typing import Callable, List

from functional import seq
from functional.pipeline import Sequence

from synapses_py.model import utilities
from synapses_py.model.net_elems import layer
from synapses_py.model.net_elems.activation import Activation
from synapses_py.model.net_elems.layer import Layer, LayerSerialized

Network = Sequence


def init(layer_sizes: Sequence,
         activation_f: Callable[[int], Activation],
         weight_init_f: Callable[[int], float]
         ) -> Network:
    return layer_sizes \
        .zip(layer_sizes.tail()) \
        .zip_with_index() \
        .map(lambda t:
             layer.init(
                 t[0][0],
                 t[0][1],
                 activation_f(t[1]),
                 lambda: lambda: weight_init_f(t[1])))


def output(input_val: Sequence,
           network_val: Network
           ) -> Sequence:
    return network_val.fold_left(
        input_val,
        lambda acc, x: layer.output(acc, x)
    )


def fed_forward_acc_f(already_fed: Sequence,
                      next_layer: Layer
                      ) -> Sequence:
    (errors_val, layer_val) = already_fed.head()
    next_input = layer.output(errors_val, layer_val)
    return utilities \
        .lazy_cons((next_input, next_layer), already_fed)


def fed_forward(input_val: Sequence,
                network: Network
                ) -> Sequence:
    init_feed = seq([(input_val, network.head())])
    return network \
        .tail() \
        .fold_left(init_feed, lambda acc, x: fed_forward_acc_f(acc, x))


def back_propagated_acc_f(learning_rate: float,
                          errors_with_already_propagated: Sequence,
                          input_with_layer: (Sequence, Layer)
                          ) -> (Sequence, Sequence):
    (errors_val, already_propagated) = errors_with_already_propagated
    (last_input, last_layer) = input_with_layer
    last_output_with_errors = layer \
        .output(last_input, last_layer) \
        .zip(errors_val)
    (next_errors, propagated_layer) = layer.back_propagated(
        learning_rate,
        last_input,
        last_output_with_errors,
        last_layer
    )
    next_already_propagated = utilities \
        .lazy_cons(propagated_layer, already_propagated)
    return next_errors, next_already_propagated


def back_propagated(learning_rate: float,
                    expected_output: Sequence,
                    reversed_inputs_with_layers: Sequence
                    ) -> (Sequence, Network):
    (last_input, last_layer) = reversed_inputs_with_layers.head()
    output_val = layer.output(last_input, last_layer)
    errors_val = output_val \
        .zip(expected_output) \
        .map(lambda t: t[0] - t[1])
    output_with_errors = output_val.zip(errors_val)
    (init_errors, first_propagated) = layer.back_propagated(
        learning_rate,
        last_input,
        output_with_errors,
        last_layer
    )
    init_acc = (init_errors, seq([first_propagated]))
    return reversed_inputs_with_layers \
        .tail() \
        .fold_left(init_acc,
                   lambda acc, x:
                   back_propagated_acc_f(learning_rate, acc, x))


def errors_with_fit_net(learning_rate: float,
                        input_val: Sequence,
                        expected_output: Sequence,
                        network: Network
                        ) -> (Sequence, Network):
    return back_propagated(
        learning_rate,
        expected_output,
        fed_forward(input_val, network)
    )


def errors(learning_rate: float,
           input_val: Sequence,
           expected_output: Sequence,
           network: Network
           ) -> Sequence:
    return errors_with_fit_net(
        learning_rate,
        input_val,
        expected_output,
        network
    )[0]


def fit(learning_rate: float,
        input_val: Sequence,
        expected_output: Sequence,
        network: Network
        ) -> Sequence:
    return errors_with_fit_net(
        learning_rate,
        input_val,
        expected_output,
        network
    )[1]


NetworkSerialized = List[LayerSerialized]


def serialized(network_val: Network) -> NetworkSerialized:
    return network_val \
        .map(lambda x: layer.serialized(x)) \
        .to_list()


def to_json(network: Network) -> str:
    return json.dumps(
        serialized(network),
        separators=(',', ':'),
        cls=utilities.EnhancedJSONEncoder
    )


def deserialized(network_serialized: NetworkSerialized) -> Network:
    return seq(network_serialized).map(lambda x: layer.deserialized(x))


def of_json(s: str) -> Network:
    return deserialized(json.loads(s))
