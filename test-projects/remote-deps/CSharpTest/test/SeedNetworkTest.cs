using SynapsesCSharp;
using Xunit;

namespace CSharpTest.test
{
    public class SeedNetworkTest
    {
        static int[] layers = {4, 6, 5, 3};

        static NeuralNetwork neuralNetwork = NeuralNetwork.initWithSeed(1000, layers);

        private static string neuralNetworkJson =
            "[[{\"activationF\":\"sigmoid\",\"weights\":[0.6968850817982504,0.5281141006984348,-0.5120263339541975,0.9978164462362493,-0.3835458389406772]},{\"activationF\":\"sigmoid\",\"weights\":[0.0866170456104991,-0.9254493089045628,0.7468386235399351,-0.3885003344102298,0.9503958001501839]},{\"activationF\":\"sigmoid\",\"weights\":[0.9551315475046316,0.6364802399820091,0.8325641480426137,-0.4495622755259099,-0.9080339520741412]},{\"activationF\":\"sigmoid\",\"weights\":[-0.10252656838974294,0.5571194065535066,-0.9893482709253898,-0.4951349941525305,-0.038948392979311075]},{\"activationF\":\"sigmoid\",\"weights\":[-0.37489226151951227,0.6305013315894181,-0.9938250621751998,0.2429909507012884,-0.4113109174237173]},{\"activationF\":\"sigmoid\",\"weights\":[-0.41858921359227463,-0.3979648954225541,0.408840041332338,-0.5196228695659073,0.11859868425810649]}],[{\"activationF\":\"sigmoid\",\"weights\":[0.2702675793647149,-0.7202986002528569,0.15772338544843878,0.8816654290452904,-0.6426420871366942,0.798039569425415,0.24059907777262812]},{\"activationF\":\"sigmoid\",\"weights\":[-0.7596449161691801,-0.19010160080627614,-0.5210675222431624,0.5978504156683806,-0.07962023144570196,0.4686872751771879,0.12509557005255278]},{\"activationF\":\"sigmoid\",\"weights\":[0.7138489055046109,0.13059630483882334,-0.1169894929588724,0.7537927300454084,-0.6810685534407703,0.2186283875343522,-0.17887985947489726]},{\"activationF\":\"sigmoid\",\"weights\":[0.16163801828475577,0.2452822352039079,0.570239870608896,0.1508470676610465,-0.9336162497911678,0.5219391628736347,0.2449827153445141]},{\"activationF\":\"sigmoid\",\"weights\":[0.4091273636599665,-0.9649566253484025,-0.5154180589669468,-0.3342893502369009,0.26646149310584255,0.4929009813316637,-0.31987177921453114]}],[{\"activationF\":\"sigmoid\",\"weights\":[0.6754301477574884,-0.5212431454664297,0.9508987189973233,-0.8069201883892156,-0.7060735214995562,0.6568743538376289]},{\"activationF\":\"sigmoid\",\"weights\":[0.3167643227226866,0.20075332988088634,-0.9740674719093683,0.36320119135230833,0.7047279699261897,-0.8381859435877697]},{\"activationF\":\"sigmoid\",\"weights\":[-0.11892063222775273,0.5291420451966775,0.4580927777374595,0.6984002793665977,-0.15175762546796245,0.08990859477310842]}]]";

        [Fact]
        public void neuralNetworkToJson()
        {
            Assert.Equal(neuralNetworkJson, NeuralNetwork.toJson(neuralNetwork));
        }

        static double[] inputValues = {1.0, 0.5625, 0.511111, 0.47619};

        static double[] prediction = NeuralNetwork.prediction(neuralNetwork, inputValues);

        [Fact]
        public void neuralNetworkPrediction()
        {
            double[] expected = {0.4642980260692742, 0.6269665696700697, 0.7027549284345667};
            Assert.Equal(expected, prediction);
        }

        static double learningRate = 0.99;

        static double[] expectedOutput = {0.6, 0.7, 0.5};

        [Fact]
        public void neuralNetworkNormalErrors()
        {
            double[] expected =
                {-0.003276283081564106, -0.0018429092333798096, -0.0016745443221013116, -0.0015601332406100115};
            Assert.Equal(
                expected,
                NeuralNetwork.errors(
                    neuralNetwork,
                    learningRate,
                    inputValues,
                    expectedOutput
                )
            );
        }

        [Fact]
        public void neuralNetworkZeroErrors()
        {
            double[] expected = {0.0, 0.0, 0.0, 0.0};
            Assert.Equal(expected, NeuralNetwork.errors(neuralNetwork, learningRate, inputValues, prediction));
        }

        static NeuralNetwork fitNetwork = NeuralNetwork.fit(neuralNetwork, learningRate, inputValues, expectedOutput);

        [Fact]
        public void fitNeuralNetworkPrediction()
        {
            double[] expected = {0.486524197649693, 0.6376023985716912, 0.6788699738913898};
            Assert.Equal(expected, NeuralNetwork.prediction(fitNetwork, inputValues));
        }
    }
}