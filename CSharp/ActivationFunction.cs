using Synapses;
using static Synapses.Model.NetElems.Activation;

namespace SynapsesCs
{
    public class ActivationFunction
    {
        Function contents;

        public ActivationFunction(Function _contents)
        {
            contents = _contents;
        }

        public ActivationFunction sigmoid =
            new ActivationFunction(ActivationFunctionModule.sigmoid);

        public ActivationFunction identity =
            new ActivationFunction(ActivationFunctionModule.identity);

        public ActivationFunction tanh =
            new ActivationFunction(ActivationFunctionModule.tanh);

        public ActivationFunction leakyReLU =
            new ActivationFunction(ActivationFunctionModule.leakyReLU);
    }
}