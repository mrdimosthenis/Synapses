from typing import List

from functional import seq

from model.net_elems import neuron, activation
from model.net_elems.neuron import Neuron
from test_dir.model.net_elems import utilities


def neuron_eq(neuron_val: Neuron,
              realized: (str, List[float])
              ) -> bool:
    return \
        utilities.realized_neuron(neuron_val) == realized


neuron_val = neuron.init(3, activation.sigmoid, lambda: -0.2)

input_val = seq([0.0, 1.1, 2.2])

output_val = 0.2973393456552685

assert neuron_eq(
    neuron_val,
    ('sigmoid', [-0.2, -0.2, -0.2, -0.2])), \
    "initialize"
