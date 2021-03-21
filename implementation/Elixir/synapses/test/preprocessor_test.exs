defmodule PreprocessorTest do
  use ExUnit.Case
  alias Synapses.{DataPreprocessor}

  def datapoints(), do: "../../../../test-projects/resources/mnist.csv"
                        |> Path.expand(__DIR__)
                        |> File.stream!
                        |> CSV.decode!(headers: true)

  def keysWithDiscreteFlags() do
    pixelKeysWithFlags = Stream.map(0..783, fn i -> {"pixel" <> to_string(i), false} end)
    {"label", true}
    |> LazyConverter.singleton
    |> Stream.concat(pixelKeysWithFlags)
    |> Enum.to_list
  end

  def preprocessor(), do: DataPreprocessor.init(keysWithDiscreteFlags(), datapoints())

  def preprocessorJson(), do: "../../../../test-projects/resources/preprocessor.json"
                              |> Path.expand(__DIR__)
                              |> File.read!

  def firstDatapoint(),
      do: datapoints()
          |> Enum.at(0)

  def firstEncodedDatapoint(), do: DataPreprocessor.encodedDatapoint(preprocessor(), firstDatapoint())

  def firstDecodedDatapointValues(), do: DataPreprocessor.decodedDatapoint(preprocessor(), firstEncodedDatapoint())

#  test "preprocessor to json" do
#    assert DataPreprocessor.toJson(preprocessor()) == preprocessorJson()
#  end

end
