# Synapses

A **neural networks** library for **FSharp**!

```fsharp
// run
dotnet add package Synapses --version 7.4.1
// in the directory of your project
```

## Neural Network

### Create a neural network

Import `Synapses`, call `NeuralNetwork.init` and provide the size of each _layer_.

```fsharp
open Synapses
let layers = [4; 6; 5; 3]
let neuralNetwork = NeuralNetwork.init(layers)
```

`neuralNetwork` has 4 layers. The first layer has 4 input nodes and the last layer has 3 output nodes.
There are 2 hidden layers with 6 and 5 neurons respectively.

### Get a prediction

```fsharp
let inputValues = [1.0; 0.5625; 0.511111; 0.47619]
let prediction =
        NeuralNetwork.prediction(neuralNetwork, inputValues)
```

`prediction` should be something like `[ 0.8296, 0.6996, 0.4541 ]`.

Note that the lengths of inputValues and prediction equal to the sizes of input and output layers respectively.

### Fit network

```fsharp
let learningRate = 0.5
let expectedOutput = [0.0; 1.0; 0.0]
let fitNetwork =
        NeuralNetwork.fit(
            neuralNetwork,
            learningRate,
            inputValues,
            expectedOutput
        )
```

`fitNetwork` is a new neural network trained with a single observation.

> to train a neural network, you should fit with multiple datapoints

### Create a customized neural network

The _activation function_ of the neurons created with `NeuralNetwork.init`, is a sigmoid one.
If you want to customize the _activation functions_ and the _weight distribution_, call `NeuralNetwork.customizedInit`.

```fsharp
let activationF (layerIndex: int)
        : ActivationFunction =
        match layerIndex with
        | 0 -> ActivationFunction.sigmoid
        | 1 -> ActivationFunction.tanh
        | 2 -> ActivationFunction.leakyReLU
        | _ -> ActivationFunction.identity

let weightInitF (_layerIndex: int): float =
        1.0 - 2.0 * System.Random().NextDouble()

let customizedNetwork =
        NeuralNetwork.customizedInit(
            layers,
            activationF,
            weightInitF
        )
```

## Visualization

Call `NeuralNetwork.toSvg` to take a brief look at its _svg drawing_.

![Network Drawing](https://github.com/mrdimosthenis/FSharp.Synapses/blob/master/network-drawing.png?raw=true)

The color of each neuron depends on its _activation function_
while the transparency of the synapses depends on their _weight_.

```fsharp
let svg = NeuralNetwork.toSvg(customizedNetwork)
```

## Save and load a neural network

JSON instances are **compatible** across platforms!
We can generate, train and save a neural network in Python
and then load and make predictions in Javascript!

### toJson

Call `NeuralNetwork.toJson` on a neural network and get a string representation of it.
Use it as you like. Save `json` in the file system or insert into a database table.

```fsharp
let loadedNetwork = NeuralNetwork.ofJson(json)
```

As the name suggests, `NeuralNetwork.ofJson` turns a json string into a neural network.

## Encoding and decoding

_One hot encoding_ is a process that turns discrete attributes into a list of _0.0_ and _1.0_.
_Minmax normalization_ scales continuous attributes into values between _0.0_ and _1.0_.
You can use `DataPreprocessor` for datapoint encoding and decoding.

The first parameter of `DataPreprocessor.init` is a list of tuples _(attributeName, discreteOrNot)_.

```fsharp
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
                
let dataPreprocessor =
        DataPreprocessor.init(
             [ ("petal_length", false)
               ("petal_width", false)
               ("sepal_length", false)
               ("sepal_width", false)
               ("species", true) ],
             dataset
        )

let encodedDatapoints =
        Seq.map (fun datapoint ->
                    DataPreprocessor.encodedDatapoint(dataPreprocessor, datapoint)
                )
                dataset
```

> `encodedDatapoints` equals to

```json
[ [ 0.0     , 0.0     , 0.0     , 1.0     , 0.0, 0.0, 1.0 ],
  [ 0.511111, 0.476190, 1.0     , 0.562500, 0.0, 1.0, 0.0 ],
  [ 1.0     , 1.0     , 0.166667, 0.0     , 1.0, 0.0, 0.0 ] ]
```

Save and load the preprocessor by calling `DataPreprocessor.toJson` and `DataPreprocessor.ofJson`.

## Evaluation

To evaluate a neural network, you can call `Statistics.rootMeanSquareError` and provide the expected and predicted values.

```fsharp
let expectedWithOutputValues =
        Seq.ofList [ ( [ 0.0; 0.0; 1.0], [ 0.0; 0.0; 1.0] )
                     ( [ 0.0; 0.0; 1.0], [ 0.0; 1.0; 1.0] ) ]

let rmse = Statistics.rootMeanSquareError(
                        expectedWithOutputValues
)
```
