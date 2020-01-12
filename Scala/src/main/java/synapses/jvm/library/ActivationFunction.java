package synapses.jvm.library;

import synapses.Library;
import synapses.model.netElems.Activation;

public class ActivationFunction {

    Activation contents;

    public ActivationFunction(Activation _contents) {
        contents = _contents;
    }

    public static ActivationFunction sigmoid() {
        Activation _contents = Library.ActivationFunction$.MODULE$.sigmoid();
        return new ActivationFunction(_contents);
    }

    public static ActivationFunction identity() {
        Activation _contents = Library.ActivationFunction$.MODULE$.identity();
        return new ActivationFunction(_contents);
    }

    public static ActivationFunction tanh() {
        Activation _contents = Library.ActivationFunction$.MODULE$.tanh();
        return new ActivationFunction(_contents);
    }

    public static ActivationFunction leakyReLU() {
        Activation _contents = Library.ActivationFunction$.MODULE$.leakyReLU();
        return new ActivationFunction(_contents);
    }

}
