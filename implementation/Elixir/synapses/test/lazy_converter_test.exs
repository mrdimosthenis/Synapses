defmodule LazyConverterTest do
  use ExUnit.Case
  import LazyConverter

  test "singleton" do
    a = 3 |> singleton |> Enum.at(0)
    b = "3" |> singleton |> Enum.at(0)
    c = '3' |> singleton |> Enum.at(0)

    assert a == 3
    assert b == "3"
    assert c == '3'
  end

  test "uncons" do
    infinite_stream = Stream.iterate(0, &(&1 + 1))
    singleton_stream = infinite_stream |> Stream.take(1)

    {a, tl1} = uncons(infinite_stream)
    {b, _} = uncons(tl1)
    {c, tl2} = uncons(singleton_stream)
    {d, tl3} = empty_stream() |> uncons

    assert a == 0
    assert b == 1
    assert c == 0
    assert Enum.empty?(tl2)
    assert d == nil
    assert Enum.empty?(tl3)
  end

  test "stream_to_iterator" do
    infinite_stream = Stream.iterate(0, &(&1 + 1))

    infinite_iterator = stream_to_iterator(infinite_stream)
    singleton_iterator = infinite_stream |> Stream.take(1) |> stream_to_iterator
    empty_iterator = empty_stream() |> stream_to_iterator

    {:next, a, tl1} = :gleam@iterator.step(infinite_iterator)
    {:next, b, _} = :gleam@iterator.step(tl1)
    {:next, c, _} = :gleam@iterator.step(singleton_iterator)
    :done = :gleam@iterator.step(empty_iterator)

    assert a == 0
    assert b == 1
    assert c == 0
  end

  test "iterator_to_stream" do
    infinite_stream = Stream.iterate(0, &(&1 + 1)) |> stream_to_iterator |> iterator_to_stream

    singleton_stream = infinite_stream |> Stream.take(1)

    {a, tl1} = uncons(infinite_stream)
    {b, _} = uncons(tl1)
    {c, tl2} = uncons(singleton_stream)
    {d, tl3} = empty_stream() |> uncons

    assert a == 0
    assert b == 1
    assert c == 0
    assert Enum.empty?(tl2)
    assert d == nil
    assert Enum.empty?(tl3)
  end

end
