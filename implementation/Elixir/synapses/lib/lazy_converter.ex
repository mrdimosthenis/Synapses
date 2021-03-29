defmodule LazyConverter do
  @moduledoc false

  def infinite_stream(),
      do: Stream.iterate(0, &(&1 + 1))

  def empty_stream(),
      do: infinite_stream()
          |> Stream.take(0)

  def singleton(x) do
    [x]
    |> Stream.cycle
    |> Stream.take(1)
  end

  def uncons(stream) do
    heads = Stream.take(stream, 1)
    case Enum.empty?(heads) do
      true ->
        {:error, nil}
      false ->
        hd = Enum.at(heads, 0)
        tl = Stream.drop(stream, 1)
        {:ok, {hd, tl}}
    end
  end

  def stream_to_iterator(stream) do
    yield = fn acc ->
      case uncons(acc) do
        {:error, nil} ->
          :done
        {:ok, {hd, tl}} ->
          {:next, hd, tl}
      end
    end
    :gleam@iterator.unfold(stream, yield)
  end

end
