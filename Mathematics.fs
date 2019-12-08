module Synapses.Mathematics

open FSharpx.Collections

let dotProduct
        (xs: LazyList<float>)
        (ys: LazyList<float>)
        : float =
        LazyList.map2 (fun x y -> x * y) xs ys
        |> LazyList.fold (fun acc z -> acc + z) 0.0

let euclideanDistance
        (x: LazyList<float>)
        (y: LazyList<float>)
        : float =
        LazyList.map2
                (fun xi yi ->
                        (xi - yi) * (xi - yi)
                )
                x
                y
        |> LazyList.fold (fun acc z -> acc + z) 0.0
        |> sqrt

let rootMeanSquareError
        (y_hats: LazyList<LazyList<float>>)
        (ys: LazyList<LazyList<float>>)
        : float =
        let (n, sum) =
                LazyList.map2
                        (fun y_hat y ->
                                let d = euclideanDistance y_hat y
                                d * d
                        )
                        y_hats
                        ys
                |> LazyList.fold
                        (fun (accI, accS) sd -> (accI + 1, accS + sd))
                        (0, 0.0)
        sqrt (sum / float n)
