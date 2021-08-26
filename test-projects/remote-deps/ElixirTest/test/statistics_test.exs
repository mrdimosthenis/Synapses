defmodule StatisticsTest do
  use ExUnit.Case
  alias Synapses.{Statistics}

  def expectedWithOutputValues(),
      do: [
            {[0.0, 0.0, 1.0], [0.0, 0.0, 1.0]},
            {[0.0, 0.0, 1.0], [0.0, 1.0, 1.0]}
          ]
          |> Stream.map(fn x -> x end)

  test "root mean square error" do
    assert expectedWithOutputValues() |> Statistics.rootMeanSquareError == 0.7071067811865476
  end
end
