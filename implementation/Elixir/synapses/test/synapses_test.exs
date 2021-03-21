defmodule SynapsesTest do
  use ExUnit.Case
  alias Synapses.{NeuralNetwork}

  test "nn" do
    assert NeuralNetwork.init([3, 2, 2]) == [3, 2, 2]
  end

end
