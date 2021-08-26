module CustomizedNetworkTest

open Xunit
open FsUnit.Xunit
open Synapses

type ``customized network tests``() =

    let layers = [ 4; 6; 8; 5; 3 ]

    let activationF layerIndex =
        match layerIndex with
        | 0 -> ActivationFunction.sigmoid
        | 1 -> ActivationFunction.identity
        | 2 -> ActivationFunction.leakyReLU
        | _ -> ActivationFunction.tanh

    let rnd = System.Random(1000)

    let weightInitF _layerIndex =
        1.0 - 2.0 * rnd.NextDouble()

    let justCreatedNeuralNetwork =
        NeuralNetwork.customizedInit (layers, activationF, weightInitF)

    let justCreatedNeuralNetworkJson =
        NeuralNetwork.toJson (justCreatedNeuralNetwork)

    [<Fact>]
    let ``just created neural network of/to json``() =
       NeuralNetwork.ofJson justCreatedNeuralNetworkJson
        |> NeuralNetwork.toJson
        |> should equal justCreatedNeuralNetworkJson

    let neuralNetwork =
        "../../../../../resources/network.json"
        |> System.IO.File.ReadAllText
        |> NeuralNetwork.ofJson
    
    let neuralNetworkSvg =
        System.IO.File.ReadAllText "../../../../../resources/drawing.svg"

    let inputValues = [ 1.0; 0.5625; 0.511111; 0.47619 ]

    let expectedOutput = [ 0.4; 0.05; 0.2 ]

    let prediction =
        NeuralNetwork.prediction (neuralNetwork, inputValues)

    [<Fact>]
    let ``neural network prediction``() =
        prediction
        |> should equal
            [ -0.013959435951885571
              -0.16770539176070537
              0.6127887629040738 ]

    let learningRate = 0.01

    [<Fact>]
    let ``neural network normal errors``() =
        NeuralNetwork.errors (
                                neuralNetwork,
                                learningRate,
                                inputValues,
                                expectedOutput
                            )
        |> should equal
            [ -0.18229373795952453
              -0.10254022760223255
              -0.09317233470223055
              -0.086806455078946 ]

    [<Fact>]
    let ``neural network zero errors``() =
        NeuralNetwork.errors (
                                neuralNetwork,
                                learningRate,
                                inputValues,
                                prediction
                            )
        |> should equal
            [ 0.0; 0.0; 0.0; 0.0 ]

    let fitNeuralNetwork =
        NeuralNetwork.fit (
                          neuralNetwork,
                          learningRate,
                          inputValues,
                          expectedOutput
                          )

    [<Fact>]
    let ``fit neural network prediction``() =
        NeuralNetwork.prediction(fitNeuralNetwork, inputValues)
        |> should equal
            [ -0.0061094645547436445; -0.1770428172237149; 0.6087944183600162 ]
    
    [<Fact>]
    let ``neural network to svg``() =
        NeuralNetwork.toSvg neuralNetwork
        |> should equal neuralNetworkSvg
