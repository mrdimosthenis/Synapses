using System;
using System.Linq;
using FSharpx.Collections;
using Microsoft.FSharp.Collections;
using Microsoft.FSharp.Core;
using Synapses;
using Synapses.Model;
using Synapses.Model.NetElems;

namespace SynapsesCSharp
{
    public class NeuralNetwork
    {
        readonly LazyList<LazyList<Neuron.Neuron>> contents;

        public NeuralNetwork(LazyList<LazyList<Neuron.Neuron>> _contents)
        {
            contents = _contents;
        }

        public static NeuralNetwork init(int[] layers)
        {
            FSharpList<int> ls = ListModule.OfSeq(layers);
            LazyList<LazyList<Neuron.Neuron>> _contents =
                NeuralNetworkModule.init(ls);
            return new NeuralNetwork(_contents);
        }

        public static NeuralNetwork initWithSeed(int seed, int[] layers)
        {
            FSharpList<int> ls = ListModule.OfSeq(layers);
            LazyList<LazyList<Neuron.Neuron>> _contents =
                NeuralNetworkModule.initWithSeed(seed, ls);
            return new NeuralNetwork(_contents);
        }

        public static NeuralNetwork customizedInit(
            int[] layers,
            Func<int, ActivationFunction> activationF,
            Func<int, Double> weightInitF)
        {
            FSharpList<int> ls = ListModule.OfSeq(layers);
            FSharpFunc<int, Activation.Function> actF =
                (Converter<int, Activation.Function>) (x => activationF(x).contents);
            FSharpFunc<int, Double> weightF =
                (Converter<int, Double>) (x => weightInitF(x));
            LazyList<LazyList<Neuron.Neuron>> _contents =
                NeuralNetworkModule.customizedInit(ls, actF, weightF);
            return new NeuralNetwork(_contents);
        }
        
        public static double[] prediction(NeuralNetwork network, double[] inputValues){
            FSharpList<double> ivs = ListModule.OfSeq(inputValues);
            return NeuralNetworkModule
                .prediction(network.contents, ivs)
                .ToArray();
        }
        
        public static double[] errors(
            NeuralNetwork network,
            double learningRate,
            double[] inputValues,
            double[] expectedOutput){
            FSharpList<double> ivs = ListModule.OfSeq(inputValues);
            FSharpList<double> eos = ListModule.OfSeq(expectedOutput);
            return NeuralNetworkModule
                .errors(network.contents, learningRate, ivs, eos)
                .ToArray();
        }
        
        public static NeuralNetwork fit(
            NeuralNetwork network,
            double learningRate,
            double[] inputValues,
            double[] expectedOutput){
            FSharpList<double> ivs = ListModule.OfSeq(inputValues);
            FSharpList<double> eos = ListModule.OfSeq(expectedOutput);
            LazyList<LazyList<Neuron.Neuron>> _contents =
                NeuralNetworkModule.fit(network.contents, learningRate, ivs, eos);
            return new NeuralNetwork(_contents);
        }
        
        public static string toJson(NeuralNetwork network){
            return NeuralNetworkModule.toJson(network.contents);
        }
        
        public static NeuralNetwork ofJson(string json){
            LazyList<LazyList<Neuron.Neuron>> _contents = NeuralNetworkModule.ofJson(json);
            return new NeuralNetwork(_contents);
        }
        
        public static string toSvg(NeuralNetwork network){
            return NeuralNetworkModule.toSvg(network.contents);
        }
        
    }
}