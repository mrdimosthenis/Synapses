from functional import seq
from functional.pipeline import Sequence

from synapses_py.model import utilities
from synapses_py.model.net_elems.layer import Layer
from synapses_py.model.net_elems.network import Network

pixels = 400.0

circle_vertical_distance = pixels * 0.02
circle_horizontal_distance = pixels * 0.15

circle_radius = pixels * 0.03
circle_stroke_width = pixels / 150

line_stroke_width = pixels / 300

circle_fill = 'white'

input_circle_stroke = 'brown'
bias_circle_stroke = 'black'

sigmoid_circle_stroke = 'blue'
identity_circle_stroke = 'orange'
tanh_circle_stroke = 'yellow'
leaky_ReLU_circle_stroke = 'pink'

positiveLineStroke = 'lawngreen'
negativeLineStroke = 'palevioletred'


def activation_name_to_stroke(activ_name: str) -> str:
    return {
        'sigmoid': sigmoid_circle_stroke,
        'identity': identity_circle_stroke,
        'tanh': tanh_circle_stroke,
        'leakyReLU': leaky_ReLU_circle_stroke,
    }[activ_name]


def layer_width(num_of_circles: int) -> float:
    return circle_vertical_distance + num_of_circles * \
           (2 * circle_radius + circle_vertical_distance)


def circle_cx(chain_order: int) -> float:
    return circle_horizontal_distance + \
           chain_order * circle_horizontal_distance


def circle_cy(max_chain_circles: int,
              num_of_chain_circles: int,
              circle_order: int) -> float:
    current_layer_width = layer_width(num_of_chain_circles)
    max_layer_width = layer_width(max_chain_circles)
    layer_y = (max_layer_width - current_layer_width) / 2
    return layer_y + (circle_order + 1) * (2 * circle_radius + circle_vertical_distance)


def circle_svg(x: float,
               y: float,
               stroke_val: str) -> str:
    return '<circle cx="%f" cy="%f" r="%f" stroke="%s" stroke-width="%f" fill="%s"></circle>' \
           % (x, y, circle_radius, stroke_val, circle_stroke_width, circle_fill)


def input_circles_svgs(max_chain_circles: int,
                       input_circles: int) -> Sequence:
    return seq \
        .range(input_circles) \
        .map(lambda i:
             circle_svg(
                 circle_cx(0),
                 circle_cy(max_chain_circles, input_circles, i),
                 bias_circle_stroke if i == 0 else input_circle_stroke))


def output_circles_svgs(max_chain_circles: int,
                        output_chain_order: int,
                        output_activations: Sequence) -> Sequence:
    return output_activations \
        .zip_with_index() \
        .map(lambda t:
             circle_svg(
                 circle_cx(output_chain_order),
                 circle_cy(max_chain_circles, output_activations.size(), t[1]),
                 activation_name_to_stroke(t[0])))


def hidden_circles_svgs(max_chain_circles: int,
                        hidden_chain_order: int,
                        hidden_activations: Sequence) -> Sequence:
    return utilities \
        .lazy_cons(None, hidden_activations) \
        .zip_with_index() \
        .map(lambda t:
             circle_svg(
                 circle_cx(hidden_chain_order),
                 circle_cy(
                     max_chain_circles,
                     hidden_activations.size() + 1,
                     t[1]
                 ),
                 bias_circle_stroke if t[0] is None else activation_name_to_stroke(t[0])))


def layer_circles_svgs(max_chain_circles: int,
                       layer_order: int,
                       num_of_layers: int,
                       layer_val: Layer) -> Sequence:
    is_last_layer = layer_order == num_of_layers - 1
    activations = layer_val \
        .map(lambda neuron_val: neuron_val.activation_f.name)
    if layer_order == 0:
        input_circles = input_circles_svgs(
            max_chain_circles,
            layer_val.head().weights.size()
        )
    else:
        input_circles = seq([])
    if is_last_layer:
        hidden_circles = seq([])
    else:
        hidden_circles = hidden_circles_svgs(
            max_chain_circles,
            layer_order + 1,
            activations
        )
    if is_last_layer:
        output_circles = output_circles_svgs(
            max_chain_circles,
            layer_order + 1,
            activations
        )
    else:
        output_circles = seq([])
    return input_circles + hidden_circles + output_circles


