import gleam/float
import gleam/int
import gleam/string
import gleam/option.{None, Some}
import gleam/result
import gleam_zlists.{ZList} as zlist
import model/utilities as ut
import model/net_elems/activation
import model/net_elems/neuron.{Neuron}
import model/net_elems/layer.{Layer}
import model/net_elems/network.{Network}

const pixels: Float = 400.0

fn circle_vertical_distance() -> Float {
  pixels *. 0.02
}

fn circle_horizontal_distance() -> Float {
  pixels *. 0.15
}

fn circle_radius() -> Float {
  pixels *. 0.03
}

fn circle_stroke_width() -> Float {
  pixels /. 150.0
}

fn line_stroke_width() -> Float {
  pixels /. 300.0
}

const circle_fill: String = "white"

const input_circle_stroke: String = "brown"

const bias_circle_stroke: String = "black"

const sigmoid_circle_stroke: String = "blue"

const identity_circle_stroke: String = "orange"

const tanh_circle_stroke: String = "yellow"

const leaky__re_lu_circle_stroke: String = "pink"

const positive_line_stroke: String = "lawngreen"

const negative_line_stroke: String = "palevioletred"

fn activation_name_to_stroke(activ_name: String) -> String {
  case activ_name {
    "sigmoid" -> sigmoid_circle_stroke
    "identity" -> identity_circle_stroke
    "tanh" -> tanh_circle_stroke
    "leakyReLU" -> leaky__re_lu_circle_stroke
  }
}

fn layer_width(num_of_circles: Int) -> Float {
  let num_of_circles_float = int.to_float(num_of_circles)
  circle_vertical_distance() +. num_of_circles_float *. {
    2.0 *. circle_radius() +. circle_vertical_distance()
  }
}

fn circle_cx(chain_order: Int) -> Float {
  let chain_order_float = int.to_float(chain_order)
  circle_horizontal_distance() +. chain_order_float *. circle_horizontal_distance()
}

fn circle_cy(
  max_chain_circles: Int,
  num_of_chain_circles: Int,
  circle_order: Int,
) -> Float {
  let current_layer_width = layer_width(num_of_chain_circles)
  let max_layer_width = layer_width(max_chain_circles)
  let layer_y = 0.5 *. { max_layer_width -. current_layer_width }
  let circle_order_float = int.to_float(circle_order)
  layer_y +. { circle_order_float +. 1.0 } *. {
    2.0 *. circle_radius() +. circle_vertical_distance()
  }
}

fn circle_svg(x: Float, y: Float, stroke_val: String) -> String {
  [
    "<circle cx=\"",
    float.to_string(x),
    "\" cy=\"",
    float.to_string(y),
    "\" r=\"",
    float.to_string(circle_radius()),
    "\" stroke=\"",
    stroke_val,
    "\" stroke-width=\"",
    float.to_string(circle_stroke_width()),
    "\" fill=\"",
    circle_fill,
    "\"></circle>",
  ]
  |> string.concat
}

fn input_circles_svgs(
  max_chain_circles: Int,
  input_circles: Int,
) -> ZList(String) {
  zlist.indices()
  |> zlist.take(input_circles)
  |> zlist.map(fn(i) {
    let stroke_val = case i == 0 {
      True -> bias_circle_stroke
      False -> input_circle_stroke
    }
    circle_svg(
      circle_cx(0),
      circle_cy(max_chain_circles, input_circles, i),
      stroke_val,
    )
  })
}

fn output_circles_svgs(
  max_chain_circles: Int,
  output_chain_order: Int,
  output_activations: ZList(String),
) -> ZList(String) {
  let num_of_activations = zlist.count(output_activations)
  output_activations
  |> zlist.with_index
  |> zlist.map(fn(t) {
    let tuple(activ, i) = t
    circle_svg(
      circle_cx(output_chain_order),
      circle_cy(max_chain_circles, num_of_activations, i),
      activation_name_to_stroke(activ),
    )
  })
}

fn hidden_circles_svgs(
  max_chain_circles: Int,
  hidden_chain_order: Int,
  hidden_activations: ZList(String),
) -> ZList(String) {
  let num_of_activations = zlist.count(hidden_activations)
  hidden_activations
  |> zlist.map(Some)
  |> zlist.cons(None)
  |> zlist.with_index
  |> zlist.map(fn(t) {
    let tuple(maybe_activ, i) = t
    let stroke_val = case maybe_activ {
      Some(activ) -> activation_name_to_stroke(activ)
      None -> bias_circle_stroke
    }
    circle_svg(
      circle_cx(hidden_chain_order),
      circle_cy(max_chain_circles, num_of_activations + 1, i),
      stroke_val,
    )
  })
}

fn layer_circles_svgs(
  max_chain_circles: Int,
  layer_order: Int,
  num_of_layers: Int,
  layer_val: Layer,
) -> ZList(String) {
  let is_last_layer = layer_order == num_of_layers - 1
  let Ok(prev_layer_size) =
    layer_val
    |> zlist.head
    |> result.map(fn(neuron_val: Neuron) { zlist.count(neuron_val.weights) })
  let activations =
    zlist.map(
      layer_val,
      fn(neuron_val: Neuron) { neuron_val.activation_f.name },
    )
  let input_circles = case layer_order == 0 {
    True -> input_circles_svgs(max_chain_circles, prev_layer_size)
    False -> zlist.new()
  }
  let hidden_circles = case is_last_layer {
    True -> zlist.new()
    False ->
      hidden_circles_svgs(max_chain_circles, layer_order + 1, activations)
  }
  let output_circles = case is_last_layer {
    True -> output_circles_svgs(max_chain_circles, layer_order + 1, activations)
    False -> zlist.new()
  }
  input_circles
  |> zlist.append(hidden_circles)
  |> zlist.append(output_circles)
}

