module Synapses.Model.Network

open FSharpx.Collections
open Synapses.Model
open Synapses.Model.NetElems

type Network = LazyList<Layer.Layer>

type SerializableNetwork =
        List<Layer.SerializableLayer>

// public
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

let errorsWithBackPropagated
        (learnRate: float)
        (netExpOutput: LazyList<float>)
        (revInputsWithLayers: LazyList<LazyList<float> * Layer.Layer>)
        : LazyList<float> * Network =
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
    LazyList.fold
        (propagate learnRate)
        (initErrors, LazyList.ofList [firstPropagated])
        (LazyList.tail revInputsWithLayers)

let backPropagated
        (learnRate: float)
        (netExpOutput: LazyList<float>)
        (revInputsWithLayers: LazyList<LazyList<float> * Layer.Layer>)
        : Network =
    let (_, propagatedNet) =
                errorsWithBackPropagated
                    learnRate
                    netExpOutput
                    revInputsWithLayers
    propagatedNet

let errorsWithFitted
        (learnRate: float)
        (input: LazyList<float>)
        (expOutput: LazyList<float>)
        (network: Network)
        : LazyList<float> * Network =
    fedForward input network
    |> errorsWithBackPropagated learnRate expOutput

// public
let errors (learnRate: float)
           (input: LazyList<float>)
           (expOutput: LazyList<float>)
           (network: Network)
           : LazyList<float> =
    let restrictedOutput = network
                           |> LazyList.rev
                           |> LazyList.head
                           |> LazyList.map2
                                (fun y neuron ->
                                    Activation.restrictedOutput
                                        neuron.activationF
                                        y
                                )
                                expOutput
    let (ers, _) =
                errorsWithFitted
                    learnRate
                    input
                    restrictedOutput
                    network
    ers

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

// public
let toJson (network: Network): string =
    network
    |> serialized
    |> Newtonsoft.Json.JsonConvert.SerializeObject

let lazyRealization
        (network: Network)
        : Network =
    let _ = serialized(network)
    network

// public
let init (layerSizes: LazyList<int>)
         (activationF: int -> Activation.Function)
         (weightInitF: int -> float)
         : Network =
    layerSizes
    |> LazyList.tail
    |> LazyList.zip layerSizes
    |> Seq.indexed
    |> LazyList.ofSeq
    |> LazyList.map
        (fun (index, (lrSz, nextLrSz)) ->
            Layer.init
                lrSz
                nextLrSz
                (activationF index)
                (fun () () ->
                    weightInitF index
                )
        )
    |> lazyRealization

// public
let fitted (learnRate: float)
           (input: LazyList<float>)
           (expOutput: LazyList<float>)
           (network: Network)
           : Network =
    let restrictedOutput = network
                           |> LazyList.rev
                           |> LazyList.head
                           |> LazyList.map2
                                (fun y neuron ->
                                    Activation.restrictedOutput
                                        neuron.activationF
                                        y
                                )
                                expOutput
    let (_, fittedNet) =
                errorsWithFitted
                    learnRate
                    input
                    restrictedOutput
                    network
    lazyRealization fittedNet

// public
let fromJson (json: string): Network =
    json
    |> Newtonsoft.Json.JsonConvert.DeserializeObject<SerializableNetwork>
    |> deserialized
    |> lazyRealization
