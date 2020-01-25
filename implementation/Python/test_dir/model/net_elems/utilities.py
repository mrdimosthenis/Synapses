from typing import List

from model.net_elems.activation import Activation
from model.net_elems.neuron import Neuron


def realized_activ_fun(activation_val: Activation)-> str:
    return activation_val.name

def realized_neuron(neuron_val: Neuron)-> (str, List[float]):
    return (
        realized_activ_fun(neuron_val.activation_f),
        neuron_val.weights.to_list()
    )
