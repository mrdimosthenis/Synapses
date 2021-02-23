import gleam/float

pub external fn exp(x: Float) -> Float =
  "math" "exp"

pub external fn log(x: Float) -> Float =
  "math" "log"

pub type Activation {
  Activation(
    name: String,
    f: fn(Float) -> Float,
    deriv: fn(Float) -> Float,
    inverse: fn(Float) -> Float,
    min_max_in_vals: tuple(Float, Float),
  )
}

fn restricted_input(activation: Activation, x: Float) -> Float {
  let tuple(min, max) = activation.min_max_in_vals
  float.clamp(x, min, max)
}

fn restricted_output(activation: Activation, y: Float) -> Float {
  let tuple(min, max) = activation.min_max_in_vals
  float.clamp(y, activation.f(min), activation.f(max))
}

fn sigmoid_f(x: Float) -> Float {
  1.0 /. { 1.0 +. exp(0.0 -. x) }
}

pub fn sigmoid() -> Activation {
  Activation(
    name: "sigmoid",
    f: sigmoid_f,
    deriv: fn(d) { sigmoid_f(d) *. { 1.0 -. sigmoid_f(d) } },
    inverse: fn(y) {
      y /. { 1.0 -. y }
      |> log
    },
    min_max_in_vals: tuple(-700.0, 20.0),
  )
}
