import unittest

from synapses_py import NeuralNetwork

layers = [4, 6, 5, 3]
neuralNetwork = NeuralNetwork.initWithSeed(1000, layers)
neuralNetworkJson = '[[{"activationF":"sigmoid","weights":[-0.5547132854011279,-0.33965111911849943,0.801720792150366,0.2940589776197091,0.06418451419831617]},{"activationF":"sigmoid","weights":[-0.06936748294175499,-0.9566181218247947,0.739369299682682,-0.3424869364605325,0.27154116810524886]},{"activationF":"sigmoid","weights":[0.022328585676028467,0.5939755785318925,-0.3323967511427184,0.5446739375861358,0.08387188340647378]},{"activationF":"sigmoid","weights":[0.9185552048900851,-0.9485795907556571,0.0250478514621868,0.07677227272528064,-0.4282943116164004]},{"activationF":"sigmoid","weights":[0.168543701000055,-0.7760233760030579,0.9534131025524566,-0.667012535569093,0.0630105180049838]},{"activationF":"sigmoid","weights":[-0.622959625288559,-0.8911829772317446,-0.9661767563897976,0.6476358488429388,-0.397311519152616]}],[{"activationF":"sigmoid","weights":[0.7822885773752342,0.6794725315875962,0.8142794519508216,0.3718758402143192,0.9669226313226655,-0.7080982514727243,0.4179679227086064]},{"activationF":"sigmoid","weights":[-0.5600951726700656,-0.09619303225113929,0.6113386466004775,0.4159234556389406,0.36109444520113265,-0.3171964470758153,0.5369579291755415]},{"activationF":"sigmoid","weights":[-0.23886067399070732,-0.7906772196044207,-0.7388684171393662,0.41238612866309543,0.08359038283890108,0.030040404257761777,0.43922346128255496]},{"activationF":"sigmoid","weights":[0.3420861072987995,-0.9684848480530084,0.9761117241582513,0.7141984634134295,-0.30395443308934245,0.8500136401061351,0.41584259543778246]},{"activationF":"sigmoid","weights":[-0.586885944383541,-0.8231862017419473,0.25921644082091255,0.5894355776266731,-0.760162653569356,-0.2651329003121663,-0.007028652117115897]}],[{"activationF":"sigmoid","weights":[0.33831288065782217,0.30519963298510877,0.41517682733510375,-0.5307948692866638,0.04311340024632537,0.5969253197068358]},{"activationF":"sigmoid","weights":[-0.7431254595374333,-0.5103570978899235,-0.7351169023697717,-0.8646473858494532,0.5165734693187258,-0.7849009677745837]},{"activationF":"sigmoid","weights":[-0.531913368841257,0.17063461540363445,0.35263478746551846,-0.12261047780393852,-0.18167195776647538,0.6688344463837941]}]]'

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
            [0.03989541577938217,
             0.022441171375902468,
             0.020390985854415802,
             0.018997798039984],
            NeuralNetwork.errors(neuralNetwork, learningRate, inputValues, expectedOutput)
        )

    def test_neuralNetworkZeroErrors(self):
        self.assertEqual(
            [0.0, 0.0, 0.0, 0.0],
            NeuralNetwork.errors(neuralNetwork, learningRate, inputValues, prediction)
        )

    def test_fitNeuralNetworkPrediction(self):
        self.assertEqual(
            [0.6526357666102395, 0.14734346885843128, 0.40855329014300334],
            NeuralNetwork.prediction(fitNetwork, inputValues)
        )


if __name__ == '__main__':
    unittest.main()
