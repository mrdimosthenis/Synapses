using static Synapses.Model.NetElems.Activation;

namespace Synapses.net
{
    public class ActivationFunction
    {
        Function contents;

        public ActivationFunction(Function _contents)
        {
            contents = _contents;
        }

        public static ActivationFunction Sigmoid()
        {
            Function _contents = ActivationFunctionModule.sigmoid;
            return new ActivationFunction(_contents);
        }

        public static ActivationFunction Identity()
        {
            Function _contents = ActivationFunctionModule.identity;
            return new ActivationFunction(_contents);
        }

        public static ActivationFunction Tanh()
        {
            Function _contents = ActivationFunctionModule.tanh;
            return new ActivationFunction(_contents);
        }

        public static ActivationFunction LeakyReLu()
        {
            Function _contents = ActivationFunctionModule.leakyReLU;
            return new ActivationFunction(_contents);
        }
    }
}