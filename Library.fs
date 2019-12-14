namespace Synapses

open FSharpx.Collections
open Synapses.Model
open Synapses.Model.NetElems
open Synapses.Model.Encoding

type ActivationFunction = Activation.Function

type NeuralNetwork = Network.Network

type DataPreprocessor = Preprocessor.Preprocessor

module ActivationFunction =

    let sigmoid: ActivationFunction =
            Activation.sigmoid

    let identity: ActivationFunction =
            Activation.identity

    let tanh: ActivationFunction =
            Activation.tanh

    let leakyReLU: ActivationFunction =
            Activation.leakyReLU

module NeuralNetwork =
    
    let initWithSeed
              (seed: Option<int>)
              (layers: List<int>)
             : NeuralNetwork =
             let layerSizes = LazyList.ofList layers
             let activationF =
                     fun _ -> Activation.sigmoid
             let rnd = match seed with
                       | Some i -> System.Random(i)
                       | None -> System.Random()
             let weightInitF =
                     fun _ -> rnd.NextDouble()
                              |> (*) -2.0
                              |> (+) 1.0
             Network.init layerSizes activationF weightInitF

    let init (layers: List<int>)
             : NeuralNetwork =
             initWithSeed None layers

    let customizedInit
            (layers: List<int>)
            (activationF: int -> ActivationFunction)
            (weightInitF: int -> float)
            : NeuralNetwork =
            let layerSizes = LazyList.ofList layers
            Network.init layerSizes activationF weightInitF

    let prediction
            (network: NeuralNetwork)
            (inputValues: List<float>)
            : List<float> =
            let input = LazyList.ofList inputValues
            Network.output input network
            |> LazyList.toList

    let errors (learningRate: float)
               (inputValues: List<float>)
               (expectedOutput: List<float>)
               (network: NeuralNetwork)
               : List<float> =
               let input = LazyList.ofList inputValues
               let expOutput = LazyList.ofList expectedOutput
               let (ers, _) = Network.errorsWithFitted
                                learningRate
                                input
                                expOutput
                                network
               LazyList.toList ers

    let fit (learningRate: float)
            (inputValues: List<float>)
            (expectedOutput: List<float>)
            (network: NeuralNetwork)
            : NeuralNetwork =
            let input = LazyList.ofList inputValues
            let expOutput = LazyList.ofList expectedOutput
            Network.fitted learningRate input expOutput network

    let toJson (network: NeuralNetwork): string =
            Network.toJson network

    let fromJson (json: string): NeuralNetwork =
            Network.fromJson json

module DataPreprocessor =

    let init (keysWithDiscreteFlags: List<string * bool>)
             (datapoints: seq<Map<string, string>>)
             : DataPreprocessor =
             let keysWithFlags =
                        LazyList.ofList keysWithDiscreteFlags
             let dataset = LazyList.ofSeq datapoints
             Preprocessor.init keysWithFlags dataset

    let encodedDatapoint
             (dataPreprocessor: DataPreprocessor)
             (datapoint: Map<string, string>)
             : List<float> =
             Preprocessor.encode dataPreprocessor datapoint
             |> LazyList.toList

    let decodedDatapoint
             (dataPreprocessor: DataPreprocessor)
             (encodedValues: List<float>)
             : Map<string, string> =
             encodedValues
             |> LazyList.ofList
             |> Preprocessor.decode dataPreprocessor

    let toJson (dataPreprocessor: DataPreprocessor)
               : string =
               Preprocessor.toJson dataPreprocessor

    let fromJson (json: string)
                 : DataPreprocessor =
                 Preprocessor.fromJson json

module Statistics =

    let rootMeanSquareError
                (expectedValues: List<List<float>>)
                (outputValues: List<List<float>>)
                : float =
                let y_hats = expectedValues
                             |> List.map LazyList.ofList
                             |> LazyList.ofList
                let ys = outputValues
                         |> List.map LazyList.ofList
                         |> LazyList.ofList
                Mathematics.rootMeanSquareError y_hats ys
