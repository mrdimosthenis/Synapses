defmodule Synapses do
  @moduledoc false

  defmodule ActivationFunction do
    def sigmoid, do: :gleam_synapses@activation_function.sigmoid()
    def identity, do: :gleam_synapses@activation_function.identity()
    def tanh, do: :gleam_synapses@activation_function.tanh()
    def leakyReLU, do: :gleam_synapses@activation_function.leaky_re_lu()
  end

  defmodule NeuralNetwork do

    def init(layers), do: :gleam_synapses@neural_network.init(layers)

    def initWithSeed(seed, layers), do: :gleam_synapses@neural_network.init_with_seed(seed, layers)

    def customizedInit(layers, activationF, weightInitF),
        do: :gleam_synapses@neural_network.customized_init(layers, activationF, weightInitF)

    def prediction(network, inputValues), do: :gleam_synapses@neural_network.prediction(network, inputValues)

    def errors(network, learningRate, inputValues, expectedOutput),
        do: :gleam_synapses@neural_network.errors(network, learningRate, inputValues, expectedOutput)

    def fit(network, learningRate, inputValues, expectedOutput),
        do: :gleam_synapses@neural_network.fit(network, learningRate, inputValues, expectedOutput)

    def toJson(network), do: :gleam_synapses@neural_network.to_json(network)

    def ofJson(json), do: :gleam_synapses@neural_network.of_json(json)

    def toSvg(network), do: :gleam_synapses@neural_network.to_svg(network)

  end

  defmodule DataPreprocessor do

    def init(keysWithDiscreteFlags, datapoints) do
      data = LazyConverter.stream_to_iterator(datapoints)
      :gleam_synapses@data_preprocessor.init(keysWithDiscreteFlags, data)
    end

    def encodedDatapoint(dataPreprocessor, datapoint),
        do: :gleam_synapses@data_preprocessor.encoded_datapoint(dataPreprocessor, datapoint)

    def decodedDatapoint(dataPreprocessor, encodedValues),
        do: :gleam_synapses@data_preprocessor.decoded_datapoint(dataPreprocessor, encodedValues)

    def toJson(dataPreprocessor), do: :gleam_synapses@data_preprocessor.to_json(dataPreprocessor)

    def ofJson(json), do: :gleam_synapses@data_preprocessor.of_json(json)

  end

  defmodule Statistics do

    def rootMeanSquareError(expectedValuesWithOutputValues) do
      expectedValuesWithOutputValues
      |> LazyConverter.stream_to_iterator
      |> :gleam_synapses@statistics.root_mean_square_error
    end

  end

end
