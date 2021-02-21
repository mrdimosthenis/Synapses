module Synapses.Model.Layer

open FSharpx.Collections
open Synapses.Model
open Synapses.Model.NetElems

type Layer = LazyList<Neuron.Neuron>

type SerializableLayer =
        List<Neuron.SerializableNeuron>

let init (inputSize: int)
         (outputSize: int)
         (activationF: Activation.Function)
         (weightInitF: unit -> unit -> float)
         : Layer =
    id
    |> Seq.initInfinite
    |> LazyList.ofSeq
    |> LazyList.take outputSize
    |> LazyList.map
        (fun _ ->
            Neuron.init
                inputSize
                activationF
                (weightInitF())
        )

let output (input: LazyList<float>)
           (layer: Layer)
           : LazyList<float> =
    LazyList.map
        (Neuron.output input)
        layer

let backPropagated
        (learnRate: float)
        (input: LazyList<float>)
        (outWithErrors: LazyList<float * float>)
        (layer: Layer)
        : LazyList<float> * Layer =
    let (errorsMulti, newLayer) =
            LazyList.map2
                (Neuron.backPropagated learnRate input)
                outWithErrors
                layer
            |> Utilities.lazyUnzip
    let errors = LazyList.fold
                    (LazyList.map2 (+))
                    (LazyList.repeat 0.0)
                    errorsMulti
    (errors, newLayer)

let serialized
        (layer: Layer):
        SerializableLayer =
    layer
    |> LazyList.toList
    |> List.map Neuron.serialized

let deserialized
    (s: SerializableLayer)
    : Layer =
    s
    |> List.map Neuron.deserialized
    |> LazyList.ofList
