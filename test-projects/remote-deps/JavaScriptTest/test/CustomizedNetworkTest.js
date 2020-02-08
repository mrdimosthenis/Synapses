const fs = require('fs');
var assert = require('assert');
require('synapses');

var seed = 1;

function random() {
    var x = Math.sin(seed++) * 10000;
    return x - Math.floor(x);
}

describe('customized network tests', function () {

    let layers = [4, 6, 8, 5, 3];

    function activationF(layerIndex) {
        switch (layerIndex) {
            case 0:
                return ActivationFunction.sigmoid;
            case 1:
                return ActivationFunction.identity;
            case 2:
                return ActivationFunction.leakyReLU;
            case 3:
                return ActivationFunction.tanh;
        }
    }

    function weightInitF(_layerIndex) {
        return 1.0 - 2.0 * random();
    }

    let justCreatedNeuralNetwork =
        NeuralNetwork.customizedInit(layers, activationF, weightInitF);

    let justCreatedNeuralNetworkJson =
        NeuralNetwork.toJson(justCreatedNeuralNetwork);

    it('just created neural network of/to json', function () {
        let netJson = NeuralNetwork.ofJson(justCreatedNeuralNetworkJson);
        assert.equal(
            NeuralNetwork.toJson(netJson),
            justCreatedNeuralNetworkJson
        );
    });

    let neuralNetworkData = fs.readFileSync('../../resources/network.json');

    let neuralNetwork = NeuralNetwork.ofJson(neuralNetworkData);

    let inputValues = [1.0, 0.5625, 0.511111, 0.47619];

    let expectedOutput = [0.4, 0.05, 0.2];

    let prediction =
        NeuralNetwork.prediction(neuralNetwork, inputValues);

    it('neural network prediction', function () {
        assert.deepEqual(
            prediction,
            [-0.013959435951885571, -0.16770539176070537, 0.6127887629040738]
        );
    });

    let learningRate = 0.01;

    it('neural network normal errors', function () {
        assert.deepEqual(
            NeuralNetwork.errors(
                neuralNetwork,
                learningRate,
                inputValues,
                expectedOutput
            ),
            [-0.18229373795952453, -0.10254022760223255, -0.09317233470223055, -0.086806455078946]
        );
    });

    it('neural network zero errors', function () {
        assert.deepEqual(
            NeuralNetwork.errors(
                neuralNetwork,
                learningRate,
                inputValues,
                prediction
            ),
            [0.0, 0.0, 0.0, 0.0]
        );
    });

    let fitNeuralNetwork =
        NeuralNetwork.fit(
            neuralNetwork,
            learningRate,
            inputValues,
            expectedOutput
        );

    it('fit neural network prediction', function () {
        assert.deepEqual(
            NeuralNetwork.prediction(fitNeuralNetwork, inputValues),
            [-0.006109464554743645, -0.1770428172237149, 0.6087944183600162]
        );
    });

});
