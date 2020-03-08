---
title: Synapses

language_tabs: # must be one of https://git.io/vQNgJ
  - javascript
  - python
  - java
  - csharp
  - scala
  - fsharp

toc_footers:
  - <a href='https://github.com/mrdimosthenis/Synapses'>GitHub Repository</a>
  - <a href='https://github.com/lord/slate'>Documentation Powered by Slate</a>
  - <a href='https://pngimage.net/icono-cerebro-png-5'> Logo Attribution </a>

search: true
---

# Synapses

**Synapses** is a **lightweight** library for **neural networks** that **runs anywhere**.

```javascript
// run
npm i synapses@7.2.1
// in the directory of your node project
```

```python
# run
pip install synapses-py==7.2.1
# in the directory of your project
```

```java
// add
<dependency>
    <groupId>com.github.mrdimosthenis</groupId>
    <artifactId>synapses_2.13</artifactId>
    <version>7.2.1</version>
</dependency>
// to pom.xml
```

```csharp
// run
dotnet add package SynapsesCSharp --version 7.2.1
// in the directory of your project
```

```scala
// add
libraryDependencies +=
  "com.github.mrdimosthenis" %% "synapses" % "7.2.1"
// to build.sbt
```

```fsharp
// run
dotnet add package Synapses --version 7.2.1
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

```python
from synapses_py import *
layers = [4, 6, 5, 3]
neuralNetwork = NeuralNetwork.init(layers)
```

```java
import synapses.jvm.library.*;
int[] layers = {4, 6, 5, 3};
NeuralNetwork neuralNetwork = NeuralNetwork.init(layers);
```

```csharp
using SynapsesCSharp;
int[] layers = {4, 6, 5, 3};
NeuralNetwork neuralNetwork = NeuralNetwork.init(layers);
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
let prediction =
        NeuralNetwork.prediction(neuralNetwork, inputValues);
```

```python
inputValues = [1.0, 0.5625, 0.511111, 0.47619]
let prediction = \
        NeuralNetwork.prediction(neuralNetwork, inputValues)
```

```java
double[] inputValues = {1.0, 0.5625, 0.511111, 0.47619};
double[] prediction =
    NeuralNetwork.prediction(neuralNetwork, inputValues);
```

```csharp
double[] inputValues = {1.0, 0.5625, 0.511111, 0.47619};
double[] prediction =
    NeuralNetwork.prediction(neuralNetwork, inputValues);
```

```scala
val inputValues = List(1.0, 0.5625, 0.511111, 0.47619)
val prediction =
        NeuralNetwork.prediction(neuralNetwork, inputValues)
```

```fsharp
let inputValues = [1.0; 0.5625; 0.511111; 0.47619]
let prediction =
        NeuralNetwork.prediction(neuralNetwork, inputValues)
```

`prediction` should be something like `[ 0.8296, 0.6996, 0.4541 ]`.

<aside class="success">
Note that the lengths of inputValues and prediction equal to the sizes of input and output layers respectively.
</aside>

### Fit network

```javascript
let learningRate = 0.5;
let expectedOutput = [0.0, 1.0, 0.0];
let fitNetwork =
        NeuralNetwork.fit(
            neuralNetwork,
            learningRate,
            inputValues,
            expectedOutput
        );
```

```python
learningRate = 0.5
expectedOutput = [0.0, 1.0, 0.0]
fitNetwork = \
        NeuralNetwork.fit(
            neuralNetwork,
            learningRate,
            inputValues,
            expectedOutput
        )
```

```java
double learningRate = 0.5;
double[] expectedOutput = {0.0, 1.0, 0.0};
NeuralNetwork fitNetwork =
    NeuralNetwork.fit(
        neuralNetwork,
        learningRate,
        inputValues,
        expectedOutput
    );
```

```csharp
double learningRate = 0.5;
double[] expectedOutput = {0.0, 1.0, 0.0};
NeuralNetwork fitNetwork =
    NeuralNetwork.fit(
        neuralNetwork,
        learningRate,
        inputValues,
        expectedOutput
    );
