package synapses

import scala.jdk.StreamConverters._
import scala.jdk.FunctionConverters._
import scala.jdk.CollectionConverters._

object JavaInterface {

  object NeuralNetwork {

    def init(layers: Array[Int]): Library.NeuralNetwork =
      Library
        .NeuralNetwork
        .init(layers.toList)

    def initWithSeed(seed: Int, layers: Array[Int]): Library.NeuralNetwork =
      Library
        .NeuralNetwork
        .initWithSeed(seed, layers.toList)

    def customizedInit(layers: Array[Int],
                       activationF: java.util.function.IntFunction[Library.ActivationFunction],
                       weightInitF: java.util.function.IntFunction[Double])
    : Library.NeuralNetwork =
      Library
        .NeuralNetwork
        .customizedInit(
          layers.toList,
          activationF.asScala,
          weightInitF.asScala
        )

    def prediction(network: Library.NeuralNetwork,
                   inputValues: Array[Double])
    : Array[Double] =
      Library
        .NeuralNetwork
        .prediction(network, inputValues.toList)
        .toArray

    def errors(network: Library.NeuralNetwork,
               learningRate: Double,
               inputValues: Array[Double],
               expectedOutput: Array[Double])
    : Array[Double] =
      Library
        .NeuralNetwork
        .errors(
          network,
          learningRate,
          inputValues.toList,
          expectedOutput.toList
        )
        .toArray

    def fit(network: Library.NeuralNetwork,
            learningRate: Double,
            inputValues: Array[Double],
            expectedOutput: Array[Double])
    : Library.NeuralNetwork =
      Library
        .NeuralNetwork
        .fit(
          network,
          learningRate,
          inputValues.toList,
          expectedOutput.toList
        )

    def toJson(network: Library.NeuralNetwork): String =
      Library
        .NeuralNetwork
        .toJson(network)

    def ofJson(json: String): Library.NeuralNetwork =
      Library
        .NeuralNetwork
        .ofJson(json)

  }

  object DataPreprocessor {

    def init(keysWithDiscreteFlags: Array[Array[java.lang.Object]],
             datapoints: java.util.stream.Stream[java.util.Map[String, String]])
    : Library.DataPreprocessor = {
      val keysWithFlags = keysWithDiscreteFlags
        .map { obj =>
          (obj(0).asInstanceOf[String], obj(1).asInstanceOf[Boolean])
        }
        .toList
      val scalaDatapoints = datapoints
        .toScala(LazyList)
        .map(_.asScala.toMap)
      Library
        .DataPreprocessor
        .init(keysWithFlags, scalaDatapoints)
    }

    def encodedDatapoint(dataPreprocessor: Library.DataPreprocessor,
                         datapoint: java.util.Map[String, String])
    : Array[Double] =
      Library
        .DataPreprocessor
        .encodedDatapoint(dataPreprocessor, datapoint.asScala.toMap)
        .toArray

    def decodedDatapoint(dataPreprocessor: Library.DataPreprocessor,
                         encodedValues: Array[Double])
    : java.util.Map[String, String] =
      Library
        .DataPreprocessor
        .decodedDatapoint(dataPreprocessor, encodedValues.toList)
        .asJava

    def toJson(dataPreprocessor: Library.DataPreprocessor): String =
      Library
        .DataPreprocessor
        .toJson(dataPreprocessor)

    def ofJson(json: String): Library.DataPreprocessor =
      Library
        .DataPreprocessor
        .ofJson(json)

  }

  object Statistics {

    def rootMeanSquareError(expectedValuesWithOutputValues: java.util.stream.Stream[Array[Array[Double]]])
    : Double = {
      val yHatsWithYs = expectedValuesWithOutputValues
        .toScala(LazyList)
        .map { arr =>
          (arr(0).toList, arr(1).toList)
        }
      Library
        .Statistics
        .rootMeanSquareError(yHatsWithYs)
    }

  }

}
