package synapses.model.netElems

import synapses.model.Mathematics
import synapses.model.netElems.Activation.ActivationSerialized

case class Neuron(activationF: Activation,
                  weights: LazyList[Double])

object Neuron {

  def init(inputSize: Int,
           activationF: Activation)
          (weightInitF: => Double)
  : Neuron = {
    val weights = LazyList
      .range(0, inputSize + 1)
      .map(_ => weightInitF)
    Neuron(activationF, weights)
  }

  def output(input: LazyList[Double])
            (neuron: Neuron)
  : Double = {
    val activationInput = Mathematics
      .dotProduct(
        LazyList.cons(1.0, input),
        neuron.weights
      )
    neuron
      .activationF
      .f(activationInput)
  }

  def backPropagated(learningRate: Double,
                     input: LazyList[Double],
                     outputWithError: (Double, Double))
                    (neuron: Neuron)
  : (LazyList[Double], Neuron) = {
    val (output, error) = outputWithError
    val outputInverse =
      neuron
        .activationF
        .inverse(output)
    val common =
      error * neuron.activationF.deriv(outputInverse)
    val inErrors = input.map(_ * common)
    val newWeights = neuron.weights
      .zip(LazyList.cons(1.0, input))
      .map { case (w, x) =>
        w - learningRate * common * x
      }
    val newNeuron = neuron.copy(weights = newWeights)
    (inErrors, newNeuron)
  }

  case class NeuronSerialized(activationF: ActivationSerialized,
                              weights: List[Double])

  def serialized(neuron: Neuron): NeuronSerialized =
    NeuronSerialized(
      Activation.serialized(neuron.activationF),
      neuron.weights.toList
    )

  def deserialized(neuronSerialized: NeuronSerialized): Neuron =
    Neuron(
      Activation.deserialized(neuronSerialized.activationF),
      neuronSerialized.weights.to(LazyList)
    )

}