```

```scala
val learningRate = 0.5
val expectedOutput = List(0.0, 1.0, 0.0)
val fitNetwork =
        NeuralNetwork.fit(
            neuralNetwork,
            learningRate,
            inputValues,
            expectedOutput
        )
```

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

let customizedNetwork =
        NeuralNetwork.customizedInit(
            layers,
            activationF,
            weightInitF
        );
```

```python
def activationF(layerIndex):
    if layerIndex == 0:
        return ActivationFunction.sigmoid
    elif layerIndex == 1:
        return ActivationFunction.identity
    elif layerIndex == 2:
        return ActivationFunction.leakyReLU
    else:
        return ActivationFunction.tanh

def weightInitF(_layerIndex):
    return 1.0 - 2.0 * random()

customizedNetwork = \
        NeuralNetwork.customizedInit(
            layers,
            activationF,
            weightInitF
        )
```

```java
ActivationFunction activationF(int layerIndex) {
    switch (layerIndex) {
        case 0:
            return ActivationFunction.sigmoid();
        case 1:
            return ActivationFunction.identity();
        case 2:
            return ActivationFunction.leakyReLU();
        default:
            return ActivationFunction.tanh();
    }
}

double weightInitF(int _layerIndex) {
    Random rnd = new Random();
    return 1.0 - 2.0 * rnd.nextDouble();
}

NeuralNetwork customizedNetwork =
    NeuralNetwork.customizedInit(
        layers,
        this::activationF,
        this::weightInitF
    );
```

```csharp
ActivationFunction activationF(int layerIndex) {
    switch (layerIndex) {
        case 0:
            return ActivationFunction.sigmoid;
        case 1:
            return ActivationFunction.identity;
        case 2:
            return ActivationFunction.leakyReLU;
        default:
            return ActivationFunction.tanh;
    }
}

double weightInitF(int _layerIndex) {
    Random rnd = new Random();
    return 1.0 - 2.0 * rnd.NextDouble();
}

NeuralNetwork customizedNetwork =
    NeuralNetwork.customizedInit(
        layers,
        activationF,
        weightInitF
    );
```

```scala
def activationF(layerIndex: Int): ActivationFunction =
  layerIndex match {
    case 0 => ActivationFunction.sigmoid
    case 1 => ActivationFunction.identity
    case 2 => ActivationFunction.leakyReLU
    case 3 => ActivationFunction.tanh
  }

def weightInitF(_layerIndex: Int): Double =
        1.0 - 2.0 * new Random().nextDouble()

val customizedNetwork =
        NeuralNetwork.customizedInit(
            layers,
            activationF,
            weightInitF
        )
```

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

## Visualize a neural network

Call `NeuralNetwork.toSvg` to take a brief look at its _svg drawing_.

