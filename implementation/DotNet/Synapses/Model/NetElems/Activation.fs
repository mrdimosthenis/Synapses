module Synapses.Model.NetElems.Activation

open System

type Function = { name: string
                  f: float -> float
                  deriv: float -> float
                  inverse: float -> float
                  minMaxInVals: float * float }

type SerializableFunction = string

exception UnknownActivationF of string

let restrictedInput
        (activation: Function)
        (x: float)
        : float =
    let (minInVal, maxInVal) = activation.minMaxInVals
    max
      (min x maxInVal)
      minInVal

let restrictedOutput
        (activation: Function)
        (y: float)
        : float =
    let (minInVal, maxInVal) = activation.minMaxInVals
    max
      (min y (activation.f maxInVal))
      (activation.f minInVal)

let sigmoid_f (x: float): float =
    1.0 / (1.0 + Math.Exp(-x))

let sigmoid = { name = "sigmoid"
                f = sigmoid_f
                deriv = fun d ->
                    sigmoid_f d * (1.0 - sigmoid_f d)
                inverse = fun y ->
                    Math.Log(y / (1.0 - y))
                minMaxInVals = (-700.0, 20.0) }

let identity = { name = "identity"
                 f = fun x -> x
                 deriv = fun _ -> 1.0
                 inverse = fun y -> y
                 minMaxInVals = (-1.7976931348623157E308, 1.7976931348623157E308) }

let tanh = { name = "tanh"
             f = Math.Tanh
             deriv = fun d ->
                 1.0 - (Math.Tanh d) * (Math.Tanh d)
             inverse = fun y ->
                 0.5 * Math.Log((1.0 + y) / (1.0 - y))
             minMaxInVals = (-10.0, 10.0) }

let leakyReLU = { name = "leakyReLU"
                  f = fun x ->
                      match x with
                      | _ when x < 0.0 -> 0.01 * x
                      | _ -> x
                  deriv = fun d ->
                      match d with
                      | _ when d < 0.0 -> 0.01
                      | _ -> 1.0
                  inverse = fun y ->
                      match y with
                      | _ when y < 0.0 -> y / 0.01
                      | _ -> y
                  minMaxInVals = (-1.7976931348623157E308, 1.7976931348623157E308) }

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
