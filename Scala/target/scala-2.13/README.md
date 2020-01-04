# Synapses

Lightweight cross-platform Neural Network library

## Installation

* JavaScript

Run `npm i synapses` in the directory of your node project.

* Scala

Add this line to the list of **sbt** `libraryDependencies`.
```scala
"synapses" % "scala_2.13" % "0.3" from "https://github.com/mrdimosthenis/Synapses/releases/download/0.3/synapses-assembly-0.3.jar"
```

* F#

Run `dotnet add package Synapses --version 7.0.2` in the directory of your project.

## Usage

### Create a neural network

Import `Synapses`, call `NeuralNetwork.init` and provide the size of each _layer_:

* JavaScript
```javascript
require('synapses');
let layers = [4, 6, 5, 3];
let neuralNetwork = NeuralNetwork.init(layers);
```

* Scala
```scala
import synapses.Library._
val layers = List(4, 6, 5, 3)
val neuralNetwork = NeuralNetwork.init(layers)
```

* F#
```fsharp
open Synapses
let layers = [4; 6; 5; 3]
let neuralNetwork = NeuralNetwork.init(layers)
```

`neuralNetwork` has 4 layers. The first layer has 4 input nodes and the last layer has 3 output nodes.
There are 2 hidden layers with 6 and 5 neurons respectively.

### Get a prediction

* JavaScript
```javascript
let inputValues = [1.0, 0.5625, 0.511111, 0.47619];
let prediction = NeuralNetwork.prediction(neuralNetwork, inputValues);
```

* Scala
```scala
val inputValues = List(1.0, 0.5625, 0.511111, 0.47619)
val prediction = NeuralNetwork.prediction(neuralNetwork, inputValues)
```

* F#
```fsharp
let inputValues = [1.0; 0.5625; 0.511111; 0.47619]
let prediction = NeuralNetwork.prediction(neuralNetwork, inputValues)
```

`prediction` should be something like `[ 0.829634, 0.699651, 0.454185 ]`.
Note that the lengths of `inputValues` and `prediction` equal to the sizes of _input_ and _output_ layers respectively.

### Fit network

* JavaScript
```javascript
let learningRate = 0.5;
let expectedOutput = [0.0, 1.0, 0.0];
let fitNetwork = NeuralNetwork.fit(neuralNetwork, learningRate, inputValues, expectedOutput);
```

* Scala
```scala
val learningRate = 0.5
val expectedOutput = List(0.0, 1.0, 0.0)
val fitNetwork = NeuralNetwork.fit(neuralNetwork, learningRate, inputValues, expectedOutput)
```

* F#
```fsharp
let learningRate = 0.5
let expectedOutput = [0.0; 1.0; 0.0]
let fitNetwork = NeuralNetwork.fit(neuralNetwork, learningRate, inputValues, expectedOutput)
```

`fitNetwork` is a new neural network trained with a single observation.

### Save and load a neural network

* JavaScript
```javascript
let json = NeuralNetwork.toJson(fitNetwork);
```

* Scala
```scala
val json = NeuralNetwork.toJson(fitNetwork)
```

* F#
```fsharp
let json = NeuralNetwork.toJson(fitNetwork)
```

Call `NeuralNetwork.toJson` on a neural network and get a string representation of it.
Use it as you like. Save `json` in the file system or insert into a database table.

JSON instances are **compatible** across platforms!
We can generate, train and save a neural network in Scala
and then load and make predictions in Javascript!

* JavaScript
```javascript
let loadedNetwork = NeuralNetwork.ofJson(json);
```

* Scala
```scala
val loadedNetwork = NeuralNetwork.ofJson(json)
```

* F#
```fsharp
let loadedNetwork = NeuralNetwork.ofJson(json)
```

As the name suggests, `NeuralNetwork.ofJson` turns a json string into a neural network.

### Customize a neural network

The _activation function_ of the neurons created with `NeuralNetwork.init`, is a sigmoid one.
If you want to customize the _activation functions_ and the _weight distribution_, call `NeuralNetwork.customizedInit`.

* JavaScript
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

* Scala
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

* F#
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

### Encoding and decoding

_One hot encoding_ is a process that turns discrete attributes into a list of _0.0_ and _1.0_.
_Minmax normalization_ scales continuous attributes into values between _0.0_ and _1.0_.
You can use `DataPreprocessor` for datapoint encoding and decoding.

The first parameter of `DataPreprocessor.init` is a list of tuples _(attributeName, discreteOrNot)_.

* Javascript
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

* Scala
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

* F#
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

`encodedDatapoints` equals to
```javascript
[ [ 0.0     , 1.0     , 0.0     , 0.0     , 0.0; 0.0; 1.0 ],
  [ 1.0     , 0.562500, 0.511111, 0.476190, 0.0; 1.0; 0.0 ],
  [ 0.166667, 0.0     , 1.0     , 1.0     , 1.0; 0.0; 0.0 ] ]
```

Save and load the preprocessor by calling `DataPreprocessor.toJson` and `DataPreprocessor.ofJson`.
