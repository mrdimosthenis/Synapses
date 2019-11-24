module Synapses.Mathematics

open System
open FSharpx.Collections

let sigmoid (x: float): float = 1.0 / (1.0 + Math.Exp(-x))

let sigmoidDerivative (x: float): float = sigmoid x * (1.0 - sigmoid x)

let logit (x: float): float = Math.Log(x / (1.0 - x))

let constantly1 (_: 'a): float = 1.0

let dotProduct (xs: LazyList<float>) (ys: LazyList<float>): float =
    LazyList.map2 (fun x y -> x * y) xs ys
    |> LazyList.fold (fun acc z -> acc + z) 0.0
