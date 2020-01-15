# Synapses

A lightweight **Neural Network** library, for **js**, **jvm** and **.net**.

## Documentation

**https://mrdimosthenis.github.io/Synapses**

The interface of the library is common across programming languages.

### Create a Neural Network

`NeuralNetwork.init` | param1: `layers` | _returns_
---                  | ---              | ---
JavaScript           | `number[]`       | `NeuralNetwork`
Java                 | `int[]`          | `NeuralNetwork`
Scala                | `List[Int]`      | `NeuralNetwork`
F#                   | `List<int>`      | `NeuralNetwork`

`NeuralNetwork.customizedInit` | param1: `layers` | param2: `activationF`             | param3: `weightInitF` | _returns_
---                            | ---              | ---                               | ---                   | ---
JavaScript                     | `number[]`       | `(number) => ActivationFunction`  | `(number) => number`  | `NeuralNetwork`
Java                           | `int[]`          | `IntFunction<ActivationFunction>` | `IntFunction<Double>` | `NeuralNetwork`
Scala                          | `List[Int]`      | `Int => ActivationFunction`       | `Int => Double`       | `NeuralNetwork`
F#                             | `List<int>`      | `int -> ActivationFunction`       | `int -> float`        | `NeuralNetwork`

### Train a Neural Network

`NeuralNetwork.fit` | param1: `neuralNetwork` | param2: `learningRate` | param3: `inputValues` | param4: `expectedOutput` | _returns_
---                 | ---                     | ---                    | ---                   | ---                      | ---
JavaScript          | `NeuralNetwork`         | `number`               | `number[]`            | `number[]`               | `NeuralNetwork`
Java                | `NeuralNetwork`         | `double`               | `double[]`            | `double[]`               | `NeuralNetwork`
Scala               | `NeuralNetwork`         | `Double`               | `List[Double]`        | `List[Double]`           | `NeuralNetwork`
F#                  | `NeuralNetwork`         | `float`                | `List<float>`         | `List<float>`            | `NeuralNetwork`


`NeuralNetwork.prediction` | param1: `neuralNetwork` | param2: `inputValues` | _returns_
---                        | ---                     | ---                   | ---
JavaScript                 | `NeuralNetwork`         | `number[]`            | `number[]`
Java                       | `NeuralNetwork`         | `double[]`            | `double[]`
Scala                      | `NeuralNetwork`         | `List[Double]`        | `List[Double]`
F#                         | `NeuralNetwork`         | `List<float>`         | `List<float>`

### Convert a Neural Network

`NeuralNetwork.toJson` | param1: `neuralNetwork` | _returns_
---                    | ---                     | ---
JavaScript             | `NeuralNetwork`         | `string`
Java                   | `NeuralNetwork`         | `String`
Scala                  | `NeuralNetwork`         | `String`
F#                     | `NeuralNetwork`         | `string`

`NeuralNetwork.ofJson` | param1: `json` | _returns_
---                    | ---            | ---
JavaScript             | `string`       | `NeuralNetwork`
Java                   | `String`       | `NeuralNetwork`
Scala                  | `String`       | `NeuralNetwork`
F#                     | `string`       | `NeuralNetwork`

### Create a Data Preprocessor

`DataPreprocessor.init` | param1: `keysWithDiscreteFlags` | param2: `datapoints`            | _returns_
---                     | ---                             | ---                             | ---
JavaScript              | `any[][]`                       | `iterable`                      | `DataPreprocessor`
Java                    | `Object[][]`                    | `Stream<Map<String,String>>`    | `DataPreprocessor`
Scala                   | `List[(String, Boolean)]`       | `LazyList[Map[String, String]]` | `DataPreprocessor`
F#                      | `List<string * bool>`           | `seq<Map<string, string>>`      | `DataPreprocessor`

### Encode with a Data Preprocessor

`DataPreprocessor.encodedDatapoint` | param1: `dataPreprocessor` | param2: `datapoint`   | _returns_
---                                 | ---                        | ---                   | ---
JavaScript                          | `DataPreprocessor`         | `object`              | `number[]`
Java                                | `DataPreprocessor`         | `Map<String,String>`  | `double[]`
Scala                               | `DataPreprocessor`         | `Map[String, String]` | `List[Double]`
F#                                  | `DataPreprocessor`         | `Map<string, string>` | `List<float>`

`DataPreprocessor.decodedDatapoint` | param1: `dataPreprocessor` | param2: `encodedDatapoint` | _returns_
---                                 | ---                        | ---                        | ---
JavaScript                          | `DataPreprocessor`         | `number[]`                 | `object`
Java                                | `DataPreprocessor`         | `double[]`                 | `Map<String,String>`
Scala                               | `DataPreprocessor`         | `List[Double]`             | `Map[String, String]`
F#                                  | `DataPreprocessor`         | `List<float>`              | `Map<string, string>`

### Convert a Data Preprocessor

`DataPreprocessor.toJson` | param1: `dataPreprocessor` | _returns_
---                       | ---                     | ---
JavaScript                | `DataPreprocessor`         | `string`
Java                      | `DataPreprocessor`         | `String`
Scala                     | `DataPreprocessor`         | `String`
F#                        | `DataPreprocessor`         | `string`

`DataPreprocessor.ofJson` | param1: `json` | _returns_
---                       | ---            | ---
JavaScript                | `string`       | `DataPreprocessor`
Java                      | `String`       | `DataPreprocessor`
Scala                     | `String`       | `DataPreprocessor`
F#                        | `string`       | `DataPreprocessor`

### Calculate the Root Mean Square Error

`Statistics.rootMeanSquareError` | param1: `expectedWithOutputValues`       | _returns_
---                              | ---                                      | ---
JavaScript                       | `iterable`                               | `number`
Java                             | `Stream<double[][]>`                     | `double`
Scala                            | `LazyList[(List[Double], List[Double])]` | `Double`
F#                               | `seq<List<float> * List<float>>`         | `float`

## Tests

* [JavaScript](test-projects%2FJavaScriptTest%2Ftest)
* [Java](test-projects%2FJavaTest%2Fsrc%2Ftest%2Fjava)
* [Scala](test-projects%2FScalaTest%2Fsrc%2Ftest%2Fscala)
* [F#](test-projects%2FF%23Test)

## Implementation

* [JavaScript](ScalaJS%2Fsrc%2Fmain%2Fscala%2Fsynapses%2FLibrary.scala) - built with [Scala.js](https://www.scala-js.org/)
* [Java](Scala%2Fsrc%2Fmain%2Fjava%2Fsynapses%2Fjvm%2Flibrary) - Scala interoperability
* [Scala](Scala%2Fsrc%2Fmain%2Fscala%2Fsynapses%2Fmodel)
* [F#](F%23%2FModel)

## Dependencies

* [circe](https://github.com/circe/circe)
* [FSharpx.Collections](https://github.com/fsprojects/FSharpx.Collections)
* [FSharp.SystemTextJson](https://github.com/Tarmil/FSharp.SystemTextJson)
