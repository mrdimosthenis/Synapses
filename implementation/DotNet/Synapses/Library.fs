﻿namespace Synapses

open FSharpx.Collections
open Synapses.Model
open Synapses.Model.NetElems
open Synapses.Model.Encoding

type ActivationFunction = Activation.Function

module ActivationFunction =

    let sigmoid: ActivationFunction =
            Activation.sigmoid

    let identity: ActivationFunction =
            Activation.identity

    let tanh: ActivationFunction =
            Activation.tanh

    let leakyReLU: ActivationFunction =
            Activation.leakyReLU

type NeuralNetwork = Network.Network

module NeuralNetwork =

    let seedInit (maybeSeed: Option<int>,
                  layers: List<int>)
                 : NeuralNetwork =
                 let layerSizes = LazyList.ofList layers
                 let activationF =
                         fun _ -> Activation.sigmoid
                 let rnd = match maybeSeed with
                           | Some i -> System.Random(i)
                           | None -> System.Random()
                 let weightInitF =
                         fun _ -> rnd.NextDouble()
                                  |> (*) -2.0
                                  |> (+) 1.0
                 Network.init layerSizes activationF weightInitF

    let init (layers: List<int>)
             : NeuralNetwork =
             seedInit (None, layers)

    let initWithSeed
                (seed: int,
                 layers: List<int>)
                : NeuralNetwork =
                seedInit (Some seed, layers)

    let customizedInit
            (layers: List<int>,
             activationF: int -> ActivationFunction,
             weightInitF: int -> float)
            : NeuralNetwork =
            let layerSizes = LazyList.ofList layers
            Network.init layerSizes activationF weightInitF

    let prediction
            (network: NeuralNetwork,
             inputValues: List<float>)
            : List<float> =
            let input = LazyList.ofList inputValues
            Network.output input network
            |> LazyList.toList

    let errors (network: NeuralNetwork,
                learningRate: float,
                inputValues: List<float>,
                expectedOutput: List<float>)
               : List<float> =
               let input = LazyList.ofList inputValues
               let expOutput = LazyList.ofList expectedOutput
               let ers = Network.errors
                                learningRate
                                input
                                expOutput
                                network
               LazyList.toList ers

    let fit (network: NeuralNetwork,
             learningRate: float,
             inputValues: List<float>,
             expectedOutput: List<float>)
            : NeuralNetwork =
            let input = LazyList.ofList inputValues
            let expOutput = LazyList.ofList expectedOutput
            Network.fitted learningRate input expOutput network

    let toJson (network: NeuralNetwork): string =
            Network.toJson network

    let ofJson (json: string): NeuralNetwork =
            Network.fromJson json
    
    let toSvg (network: NeuralNetwork): string =
            Draw.networkSVG network

type DataPreprocessor = Preprocessor.Preprocessor

module DataPreprocessor =

    let init (keysWithDiscreteFlags: List<string * bool>,
              datapoints: seq<Map<string, string>>)
             : DataPreprocessor =
             let keysWithFlags =
                        LazyList.ofList keysWithDiscreteFlags
             let dataset = LazyList.ofSeq datapoints
             Preprocessor.init keysWithFlags dataset

    let encodedDatapoint
             (dataPreprocessor: DataPreprocessor,
              datapoint: Map<string, string>)
             : List<float> =
             Preprocessor.encode dataPreprocessor datapoint
             |> LazyList.toList

    let decodedDatapoint
             (dataPreprocessor: DataPreprocessor,
              encodedValues: List<float>)
             : Map<string, string> =
             encodedValues
             |> LazyList.ofList
             |> Preprocessor.decode dataPreprocessor

    let toJson (dataPreprocessor: DataPreprocessor)
               : string =
               Preprocessor.toJson dataPreprocessor

    let ofJson (json: string)
                 : DataPreprocessor =
                 Preprocessor.fromJson json

module Statistics =

    let rootMeanSquareError
                (expectedWithOutputValues: seq<List<float> * List<float>>)
                : float =
                expectedWithOutputValues
                |> LazyList.ofSeq
                |> LazyList.map (fun (yHat, y) -> (LazyList.ofSeq yHat, LazyList.ofSeq y))
                |> Mathematics.rootMeanSquareError
