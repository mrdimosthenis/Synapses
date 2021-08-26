using System;
using System.IO;
using SynapsesCSharp;
using Xunit;

namespace CSharpTest.test
{
    public class CustomizedNetworkTest
    {
        static int[] layers = {4, 6, 8, 5, 3};

        static ActivationFunction activationF(int layerIndex)
        {
            switch (layerIndex)
            {
                case 0:
                    return ActivationFunction.sigmoid;
                case 1:
                    return ActivationFunction.identity;
                case 2:
                    return ActivationFunction.leakyReLU;
                default:
                    return ActivationFunction.tanh;
            }
        }

        static Random rnd = new Random();

        static double weightInitF(int _layerIndex)
        {
            return 1.0 - 2.0 * rnd.NextDouble();
        }

        static NeuralNetwork justCreatedNeuralNetwork = NeuralNetwork.customizedInit(
            layers,
            activationF,
            weightInitF
        );

        static string justCreatedNeuralNetworkJson = NeuralNetwork.toJson(justCreatedNeuralNetwork);

        [Fact]
        public void neuralNetworkOfToJson()
        {
            NeuralNetwork netJson = NeuralNetwork.ofJson(justCreatedNeuralNetworkJson);
            Assert.Equal(
                justCreatedNeuralNetworkJson,
                NeuralNetwork.toJson(netJson)
            );
        }
        
        static string neuralNetworkJson = File.ReadAllText("../../../../../resources/network.json");
        
        static string neuralNetworkSvg = File.ReadAllText("../../../../../resources/drawing.svg");

        static NeuralNetwork neuralNetwork = NeuralNetwork.ofJson(neuralNetworkJson);

        static double[] inputValues = {1.0, 0.5625, 0.511111, 0.47619};

        static double[] expectedOutput = {0.4, 0.05, 0.2};

        static double[] prediction = NeuralNetwork.prediction(neuralNetwork, inputValues);
        
        [Fact]
        public void neuralNetworkPrediction()
        {
            double[] expected = {-0.013959435951885571, -0.16770539176070537, 0.6127887629040738};
            Assert.Equal(expected, prediction);
        }
        
        static double learningRate = 0.01;
        
        [Fact]
        public void neuralNetworkNormalErrors()
        {
            double[] expected = {-0.18229373795952453, -0.10254022760223255, -0.09317233470223055, -0.086806455078946};
            Assert.Equal(
                expected, 
                NeuralNetwork.errors(neuralNetwork, learningRate, inputValues, expectedOutput)
                );
        }
        
        [Fact]
        public void neuralNetworkZeroErrors()
        {
            double[] expected = {0.0, 0.0, 0.0, 0.0};
            Assert.Equal(
                expected, 
                NeuralNetwork.errors(neuralNetwork, learningRate, inputValues, prediction)
            );
        }
        
        static NeuralNetwork fitNeuralNetwork = NeuralNetwork.fit(
            neuralNetwork,
            learningRate,
            inputValues,
            expectedOutput
        );
        
        [Fact]
        public void fitNeuralNetworkPrediction()
        {
            double[] expected = {-0.0061094645547436445, -0.1770428172237149, 0.6087944183600162};
            Assert.Equal(
                expected, 
                NeuralNetwork.prediction(fitNeuralNetwork, inputValues)
            );
        }
        
        [Fact]
        public void neuralNetworkOfToSvg()
        {
            Assert.Equal(
                neuralNetworkSvg,
                NeuralNetwork.toSvg(neuralNetwork)
            );
        }
        
    }
}