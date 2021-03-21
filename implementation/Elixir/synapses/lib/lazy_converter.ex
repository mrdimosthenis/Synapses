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

  def recurrent(x0, s0, rec_fun) do
    Stream.concat(
      singleton(x0),
      case rec_fun.(x0, s0) do
        nil ->
          empty_stream()
        {x1, s1} ->
          recurrent(x1, s1, rec_fun)
      end
    )
  end

  def iterator_to_stream(iterator) do
    rec_fun = fn (_, t1) ->
      case :gleam@iterator.step(t1) do
        {:next, e, es} ->
          {e, es}
        :done ->
          nil
      end
    end

    case :gleam@iterator.step(iterator) do
      {:next, e, es} ->
        recurrent(e, es, rec_fun)
      :done ->
        empty_stream()
    end
  end

end
