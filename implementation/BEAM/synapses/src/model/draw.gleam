import gleam/float
import gleam/int
import gleam/string
import gleam/option.{None, Some}
import gleam_zlists.{ZList} as zlist
import model/net_elems/activation.{Activation}

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
