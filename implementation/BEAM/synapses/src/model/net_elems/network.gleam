import gleam/pair
import decode.{Decoder}
import gleam/jsone
import gleam_zlists.{ZList} as zlist
import model/utilities as ut
import model/net_elems/activation.{Activation}
import model/net_elems/neuron.{Neuron}
import model/net_elems/layer.{Layer, LayerSerialized}

pub type Network =
  ZList(Layer)

fn lazy_realization(network: Network) -> Network {
  serialized(network)
  network
}

// public
pub fn init(
  layer_sizes: ZList(Int),
  activation_f: fn(Int) -> Activation,
  weight_init_f: fn(Int) -> Float,
) -> Network {
  let Ok(tl) = zlist.tail(layer_sizes)
  zlist.zip(layer_sizes, tl)
  |> zlist.with_index
  |> zlist.map(fn(t) {
    let tuple(tuple(lr_sz, next_lr_sz), index) = t
    layer.init(
      lr_sz,
      next_lr_sz,
      activation_f(index),
      fn() { fn() { weight_init_f(index) } },
    )
  })
  |> lazy_realization
}

// public
pub fn output(network: Network, input_val: ZList(Float)) -> ZList(Float) {
  zlist.reduce(network, input_val, fn(x, acc) { layer.output(x, acc) })
}

fn fed_forward_acc_f(
  already_fed: ZList(tuple(ZList(Float), Layer)),
  next_layer: Layer,
) -> ZList(tuple(ZList(Float), Layer)) {
  let Ok(tuple(errors_val, layer_val)) = zlist.head(already_fed)
  let next_input = layer.output(layer_val, errors_val)
  zlist.cons(already_fed, tuple(next_input, next_layer))
}

fn fed_forward(
  network: Network,
  input_val: ZList(Float),
) -> ZList(tuple(ZList(Float), Layer)) {
  let Ok(tuple(net_hd, net_tl)) = zlist.uncons(network)
  let init_feed =
    tuple(input_val, net_hd)
    |> zlist.singleton
  zlist.reduce(net_tl, init_feed, fn(x, acc) { fed_forward_acc_f(acc, x) })
}

fn back_propagated_acc_f(
  learning_rate: Float,
  errors_with_already_propagated: tuple(ZList(Float), ZList(Layer)),
  input_with_layer: tuple(ZList(Float), Layer),
) -> tuple(ZList(Float), ZList(Layer)) {
  let tuple(errors_val, already_propagated) = errors_with_already_propagated
  let tuple(last_input, last_layer) = input_with_layer
  let last_output_with_errors =
    layer.output(last_layer, last_input)
    |> zlist.zip(errors_val)
  let tuple(next_errors, propagated_layer) =
    layer.back_propagated(
      last_layer,
      learning_rate,
      last_input,
      last_output_with_errors,
    )
  let next_already_propagated = zlist.cons(already_propagated, propagated_layer)
  tuple(next_errors, next_already_propagated)
}

fn back_propagated(
  learning_rate: Float,
  expected_output: ZList(Float),
  reversed_inputs_with_layers: ZList(tuple(ZList(Float), Layer)),
) -> tuple(ZList(Float), Network) {
  let Ok(tuple(tuple(last_input, last_layer), reversed_inputs_with_layers_tl)) =
    zlist.uncons(reversed_inputs_with_layers)
  let output_val = layer.output(last_layer, last_input)
  let errors_val =
    zlist.zip(output_val, expected_output)
    |> zlist.map(fn(t) {
      let tuple(a, b) = t
      a -. b
    })
  let output_with_errors = zlist.zip(output_val, errors_val)
  let tuple(init_errors, first_propagated) =
    layer.back_propagated(
      last_layer,
      learning_rate,
      last_input,
      output_with_errors,
    )
  let init_acc = tuple(init_errors, zlist.singleton(first_propagated))
  zlist.reduce(
    reversed_inputs_with_layers_tl,
    init_acc,
    fn(x, acc) { back_propagated_acc_f(learning_rate, acc, x) },
  )
}

fn errors_with_fit_net(
  network: Network,
  learning_rate: Float,
  input_val: ZList(Float),
  expected_output: ZList(Float),
) -> tuple(ZList(Float), Network) {
  back_propagated(
    learning_rate,
    expected_output,
    fed_forward(network, input_val),
  )
}

// public
pub fn errors(
  network: Network,
  learning_rate: Float,
  input_val: ZList(Float),
  expected_output: ZList(Float),
) -> ZList(Float) {
  let Ok(last_layer) =
    network
    |> zlist.reverse
    |> zlist.head
  let restricted_output =
    zlist.zip(last_layer, expected_output)
    |> zlist.map(fn(t: tuple(Neuron, Float)) {
      let tuple(a, b) = t
      activation.restricted_output(a.activation_f, b)
    })
  network
  |> errors_with_fit_net(learning_rate, input_val, restricted_output)
  |> pair.first
}

// public
pub fn fit(
  network: Network,
  learning_rate: Float,
  input_val: ZList(Float),
  expected_output: ZList(Float),
) -> Network {
  let Ok(last_layer) =
    network
    |> zlist.reverse
    |> zlist.head
  let restricted_output =
    zlist.zip(last_layer, expected_output)
    |> zlist.map(fn(t: tuple(Neuron, Float)) {
      let tuple(a, b) = t
      activation.restricted_output(a.activation_f, b)
    })
  network
  |> errors_with_fit_net(learning_rate, input_val, restricted_output)
  |> pair.second
  |> lazy_realization
}

pub type NetworkSerialized =
  List(LayerSerialized)

pub fn serialized(network: Network) -> NetworkSerialized {
  network
  |> zlist.map(layer.serialized)
  |> zlist.to_list
}

pub fn deserialized(network_serialized: NetworkSerialized) -> Network {
  network_serialized
  |> zlist.of_list
  |> zlist.map(layer.deserialized)
}

pub fn decoder() -> Decoder(NetworkSerialized) {
  decode.list(layer.decoder())
}

pub fn of_json(s: String) -> Network {
  let Ok(dyn) = jsone.decode(s)
  let Ok(res) = decode.decode_dynamic(dyn, decoder())
  deserialized(res)
}
