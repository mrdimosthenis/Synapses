# Synapses

A **cross-platform** library for **Neural Networks**.

## Documentation

**https://mrdimosthenis.github.io/Synapses**

The interface of the library is common across programming languages.

### Create a Neural Network

`NeuralNetwork.init` | param1: `layers` | _returns_
---                  | ---              | ---
JavaScript           | `number[]`       | `NeuralNetwork`
Python               | `List[int]`      | `NeuralNetwork`
Java                 | `int[]`          | `NeuralNetwork`
C#                   | `int[]`          | `NeuralNetwork`
Scala                | `List[Int]`      | `NeuralNetwork`
F#                   | `List<int>`      | `NeuralNetwork`

`NeuralNetwork.customizedInit` | param1: `layers` | param2: `activationF`                 | param3: `weightInitF`    | _returns_
---                            | ---              | ---                                   | ---                      | ---
JavaScript                     | `number[]`       | `(number) => ActivationFunction`      | `(number) => number`     | `NeuralNetwork`
Python                         | `List[int]`      | `Callable[[int], ActivationFunction]` | `Callable[[int], float]` | `NeuralNetwork`
Java                           | `int[]`          | `IntFunction<ActivationFunction>`     | `IntFunction<Double>`    | `NeuralNetwork`
C#                             | `int[]`          | `Func<int, ActivationFunction>`       | `Func<int, Double>`      | `NeuralNetwork`
Scala                          | `List[Int]`      | `Int => ActivationFunction`           | `Int => Double`          | `NeuralNetwork`
F#                             | `List<int>`      | `int -> ActivationFunction`           | `int -> float`           | `NeuralNetwork`

### Use a Neural Network

`NeuralNetwork.fit` | param1: `neuralNetwork` | param2: `learningRate` | param3: `inputValues` | param4: `expectedOutput` | _returns_
---                 | ---                     | ---                    | ---                   | ---                      | ---
JavaScript          | `NeuralNetwork`         | `number`               | `number[]`            | `number[]`               | `NeuralNetwork`
Python              | `NeuralNetwork`         | `float`                | `List[float]`         | `List[float]`            | `NeuralNetwork`
Java                | `NeuralNetwork`         | `double`               | `double[]`            | `double[]`               | `NeuralNetwork`
C#                  | `NeuralNetwork`         | `double`               | `double[]`            | `double[]`               | `NeuralNetwork`
Scala               | `NeuralNetwork`         | `Double`               | `List[Double]`        | `List[Double]`           | `NeuralNetwork`
F#                  | `NeuralNetwork`         | `float`                | `List<float>`         | `List<float>`            | `NeuralNetwork`

`NeuralNetwork.prediction` | param1: `neuralNetwork` | param2: `inputValues` | _returns_
---                        | ---                     | ---                   | ---
JavaScript                 | `NeuralNetwork`         | `number[]`            | `number[]`
Python                     | `NeuralNetwork`         | `List[float]`         | `List[float]`
Java                       | `NeuralNetwork`         | `double[]`            | `double[]`
C#                         | `NeuralNetwork`         | `double[]`            | `double[]`
Scala                      | `NeuralNetwork`         | `List[Double]`        | `List[Double]`
F#                         | `NeuralNetwork`         | `List<float>`         | `List<float>`

### Convert a Neural Network

`NeuralNetwork.toJson` | param1: `neuralNetwork` | _returns_
---                    | ---                     | ---
JavaScript             | `NeuralNetwork`         | `string`
Python                 | `NeuralNetwork`         | `str`
Java                   | `NeuralNetwork`         | `String`
C#                     | `NeuralNetwork`         | `string`
Scala                  | `NeuralNetwork`         | `String`
F#                     | `NeuralNetwork`         | `string`

`NeuralNetwork.ofJson` | param1: `json` | _returns_
---                    | ---            | ---
JavaScript             | `string`       | `NeuralNetwork`
Python                 | `str`          | `NeuralNetwork`
Java                   | `String`       | `NeuralNetwork`
C#                     | `string`       | `NeuralNetwork`
Scala                  | `String`       | `NeuralNetwork`
F#                     | `string`       | `NeuralNetwork`

