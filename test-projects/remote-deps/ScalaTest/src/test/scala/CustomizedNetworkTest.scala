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

  test("neural network svg") {
    assertEquals(
      NeuralNetwork.toSvg(neuralNetwork),
      """<svg width="360.000000" height="288.000000"><line stroke-opacity="0.938516" x1="60.000000" y1="80.000000" x2="120.000000" y2="80.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.027692" x1="60.000000" y1="112.000000" x2="120.000000" y2="80.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.110568" x1="60.000000" y1="144.000000" x2="120.000000" y2="80.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.205421" x1="60.000000" y1="176.000000" x2="120.000000" y2="80.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.102648" x1="60.000000" y1="208.000000" x2="120.000000" y2="80.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.966844" x1="60.000000" y1="80.000000" x2="120.000000" y2="112.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.265051" x1="60.000000" y1="112.000000" x2="120.000000" y2="112.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.711015" x1="60.000000" y1="144.000000" x2="120.000000" y2="112.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.292908" x1="60.000000" y1="176.000000" x2="120.000000" y2="112.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.741302" x1="60.000000" y1="208.000000" x2="120.000000" y2="112.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.953324" x1="60.000000" y1="80.000000" x2="120.000000" y2="144.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.932742" x1="60.000000" y1="112.000000" x2="120.000000" y2="144.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.930104" x1="60.000000" y1="144.000000" x2="120.000000" y2="144.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.711028" x1="60.000000" y1="176.000000" x2="120.000000" y2="144.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.821560" x1="60.000000" y1="208.000000" x2="120.000000" y2="144.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.484836" x1="60.000000" y1="80.000000" x2="120.000000" y2="176.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.578842" x1="60.000000" y1="112.000000" x2="120.000000" y2="176.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="1.000000" x1="60.000000" y1="144.000000" x2="120.000000" y2="176.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.629164" x1="60.000000" y1="176.000000" x2="120.000000" y2="176.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.681386" x1="60.000000" y1="208.000000" x2="120.000000" y2="176.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.184057" x1="60.000000" y1="80.000000" x2="120.000000" y2="208.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.911136" x1="60.000000" y1="112.000000" x2="120.000000" y2="208.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.300488" x1="60.000000" y1="144.000000" x2="120.000000" y2="208.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.142090" x1="60.000000" y1="176.000000" x2="120.000000" y2="208.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.993449" x1="60.000000" y1="208.000000" x2="120.000000" y2="208.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.452065" x1="60.000000" y1="80.000000" x2="120.000000" y2="240.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.274129" x1="60.000000" y1="112.000000" x2="120.000000" y2="240.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.334278" x1="60.000000" y1="144.000000" x2="120.000000" y2="240.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.176498" x1="60.000000" y1="176.000000" x2="120.000000" y2="240.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.093953" x1="60.000000" y1="208.000000" x2="120.000000" y2="240.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.909633" x1="120.000000" y1="48.000000" x2="180.000000" y2="48.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.322063" x1="120.000000" y1="80.000000" x2="180.000000" y2="48.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.219007" x1="120.000000" y1="112.000000" x2="180.000000" y2="48.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.262916" x1="120.000000" y1="144.000000" x2="180.000000" y2="48.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.828497" x1="120.000000" y1="176.000000" x2="180.000000" y2="48.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.394037" x1="120.000000" y1="208.000000" x2="180.000000" y2="48.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.462419" x1="120.000000" y1="240.000000" x2="180.000000" y2="48.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.607700" x1="120.000000" y1="48.000000" x2="180.000000" y2="80.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.805177" x1="120.000000" y1="80.000000" x2="180.000000" y2="80.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.922216" x1="120.000000" y1="112.000000" x2="180.000000" y2="80.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.092942" x1="120.000000" y1="144.000000" x2="180.000000" y2="80.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.665851" x1="120.000000" y1="176.000000" x2="180.000000" y2="80.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.231763" x1="120.000000" y1="208.000000" x2="180.000000" y2="80.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.138721" x1="120.000000" y1="240.000000" x2="180.000000" y2="80.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.632487" x1="120.000000" y1="48.000000" x2="180.000000" y2="112.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.341695" x1="120.000000" y1="80.000000" x2="180.000000" y2="112.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.767552" x1="120.000000" y1="112.000000" x2="180.000000" y2="112.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.209438" x1="120.000000" y1="144.000000" x2="180.000000" y2="112.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.966661" x1="120.000000" y1="176.000000" x2="180.000000" y2="112.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.074767" x1="120.000000" y1="208.000000" x2="180.000000" y2="112.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.811854" x1="120.000000" y1="240.000000" x2="180.000000" y2="112.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.362952" x1="120.000000" y1="48.000000" x2="180.000000" y2="144.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.064132" x1="120.000000" y1="80.000000" x2="180.000000" y2="144.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.570497" x1="120.000000" y1="112.000000" x2="180.000000" y2="144.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.365421" x1="120.000000" y1="144.000000" x2="180.000000" y2="144.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.529764" x1="120.000000" y1="176.000000" x2="180.000000" y2="144.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.681780" x1="120.000000" y1="208.000000" x2="180.000000" y2="144.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.916898" x1="120.000000" y1="240.000000" x2="180.000000" y2="144.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.270453" x1="120.000000" y1="48.000000" x2="180.000000" y2="176.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.382668" x1="120.000000" y1="80.000000" x2="180.000000" y2="176.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.297823" x1="120.000000" y1="112.000000" x2="180.000000" y2="176.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.906368" x1="120.000000" y1="144.000000" x2="180.000000" y2="176.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.541106" x1="120.000000" y1="176.000000" x2="180.000000" y2="176.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.629869" x1="120.000000" y1="208.000000" x2="180.000000" y2="176.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.207905" x1="120.000000" y1="240.000000" x2="180.000000" y2="176.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.191398" x1="120.000000" y1="48.000000" x2="180.000000" y2="208.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.515031" x1="120.000000" y1="80.000000" x2="180.000000" y2="208.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.046095" x1="120.000000" y1="112.000000" x2="180.000000" y2="208.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.121156" x1="120.000000" y1="144.000000" x2="180.000000" y2="208.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.923970" x1="120.000000" y1="176.000000" x2="180.000000" y2="208.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.501145" x1="120.000000" y1="208.000000" x2="180.000000" y2="208.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.983703" x1="120.000000" y1="240.000000" x2="180.000000" y2="208.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.238146" x1="120.000000" y1="48.000000" x2="180.000000" y2="240.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.034115" x1="120.000000" y1="80.000000" x2="180.000000" y2="240.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.571843" x1="120.000000" y1="112.000000" x2="180.000000" y2="240.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.799483" x1="120.000000" y1="144.000000" x2="180.000000" y2="240.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.419293" x1="120.000000" y1="176.000000" x2="180.000000" y2="240.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.582311" x1="120.000000" y1="208.000000" x2="180.000000" y2="240.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.571349" x1="120.000000" y1="240.000000" x2="180.000000" y2="240.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.143033" x1="120.000000" y1="48.000000" x2="180.000000" y2="272.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.913328" x1="120.000000" y1="80.000000" x2="180.000000" y2="272.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.850667" x1="120.000000" y1="112.000000" x2="180.000000" y2="272.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.318298" x1="120.000000" y1="144.000000" x2="180.000000" y2="272.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.655806" x1="120.000000" y1="176.000000" x2="180.000000" y2="272.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.834275" x1="120.000000" y1="208.000000" x2="180.000000" y2="272.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.516727" x1="120.000000" y1="240.000000" x2="180.000000" y2="272.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.152488" x1="180.000000" y1="16.000000" x2="240.000000" y2="96.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.962430" x1="180.000000" y1="48.000000" x2="240.000000" y2="96.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.091876" x1="180.000000" y1="80.000000" x2="240.000000" y2="96.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.077185" x1="180.000000" y1="112.000000" x2="240.000000" y2="96.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.071625" x1="180.000000" y1="144.000000" x2="240.000000" y2="96.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.116215" x1="180.000000" y1="176.000000" x2="240.000000" y2="96.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.760146" x1="180.000000" y1="208.000000" x2="240.000000" y2="96.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.696187" x1="180.000000" y1="240.000000" x2="240.000000" y2="96.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.816093" x1="180.000000" y1="272.000000" x2="240.000000" y2="96.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.097391" x1="180.000000" y1="16.000000" x2="240.000000" y2="128.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.802760" x1="180.000000" y1="48.000000" x2="240.000000" y2="128.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.815050" x1="180.000000" y1="80.000000" x2="240.000000" y2="128.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.783089" x1="180.000000" y1="112.000000" x2="240.000000" y2="128.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.257242" x1="180.000000" y1="144.000000" x2="240.000000" y2="128.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.014699" x1="180.000000" y1="176.000000" x2="240.000000" y2="128.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.116768" x1="180.000000" y1="208.000000" x2="240.000000" y2="128.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.214491" x1="180.000000" y1="240.000000" x2="240.000000" y2="128.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.902145" x1="180.000000" y1="272.000000" x2="240.000000" y2="128.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.914669" x1="180.000000" y1="16.000000" x2="240.000000" y2="160.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.281125" x1="180.000000" y1="48.000000" x2="240.000000" y2="160.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.364696" x1="180.000000" y1="80.000000" x2="240.000000" y2="160.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.281825" x1="180.000000" y1="112.000000" x2="240.000000" y2="160.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.462189" x1="180.000000" y1="144.000000" x2="240.000000" y2="160.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.172942" x1="180.000000" y1="176.000000" x2="240.000000" y2="160.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.068808" x1="180.000000" y1="208.000000" x2="240.000000" y2="160.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.811603" x1="180.000000" y1="240.000000" x2="240.000000" y2="160.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.105605" x1="180.000000" y1="272.000000" x2="240.000000" y2="160.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.159080" x1="180.000000" y1="16.000000" x2="240.000000" y2="192.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.744450" x1="180.000000" y1="48.000000" x2="240.000000" y2="192.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.308143" x1="180.000000" y1="80.000000" x2="240.000000" y2="192.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.916542" x1="180.000000" y1="112.000000" x2="240.000000" y2="192.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.558690" x1="180.000000" y1="144.000000" x2="240.000000" y2="192.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.502641" x1="180.000000" y1="176.000000" x2="240.000000" y2="192.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.904427" x1="180.000000" y1="208.000000" x2="240.000000" y2="192.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.259743" x1="180.000000" y1="240.000000" x2="240.000000" y2="192.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.050975" x1="180.000000" y1="272.000000" x2="240.000000" y2="192.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.210288" x1="180.000000" y1="16.000000" x2="240.000000" y2="224.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.813228" x1="180.000000" y1="48.000000" x2="240.000000" y2="224.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.438253" x1="180.000000" y1="80.000000" x2="240.000000" y2="224.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.635313" x1="180.000000" y1="112.000000" x2="240.000000" y2="224.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.397507" x1="180.000000" y1="144.000000" x2="240.000000" y2="224.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.493873" x1="180.000000" y1="176.000000" x2="240.000000" y2="224.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.042389" x1="180.000000" y1="208.000000" x2="240.000000" y2="224.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.557540" x1="180.000000" y1="240.000000" x2="240.000000" y2="224.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.813072" x1="180.000000" y1="272.000000" x2="240.000000" y2="224.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.428277" x1="240.000000" y1="64.000000" x2="300.000000" y2="112.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.752486" x1="240.000000" y1="96.000000" x2="300.000000" y2="112.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.181682" x1="240.000000" y1="128.000000" x2="300.000000" y2="112.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.995563" x1="240.000000" y1="160.000000" x2="300.000000" y2="112.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.399872" x1="240.000000" y1="192.000000" x2="300.000000" y2="112.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.450047" x1="240.000000" y1="224.000000" x2="300.000000" y2="112.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.652949" x1="240.000000" y1="64.000000" x2="300.000000" y2="144.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.127892" x1="240.000000" y1="96.000000" x2="300.000000" y2="144.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.523559" x1="240.000000" y1="128.000000" x2="300.000000" y2="144.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.873707" x1="240.000000" y1="160.000000" x2="300.000000" y2="144.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.378345" x1="240.000000" y1="192.000000" x2="300.000000" y2="144.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.067699" x1="240.000000" y1="224.000000" x2="300.000000" y2="144.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.882395" x1="240.000000" y1="64.000000" x2="300.000000" y2="176.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.352720" x1="240.000000" y1="96.000000" x2="300.000000" y2="176.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.443600" x1="240.000000" y1="128.000000" x2="300.000000" y2="176.000000" stroke="palevioletred" stroke-width="1.333333"></line><line stroke-opacity="0.005604" x1="240.000000" y1="160.000000" x2="300.000000" y2="176.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.106402" x1="240.000000" y1="192.000000" x2="300.000000" y2="176.000000" stroke="lawngreen" stroke-width="1.333333"></line><line stroke-opacity="0.962558" x1="240.000000" y1="224.000000" x2="300.000000" y2="176.000000" stroke="palevioletred" stroke-width="1.333333"></line><circle cx="60.000000" cy="80.000000" r="12.000000" stroke="black" stroke-width="2.666667" fill="white"></circle><circle cx="60.000000" cy="112.000000" r="12.000000" stroke="brown" stroke-width="2.666667" fill="white"></circle><circle cx="60.000000" cy="144.000000" r="12.000000" stroke="brown" stroke-width="2.666667" fill="white"></circle><circle cx="60.000000" cy="176.000000" r="12.000000" stroke="brown" stroke-width="2.666667" fill="white"></circle><circle cx="60.000000" cy="208.000000" r="12.000000" stroke="brown" stroke-width="2.666667" fill="white"></circle><circle cx="120.000000" cy="48.000000" r="12.000000" stroke="black" stroke-width="2.666667" fill="white"></circle><circle cx="120.000000" cy="80.000000" r="12.000000" stroke="blue" stroke-width="2.666667" fill="white"></circle><circle cx="120.000000" cy="112.000000" r="12.000000" stroke="blue" stroke-width="2.666667" fill="white"></circle><circle cx="120.000000" cy="144.000000" r="12.000000" stroke="blue" stroke-width="2.666667" fill="white"></circle><circle cx="120.000000" cy="176.000000" r="12.000000" stroke="blue" stroke-width="2.666667" fill="white"></circle><circle cx="120.000000" cy="208.000000" r="12.000000" stroke="blue" stroke-width="2.666667" fill="white"></circle><circle cx="120.000000" cy="240.000000" r="12.000000" stroke="blue" stroke-width="2.666667" fill="white"></circle><circle cx="180.000000" cy="16.000000" r="12.000000" stroke="black" stroke-width="2.666667" fill="white"></circle><circle cx="180.000000" cy="48.000000" r="12.000000" stroke="orange" stroke-width="2.666667" fill="white"></circle><circle cx="180.000000" cy="80.000000" r="12.000000" stroke="orange" stroke-width="2.666667" fill="white"></circle><circle cx="180.000000" cy="112.000000" r="12.000000" stroke="orange" stroke-width="2.666667" fill="white"></circle><circle cx="180.000000" cy="144.000000" r="12.000000" stroke="orange" stroke-width="2.666667" fill="white"></circle><circle cx="180.000000" cy="176.000000" r="12.000000" stroke="orange" stroke-width="2.666667" fill="white"></circle><circle cx="180.000000" cy="208.000000" r="12.000000" stroke="orange" stroke-width="2.666667" fill="white"></circle><circle cx="180.000000" cy="240.000000" r="12.000000" stroke="orange" stroke-width="2.666667" fill="white"></circle><circle cx="180.000000" cy="272.000000" r="12.000000" stroke="orange" stroke-width="2.666667" fill="white"></circle><circle cx="240.000000" cy="64.000000" r="12.000000" stroke="black" stroke-width="2.666667" fill="white"></circle><circle cx="240.000000" cy="96.000000" r="12.000000" stroke="pink" stroke-width="2.666667" fill="white"></circle><circle cx="240.000000" cy="128.000000" r="12.000000" stroke="pink" stroke-width="2.666667" fill="white"></circle><circle cx="240.000000" cy="160.000000" r="12.000000" stroke="pink" stroke-width="2.666667" fill="white"></circle><circle cx="240.000000" cy="192.000000" r="12.000000" stroke="pink" stroke-width="2.666667" fill="white"></circle><circle cx="240.000000" cy="224.000000" r="12.000000" stroke="pink" stroke-width="2.666667" fill="white"></circle><circle cx="300.000000" cy="112.000000" r="12.000000" stroke="yellow" stroke-width="2.666667" fill="white"></circle><circle cx="300.000000" cy="144.000000" r="12.000000" stroke="yellow" stroke-width="2.666667" fill="white"></circle><circle cx="300.000000" cy="176.000000" r="12.000000" stroke="yellow" stroke-width="2.666667" fill="white"></circle></svg>"""
    )
  }

  jsonSource.close()

}
