module Synapses.Model.Neuron

open FSharpx.Collections
open Synapses.Model
open Synapses.Model.NetElems
open System.Text.Json.Serialization

type Neuron = { activationF: Activation.Function
                weights: LazyList<float> }

[<JsonFSharpConverter>]
type SerializableNeuron =
    { activationF: Activation.SerializableFunction
      weights: List<float> }

let init (inputSize: int)
         (activationF: Activation.Function)
         (weightInitF: unit -> float)
         : Neuron =
    let weights = id
                  |> Seq.initInfinite
                  |> LazyList.ofSeq
                  |> LazyList.take (1 + inputSize)
                  |> LazyList.map
                        (fun _ -> weightInitF())
    { activationF = activationF
      weights = weights }

let output (input: LazyList<float>)
           (neuron: Neuron)
           : float =
    LazyList.cons 1.0 input
    |> Mathematics.dotProduct neuron.weights
    |> Activation.restrictedInput neuron.activationF
    |> neuron.activationF.f

let backPropagated
        (learnRate: float)
        (input: LazyList<float>)
        (outWithDiff: float * float)
        (neuron: Neuron)
        : LazyList<float> * Neuron =
    let (out, diff) = outWithDiff
    let common = out
                 |> neuron.activationF.inverse
                 |> neuron.activationF.deriv
                 |> (*) diff
    let inDiffs = LazyList.map ((*) common) input
    let newWeights = neuron.weights
                     |> LazyList.map2
                         (fun x w ->
                            w - learnRate * common * x
                         )
                         (LazyList.cons 1.0 input)
    let newNeuron = { neuron
                      with weights = newWeights }
    (inDiffs, newNeuron)

let serialized
        (neuron: Neuron):
        SerializableNeuron =
    { activationF =
          Activation.serialized neuron.activationF
      weights =
          LazyList.toList neuron.weights }

let deserialized
    (s: SerializableNeuron)
    : Neuron =
    { activationF =
          Activation.deserialized s.activationF
      weights =
          LazyList.ofList s.weights }
