module Synapses.Model.Utilities

open FSharpx.Collections

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
