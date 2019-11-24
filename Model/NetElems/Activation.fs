module Synapses.Model.NetElems.Activation

open System
open System.Text.Json.Serialization

type Function = { name: string
                  f: float -> float
                  deriv: float -> float
                  inverse: float -> float }

[<JsonFSharpConverter>]
type SerializableFunction = string

exception UnknownActivationF of string

let safeSigmoid (x: float): float =
    -x
    |> min 500.0
    |> Math.Exp
    |> (+) 1.0
    |> (/) 1.0

let sigmoid = { name = "sigmoid"
                f = safeSigmoid
                deriv = fun x ->
                    safeSigmoid x * (1.0 - safeSigmoid x)
                inverse = fun x ->
                    Math.Log(x / (1.0 - x)) }

let identity = { name = "identity"
                 f = fun x -> x
                 deriv = fun _ -> 1.0
                 inverse = fun x -> x }

let tanh = { name = "tanh"
             f = Math.Tanh
             deriv = fun x ->
                 1.0 - (Math.Tanh x) * (Math.Tanh x)
             inverse = fun x ->
                 0.5 * Math.Log((1.0 + x) / (1.0 - x)) }

let leakyReLU = { name = "leakyReLU"
                  f = fun x ->
                      match x with
                      | _ when x < 0.0 -> 0.01 * x
                      | _ -> x
                  deriv = fun x ->
                      match x with
                      | _ when x < 0.0 -> 0.01
                      | _ -> 1.0
                  inverse = fun x ->
                      match x with
                      | _ when x < 0.0 -> x / 0.01
                      | _ -> x }

let serialized
        (activF: Function):
        SerializableFunction =
    activF.name

let deserialized
    (s: SerializableFunction)
    : Function =
    match s with
    | "sigmoid" -> sigmoid
    | "identity" -> identity
    | "tanh" -> tanh
    | "leakyReLU" -> leakyReLU
    | _ -> raise (UnknownActivationF(s))
