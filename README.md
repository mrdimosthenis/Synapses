# Synapses

A lightweight **Neural Network** library, for **js**, **jvm** and **.net**.

**Documentation**: https://mrdimosthenis.github.io/Synapses

The interface of the library is common across programming languages.
Take a look at the function parameters.

##### Neural Network

|`NeuralNetwork.init`|JavaScript     |Java           |Scala          |F#             |
|--------------------|---------------|---------------|---------------|---------------|
|param1: `layers`    |`number[]`     |`int[]`        |`List[Int]`    |`List<int>`    |
|_returns_           |`NeuralNetwork`|`NeuralNetwork`|`NeuralNetwork`|`NeuralNetwork`|

|`NeuralNetwork.prediction`|JavaScript     |Java           |Scala          |F#             |
|--------------------------|---------------|---------------|---------------|---------------|
|param1: `neuralNetwork`   |`NeuralNetwork`|`NeuralNetwork`|`NeuralNetwork`|`NeuralNetwork`|
|param2: `inputValues`     |`number[]`     |`double[]`     |`List[Double]` |`List<float>`  |
|_returns_                 |`number[]`     |`double[]`     |`List[Double]` |`List<float>`  |

|`NeuralNetwork.fit`       |JavaScript     |Java           |Scala          |F#             |
|--------------------------|---------------|---------------|---------------|---------------|
|param1: `neuralNetwork`   |`NeuralNetwork`|`NeuralNetwork`|`NeuralNetwork`|`NeuralNetwork`|
|param2: `learningRate`    |`number`       |`double`       |`Double`       |`float`        |
|param3: `inputValues`     |`number[]`     |`double[]`     |`List[Double]` |`List<float>`  |
|param4: `expectedOutpu`   |`number[]`     |`double[]`     |`List[Double]` |`List<float>`  |
|_returns_                 |`NeuralNetwork`|`NeuralNetwork`|`NeuralNetwork`|`NeuralNetwork`|

|`NeuralNetwork.customizedInit`|JavaScript                      |Java                             |Scala                      |F#                         |
|------------------------------|--------------------------------|---------------------------------|---------------------------|---------------------------|
|param1: `layers`              |`number[]`                      |`int[]`                          |`List[Int]`                |`List<int>`                |
|param2: `activationF`         |`(number) => ActivationFunction`|`IntFunction<ActivationFunction>`|`Int => ActivationFunction`|`int -> ActivationFunction`|
|param3: `weightInitF`         |`(number) => number`            |`IntFunction<Double>`            |`Int => Double`            |`int -> float`             |
|_returns_                     |`NeuralNetwork`                 |`NeuralNetwork`                  |`NeuralNetwork`            |`NeuralNetwork`            |

##### Data Preprocessor

|`DataPreprocessor.init`        |JavaScript                      |Java                             |Scala                          |F#                        |
|-------------------------------|--------------------------------|---------------------------------|-------------------------------|--------------------------|
|param1: `keysWithDiscreteFlags`|`any[][]`                       |`Object[][]`                     |`List[(String, Boolean)]`      |`List<string * bool>`     |
|param2: `datapoints`           |`iterable`                      |`Stream<Map<String,String>>`     |`LazyList[Map[String, String]]`|`seq<Map<string, string>>`|
|_returns_                      |`DataPreprocessor`              |`DataPreprocessor`               |`DataPreprocessor`             |`DataPreprocessor`        |

|`DataPreprocessor.encodedDatapoint`|JavaScript        |Java                |Scala                |F#                   |
|-----------------------------------|------------------|--------------------|---------------------|---------------------|
|param1: `dataPreprocessor`         |`DataPreprocessor`|`DataPreprocessor`  |`DataPreprocessor`   |`DataPreprocessor`   |
|param2: `datapoint`                |`object`          |`Map<String,String>`|`Map[String, String]`|`Map<string, string>`|
|_returns_                          |`number[]`        |`double[]`          |`List[Double]`       |`List<float>`        |

|`DataPreprocessor.decodedDatapoint`|JavaScript        |Java                |Scala                |F#                   |
|-----------------------------------|------------------|--------------------|---------------------|---------------------|
|param1: `dataPreprocessor`         |`DataPreprocessor`|`DataPreprocessor`  |`DataPreprocessor`   |`DataPreprocessor`   |
|param2: `encodedDatapoint`         |`number[]`        |`double[]`          |`List[Double]`       |`List<float>`        |
|_returns_                          |`object`          |`Map<String,String>`|`Map[String, String]`|`Map<string, string>`|

##### Statistics

|`Statistics.rootMeanSquareError`  |JavaScript|Java                |Scala                                   |F#                              |
|----------------------------------|----------|--------------------|----------------------------------------|--------------------------------|
|param1: `expectedWithOutputValues`|`iterable`|`Stream<double[][]>`|`LazyList[(List[Double], List[Double])]`|`seq<List<float> * List<float>>`|
|_returns_                         |`number`  |`double`            |`Double`                                |`float`                         |