![Network Drawing](https://github.com/mrdimosthenis/Synapses/blob/master/network-drawing.png?raw=true)

The color of each neuron depends on its _activation function_
while the transparency of the synapses depends on their _weight_.

```javascript
let svg = NeuralNetwork.toSvg(customizedNetwork);
```

```python
svg = NeuralNetwork.toSvg(customizedNetwork)
```

```java
String svg = NeuralNetwork.toSvg(customizedNetwork);
```

```csharp
string svg = NeuralNetwork.toSvg(customizedNetwork);
```

```scala
val svg = NeuralNetwork.toSvg(customizedNetwork)
```

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

```javascript
let json = NeuralNetwork.toJson(customizedNetwork);
```

```python
json = NeuralNetwork.toJson(customizedNetwork)
```

```java
String json = NeuralNetwork.toJson(customizedNetwork);
```

```csharp
string json = NeuralNetwork.toJson(customizedNetwork);
```

```scala
val json = NeuralNetwork.toJson(customizedNetwork)
```

```fsharp
let json = NeuralNetwork.toJson(customizedNetwork)
```

### ofJson

```javascript
let loadedNetwork = NeuralNetwork.ofJson(json);
```

```python
loadedNetwork = NeuralNetwork.ofJson(json)
```

```java
NeuralNetwork loadedNetwork = NeuralNetwork.ofJson(json);
```

```csharp
NeuralNetwork loadedNetwork = NeuralNetwork.ofJson(json);
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

let datasetArr = [ setosaDatapoint,
                   versicolorDatapoint,
                   virginicaDatapoint ];

let datasetIter = datasetArr[Symbol.iterator]();
                
let dataPreprocessor =
        DataPreprocessor.init(
             [ ["petal_length", false],
               ["petal_width", false],
               ["sepal_length", false],
               ["sepal_width", false],
               ["species", true] ],
             datasetIter
        );

let encodedDatapoints = datasetArr.map(x =>
        DataPreprocessor.encodedDatapoint(dataPreprocessor, x)
);
```

```python
setosaDatapoint = {
    "petal_length": "1.5",
    "petal_width": "0.1",
    "sepal_length": "4.9",
    "sepal_width": "3.1",
    "species": "setosa"
}

versicolorDatapoint = {
    "petal_length": "3.8",
    "petal_width": "1.1",
    "sepal_length": "5.5",
    "sepal_width": "2.4",
    "species": "versicolor"
}

virginicaDatapoint = {
    "petal_length": "6.0",
    "petal_width": "2.2",
    "sepal_length": "5.0",
    "sepal_width": "1.5",
    "species": "virginica"
}

datasetList = [ setosaDatapoint,
                versicolorDatapoint,
                virginicaDatapoint ]

dataPreprocessor = \
        DataPreprocessor.init(
             [ ("petal_length", False),
               ("petal_width", False),
               ("sepal_length", False),
               ("sepal_width", False),
               ("species", True) ],
             iter(datasetList)
        )

encodedDatapoints = map(lambda x:
        DataPreprocessor.encodedDatapoint(dataPreprocessor, x),
        datasetList
)
```

```java
Map<String, String> setosaDatapoint =
        new HashMap<String, String>() {
            {
                put("petal_length", "1.5");
                put("petal_width", "0.1");
                put("sepal_length", "4.9");
                put("sepal_width", "3.1");
                put("species", "setosa");
            }
        };

Map<String, String> versicolorDatapoint =
        new HashMap<String, String>() {
            {
                put("petal_length", "3.8");
                put("petal_width", "1.1");
                put("sepal_length", "5.5");
                put("sepal_width", "2.4");
                put("species", "versicolor");
            }
        };

Map<String, String> virginicaDatapoint =
        new HashMap<String, String>() {
            {
                put("petal_length", "6.0");
                put("petal_width", "2.2");
                put("sepal_length", "5.0");
                put("sepal_width", "1.5");
                put("species", "virginica");
            }
        };

Map[] datasetArr = {
        setosaDatapoint,
        versicolorDatapoint,
        virginicaDatapoint
};

Stream datasetStream =
        Arrays.stream(datasetArr);

DataPreprocessor dataPreprocessor =
        DataPreprocessor.init(
                new Object[][]{
                        {"petal_length", false},
                        {"petal_width", false},
                        {"sepal_length", false},
                        {"sepal_width", false},
                        {"species", true}
                },
                datasetStream
        );

Stream<double[]> encodedDatapoints = datasetStream.map(x ->
    DataPreprocessor
        .encodedDatapoint(
            dataPreprocessor, (Map<String, String>) x
        )
);
```

```csharp
Dictionary<string, string> setosaDatapoint =
    new Dictionary<string, string>()
        {
            {"petal_length", "1.5"},
            {"petal_length", "1.5"},
            {"petal_width", "0.1"},
            {"sepal_length", "4.9"},
            {"sepal_width", "3.1"},
            {"species", "setosa"}
        };

Dictionary<string, string> versicolorDatapoint =
    new Dictionary<string, string>()
        {
            {"petal_length", "3.8"},
            {"petal_width", "1.1"},
            {"sepal_length", "5.5"},
            {"sepal_width", "2.4"},
            {"species", "versicolor"}
        };

Dictionary<string, string> virginicaDatapoint =
    new Dictionary<string, string>()
        {
            {"petal_length", "6.0"},
            {"petal_width", "2.2"},
            {"sepal_length", "5.0"},
            {"sepal_width", "1.5"},
            {"species", "virginica"}
        };

IEnumerable<Dictionary<string, string>> dataset =
    new List<Dictionary<string, string>>()
    {
        setosaDatapoint,
        versicolorDatapoint,
        virginicaDatapoint
    };

DataPreprocessor dataPreprocessor =
    DataPreprocessor.init(
        new (string, bool)[]
            {
                ("petal_length", false),
                ("petal_width", false),
                ("sepal_length", false),
                ("sepal_width", false),
                ("species", true)
            },
        dataset
    );

IEnumerable<double[]> encodedDatapoints = dataset.Select(x =>
    DataPreprocessor
        .encodedDatapoint(dataPreprocessor, x)
);
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

val dataset = LazyList(
                        setosaDatapoint,
                        versicolorDatapoint,
                        virginicaDatapoint
                      )

val dataPreprocessor =
        DataPreprocessor.init(
             List( ("petal_length", false),
                   ("petal_width", false),
                   ("sepal_length", false),
                   ("sepal_width", false),
                   ("species", true) ),
             dataset
        )

val encodedDatapoints = dataset.map(x =>
        DataPreprocessor.encodedDatapoint(dataPreprocessor, x)
)
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

```javascript
let expectedWithOutputValuesArr =
        [ [ [ 0.0, 0.0, 1.0], [ 0.0, 0.0, 1.0] ],
          [ [ 0.0, 0.0, 1.0], [ 0.0, 1.0, 1.0] ] ];

let expectedWithOutputValuesIter =
        expectedWithOutputValuesArr[Symbol.iterator]();

let rmse = Statistics.rootMeanSquareError(
                        expectedWithOutputValuesIter
);
```

```python
expectedWithOutputValuesList = \
        [ ( [ 0.0, 0.0, 1.0], [ 0.0, 0.0, 1.0] ),
          ( [ 0.0, 0.0, 1.0], [ 0.0, 1.0, 1.0] ) ];

expectedWithOutputValuesIter = \
        iter(expectedWithOutputValuesList)

rmse = Statistics.rootMeanSquareError(
                        expectedWithOutputValuesIter
)
```

```java
double[][][] expectedWithOutputValuesArr = {
        {{0.0, 0.0, 1.0}, {0.0, 0.0, 1.0}},
        {{0.0, 0.0, 1.0}, {0.0, 1.0, 1.0}}
};

Stream<double[][]> expectedWithOutputValuesStream =
    Arrays.stream(expectedWithOutputValuesArr);

double rmse = Statistics
    .rootMeanSquareError(expectedWithOutputValuesStream);
```

```csharp
IEnumerable<(double[], double[])> expectedWithOutputValues =
    new List<(double[], double[])>()
    {
        (new double[] {0.0, 0.0, 1.0}, new double[] {0.0, 0.0, 1.0}),
        (new double[] {0.0, 0.0, 1.0}, new double[] {0.0, 1.0, 1.0})
    };

double rmse = Statistics
    .rootMeanSquareError(expectedWithOutputValues);
```

```scala
val expectedWithOutputValues =
    LazyList(
      (List(0.0, 0.0, 1.0), List(0.0, 0.0, 1.0)),
      (List(0.0, 0.0, 1.0), List(0.0, 1.0, 1.0))
    )

val rmse = Statistics.rootMeanSquareError(
                        expectedWithOutputValues
)
```

```fsharp
let expectedWithOutputValues =
        Seq.ofList [ ( [ 0.0; 0.0; 1.0], [ 0.0; 0.0; 1.0] )
                     ( [ 0.0; 0.0; 1.0], [ 0.0; 1.0; 1.0] ) ]

let rmse = Statistics.rootMeanSquareError(
                        expectedWithOutputValues
)
```
