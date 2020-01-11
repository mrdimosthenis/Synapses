import org.junit.Test;

import scala.collection.immutable.LazyList;

import synapses.Library.*;
import synapses.model.netElems.Activation;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.Random;

import static org.junit.Assert.assertArrayEquals;
import static org.junit.Assert.assertEquals;

public class CustomizedNetworkTest {

    int[] layers = {4, 6, 5, 3};

    public CustomizedNetworkTest() throws IOException {
    }

    Activation activationF(int layerIndex) {
        switch (layerIndex) {
            case 0:
                return ActivationFunction$.MODULE$.sigmoid();
            case 1:
                return ActivationFunction$.MODULE$.identity();
            case 2:
                return ActivationFunction$.MODULE$.leakyReLU();
            default:
                return ActivationFunction$.MODULE$.tanh();
        }
    }

    ;

    Random rnd = new Random();

    double weightInitF(int _layerIndex) {
        return 1.0 - 2.0 * rnd.nextDouble();
    }

    LazyList justCreatedNeuralNetwork = NeuralNetwork$.MODULE$.customizedInit(
            layers,
            this::activationF,
            this::weightInitF
    );

    String justCreatedNeuralNetworkJson = NeuralNetwork$.MODULE$.toJson(justCreatedNeuralNetwork);

    @Test
    public void neuralNetworkOfToJson() {
        LazyList netJson = NeuralNetwork$.MODULE$.ofJson(justCreatedNeuralNetworkJson);
        assertEquals(
                justCreatedNeuralNetworkJson,
                NeuralNetwork$.MODULE$.toJson(netJson)
        );
    }

    String neuralNetworkJson = Files.readString(Paths.get("../resources/network.json"));

    LazyList neuralNetwork = NeuralNetwork$.MODULE$.ofJson(neuralNetworkJson);

    double[] inputValues = {1.0, 0.5625, 0.511111, 0.47619};

    double[] expectedOutput = {0.4, 0.05, 0.2};

    double[] prediction = NeuralNetwork$.MODULE$.prediction(neuralNetwork, inputValues);

    @Test
    public void neuralNetworkPrediction() {
        double[] expected = {-0.013959435951885571, -0.16770539176070537, 0.6127887629040738};
        assertArrayEquals(
                expected,
                prediction,
                0.0
        );
    }

    double learningRate = 0.01;

    @Test
    public void neuralNetworkNormalErrors() {
        double[] expected = {-0.18229373795952453, -0.10254022760223255, -0.09317233470223055, -0.086806455078946};
        assertArrayEquals(
                expected,
                NeuralNetwork$.MODULE$.errors(neuralNetwork, learningRate, inputValues, expectedOutput),
                0.0
        );
    }

    @Test
    public void neuralNetworkZeroErrors() {
        double[] expected = {0, 0, 0, 0};
        assertArrayEquals(
                expected,
                NeuralNetwork$.MODULE$.errors(neuralNetwork, learningRate, inputValues, prediction),
                0.0
        );
    }

    LazyList fitNeuralNetwork = NeuralNetwork$.MODULE$.fit(
            neuralNetwork,
            learningRate,
            inputValues,
            expectedOutput
    );

    @Test
    public void fitNeuralNetworkPrediction() {
        double[] expected = {-0.006109464554743645, -0.1770428172237149, 0.6087944183600162};
        assertArrayEquals(
                expected,
                NeuralNetwork$.MODULE$.prediction(fitNeuralNetwork, inputValues),
                0.0
        );
    }

}
