using Synapses;
using static Synapses.Model.NetElems.Activation;

namespace SynapsesCSharp
{
    public class ActivationFunction
    {
        public Function contents;

        public ActivationFunction(Function _contents)
        {
            contents = _contents;
        }

        public static ActivationFunction sigmoid =
            new ActivationFunction(ActivationFunctionModule.sigmoid);

        public static ActivationFunction identity =
            new ActivationFunction(ActivationFunctionModule.identity);

        public static ActivationFunction tanh =
            new ActivationFunction(ActivationFunctionModule.tanh);

        public static ActivationFunction leakyReLU =
            new ActivationFunction(ActivationFunctionModule.leakyReLU);
    }
}