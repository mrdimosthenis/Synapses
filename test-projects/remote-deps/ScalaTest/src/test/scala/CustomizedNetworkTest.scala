import minitest._
import synapses.Library._

import scala.io.Source
import scala.util.Random

object CustomizedNetworkTest extends SimpleTestSuite {

  val inputValues = List(1.0, 0.5625, 0.511111, 0.47619)

  val expectedOutput = List(0.4, 0.05, 0.2)

  val layers = List(4, 6, 8, 5, 3)

  def activationF(layerIndex: Int): ActivationFunction =
    layerIndex match {
      case 0 => ActivationFunction.sigmoid
      case 1 => ActivationFunction.identity
      case 2 => ActivationFunction.leakyReLU
      case 3 => ActivationFunction.tanh
    }

  val rnd = new Random(1000)

  def weightInitF(_layerIndex: Int): Double =
    1.0 - 2.0 * rnd.nextDouble()

  val justCreatedNeuralNetwork: NeuralNetwork =
    NeuralNetwork.customizedInit(layers, activationF, weightInitF)

  val justCreatedNeuralNetworkJson: String =
    NeuralNetwork.toJson(justCreatedNeuralNetwork)


  test("neural network of to json") {
    val netJson = NeuralNetwork.ofJson(justCreatedNeuralNetworkJson)
    assertEquals(
      justCreatedNeuralNetworkJson,
      NeuralNetwork.toJson(netJson)
    )
  }

  val jsonSource: Source = Source.fromFile("../../resources/network.json")

  val neuralNetworkJson: String = jsonSource.getLines.mkString

  val neuralNetwork: NeuralNetwork =
    NeuralNetwork.ofJson(neuralNetworkJson)

  val prediction: List[Double] =
    NeuralNetwork.prediction(neuralNetwork, inputValues)

  test("neural network prediction") {
    assertEquals(
      prediction,
      List(-0.013959435951885571, -0.16770539176070537, 0.6127887629040738)
    )
  }

  val learningRate = 0.01

  test("neural network normal errors") {
    assertEquals(
      NeuralNetwork.errors(neuralNetwork, learningRate, inputValues, expectedOutput),
      List(-0.18229373795952453, -0.10254022760223255, -0.09317233470223055, -0.086806455078946)
    )
  }

  test("neural network zero errors") {
    assertEquals(
      NeuralNetwork.errors(neuralNetwork, learningRate, inputValues, prediction),
      List(0, 0, 0, 0)
    )
  }

  val fitNeuralNetwork: NeuralNetwork =
    NeuralNetwork.fit(neuralNetwork, learningRate, inputValues, expectedOutput)

  test("fit neural network prediction") {
    assertEquals(
      NeuralNetwork.prediction(fitNeuralNetwork, inputValues),
      List(-0.006109464554743645, -0.1770428172237149, 0.6087944183600162)
    )
  }

  jsonSource.close()

}
