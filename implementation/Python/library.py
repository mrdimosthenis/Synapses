from random import seed, random
from typing import List, Optional, Callable, Iterable, Dict
from functional import seq

from model import mathematics
from model.encoding import preprocessor
from model.encoding.serialization import Preprocessor
from model.net_elems import activation, network
from model.net_elems.activation import Activation
from model.net_elems.network import Network

ActivationFunction = Activation


class ActivationFunction:
    sigmoid: ActivationFunction = activation.sigmoid
    identity: ActivationFunction = activation.identity
    tanh: ActivationFunction = activation.tanh
    leakyReLU: ActivationFunction = activation.leakyReLU


NeuralNetwork = Network


def seed_init_network(maybe_seed: Optional[int],
                      layers: List[int]
                      ) -> NeuralNetwork:
    layer_sizes = seq(layers)
    if maybe_seed is not None:
        seed(maybe_seed)
    return network.init(
        layer_sizes,
        lambda _: activation.sigmoid,
        lambda _: 1.0 - 2.0 * random()
    )


class NeuralNetwork:

    @staticmethod
    def init(layers: List[int]) -> NeuralNetwork:
        return seed_init_network(None, layers)

    @staticmethod
    def initWithSeed(seed_val: int,
                     layers: List[int]
                     ) -> NeuralNetwork:
        return seed_init_network(seed_val, layers)

    @staticmethod
    def customizedInit(layers: List[int],
                       activation_f: Callable[[int], Activation],
                       weight_init_f: Callable[[int], float]
                       ) -> NeuralNetwork:
        layer_sizes = seq(layers)
        return network.init(
            layer_sizes,
            activation_f,
            weight_init_f
        )

    @staticmethod
    def prediction(network_val: NeuralNetwork,
                   input_values: List[float]
                   ) -> List[float]:
        input_val = seq(input_values)
        return network \
            .output(input_val, network_val) \
            .to_list()

    @staticmethod
    def errors(network_val: NeuralNetwork,
               learning_rate: float,
               input_values: List[float],
               expected_output: List[float]
               ) -> List[float]:
        input_val = seq(input_values)
        expected_val = seq(expected_output)
        return network \
            .errors(learning_rate,
                    input_val,
                    expected_val,
                    network_val) \
            .to_list()

    @staticmethod
    def fit(network_val: NeuralNetwork,
            learning_rate: float,
            input_values: List[float],
            expected_output: List[float]
            ) -> NeuralNetwork:
        input_val = seq(input_values)
        expected_val = seq(expected_output)
        return network.fit(
            learning_rate,
            input_val,
            expected_val,
            network_val
        )

    @staticmethod
    def toJson(network_val: NeuralNetwork) -> str:
        return network.to_json(network_val)

    @staticmethod
    def ofJson(json_val: str) -> NeuralNetwork:
        return network.of_json(json_val)


DataPreprocessor = Preprocessor


class DataPreprocessor:

    @staticmethod
    def init(keys_with_discrete_flags: List[(str, bool)],
             datapoints: Iterable[Dict[str, str]]
             ) -> DataPreprocessor:
        keys_with_flags = seq(keys_with_discrete_flags)
        dataset = seq(datapoints)
        return preprocessor.init(keys_with_flags, dataset)

    @staticmethod
    def encodedDatapoint(preprocessor_val: DataPreprocessor,
                         datapoint: Dict[str, str]
                         ) -> List[float]:
        return preprocessor \
            .encode(datapoint, preprocessor_val) \
            .to_list()

    @staticmethod
    def decodedDatapoint(preprocessor_val: DataPreprocessor,
                         encoded_values: List[float]
                         ) -> Dict[str, str]:
        return preprocessor \
            .decode(encoded_values, preprocessor_val)

    @staticmethod
    def toJson(preprocessor_val: DataPreprocessor) -> str:
        return preprocessor.to_json(preprocessor_val)

    @staticmethod
    def ofJson(json_val: str) -> DataPreprocessor:
        return preprocessor.of_json(json_val)


class Statistics:

    @staticmethod
    def rootMeanSquareError(
            expected_values_with_output_values:
            Iterable[(List[float], List[float])]
    ) -> float:
        y_hats_with_ys = seq(expected_values_with_output_values) \
            .map(lambda t: (seq(t[0]), seq(t[1])))
        return mathematics.root_mean_square_error(y_hats_with_ys)
