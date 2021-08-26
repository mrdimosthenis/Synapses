package synapses.jvm.library;

import scala.collection.immutable.LazyList;
import synapses.JavaInterface.NeuralNetwork$;

import java.util.function.IntFunction;

public class NeuralNetwork {

    LazyList contents;

    public NeuralNetwork(LazyList _contents) {
        contents = _contents;
    }

    public static NeuralNetwork init(int[] layers) {
        LazyList _contents = NeuralNetwork$.MODULE$.init(layers);
        return new NeuralNetwork(_contents);
    }

    public static NeuralNetwork initWithSeed(int seed, int[] layers) {
        LazyList _contents = NeuralNetwork$.MODULE$.initWithSeed(seed, layers);
        return new NeuralNetwork(_contents);
    }

    public static NeuralNetwork customizedInit(
            int[] layers,
            IntFunction<ActivationFunction> activationF,
            IntFunction<Double> weightInitF) {
        LazyList _contents = NeuralNetwork$.MODULE$.customizedInit(
                layers,
                (i) -> activationF.apply(i).contents,
                weightInitF::apply
        );
        return new NeuralNetwork(_contents);
    }

    public static double[] prediction(NeuralNetwork network, double[] inputValues){
        return NeuralNetwork$.MODULE$.prediction(network.contents, inputValues);
    }

    public static double[] errors(
            NeuralNetwork network,
            double learningRate,
            double[] inputValues,
            double[] expectedOutput){
        return NeuralNetwork$.MODULE$.errors(network.contents, learningRate, inputValues, expectedOutput);
    }

    public static NeuralNetwork fit(
            NeuralNetwork network,
            double learningRate,
            double[] inputValues,
            double[] expectedOutput){
        LazyList _contents = NeuralNetwork$.MODULE$.fit(network.contents, learningRate, inputValues, expectedOutput);
        return new NeuralNetwork(_contents);
    }

    public static String toJson(NeuralNetwork network){
        return NeuralNetwork$.MODULE$.toJson(network.contents);
    }

    public static NeuralNetwork ofJson(String json){
        LazyList _contents = NeuralNetwork$.MODULE$.ofJson(json);
        return new NeuralNetwork(_contents);
    }

    public static String toSvg(NeuralNetwork network){
        return NeuralNetwork$.MODULE$.toSvg(network.contents);
    }

}
