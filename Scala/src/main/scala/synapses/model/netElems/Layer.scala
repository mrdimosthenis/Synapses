package synapses.model.netElems

import synapses.model.netElems.Neuron.NeuronSerialized

object Layer {

  type Layer = LazyList[Neuron]

  def init(inputSize: Int,
           outputSize: Int,
           activation: Activation)
          (weightInitF: => Double)
  : Layer = LazyList
    .range(0, outputSize)
    .map { _ =>
      Neuron.init(
        inputSize,
        activation
      )(weightInitF)
    }

  def output(input: LazyList[Double])
            (layer: Layer)
  : LazyList[Double] =
    layer.map(Neuron.output(input))

  def backPropagated(learningRate: Double,
                     input: LazyList[Double],
                     outputWithErrors: LazyList[(Double, Double)])
                    (layer: Layer)
  : (LazyList[Double], Layer) = {
    val (errorsMulti, newNeurons) = outputWithErrors
      .zip(layer)
      .map { case (outWithError, neuron) =>
        Neuron.backPropagated(
          learningRate,
          input,
          outWithError
        )(neuron)
      }
      .unzip
    val inErrors =
      errorsMulti.foldLeft(LazyList.continually(0.0)) {
        case (acc, x) => acc
          .zip(x)
          .map(t => t._1 + t._2)
      }
    (inErrors, newNeurons)
  }

  type LayerSerialized = List[NeuronSerialized]

  def serialized(layer: Layer): LayerSerialized =
    layer
      .map(Neuron.serialized)
      .toList

  def deserialized(layerSerialized: LayerSerialized): Layer =
    layerSerialized
      .map(Neuron.deserialized)
      .to(LazyList)

}
