import gleam_zlists.{ZList} as zlist
import model/utilities as utils
import model/net_elems/activation.{Activation}

pub type Neuron {
  Neuron(activation_f: Activation, weights: ZList(Float))
}

pub fn init(
  input_size: Int,
  activation_f: Activation,
  weight_init_f: fn() -> Float,
) -> Neuron {
  let weights =
    utils.infinite_indices()
    |> zlist.map(fn(_) { weight_init_f() })
    |> zlist.take(input_size + 1)

  Neuron(activation_f: activation_f, weights: weights)
}
