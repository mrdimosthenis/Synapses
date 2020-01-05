---
title: Synapses

language_tabs: # must be one of https://git.io/vQNgJ
  - javascript
  - scala
  - fsharp

toc_footers:
  - <a href='https://github.com/mrdimosthenis/Synapses'>GitHub Repository</a>
  - <a href='https://github.com/lord/slate'>Documentation Powered by Slate</a>
  - <a href='https://pngimage.net/icono-cerebro-png-5'> Logo Attribution </a>

search: true
---

# Synapses

**Synapses** is a lightweight **Neural Network** library, for **js**, **jvm** and **.net**.

```javascript
// run
npm i synapses@7.1.0
// in the directory of your node project
```

```scala
// add
libraryDependencies +=
  "synapses" % "scala_2.13" % "7.1.0" from
    "https://github.com/mrdimosthenis/Synapses/releases/download/7.1.0/synapses-assembly-7.1.0.jar"
// to build.sbt
```

```fsharp
// run
dotnet add package Synapses --version 7.1.0
// in the directory of your project
```

## Neural Network

### Create a neural network

Import `Synapses`, call `NeuralNetwork.init` and provide the size of each _layer_.

```javascript
require('synapses');
let layers = [4, 6, 5, 3];
let neuralNetwork = NeuralNetwork.init(layers);
```

```scala
import synapses.Library._
val layers = List(4, 6, 5, 3)
val neuralNetwork = NeuralNetwork.init(layers)
```

```fsharp
open Synapses
let layers = [4; 6; 5; 3]
let neuralNetwork = NeuralNetwork.init(layers)
```

`neuralNetwork` has 4 layers. The first layer has 4 input nodes and the last layer has 3 output nodes.
There are 2 hidden layers with 6 and 5 neurons respectively.

### Get a prediction

```javascript
let inputValues = [1.0, 0.5625, 0.511111, 0.47619];
let prediction = NeuralNetwork.prediction(neuralNetwork, inputValues);
```

```scala
val inputValues = List(1.0, 0.5625, 0.511111, 0.47619)
val prediction = NeuralNetwork.prediction(neuralNetwork, inputValues)
```

```fsharp
let inputValues = [1.0; 0.5625; 0.511111; 0.47619]
let prediction = NeuralNetwork.prediction(neuralNetwork, inputValues)
```

Note that the lengths of `inputValues` and `prediction` equal to the sizes of _input_ and _output_ layers respectively.

### Fit network

```javascript
let learningRate = 0.5;
let expectedOutput = [0.0, 1.0, 0.0];
let fitNetwork = NeuralNetwork.fit(neuralNetwork, learningRate, inputValues, expectedOutput);
```

```scala
val learningRate = 0.5
val expectedOutput = List(0.0, 1.0, 0.0)
val fitNetwork = NeuralNetwork.fit(neuralNetwork, learningRate, inputValues, expectedOutput)
```

```fsharp
let learningRate = 0.5
let expectedOutput = [0.0; 1.0; 0.0]
let fitNetwork = NeuralNetwork.fit(neuralNetwork, learningRate, inputValues, expectedOutput)
```

`fitNetwork` is a new neural network trained with a single observation.

### Create a customized neural network

The _activation function_ of the neurons created with `NeuralNetwork.init`, is a sigmoid one.
If you want to customize the _activation functions_ and the _weight distribution_, call `NeuralNetwork.customizedInit`.

```javascript
function activationF(layerIndex) {
    switch (layerIndex) {
        case 0:
            return ActivationFunction.sigmoid;
        case 1:
            return ActivationFunction.identity;
        case 2:
            return ActivationFunction.leakyReLU;
        case 3:
            return ActivationFunction.tanh;
    }
}

function weightInitF(_layerIndex) {
    return 1.0 - 2.0 * Math.random();
}

let customizedNetwork = NeuralNetwork.customizedInit(layers, activationF, weightInitF);
```

```scala
def activationF(layerIndex: Int): ActivationFunction =
  layerIndex match {
    case 0 => ActivationFunction.sigmoid
    case 1 => ActivationFunction.identity
    case 2 => ActivationFunction.leakyReLU
    case 3 => ActivationFunction.tanh
  }

def weightInitF(_layerIndex: Int): Double = 1.0 - 2.0 * new Random().nextDouble()

val customizedNetwork = NeuralNetwork.customizedInit(layers, activationF, weightInitF)
```

```fsharp
let activationF (layerIndex: int)
        : ActivationFunction =
        match layerIndex with
        | 0 -> ActivationFunction.sigmoid
        | 1 -> ActivationFunction.tanh
        | 2 -> ActivationFunction.leakyReLU
        | _ -> ActivationFunction.identity

let weightInitF (_layerIndex: int): float = 1.0 - 2.0 * System.Random().NextDouble()

let customizedNetwork = NeuralNetwork.customizedInit(layers, activationF, weightInitF)
```

## Save and load a neural network

JSON instances are **compatible** across platforms!
We can generate, train and save a neural network in Scala
and then load and make predictions in Javascript!

### toJson

