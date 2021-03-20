defmodule SynapsesTest do
  use ExUnit.Case
  import Synapses
  alias Synapses.{NeuralNetwork}
  doctest Synapses

  test "uncons" do
    infinite_stream = Stream.iterate(0, &(&1 + 1))
    singleton_stream = infinite_stream |> Stream.take(1)
    empty_stream = infinite_stream |> Stream.take(0)

    {a, tl1} = uncons(infinite_stream)
    {b, _} = uncons(tl1)
    {c, tl2} = uncons(singleton_stream)
    {d, tl3} = uncons(empty_stream)

    assert a == 0
    assert b == 1
    assert c == 0
    assert Enum.empty?(tl2)
    assert d == nil
    assert Enum.empty?(tl3)
  end

  test "nn" do
    assert NeuralNetwork.init([3, 2, 2]) == [3, 2, 2]
  end

end
