defmodule Synapses do
  @moduledoc false

  defmodule NeuralNetwork do
    def init(layers) do
      :gleam_synapses@neural_network.init(layers)
      layers
    end
  end

end