### Create a Data Preprocessor

`DataPreprocessor.init` | param1: `keysWithDiscreteFlags` | param2: `datapoints`                      | _returns_
---                     | ---                             | ---                                       | ---
JavaScript              | `any[][]`                       | `iterable`                                | `DataPreprocessor`
Python                  | `List[Tuple[str, bool]]`        | `Iterable[Dict[str, str]]`                | `DataPreprocessor`
Java                    | `Object[][]`                    | `Stream<Map<String,String>>`              | `DataPreprocessor`
C#                      | `(string, bool)[]`              | `IEnumerable<Dictionary<string, string>>` | `DataPreprocessor`
Scala                   | `List[(String, Boolean)]`       | `LazyList[Map[String, String]]`           | `DataPreprocessor`
F#                      | `List<string * bool>`           | `seq<Map<string, string>>`                | `DataPreprocessor`

### Use a Data Preprocessor

`DataPreprocessor.encodedDatapoint` | param1: `dataPreprocessor` | param2: `datapoint`          | _returns_
---                                 | ---                        | ---                          | ---
JavaScript                          | `DataPreprocessor`         | `object`                     | `number[]`
Python                              | `DataPreprocessor`         | `Dict[str, str]`             | `List[float]`
Java                                | `DataPreprocessor`         | `Map<String,String>`         | `double[]`
C#                                  | `DataPreprocessor`         | `Dictionary<string, string>` | `double[]`
Scala                               | `DataPreprocessor`         | `Map[String, String]`        | `List[Double]`
F#                                  | `DataPreprocessor`         | `Map<string, string>`        | `List<float>`

`DataPreprocessor.decodedDatapoint` | param1: `dataPreprocessor` | param2: `encodedDatapoint` | _returns_
---                                 | ---                        | ---                        | ---
JavaScript                          | `DataPreprocessor`         | `number[]`                 | `object`
Python                              | `DataPreprocessor`         | `List[float]`              | `Dict[str, str]`
Java                                | `DataPreprocessor`         | `double[]`                 | `Map<String,String>`
C#                                  | `DataPreprocessor`         | `double[]`                 | `Dictionary<string, string>`
Scala                               | `DataPreprocessor`         | `List[Double]`             | `Map[String, String]`
F#                                  | `DataPreprocessor`         | `List<float>`              | `Map<string, string>`

### Convert a Data Preprocessor

`DataPreprocessor.toJson` | param1: `dataPreprocessor` | _returns_
---                       | ---                     | ---
JavaScript                | `DataPreprocessor`         | `string`
Python                    | `DataPreprocessor`         | `str`
Java                      | `DataPreprocessor`         | `String`
C#                        | `DataPreprocessor`         | `string`
Scala                     | `DataPreprocessor`         | `String`
F#                        | `DataPreprocessor`         | `string`

`DataPreprocessor.ofJson` | param1: `json` | _returns_
---                       | ---            | ---
JavaScript                | `string`       | `DataPreprocessor`
Python                    | `str`          | `DataPreprocessor`
Java                      | `String`       | `DataPreprocessor`
C#                        | `string`       | `DataPreprocessor`
Scala                     | `String`       | `DataPreprocessor`
F#                        | `string`       | `DataPreprocessor`

### Evaluate

`Statistics.rootMeanSquareError` | param1: `expectedWithOutputValues`       | _returns_
---                              | ---                                      | ---
JavaScript                       | `iterable`                               | `number`
Python                           | `[Tuple[List[float], List[float]]]`      | `float`
Java                             | `Stream<double[][]>`                     | `double`
C#                               | `IEnumerable<(double[], double[])>`      | `double`
Scala                            | `LazyList[(List[Double], List[Double])]` | `Double`
F#                               | `seq<List<float> * List<float>>`         | `float`

## Dependencies

* [circe](https://github.com/circe/circe)
* [FSharpx.Collections](https://github.com/fsprojects/FSharpx.Collections)
* [FSharp.SystemTextJson](https://github.com/Tarmil/FSharp.SystemTextJson)
* [PyFunctional](https://github.com/EntilZha/PyFunctional)
