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
    {:ok, {a, tl1}} = uncons(infinite_stream())
    {:ok, {b, _}} = uncons(tl1)
    {:ok, {c, tl2}} = uncons(singleton(0))
    {:error, nil} = empty_stream() |> uncons

    assert a == 0
    assert b == 1
    assert c == 0
    assert Enum.empty?(tl2)
  end

  test "stream_to_iterator" do
    infinite_iterator = infinite_stream() |> stream_to_iterator()
    singleton_iterator = infinite_stream() |> Stream.take(1) |> stream_to_iterator
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
    my_infinite_stream = infinite_stream() |> stream_to_iterator |> iterator_to_stream
    my_singleton_stream = Stream.take(my_infinite_stream, 1)
    my_empty_stream = Stream.take(my_infinite_stream, 0)

    {:ok, {a, tl1}} = uncons(my_infinite_stream)
    {:ok, {b, _}} = uncons(tl1)
    {:ok, {c, tl2}} = uncons(my_singleton_stream)
    {:error, nil} = uncons(my_empty_stream)

    assert a == 0
    assert b == 1
    assert c == 0
    assert Enum.empty?(tl2)
  end

end
