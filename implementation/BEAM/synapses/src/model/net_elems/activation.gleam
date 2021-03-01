import gleam/float
import decode.{Decoder}

pub external fn math_exp(x: Float) -> Float =
  "math" "exp"

pub external fn math_log(x: Float) -> Float =
  "math" "log"

pub external fn math_tanh(x: Float) -> Float =
  "math" "tanh"

pub type Activation {
  Activation(
    name: String,
    f: fn(Float) -> Float,
    deriv: fn(Float) -> Float,
    inverse: fn(Float) -> Float,
    min_max_in_vals: tuple(Float, Float),
  )
}

pub fn restricted_input(activation: Activation, x: Float) -> Float {
  let tuple(min, max) = activation.min_max_in_vals
  float.clamp(x, min, max)
}

pub fn restricted_output(activation: Activation, y: Float) -> Float {
  let tuple(min, max) = activation.min_max_in_vals
  float.clamp(y, activation.f(min), activation.f(max))
}

fn min_abs_float(is_positive: Bool) -> Float {
  let x = 1.7976931348623157 *. float.power(10.0, 308.0)
  case is_positive {
    True -> x
    False -> 0.0 -. x
  }
}

fn sigmoid_f(x: Float) -> Float {
  1.0 /. { 1.0 +. math_exp(0.0 -. x) }
}

pub fn sigmoid() -> Activation {
  Activation(
    name: "sigmoid",
    f: sigmoid_f,
    deriv: fn(d) { sigmoid_f(d) *. { 1.0 -. sigmoid_f(d) } },
    inverse: fn(y) {
      y /. { 1.0 -. y }
      |> math_log
    },
    min_max_in_vals: tuple(-700.0, 20.0),
  )
}

pub fn identity() -> Activation {
  Activation(
    name: "identity",
    f: fn(x) { x },
    deriv: fn(_) { 1.0 },
    inverse: fn(y) { y },
    min_max_in_vals: tuple(min_abs_float(False), min_abs_float(True)),
  )
}

pub fn tanh() -> Activation {
  Activation(
    name: "tanh",
    f: math_tanh,
    deriv: fn(d) { 1.0 -. math_tanh(d) *. math_tanh(d) },
    inverse: fn(y) { 0.5 *. math_log({ 1.0 +. y } /. { 1.0 -. y }) },
    min_max_in_vals: tuple(-10.0, 10.0),
  )
}

pub fn leaky_re_lu() -> Activation {
  Activation(
    name: "leakyReLU",
    f: fn(x) {
      case x <. 0.0 {
        True -> 0.01 *. x
        False -> x
      }
    },
    deriv: fn(d) {
      case d <. 0.0 {
        True -> 0.01
        False -> 1.0
      }
    },
    inverse: fn(y) {
      case y <. 0.0 {
        True -> y /. 0.01
        False -> y
      }
    },
    min_max_in_vals: tuple(min_abs_float(False), min_abs_float(True)),
  )
}

pub type ActivationSerialized =
  String

pub fn serialized(activ_f: Activation) -> ActivationSerialized {
  activ_f.name
}

pub fn deserialized(s: ActivationSerialized) -> Activation {
  case s {
    "sigmoid" -> sigmoid()
    "identity" -> identity()
    "tanh" -> tanh()
    "leakyReLU" -> leaky_re_lu()
  }
}

pub fn decoder() -> Decoder(ActivationSerialized) {
  decode.string()
}