def line_svg(max_chain_circles: int,
             base_chain_order: int,
             num_of_circles_in_base_chain: int,
             num_of_circles_in_target_chain: int,
             base_circle_order: int,
             target_circle_order: int,
             weight: float,
             max_abs_weight: float) -> str:
    alpha = abs(weight) / max_abs_weight
    x1_val = circle_cx(base_chain_order)
    y1_val = circle_cy(
        max_chain_circles,
        num_of_circles_in_base_chain,
        base_circle_order
    )
    x2_val = circle_cx(base_chain_order + 1)
    y2_val = circle_cy(
        max_chain_circles,
        num_of_circles_in_target_chain,
        target_circle_order
    )
    stroke_val = positiveLineStroke if weight > 0 else negativeLineStroke
    return '<line stroke-opacity="%f" x1="%f" y1="%f" x2="%f" y2="%f" stroke="%s" stroke-width="%f"></line>' \
           % (alpha, x1_val, y1_val, x2_val, y2_val, stroke_val, line_stroke_width)


def neuron_lines_svgs(max_chain_circles: int,
                      layer_size: int,
                      layer_order: int,
                      num_of_layers: int,
                      neuron_order_in_layer: int,
                      max_abs_weight: float,
                      weights: Sequence) -> Sequence:
    is_output_layer = \
        layer_order == num_of_layers - 1
    num_of_circles_in_base_chain = weights.size()
    if is_output_layer:
        num_of_circles_in_target_chain = layer_size
    else:
        num_of_circles_in_target_chain = layer_size + 1
    if is_output_layer:
        target_circle_order = neuron_order_in_layer
    else:
        target_circle_order = neuron_order_in_layer + 1
    return weights \
        .zip_with_index() \
        .map(lambda t:
             line_svg(
                 max_chain_circles,
                 layer_order,
                 num_of_circles_in_base_chain,
                 num_of_circles_in_target_chain,
                 t[1],
                 target_circle_order,
                 t[0],
                 max_abs_weight))


def layer_lines_svgs(max_chain_circles: int,
                     layer_order: int,
                     num_of_layers: int,
                     max_abs_weight: float,
                     layer_val: Layer) -> Sequence:
    return layer_val \
        .zip_with_index() \
        .flat_map(lambda t:
                  neuron_lines_svgs(
                      max_chain_circles,
                      layer_val.size(),
                      layer_order,
                      num_of_layers,
                      t[1],
                      max_abs_weight,
                      t[0].weights))


def network_svg(network_val: Network) -> str:
    max_chain_circles = \
        network_val \
            .zip_with_index() \
            .map(lambda t:
                 t[0].size() + 1 if t[1] == network_val.size() - 1 \
                     else t[0].size()) \
            .max()
    num_of_layers = network_val.size()
    max_abs_weight = network_val \
        .flat_map(
        lambda layer_val:
        layer_val.flat_map(
            lambda neuron_val:
            neuron_val.weights.map(
                lambda weight:
                abs(weight)
            )
        )
    ).max()
    circles_svgs = network_val \
        .zip_with_index() \
        .flat_map(lambda t:
                  layer_circles_svgs(
                      max_chain_circles,
                      t[1],
                      num_of_layers,
                      t[0]))
    lines_svgs = network_val \
        .zip_with_index() \
        .flat_map(lambda t:
                  layer_lines_svgs(
                      max_chain_circles,
                      t[1],
                      num_of_layers,
                      max_abs_weight,
                      t[0]))
    w = circle_cx(num_of_layers + 1)
    h = circle_cy(
        max_chain_circles,
        max_chain_circles,
        max_chain_circles
    )
    net_svgs = lines_svgs + circles_svgs
    return '<svg width="%f" height="%f">%s</svg>' \
           % (w, h, net_svgs.fold_left('', lambda acc, x: acc + x))
