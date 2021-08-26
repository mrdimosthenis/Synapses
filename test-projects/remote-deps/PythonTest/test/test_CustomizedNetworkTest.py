import unittest
from random import seed, random

from synapses_py import ActivationFunction, NeuralNetwork

layers = [4, 6, 5, 3]


def activationF(layerIndex: int) -> ActivationFunction:
    if layerIndex == 0:
        return ActivationFunction.sigmoid
    elif layerIndex == 1:
        return ActivationFunction.identity
    elif layerIndex == 2:
        return ActivationFunction.leakyReLU
    else:
        return ActivationFunction.tanh


seed(1000)


def weightInitF(_layerIndex: int) -> float:
    return 1.0 - 2.0 * random()


justCreatedNeuralNetwork = NeuralNetwork.customizedInit(layers, activationF, weightInitF)
justCreatedNeuralNetworkJson = NeuralNetwork.toJson(justCreatedNeuralNetwork)

neuralNetworkJsonFile = open("../../resources/network.json", "r")
neuralNetworkJson = neuralNetworkJsonFile.read()
neuralNetworkSvgFile = open("../../resources/drawing.svg", "r")
neuralNetworkSvg = neuralNetworkSvgFile.read()
neuralNetwork = NeuralNetwork.ofJson(neuralNetworkJson)
inputValues = [1.0, 0.5625, 0.511111, 0.47619]
expectedOutput = [0.4, 0.05, 0.2]
prediction = NeuralNetwork.prediction(neuralNetwork, inputValues)

learningRate = 0.01

fitNeuralNetwork = NeuralNetwork.fit(neuralNetwork, learningRate, inputValues, expectedOutput)


class TestCustomizedNetworkTest(unittest.TestCase):

    def test_neuralNetworkOfToJson(self):
        netJson = NeuralNetwork.ofJson(justCreatedNeuralNetworkJson)
        self.assertEqual(
            justCreatedNeuralNetworkJson,
            NeuralNetwork.toJson(netJson)
        )

    def test_neuralNetworkPrediction(self):
        self.assertEqual(
            [-0.013959435951885571, -0.16770539176070537, 0.6127887629040738],
            prediction
        )

    def test_neuralNetworkNormalErrors(self):
        self.assertEqual(
            [-0.18229373795952453, -0.10254022760223255, -0.09317233470223055, -0.086806455078946],
            NeuralNetwork.errors(neuralNetwork, learningRate, inputValues, expectedOutput)
        )

    def test_neuralNetworkZeroErrors(self):
        self.assertEqual(
            [0.0, 0.0, 0.0, 0.0],
            NeuralNetwork.errors(neuralNetwork, learningRate, inputValues, prediction)
        )

    def test_fitNeuralNetworkPrediction(self):
        self.assertEqual(
            [-0.0061094645547436445, -0.1770428172237149, 0.6087944183600162],
            NeuralNetwork.prediction(fitNeuralNetwork, inputValues)
        )

    def test_neuralNetworkOfToSvg(self):
        self.assertEqual(
            neuralNetworkSvg,
            NeuralNetwork.toSvg(neuralNetwork)
        )


neuralNetworkJsonFile.close()
neuralNetworkSvgFile.close()

if __name__ == '__main__':
    unittest.main()
