module Synapses.Model.Draw

open FSharpx.Collections
open Synapses.Model
open Synapses.Model.Layer
open Synapses.Model.Network
open Synapses.Model.Neuron

exception UnknownActivationF of string

let pixels = 400.0

let circleVerticalDistance = pixels * 0.02
let circleHorizontalDistance = pixels * 0.15

let circleRadius = pixels * 0.03
let circleStrokeWidth = pixels / 150.0

let lineStrokeWidth = pixels / 300.0

let circleFill = "white"

let inputCircleStroke = "brown"
let biasCircleStroke = "black"

let sigmoidCircleStroke = "blue"
let identityCircleStroke = "orange"
let tanhCircleStroke = "yellow"
let leakyReLUCircleStroke = "pink"

let positiveLineStroke = "lawngreen"
let negativeLineStroke = "palevioletred"

let activationNameToStroke (activName: string): string =
    match activName with
    | "sigmoid" -> sigmoidCircleStroke
    | "identity" -> identityCircleStroke
    | "tanh" -> tanhCircleStroke
    | "leakyReLU" -> leakyReLUCircleStroke
    | _ -> raise (UnknownActivationF(activName))

let layerWidth (numOfCircles: int): float =
    2.0 * circleRadius
    |> (+) circleVerticalDistance
    |> (*) (float numOfCircles)
    |> (+) circleVerticalDistance

let circleCX (chainOrder: int): float =
    circleHorizontalDistance
    |> (*) (float chainOrder)
    |> (+) circleHorizontalDistance

let circleCY (maxChainCircles: int) (numOfChainCircles: int) (circleOrder: int): float =
    let currentLayerWidth = layerWidth (numOfChainCircles)
    let maxLayerWidth = layerWidth (maxChainCircles)
    let layerY = (maxLayerWidth - currentLayerWidth) / 2.0
    2.0 * circleRadius
    |> (+) circleVerticalDistance
    |> (*) (float circleOrder + 1.0)
    |> (+) layerY

let circleSVG (x: float) (y: float) (stroke_val: string): string =
    sprintf "<circle cx=\"%f\" cy=\"%f\" r=\"%f\" stroke=\"%s\" stroke-width=\"%f\" fill=\"%s\"></circle>"
        x y circleRadius stroke_val circleStrokeWidth circleFill

let inputCirclesSVGs (maxChainCircles: int) (inputCircles: int): LazyList<string> =
    Utilities.lazyRange()
    |> LazyList.take inputCircles
    |> LazyList.map (fun i ->
        let x = circleCX 0
        let y = circleCY maxChainCircles inputCircles i

        let stroke_val =
            match i with
            | 0 -> biasCircleStroke
            | _ -> inputCircleStroke
        circleSVG x y stroke_val)

let outputCirclesSVGs (maxChainCircles: int) (outputChainOrder: int) (outputActivations: LazyList<string>): LazyList<string> =
    outputActivations
    |> Utilities.lazyZipWithIndex
    |> LazyList.map (fun (activ, i) ->
        let x = circleCX (outputChainOrder)
        let outputActivationsLength = LazyList.length outputActivations
        let y = circleCY maxChainCircles outputActivationsLength i
        let stroke_val = activationNameToStroke activ
        circleSVG x y stroke_val)

let hiddenCirclesSVGs (maxChainCircles: int) (hiddenChainOrder: int) (hiddenActivations: LazyList<string>): LazyList<string> =
    hiddenActivations
    |> LazyList.map Some
    |> LazyList.cons None
    |> Utilities.lazyZipWithIndex
    |> LazyList.map (fun (activ, i) ->
        let x = circleCX hiddenChainOrder
        let hiddenActivationsLength = LazyList.length hiddenActivations
        let y = circleCY maxChainCircles (hiddenActivationsLength + 1) i

        let stroke_val =
            match activ with
            | None -> biasCircleStroke
            | Some act -> activationNameToStroke act
        circleSVG x y stroke_val)

let layerCirclesSVGs (maxChainCircles: int) (layerOrder: int) (numOfLayers: int) (layer: Layer): LazyList<string> =
    let isLastLayer = (layerOrder = numOfLayers - 1)
    let activations = LazyList.map (fun (neuron: Neuron) -> neuron.activationF.name) layer
    let weightsLength = LazyList.length (LazyList.head layer).weights

    let inputCircles =
        match layerOrder with
        | 0 -> inputCirclesSVGs maxChainCircles weightsLength
        | _ -> LazyList.empty

    let hiddenCircles =
        match isLastLayer with
        | true -> LazyList.empty
        | false -> hiddenCirclesSVGs maxChainCircles (layerOrder + 1) activations

    let outputCircles =
        match isLastLayer with
        | true -> outputCirclesSVGs maxChainCircles (layerOrder + 1) activations
        | false -> LazyList.empty

    [ inputCircles; hiddenCircles; outputCircles ]
    |> LazyList.ofList
    |> LazyList.concat

