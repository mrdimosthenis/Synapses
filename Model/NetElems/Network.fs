module Synapses.Model.Network

open FSharpx.Collections
open Synapses
open Synapses.Model.NetElems
open System.Text.Json
open System.Text.Json.Serialization

type Network = LazyList<Layer.Layer>

[<JsonFSharpConverter>]
type SerializableNetwork =
        List<Layer.SerializableLayer>

let init (layerSizes: LazyList<int>)
         (activationF: int -> Activation.Function)
         (weightInitF: int -> float)
         : Network =
    layerSizes
    |> LazyList.tail
    |> LazyList.zip layerSizes
    |> Utilities.lazyZipWithIndex
    |> LazyList.map
        (fun ((lrSz, nextLrSz), index) ->
            Layer.init
                lrSz
                nextLrSz
                (activationF index)
                (fun () () ->
                    weightInitF index
                )
        )

let output (input: LazyList<float>)
           (network: Network)
           : LazyList<float> =
    LazyList.fold
        Layer.output
        input
        network

let feed (alreadyFed: LazyList<LazyList<float> * Layer.Layer>)
         (nextLayer: Layer.Layer)
         : LazyList<LazyList<float> * Layer.Layer> =
    let (lastInput, lastLayer) = LazyList.head alreadyFed
    let nextInput = Layer.output lastInput lastLayer
    LazyList.cons (nextInput, nextLayer) alreadyFed

let fedForward
        (input: LazyList<float>)
        (network: Network)
        : LazyList<LazyList<float> * Layer.Layer> =
    let initFeed = LazyList.ofList
                    [(input, LazyList.head network)]
    LazyList.fold
        feed
        initFeed
        (LazyList.tail network)

let propagate
        (learnRate: float)
        (errorsWithAlreadyPropagated: LazyList<float> * LazyList<Layer.Layer>)
        (inputWithLayer: LazyList<float> * Layer.Layer)
        : LazyList<float> * LazyList<Layer.Layer> =
    let (errors, alreadyPropagated) = errorsWithAlreadyPropagated
    let (lastInput, lastLayer) = inputWithLayer
    let lastOutput = Layer.output lastInput lastLayer
    let last0utWithErrors = LazyList.zip lastOutput errors
    let (nextErrors, propagatedLayer) =
                        Layer.backPropagated
                                learnRate
                                lastInput
                                last0utWithErrors
                                lastLayer
    let nextAlreadyPropagated =
            LazyList.cons propagatedLayer alreadyPropagated
    (nextErrors, nextAlreadyPropagated)

let backPropagated
        (learnRate: float)
        (netExpOutput: LazyList<float>)
        (revInputsWithLayers: LazyList<LazyList<float> * Layer.Layer>)
        : Network =
    let (lastInput, lastLayer) = LazyList.head revInputsWithLayers
    let netOutput = Layer.output lastInput lastLayer
    let netErrors = LazyList.map2 (-) netOutput netExpOutput
    let net0utWithErrors = LazyList.zip netOutput netErrors
    let (initErrors, firstPropagated) =
                        Layer.backPropagated
                                learnRate
                                lastInput
                                net0utWithErrors
                                lastLayer
    let (_, propagatedNet) =
                LazyList.fold
                    (propagate learnRate)
                    (initErrors, LazyList.ofList [firstPropagated])
                    (LazyList.tail revInputsWithLayers)
    propagatedNet

let fitted (learnRate: float)
           (input: LazyList<float>)
           (expOutput: LazyList<float>)
           (network: Network)
           : Network =
    fedForward input network
    |> backPropagated learnRate expOutput

let serialized
        (network: Network):
        SerializableNetwork =
    network
    |> LazyList.toList
    |> List.map Layer.serialized

let deserialized
    (s: SerializableNetwork)
    : Network =
    s
    |> List.map Layer.deserialized
    |> LazyList.ofList

let options = JsonSerializerOptions()
options.WriteIndented <- true
options.Converters.Add(JsonFSharpConverter())

let toJson (network: Network): string =
    JsonSerializer.Serialize
        (serialized network, options)

let fromJson (json: string): Network =
    (json, options)
    |> JsonSerializer.Deserialize<SerializableNetwork>
    |> deserialized
