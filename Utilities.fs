module Synapses.Utilities

open FSharpx.Collections
open System.Text.Json
open System.Text.Json.Serialization

let lazyRange (): LazyList<int> =
    LazyList.unfold
        (fun x ->
            Some(x, x + 1)
        )
        0

let lazyZipWithIndex
        (ls: LazyList<'a>)
        : LazyList<'a * int> =
    LazyList.map2
        (fun x i ->
            (x, i)
        )
        ls
        (lazyRange ())

let lazyUnzip (ls: LazyList<'a * 'b>)
              : LazyList<'a> * LazyList<'b> =
    let (reversedXs, reversedYs) =
            LazyList.fold
                (fun (accXs, accYs) (x, y) ->
                    (
                     LazyList.cons x accXs,
                     LazyList.cons y accYs
                    )
                )
                (LazyList.empty, LazyList.empty)
                ls
    let xs = LazyList.rev reversedXs
    let ys = LazyList.rev reversedYs
    (xs, ys)

let lazySplitAt
        (n: int)
        (ls: LazyList<'a>)
        : LazyList<'a> * LazyList<'a> =
    let rec go (i: int)
               (xWithY: LazyList<'a> * LazyList<'a>)
           : LazyList<'a> * LazyList<'a> =
           let (x, y) = xWithY
           match LazyList.isEmpty y || i <= 0 with
           | true ->
               xWithY
           | false ->
               let (yHd, yTl) = LazyList.uncons y
               go (i - 1) (LazyList.cons yHd x, yTl)
    let (x, y) = go n (LazyList.empty, ls)
    (LazyList.rev x, y)

let jsonOptions = JsonSerializerOptions()
jsonOptions.WriteIndented <- true
jsonOptions.Converters.Add(JsonFSharpConverter())
