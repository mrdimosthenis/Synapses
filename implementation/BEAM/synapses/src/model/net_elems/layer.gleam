import decode.{Decoder}
import gleam/jsone.{JsonValue}
import gleam_zlists.{ZList} as zlist
import model/net_elems/activation.{Activation}
import model/net_elems/neuron.{Neuron, NeuronSerialized}

pub type Layer =
  ZList(Neuron)

pub fn init(
  input_size: Int,
  output_size: Int,
  activation_f: Activation,
  weight_init_f: fn() -> fn() -> Float,
) -> Layer {
  zlist.indices()
  |> zlist.take(output_size)
  |> zlist.map(fn(_) { neuron.init(input_size, activation_f, weight_init_f()) })
}

pub fn output(layer: Layer, input_val: ZList(Float)) -> ZList(Float) {
  zlist.map(layer, fn(x) { neuron.output(x, input_val) })
}

pub fn back_propagated(
  layer: Layer,
  learning_rate: Float,
  input_val: ZList(Float),
  output_with_error: ZList(tuple(Float, Float)),
) -> tuple(ZList(Float), Layer) {
  let tuple(errors_multi, new_layer) =
    zlist.zip(output_with_error, layer)
    |> zlist.map(fn(t) {
      let tuple(a, b) = t
      neuron.back_propagated(b, learning_rate, input_val, a)
    })
    |> zlist.unzip
  let errors =
    zlist.reduce(
      errors_multi,
      zlist.indices()
      |> zlist.map(fn(_) { 0.0 }),
      fn(x, acc) {
        zlist.zip(acc, x)
        |> zlist.map(fn(t) {
          let tuple(a, b) = t
          a +. b
        })
      },
    )
  tuple(errors, new_layer)
}

pub type LayerSerialized =
  List(NeuronSerialized)

pub fn serialized(layer: Layer) -> LayerSerialized {
  layer
  |> zlist.map(neuron.serialized)
  |> zlist.to_list
}

pub fn deserialized(layer_serialized: LayerSerialized) -> Layer {
  layer_serialized
  |> zlist.of_list
  |> zlist.map(neuron.deserialized)
}

pub fn json_encoded(layer_serialized: LayerSerialized) -> JsonValue {
  jsone.array(layer_serialized, neuron.json_encoded)
}

pub fn json_decoder() -> Decoder(LayerSerialized) {
  decode.list(neuron.json_decoder())
}
