package synapses

import synapses.model.Mathematics
import synapses.model.encoding.{Preprocessor, Serialization}
import synapses.model.netElems.Network.Network
import synapses.model.netElems._

import scala.jdk.StreamConverters._
import scala.jdk.FunctionConverters._
import scala.jdk.CollectionConverters._
import scala.util.Random

object Library {

  type ActivationFunction = Activation

  object ActivationFunction {
    val sigmoid: ActivationFunction = Activation.sigmoid
    val identity: ActivationFunction = Activation.identity
    val tanh: ActivationFunction = Activation.tanh
    val leakyReLU: ActivationFunction = Activation.leakyReLU
  }

  type NeuralNetwork = Network

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

    def init(layers: List[Int]): NeuralNetwork =
      seedInit(None, layers)

    def init(layers: Array[Int]): NeuralNetwork =
      seedInit(None, layers.toList)

    def initWithSeed(seed: Long, layers: List[Int]): NeuralNetwork =
      seedInit(Some(seed), layers)

    def initWithSeed(seed: Int, layers: Array[Int]): NeuralNetwork =
      seedInit(Some(seed), layers.toList)

    def customizedInit(layers: List[Int],
                       activationF: Int => Activation,
                       weightInitF: Int => Double)
    : NeuralNetwork = {
      val layerSizes = layers.to(LazyList)
      Network.init(layerSizes, activationF)(weightInitF)
    }

    def customizedInit(layers: Array[Int],
                       activationF: java.util.function.IntFunction[Activation],
                       weightInitF: java.util.function.IntFunction[Double])
    : NeuralNetwork = {
      val layerSizes = layers.to(LazyList)
      Network.init(layerSizes, activationF.asScala)(weightInitF.asScala)
    }

    def prediction(network: NeuralNetwork,
                   inputValues: List[Double])
    : List[Double] = {
      val input = inputValues.to(LazyList)
      Network
        .output(input)(network)
        .toList
    }

    def prediction(network: NeuralNetwork,
                   inputValues: Array[Double])
    : Array[Double] = {
      val input = inputValues.to(LazyList)
      Network
        .output(input)(network)
        .toArray
    }

    def errors(network: NeuralNetwork,
               learningRate: Double,
               inputValues: List[Double],
               expectedOutput: List[Double])
    : List[Double] = {
      val input = inputValues.to(LazyList)
      val expected = expectedOutput.to(LazyList)
      Network
        .errors(learningRate, input, expected)(network)
        .toList
    }

    def errors(network: NeuralNetwork,
               learningRate: Double,
               inputValues: Array[Double],
               expectedOutput: Array[Double])
    : Array[Double] = {
      val input = inputValues.to(LazyList)
      val expected = expectedOutput.to(LazyList)
      Network
        .errors(learningRate, input, expected)(network)
        .toArray
    }

    def fit(network: NeuralNetwork,
            learningRate: Double,
            inputValues: List[Double],
            expectedOutput: List[Double])
    : NeuralNetwork = {
      val input = inputValues.to(LazyList)
      val expected = expectedOutput.to(LazyList)
      Network.fit(learningRate, input, expected)(network)
    }

    def fit(network: NeuralNetwork,
            learningRate: Double,
            inputValues: Array[Double],
            expectedOutput: Array[Double])
    : NeuralNetwork = {
      val input = inputValues.to(LazyList)
      val expected = expectedOutput.to(LazyList)
      Network.fit(learningRate, input, expected)(network)
    }

    def toJson(network: NeuralNetwork): String =
      Network.toJson(network)

    def ofJson(json: String): NeuralNetwork =
      Network
        .ofJson(json)
        .toOption
        .get

  }

  type DataPreprocessor = Serialization.Preprocessor

  object DataPreprocessor {

    def init(keysWithDiscreteFlags: List[(String, Boolean)],
             datapoints: LazyList[Map[String, String]])
    : DataPreprocessor = {
      val keysWithFlags = keysWithDiscreteFlags.to(LazyList)
      Preprocessor.init(keysWithFlags, datapoints)
    }

    def init(keysWithDiscreteFlags: Array[Array[java.lang.Object]],
             datapoints: java.util.stream.Stream[java.util.Map[String, String]])
    : DataPreprocessor = {
      val keysWithFlags = keysWithDiscreteFlags
        .to(LazyList)
        .map { obj =>
          (obj(0).asInstanceOf[String], obj(1).asInstanceOf[Boolean])
        }
      val scalaDatapoints = datapoints
        .toScala(LazyList)
        .map(_.asScala.toMap)
      Preprocessor.init(keysWithFlags, scalaDatapoints)
    }

    def encodedDatapoint(dataPreprocessor: DataPreprocessor,
                         datapoint: Map[String, String])
    : List[Double] = Preprocessor
      .encode(datapoint)(dataPreprocessor)
      .toList

    def encodedDatapoint(dataPreprocessor: DataPreprocessor,
                         datapoint: java.util.Map[String, String])
    : Array[Double] = Preprocessor
      .encode(datapoint.asScala.toMap)(dataPreprocessor)
      .toArray

    def decodedDatapoint(dataPreprocessor: DataPreprocessor,
                         encodedValues: List[Double])
    : Map[String, String] = {
      val values = encodedValues.to(LazyList)
      Preprocessor.decode(values)(dataPreprocessor)
    }

    def decodedDatapoint(dataPreprocessor: DataPreprocessor,
                         encodedValues: Array[Double])
    : java.util.Map[String, String] = {
      val values = encodedValues.to(LazyList)
      Preprocessor
        .decode(values)(dataPreprocessor)
        .asJava
    }

    def toJson(dataPreprocessor: DataPreprocessor): String =
      Preprocessor.toJson(dataPreprocessor)

    def ofJson(json: String): DataPreprocessor =
      Preprocessor.ofJson(json)

  }

  object Statistics {

    def rootMeanSquareError(expectedValuesWithOutputValues: LazyList[(List[Double], List[Double])])
    : Double = {
      val yHatsWithYs = expectedValuesWithOutputValues.map { case (yHat, y) =>
        (yHat.to(LazyList), y.to(LazyList))
      }
      Mathematics.rootMeanSquareError(yHatsWithYs)
    }

    def rootMeanSquareError(expectedValuesWithOutputValues: java.util.stream.Stream[Array[Array[Double]]])
    : Double = {
      val yHatsWithYs = expectedValuesWithOutputValues
        .toScala(LazyList)
        .map { arr =>
          (arr(0).to(LazyList), arr(1).to(LazyList))
        }
      Mathematics.rootMeanSquareError(yHatsWithYs)
    }

  }

}
