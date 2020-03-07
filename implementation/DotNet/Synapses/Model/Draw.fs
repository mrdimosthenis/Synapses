module Synapses.Model.Draw

open FSharpx.Collections
open Svg
open Synapses.Model
open Synapses.Model
open Synapses.Model
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

let circleSVG (x: float) (y: float) (stroke_val: string): SvgCircle =
    let c = SvgCircle()
    let _ = (c.CenterX.Value = float32 x)
    let _ = (c.CenterY.Value = float32 y)
    let _ = (c.Radius.Value = float32 circleRadius)
    let _ = (c.Stroke.Content = stroke_val)
    let _ = (c.StrokeWidth.Value = float32 circleStrokeWidth)
    let _ = (c.Fill.Content = circleFill)
    c

let inputCirclesSVGs (maxChainCircles: int) (inputCircles: int): LazyList<SvgCircle> =
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

let outputCirclesSVGs (maxChainCircles: int) (outputChainOrder: int) (outputActivations: LazyList<string>): LazyList<SvgCircle> =
    outputActivations
    |> Utilities.lazyZipWithIndex
    |> LazyList.map (fun (activ, i) ->
        let x = circleCX (outputChainOrder)
        let outputActivationsLength = LazyList.length outputActivations
        let y = circleCY maxChainCircles outputActivationsLength i
        let stroke_val = activationNameToStroke activ
        circleSVG x y stroke_val)

let hiddenCirclesSVGs (maxChainCircles: int) (hiddenChainOrder: int) (hiddenActivations: LazyList<string>): LazyList<SvgCircle> =
    hiddenActivations
    |> LazyList.map Some
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

let layerCirclesSVGs (maxChainCircles: int) (layerOrder: int) (numOfLayers: int) (layer: Layer): LazyList<SvgCircle> =
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
    (baseCircleOrder: int) (targetCircleOrder: int) (weight: float) (maxAbsWeight: float): SvgLine =
    let alpha = abs weight / maxAbsWeight
    let x1_val = circleCX baseChainOrder
    let y1_val = circleCY maxChainCircles numOfCirclesInBaseChain baseCircleOrder
    let x2_val = circleCX (baseChainOrder + 1)
    let y2_val = circleCY maxChainCircles numOfCirclesInTargetChain targetCircleOrder

    let stroke_val =
        match weight > 0.0 with
        | true -> positiveLineStroke
        | false -> negativeLineStroke

    let ln = SvgLine()
    let _ = (ln.Opacity = float32 alpha)
    let _ = (ln.StartX.Value = float32 x1_val)
    let _ = (ln.StartY.Value = float32 y1_val)
    let _ = (ln.EndX.Value = float32 x2_val)
    let _ = (ln.EndY.Value = float32 y2_val)
    let _ = (ln.Stroke.Content = stroke_val)
    let _ = (ln.StrokeWidth.Value = float32 lineStrokeWidth)
    ln

let neuronLinesSVGs (maxChainCircles: int) (layerSize: int) (layerOrder: int) (numOfLayers: int)
    (neuronOrderInLayer: int) (maxAbsWeight: float) (weights: LazyList<float>): LazyList<SvgLine> =
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

let layerLinesSVGs (maxChainCircles: int) (layerOrder: int) (numOfLayers: int) (maxAbsWeight: float) (layer: Layer): LazyList<SvgLine> =
    layer
    |> Utilities.lazyZipWithIndex
    |> LazyList.map
        (fun (neuron, neuronOrderInLayer) ->
        neuronLinesSVGs maxChainCircles (LazyList.length layer) layerOrder numOfLayers neuronOrderInLayer maxAbsWeight
            neuron.weights)
    |> LazyList.concat

let networkSVG (network: Network): SvgGroup =
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
    let svg = SvgGroup()
    let _ = (svg.Bounds.Width = float32 w)
    let _ = (svg.Bounds.Height = float32 h)
    LazyList.iter (fun line -> svg.Children.Add(line)) linesSVGs
    LazyList.iter (fun circle -> svg.Children.Add(circle)) circlesSVGs
    svg
