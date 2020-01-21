package synapses.jvm.library;

import synapses.Library;
import synapses.model.netElems.Activation;

public class ActivationFunction {

    Activation contents;

    public ActivationFunction(Activation _contents) {
        contents = _contents;
    }

    public static ActivationFunction sigmoid =
            new ActivationFunction(
                    Library.ActivationFunction$
                            .MODULE$
                            .sigmoid()
            );

    public static ActivationFunction identity =
            new ActivationFunction(
                    Library.ActivationFunction$
                            .MODULE$
                            .identity()
            );

    public static ActivationFunction tanh =
            new ActivationFunction(
                    Library.ActivationFunction$
                            .MODULE$
                            .tanh()
            );

    public static ActivationFunction leakyReLU =
            new ActivationFunction(
                    Library.ActivationFunction$
                            .MODULE$
                            .leakyReLU()
            );

}
