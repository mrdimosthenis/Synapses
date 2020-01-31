import unittest

from synapses_py import NeuralNetwork

layers = [4, 6, 5, 3]
neuralNetwork = NeuralNetwork.initWithSeed(1000, layers)
neuralNetworkJson = '[[{"activationF":"sigmoid","weights":[-0.10082091137345839,-0.5846791301128591,-0.14168165545564104,0.4865532503669947,0.10329319724172592]},{"activationF":"sigmoid","weights":[0.03763480548353337,0.9363522930973693,0.0031553351781352657,0.05621420948305533,-0.6321955585269339]},{"activationF":"sigmoid","weights":[0.7820091717424988,-0.2684716020149025,-0.7654212318427913,0.6460297521992198,0.8460054211181609]},{"activationF":"sigmoid","weights":[0.059986988525195795,0.34237143996340746,0.45223601789288215,-0.15294773150435503,-0.6653183299701018]},{"activationF":"sigmoid","weights":[-0.5653408331835108,-0.527956157954883,-0.2517674468401354,-0.6905014951102966,-0.7216643330725843]},{"activationF":"sigmoid","weights":[-0.4559640640355529,0.3490520610183352,0.34456200436906004,0.6448668284269514,0.020790883460047427]}],[{"activationF":"sigmoid","weights":[-0.32897055825878585,0.9743375787928275,-0.3554046937034967,0.138949441938371,0.05742199662414116,0.15325323779951083,-0.2624913094289971]},{"activationF":"sigmoid","weights":[-0.12568674996737395,0.6318296787114552,0.3909659053848076,-0.45182206707540185,-0.3800747802736606,-0.5847546334429978,-0.8326808915645769]},{"activationF":"sigmoid","weights":[0.4400029587149674,-0.05690616430600648,0.2736852814421955,0.008659955068559988,0.6024530256780711,0.7440432318465313,0.7551472302711546]},{"activationF":"sigmoid","weights":[-0.9426384618510155,0.6427840331914574,-0.14521450313863338,-0.515103731162522,-0.9952912557711615,-0.4522024407769685,-0.6922616578194603]},{"activationF":"sigmoid","weights":[0.4477894618107183,0.15780904259880257,-0.11774364571871843,0.9666621941542197,-0.08183902270159815,-0.5257097588083122,0.25789109142160394]}],[{"activationF":"sigmoid","weights":[0.43860341104029477,0.42105355463983396,0.5324931975008524,-0.9815895789679836,0.5658778892354976,-0.09489892252577992]},{"activationF":"sigmoid","weights":[0.00906198660573132,-0.47961086480733983,0.9339936392833597,-0.9688390740762609,0.14727673708301103,-0.7327636625755827]},{"activationF":"sigmoid","weights":[0.6334469933648916,0.36726151823919073,-0.6602419533134223,-0.9148637720021597,-0.07628301762370615,0.6719585259438856]}]]'

inputValues = [1.0, 0.5625, 0.511111, 0.47619]
prediction = NeuralNetwork.prediction(neuralNetwork, inputValues)

learningRate = 0.99
expectedOutput = [0.4, 0.05, 0.2]

fitNetwork = NeuralNetwork.fit(neuralNetwork, learningRate, inputValues, expectedOutput)


class TestSeedNetwork(unittest.TestCase):

    def test_neuralNetworkToJson(self):
        self.assertEqual(
            neuralNetworkJson,
            NeuralNetwork.toJson(neuralNetwork)
        )

    def test_neuralNetworkPrediction(self):
        self.assertEqual(
            [0.6907499310213903, 0.15020551903310392, 0.4506408982648829],
            prediction
        )

    def test_neuralNetworkNormalErrors(self):
        self.assertEqual(
            [0.1899789732239445,
             0.10686317243846878,
             0.09710034298346348,
             0.09046608725951012],
            NeuralNetwork.errors(neuralNetwork, learningRate, inputValues, expectedOutput)
        )

    def test_neuralNetworkZeroErrors(self):
        self.assertEqual(
            [0.0, 0.0, 0.0, 0.0],
            NeuralNetwork.errors(neuralNetwork, learningRate, inputValues, prediction)
        )

    def test_fitNeuralNetworkPrediction(self):
        self.assertEqual(
            [0.28038066848381893, 0.19747195907627696, 0.1530174534759267],
            NeuralNetwork.prediction(fitNetwork, inputValues)
        )


if __name__ == '__main__':
    unittest.main()
