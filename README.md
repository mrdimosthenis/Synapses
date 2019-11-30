# Synapses
Neural network library in F#

## Installation
Run `dotnet add package Synapses --version 4.0.0` in the directory of your project.

## Usage

### Create a neural network
Open `Synapses`, call `NeuralNetwork.init` and provide the size of each _layer_:
```
open Synapses

let layers = [4; 6; 5; 3]

let neuralNetwork = NeuralNetwork.init layers
```
`neuralNetwork` has 4 layers. The first layer has 4 input nodes and the last layer has 3 output nodes.
There are 2 hidden layers with 6 and 5 neurons respectively.

### Get a prediction
```
let inputValues =
        [ 1.0; 0.5625; 0.511111; 0.47619 ]

let prediction =
        NeuralNetwork.prediction
                inputValues
                neuralNetwork
```
`prediction` should be something like `[ 0.829634; 0.699651; 0.454185 ]`.
Note that the lengths of `inputValues` and `prediction` equal to the sizes of _input_ and _output_ layers respectively.

### Fit network
```
let learningRate = 0.5

let expectedOutput =
        [ 0.0; 1.0; 0.0 ]

let fitNetwork =
        NeuralNetwork.fit
             learningRate
             inputValues
             expectedOutput
             neuralNetwork
```
`fitNetwork` is a new neural network trained with a single observation.

### Save and load a neural network
```
let json = NeuralNetwork.toJson
                fitNetwork
```
Call `NeuralNetwork.toJson` on a neural network and get a string representation of it.
Use it as you like. Save `json` in the file system or insert into a database table.

```
let loadedNetwork =
        NeuralNetwork.fromJson
              json
```
As the name suggests, `NeuralNetwork.fromJson` turns a json string into a neural network.

### Customize a neural network
```
let activationF (layerIndex: int)
        : ActivationFunction =
        match layerIndex with
        | 0 -> ActivationFunction.sigmoid
        | 1 -> ActivationFunction.tanh
        | 2 -> ActivationFunction.leakyReLU
        | _ -> ActivationFunction.identity

let weightInitF (_layerIndex: int)
        : float =
        System.Random().NextDouble()

let customizedNetwork =
        NeuralNetwork.customizedInit
                layers
                activationF
                weightInitF
```
The _activation function_ of the neurons created with `NeuralNetwork.init`, is a sigmoid one.
If you want to customize the _activation functions_ and the _weight distribution_, call `NeuralNetwork.customizedInit`.

### Encoding and decoding
_One hot encoding_ is a process that turns discrete attributes into a list of _0.0_ and _1.0_.
_Minmax normalization_ scales continuous attributes into values between _0.0_ and _1.0_.
You can use `DataPreprocessor` for datapoint encoding and decoding.

The first parameter of `DataPreprocessor.init` is a list of tuples _(attributeName, discreteOrNot)_.
```
let setosaDatapoint =
        Map.ofList
            [ ("petal_length", "1.5")
              ("petal_width", "0.1")
              ("sepal_length", "4.9")
              ("sepal_width", "3.1")
              ("species", "setosa") ]

let versicolorDatapoint =
        Map.ofList
            [ ("petal_length", "3.8")
              ("petal_width", "1.1")
              ("sepal_length", "5.5")
              ("sepal_width", "2.4")
              ("species", "versicolor") ]

let virginicaDatapoint =
        Map.ofList
            [ ("petal_length", "6.0")
              ("petal_width", "2.2")
              ("sepal_length", "5.0")
              ("sepal_width", "1.5")
              ("species", "virginica") ]

let dataset = Seq.ofList
                [ setosaDatapoint
                  versicolorDatapoint
                  virginicaDatapoint ]
                
let dataPreprocessor = DataPreprocessor.init
                            [ ("sepal_length", false)
                              ("sepal_width", false)
                              ("petal_length", false)
                              ("petal_width", false)
                              ("species", true) ]
                            dataset

let encodedDatapoints =
        Seq.map (DataPreprocessor.encodedDatapoint dataPreprocessor)
                dataset
```

`encodedDatapoints` equals to
```
[ [ 0.0     ; 1.0     ; 0.0     ; 0.0     ; 0.0; 0.0; 1.0 ]
  [ 1.0     ; 0.562500; 0.511111; 0.476190; 0.0; 1.0; 0.0 ]
  [ 0.166667; 0.0     ; 1.0     ; 1.0     ; 1.0; 0.0; 0.0 ] ]
```

Save and load the preprocessor by calling `DataPreprocessor.toJson` and `DataPreprocessor.fromJson`.