Call `NeuralNetwork.toJson` on a neural network and get a string representation of it.
Use it as you like. Save `json` in the file system or insert into a database table.

```javascript
let json = NeuralNetwork.toJson(fitNetwork);
```

```scala
val json = NeuralNetwork.toJson(fitNetwork)
```

```fsharp
let json = NeuralNetwork.toJson(fitNetwork)
```

### ofJson

```javascript
let loadedNetwork = NeuralNetwork.ofJson(json);
```

```scala
val loadedNetwork = NeuralNetwork.ofJson(json)
```

```fsharp
let loadedNetwork = NeuralNetwork.ofJson(json)
```

As the name suggests, `NeuralNetwork.ofJson` turns a json string into a neural network.

## Encoding and decoding

_One hot encoding_ is a process that turns discrete attributes into a list of _0.0_ and _1.0_.
_Minmax normalization_ scales continuous attributes into values between _0.0_ and _1.0_.
You can use `DataPreprocessor` for datapoint encoding and decoding.

The first parameter of `DataPreprocessor.init` is a list of tuples _(attributeName, discreteOrNot)_.

```javascript
let setosaDatapoint = {
    petal_length: "1.5",
    petal_width: "0.1",
    sepal_length: "4.9",
    sepal_width: "3.1",
    species: "setosa"
};

let versicolorDatapoint = {
    petal_length: "3.8",
    petal_width: "1.1",
    sepal_length: "5.5",
    sepal_width: "2.4",
    species: "versicolor"
};

let virginicaDatapoint = {
    petal_length: "6.0",
    petal_width: "2.2",
    sepal_length: "5.0",
    sepal_width: "1.5",
    species: "virginica"
};

let datasetArr = [setosaDatapoint, versicolorDatapoint, virginicaDatapoint];

let datasetIter = datasetArr[Symbol.iterator]();
                
let dataPreprocessor = DataPreprocessor.init(
                            [ ["sepal_length", false],
                              ["sepal_width", false],
                              ["petal_length", false],
                              ["petal_width", false],
                              ["species", true] ],
                            datasetIter
                       );

let encodedDatapoints = datasetArr.map(x => DataPreprocessor.encodedDatapoint(dataPreprocessor, x));
```

```scala
val setosaDatapoint = Map(
  "petal_length" -> "1.5",
  "petal_width" -> "0.1",
  "sepal_length" -> "4.9",
  "sepal_width" -> "3.1",
  "species" -> "setosa"
)

val versicolorDatapoint = Map(
  "petal_length" -> "3.8",
  "petal_width" -> "1.1",
  "sepal_length" -> "5.5",
  "sepal_width" -> "2.4",
  "species" -> "versicolor"
)

val virginicaDatapoint = Map(
  "petal_length" -> "6.0",
  "petal_width" -> "2.2",
  "sepal_length" -> "5.0",
  "sepal_width" -> "1.5",
  "species" -> "virginica"
)

val dataset = LazyList(setosaDatapoint, versicolorDatapoint, virginicaDatapoint)

val dataPreprocessor = DataPreprocessor.init(
                            List( ("sepal_length", false),
                                  ("sepal_width", false),
                                  ("petal_length", false),
                                  ("petal_width", false),
                                  ("species", true) ),
                            dataset
                       )

val encodedDatapoints = dataset.map(x => DataPreprocessor.encodedDatapoint(dataPreprocessor, x))
```

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
                
let dataPreprocessor = DataPreprocessor.init(
                            [ ("sepal_length", false)
                              ("sepal_width", false)
                              ("petal_length", false)
                              ("petal_width", false)
                              ("species", true) ],
                            dataset
                       )

let encodedDatapoints =
        Seq.map (fun datapoint -> DataPreprocessor.encodedDatapoint(dataPreprocessor, datapoint))
                dataset
```

Save and load the preprocessor by calling `DataPreprocessor.toJson` and `DataPreprocessor.ofJson`.

## Evaluation

To evaluate a neural network, you can call `Statistics.rootMeanSquareError` and provide the expected and predicted values.

```javascript
let expectedWithOutputValuesArr =
        [ [ [ 0.0, 0.0, 1.0], [ 0.0, 0.0, 1.0] ],
          [ [ 0.0, 0.0, 1.0], [ 0.0, 1.0, 1.0] ] ];

let expectedWithOutputValuesIter = expectedWithOutputValuesArr[Symbol.iterator]();

let rmse = Statistics.rootMeanSquareError(expectedWithOutputValuesIter);
```

```scala
val expectedWithOutputValues =
    LazyList(
      (List(0.0, 0.0, 1.0), List(0.0, 0.0, 1.0)),
      (List(0.0, 0.0, 1.0), List(0.0, 1.0, 1.0))
    )

val rmse = Statistics.rootMeanSquareError(expectedWithOutputValues)
```

```fsharp
let expectedWithOutputValues =
        Seq.ofList [ ( [ 0.0; 0.0; 1.0], [ 0.0; 0.0; 1.0] )
                     ( [ 0.0; 0.0; 1.0], [ 0.0; 1.0; 1.0] ) ]

let rmse = Statistics.rootMeanSquareError expectedWithOutputValues
```
