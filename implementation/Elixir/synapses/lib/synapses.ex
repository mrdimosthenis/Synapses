defmodule Synapses do
  @moduledoc false

  defmodule NeuralNetwork do
    def init(layers) do
      :gleam_synapses@neural_network.init(layers)
      layers
    end
  end

  def uncons(stream) do
    hd = Stream.take(stream, 1)
         |> Enum.at(0)
    tl = Stream.drop(stream, 1)
    {hd, tl}
  end

end