fn line_svg(
  max_chain_circles: Int,
  base_chain_order: Int,
  num_of_circles_in_base_chain: Int,
  num_of_circles_in_target_chain: Int,
  base_circle_order: Int,
  target_circle_order: Int,
  weight: Float,
  max_abs_weight: Float,
) -> String {
  let alpha = float.absolute_value(weight) /. max_abs_weight
  let x1_val = circle_cx(base_chain_order)
  let y1_val =
    circle_cy(
      max_chain_circles,
      num_of_circles_in_base_chain,
      base_circle_order,
    )
  let x2_val = circle_cx(base_chain_order + 1)
  let y2_val =
    circle_cy(
      max_chain_circles,
      num_of_circles_in_target_chain,
      target_circle_order,
    )
  let stroke_val = case weight >. 0.0 {
    True -> positive_line_stroke
    False -> negative_line_stroke
  }
  [
    "<line stroke-opacity=\"",
    float.to_string(alpha),
    "\" x1=\"",
    float.to_string(x1_val),
    "\" y1=\"",
    float.to_string(y1_val),
    "\" x2=\"",
    float.to_string(x2_val),
    "\" y2=\"",
    float.to_string(y2_val),
    "\" stroke=\"",
    stroke_val,
    "\" stroke-width=\"",
    float.to_string(line_stroke_width()),
    "\"></line>",
  ]
  |> string.concat
}

fn neuron_lines_svgs(
  max_chain_circles: Int,
  layer_size: Int,
  layer_order: Int,
  num_of_layers: Int,
  neuron_order_in_layer: Int,
  max_abs_weight: Float,
  weights: ZList(Float),
) -> ZList(String) {
  let is_output_layer = layer_order == num_of_layers - 1
  let num_of_circles_in_base_chain = zlist.count(weights)
  let num_of_circles_in_target_chain = case is_output_layer {
    True -> layer_size
    False -> layer_size + 1
  }
  let target_circle_order = case is_output_layer {
    True -> neuron_order_in_layer
    False -> neuron_order_in_layer + 1
  }
  weights
  |> zlist.with_index
  |> zlist.map(fn(t) {
    let tuple(w, i) = t
    line_svg(
      max_chain_circles,
      layer_order,
      num_of_circles_in_base_chain,
      num_of_circles_in_target_chain,
      i,
      target_circle_order,
      w,
      max_abs_weight,
    )
  })
}

fn layer_lines_svgs(
  max_chain_circles: Int,
  layer_order: Int,
  num_of_layers: Int,
  max_abs_weight: Float,
  layer_val: Layer,
) -> ZList(String) {
  let num_of_neurons = zlist.count(layer_val)
  layer_val
  |> zlist.with_index
  |> zlist.flat_map(fn(t: tuple(Neuron, Int)) {
    let tuple(neuron, i) = t
    neuron_lines_svgs(
      max_chain_circles,
      num_of_neurons,
      layer_order,
      num_of_layers,
      i,
      max_abs_weight,
      neuron.weights,
    )
  })
}

pub fn network_svg(network_val: Network) -> String {
  let num_of_layers = zlist.count(network_val)
  let max_chain_circles =
    network_val
    |> zlist.with_index
    |> zlist.map(fn(t: tuple(Layer, Int)) {
      let tuple(layer_val, i) = t
      case i == num_of_layers - 1 {
        True -> zlist.count(layer_val) + 1
        False -> zlist.count(layer_val)
      }
    })
    |> ut.lazy_max_int
  let max_abs_weight =
    network_val
    |> zlist.flat_map(fn(layer_val) {
      zlist.flat_map(
        layer_val,
        fn(neuron_val: Neuron) {
          zlist.map(neuron_val.weights, fn(w) { float.absolute_value(w) })
        },
      )
    })
    |> ut.lazy_max_float
  let circles_svgs =
    network_val
    |> zlist.with_index
    |> zlist.flat_map(fn(t) {
      let tuple(layer_val, i) = t
      layer_circles_svgs(max_chain_circles, i, num_of_layers, layer_val)
    })
  let lines_svgs =
    network_val
    |> zlist.with_index
    |> zlist.flat_map(fn(t) {
      let tuple(layer_val, i) = t
      layer_lines_svgs(
        max_chain_circles,
        i,
        num_of_layers,
        max_abs_weight,
        layer_val,
      )
    })
  let w = circle_cx(num_of_layers + 1)
  let h = circle_cy(max_chain_circles, max_chain_circles, max_chain_circles)
  let net_svgs = zlist.append(lines_svgs, circles_svgs)
  [
    "<svg width=\"",
    float.to_string(w),
    "\" height=\"",
    float.to_string(h),
    "\">",
    zlist.reduce(net_svgs, "", fn(x, acc) { string.append(acc, x) }),
    "</svg>",
  ]
  |> string.concat
}
