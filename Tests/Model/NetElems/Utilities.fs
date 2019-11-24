module Synapses.Tests.Model.NetElems.Utilities

open FSharpx.Collections
open Synapses.Model
open Synapses.Model.NetElems

let realizedActivFun
        (activationF: Activation.Function)
        : string =
        activationF.name

let realizedNeuron
        (neuron: Neuron.Neuron)
        : string * List<string> =
        (
                realizedActivFun
                       neuron.activationF,
                neuron.weights
                |> LazyList.map (sprintf "%f")
                |> LazyList.toList
        )

let realizedLayer
        (layer: Layer.Layer)
        : List<string * List<string>> =
        layer
        |> LazyList.map realizedNeuron
        |> LazyList.toList

let realizedNetwork
        (network: Network.Network)
        : List<List<string * List<string>>> =
        network
        |> LazyList.map realizedLayer
        |> LazyList.toList

