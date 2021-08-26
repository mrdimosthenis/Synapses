package synapses

import synapses.model.{Mathematics, Draw}
import synapses.model.encoding.{Preprocessor, Serialization}
import synapses.model.netElems.Network.Network
import synapses.model.netElems._

import scala.scalajs.js
import scala.scalajs.js.JSConverters._
import scala.scalajs.js.annotation.{JSExport, JSExportTopLevel}
import scala.util.Random

object Library {

  type ActivationFunction = Activation

  @JSExportTopLevel("ActivationFunction")
  object ActivationFunction {
    @JSExport
    val sigmoid: ActivationFunction = Activation.sigmoid
    @JSExport
    val identity: ActivationFunction = Activation.identity
    @JSExport
    val tanh: ActivationFunction = Activation.tanh
    @JSExport
    val leakyReLU: ActivationFunction = Activation.leakyReLU
  }

  type NeuralNetwork = Network

  @JSExportTopLevel("NeuralNetwork")
  object NeuralNetwork {

    private def seedInit(maybeSeed: Option[Long],
                         layers: List[Int])
    : NeuralNetwork = {
      val layerSizes = layers.to(LazyList)
      val activationF = (_: Int) => Activation.sigmoid
      val rnd = maybeSeed match {
        case Some(i) => new Random(i)
        case None => new Random()
      }
      val weightInitF =
        (_: Int) => 1.0 - 2.0 * rnd.nextDouble()
      Network.init(layerSizes, activationF)(weightInitF)
    }

    private def throwIfInputNotMatch(network: NeuralNetwork,
                                     inputValues: js.Array[Double])
    : Unit = {
      val numOfInputVals = inputValues.length
      val inputLayerSize = network.head.head.weights.length - 1
      if (numOfInputVals != inputLayerSize)
        throw new Exception(
          s"the number of input values ($numOfInputVals) " +
            s"does not match the size of the input layer ($inputLayerSize)"
        )
    }

    private def throwIfExpectedNotMatch(network: NeuralNetwork,
                                        expectedOutput: js.Array[Double])
    : Unit = {
      val numOfExpectedVals = expectedOutput.length
      val outputLayerSize = network.last.length
      if (numOfExpectedVals != outputLayerSize)
        throw new Exception(
          s"the number of expected values ($numOfExpectedVals)" +
            s" does not match the size of the output layer ($outputLayerSize)"
        )
    }

    @JSExport
    def init(layers: js.Array[Int]): NeuralNetwork =
      seedInit(None, layers.toList)

    @JSExport
    def initWithSeed(seed: Int, layers: js.Array[Int]): NeuralNetwork =
      seedInit(Some(seed), layers.toList)

    @JSExport
    def customizedInit(layers: js.Array[Int],
                       activationF: js.Function1[Int, Activation],
                       weightInitF: js.Function1[Int, Double])
    : NeuralNetwork = {
      val layerSizes = layers.to(LazyList)
      Network.init(layerSizes, activationF)(weightInitF)
    }

    @JSExport
    def prediction(network: NeuralNetwork,
                   inputValues: js.Array[Double])
    : js.Array[Double] = {
      throwIfInputNotMatch(network, inputValues)
      val input = inputValues.to(LazyList)
      Network
        .output(input)(network)
        .toJSArray
    }

    @JSExport
    def errors(network: NeuralNetwork,
               learningRate: Double,
               inputValues: js.Array[Double],
               expectedOutput: js.Array[Double])
    : js.Array[Double] = {
      throwIfInputNotMatch(network, inputValues)
      throwIfExpectedNotMatch(network, expectedOutput)
      val input = inputValues.to(LazyList)
      val expected = expectedOutput.to(LazyList)
      Network
        .errors(learningRate, input, expected)(network)
        .toJSArray
    }

    @JSExport
    def fit(network: NeuralNetwork,
            learningRate: Double,
            inputValues: js.Array[Double],
            expectedOutput: js.Array[Double])
    : NeuralNetwork = {
      throwIfInputNotMatch(network, inputValues)
      throwIfExpectedNotMatch(network, expectedOutput)
      val input = inputValues.to(LazyList)
      val expected = expectedOutput.to(LazyList)
      Network.fit(learningRate, input, expected)(network)
    }

    @JSExport
    def toJson(network: NeuralNetwork): String =
      Network.toJson(network)

    @JSExport
    def ofJson(json: String): NeuralNetwork =
      Network
        .ofJson(json)
        .toOption
        .get

    @JSExport
    def toSvg(network: NeuralNetwork): String =
      Draw.networkSVG(network)

  }

  type DataPreprocessor = Serialization.Preprocessor

  @JSExportTopLevel("DataPreprocessor")
  object DataPreprocessor {

    @JSExport
    def init[A](keysWithDiscreteFlags: js.Array[js.Array[A]],
                datapoints: js.Iterable[js.Dictionary[String]])
    : DataPreprocessor = {
      val keysWithFlags = keysWithDiscreteFlags
        .map { arr =>
          (arr(0).asInstanceOf[String], arr(1).asInstanceOf[Boolean])
        }
        .to(LazyList)
      Preprocessor.init(keysWithFlags, datapoints.map(_.toMap).to(LazyList))
    }

    @JSExport
    def encodedDatapoint(dataPreprocessor: DataPreprocessor,
                         datapoint: js.Dictionary[String])
    : js.Array[Double] = {
      Preprocessor
        .encode(datapoint.toMap)(dataPreprocessor)
        .toList
        .toJSArray
    }

    @JSExport
    def decodedDatapoint(dataPreprocessor: DataPreprocessor,
                         encodedValues: js.Array[Double])
    : js.Dictionary[String] = {
      val values = encodedValues.to(LazyList)
      Preprocessor
        .decode(values)(dataPreprocessor)
        .toJSDictionary
    }

    @JSExport
    def toJson(dataPreprocessor: DataPreprocessor): String =
      Preprocessor.toJson(dataPreprocessor)

    @JSExport
    def ofJson(json: String): DataPreprocessor =
      Preprocessor.ofJson(json)

  }

  @JSExportTopLevel("Statistics")
  object Statistics {

    @JSExport
    def rootMeanSquareError(expectedValuesWithOutputValues: js.Iterable[js.Array[js.Array[Double]]])
    : Double = {
      val yHatsWithYs = expectedValuesWithOutputValues
        .to(LazyList)
        .map{ arr =>
          (arr(0).to(LazyList), arr(1).to(LazyList))
        }
      Mathematics.rootMeanSquareError(yHatsWithYs)
    }

  }

}
