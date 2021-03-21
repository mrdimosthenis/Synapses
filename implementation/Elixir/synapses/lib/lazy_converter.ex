defmodule LazyConverter do
  @moduledoc false

  def empty_stream(),
      do: Stream.iterate(0, &(&1 + 1))
          |> Stream.take(0)

  def singleton(x) do
    [x]
    |> Stream.cycle
    |> Stream.take(1)
  end

  def uncons(stream) do
    hd = Stream.take(stream, 1)
         |> Enum.at(0)
    tl = Stream.drop(stream, 1)
    {hd, tl}
  end

  def stream_to_iterator(stream) do
    yield = fn acc ->
      case uncons(acc) do
        {nil, _} ->
          :done
        {hd, tl} ->
          {:next, hd, tl}
      end
    end
    :gleam@iterator.unfold(stream, yield)
  end

  def iterator_to_stream(iterator) do
    init =
      case :gleam@iterator.step(iterator) do
        :done ->
          nil
        {:next, hd, tl} ->
          {hd, tl}
      end

    yield = fn {x, acc} ->
      case :gleam@iterator.step(acc) do
        :done ->
          {x, nil}
        {:next, hd, tl} ->
          {x, {hd, tl}}
      end
    end

    Stream.unfold(init, yield)
  end

end