let lineSVG (maxChainCircles: int) (baseChainOrder: int) (numOfCirclesInBaseChain: int) (numOfCirclesInTargetChain: int)
    (baseCircleOrder: int) (targetCircleOrder: int) (weight: float) (maxAbsWeight: float): string =
    let alpha = abs weight / maxAbsWeight
    let x1_val = circleCX baseChainOrder
    let y1_val = circleCY maxChainCircles numOfCirclesInBaseChain baseCircleOrder
    let x2_val = circleCX (baseChainOrder + 1)
    let y2_val = circleCY maxChainCircles numOfCirclesInTargetChain targetCircleOrder

    let stroke_val =
        match weight > 0.0 with
        | true -> positiveLineStroke
        | false -> negativeLineStroke
    sprintf "<line stroke-opacity=\"%f\" x1=\"%f\" y1=\"%f\" x2=\"%f\" y2=\"%f\" stroke=\"%s\" stroke-width=\"%f\"></line>"
        alpha x1_val y1_val x2_val y2_val stroke_val lineStrokeWidth

let neuronLinesSVGs (maxChainCircles: int) (layerSize: int) (layerOrder: int) (numOfLayers: int)
    (neuronOrderInLayer: int) (maxAbsWeight: float) (weights: LazyList<float>): LazyList<string> =
    let isOutputLayer = (layerOrder = numOfLayers - 1)
    let numOfCirclesInBaseChain = LazyList.length weights

    let numOfCirclesInTargetChain =
        match isOutputLayer with
        | true -> layerSize
        | false -> layerSize + 1

    let targetCircleOrder =
        match isOutputLayer with
        | true -> neuronOrderInLayer
        | false -> neuronOrderInLayer + 1

    weights
    |> Utilities.lazyZipWithIndex
    |> LazyList.map
        (fun (w, i) ->
        lineSVG maxChainCircles layerOrder numOfCirclesInBaseChain numOfCirclesInTargetChain i targetCircleOrder w
            maxAbsWeight)

let layerLinesSVGs (maxChainCircles: int) (layerOrder: int) (numOfLayers: int) (maxAbsWeight: float) (layer: Layer): LazyList<string> =
    layer
    |> Utilities.lazyZipWithIndex
    |> LazyList.map
        (fun (neuron, neuronOrderInLayer) ->
        neuronLinesSVGs maxChainCircles (LazyList.length layer) layerOrder numOfLayers neuronOrderInLayer maxAbsWeight
            neuron.weights)
    |> LazyList.concat

let networkSVG (network: Network): string =
    let maxChainCircles =
        network
        |> Utilities.lazyZipWithIndex
        |> LazyList.map (fun (layer, i) ->
            match i = LazyList.length network - 1 with
            | true -> LazyList.length layer + 1
            | false -> LazyList.length layer)
        |> LazyList.fold (fun acc x ->
            match x > acc with
            | true -> x
            | false -> acc) 0

    let numOfLayers = LazyList.length network

    let maxAbsWeight =
        network
        |> LazyList.map (fun layer ->
            layer
            |> LazyList.map (fun (neuron: Neuron) -> LazyList.map (fun w -> abs (w)) neuron.weights)
            |> LazyList.concat)
        |> LazyList.concat
        |> LazyList.fold (fun acc x ->
            match x > acc with
            | true -> x
            | false -> acc) 0.0
    let circlesSVGs =
        network
        |> Utilities.lazyZipWithIndex
        |> LazyList.map (fun (layer, i) -> layerCirclesSVGs maxChainCircles i numOfLayers layer)
        |> LazyList.concat

    let linesSVGs =
        network
        |> Utilities.lazyZipWithIndex
        |> LazyList.map (fun (layer, i) -> layerLinesSVGs maxChainCircles i numOfLayers maxAbsWeight layer)
        |> LazyList.concat
    
    let w = circleCX (numOfLayers + 1)
    let h = circleCY maxChainCircles maxChainCircles maxChainCircles

    let netSVGs = LazyList.append linesSVGs circlesSVGs
    
    sprintf "<svg width=\"%f\" height=\"%f\">%s</svg>"
        w h (LazyList.fold (+) "" netSVGs)
