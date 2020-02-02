package synapses.model.netElems

import io.circe._
import io.circe.generic.auto._
import io.circe.parser._
import io.circe.syntax._
import synapses.model.Utilities
import synapses.model.netElems.Layer.{Layer, LayerSerialized}

object Network {

  type Network = LazyList[Layer]

  // public
  def init(layerSizes: LazyList[Int],
           activationF: Int => Activation)
          (weightInitF: Int => Double)
  : Network = Utilities.lazyNetworkRealization(
    layerSizes
      .zip(layerSizes.tail)
      .zip(LazyList.from(0, 1))
      .map { case ((lrSz, nextLrSz), index) =>
        Layer.init(
          lrSz,
          nextLrSz,
          activationF(index)
        )(weightInitF(index))
      }
  )

  // public
  def output(input: LazyList[Double])
            (network: Network)
  : LazyList[Double] =
    network.foldLeft(input) { case (acc, x) =>
      Layer.output(acc)(x)
    }

  private def fedForwardAccF(alreadyFed: LazyList[(LazyList[Double], Layer)],
                             nextLayer: Layer)
  : LazyList[(LazyList[Double], Layer)] = {
    val (errors, layer) = alreadyFed.head
    val nextInput = Layer
      .output(errors)(layer)
    LazyList.cons((nextInput, nextLayer), alreadyFed)
  }

  private def fedForward(input: LazyList[Double])
                        (network: Network)
  : LazyList[(LazyList[Double], Layer)] = {
    val initFeed = LazyList((input, network.head))
    network
      .tail
      .foldLeft(initFeed)(fedForwardAccF)
  }

  private def backPropagatedAccF(learningRate: Double)
                                (errorsWithAlreadyPropagated: (LazyList[Double], LazyList[Layer]),
                                 inputWithLayer: (LazyList[Double], Layer))
  : (LazyList[Double], LazyList[Layer]) = {
    val (errors, alreadyPropagated) = errorsWithAlreadyPropagated
    val (lastInput, lastLayer) = inputWithLayer
    val lastOutputWithErrors = Layer
      .output(lastInput)(lastLayer)
      .zip(errors)
    val (nextErrors, propagatedLayer) =
      Layer.backPropagated(
        learningRate,
        lastInput,
        lastOutputWithErrors
      )(lastLayer)
    val nextAlreadyPropagated =
      LazyList.cons(
        propagatedLayer,
        alreadyPropagated
      )
    (nextErrors, nextAlreadyPropagated)
  }

  private def backPropagated(learningRate: Double,
                             expectedOutput: LazyList[Double],
                             reversedInputsWithLayers: LazyList[(LazyList[Double], Layer)])
  : (LazyList[Double], Network) = {
    val (lastInput, lastLayer) = reversedInputsWithLayers.head
    val output = Layer.output(lastInput)(lastLayer)
    val errors = output
      .zip(expectedOutput)
      .map(t => t._1 - t._2)
    val outputWithErrors = output.zip(errors)
    val (initErrors, firstPropagated) =
      Layer.backPropagated(
        learningRate,
        lastInput,
        outputWithErrors
      )(lastLayer)
    val initAcc = (initErrors, LazyList(firstPropagated))
    reversedInputsWithLayers
      .tail
      .foldLeft(initAcc)(backPropagatedAccF(learningRate))
  }

  private def errorsWithFitNet(learningRate: Double,
                               input: LazyList[Double],
                               expectedOutput: LazyList[Double])
                              (network: Network)
  : (LazyList[Double], Network) =
    backPropagated(
      learningRate,
      expectedOutput,
      fedForward(input)(network)
    )

  // public
  def errors(learningRate: Double,
             input: LazyList[Double],
             expectedOutput: LazyList[Double])
            (network: Network)
  : LazyList[Double] =
    errorsWithFitNet(
      learningRate,
      input,
      expectedOutput
    )(network)._1

  // public
  def fit(learningRate: Double,
          input: LazyList[Double],
          expectedOutput: LazyList[Double])
         (network: Network)
  : Network = Utilities.lazyNetworkRealization(
    errorsWithFitNet(
      learningRate,
      input,
      expectedOutput
    )(network)._2
  )

  type NetworkSerialized = List[LayerSerialized]

  def serialized(network: Network): NetworkSerialized =
    network
      .map(Layer.serialized)
      .toList

  // public
  def toJson(network: Network): String = serialized(network)
    .asJson
    .noSpaces

  def deserialized(networkSerialized: NetworkSerialized): Network =
    networkSerialized
      .map(Layer.deserialized)
      .to(LazyList)

  // public
  def ofJson(s: String): Either[Error, Network] =
    decode[NetworkSerialized](s)
      .map(deserialized)
      .map(Utilities.lazyNetworkRealization)


}
