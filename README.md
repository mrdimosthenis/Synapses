# Synapses

A **lightweight** library for **neural networks** that **runs anywhere**.

## [Getting Started](https://mrdimosthenis.github.io/Synapses)

### Why Sypapses?

#### It's easy

1. Add **one dependency** to your project.
2. Write a **single import statement**.
3. Use **a few pure functions**.

You are all set!

#### It runs anywhere <sup>[1](#myfootnote1)</sup>

Supported languages:

* [JavaScript](https://mrdimosthenis.github.io/Synapses/?javascript)
* [Python](https://mrdimosthenis.github.io/Synapses/?python)
* [Java](https://mrdimosthenis.github.io/Synapses/?java)
* [C#](https://mrdimosthenis.github.io/Synapses/?csharp)
* [Scala](https://mrdimosthenis.github.io/Synapses/?scala)
* [F#](https://mrdimosthenis.github.io/Synapses/?fsharp)

#### It's compatible across languages

1. The [interface](https://github.com/mrdimosthenis/Synapses/blob/master/interface.md) is **common** across languages.
2. You can transfer a network from one platform to another via its **json instance**.
Create a neural network in *Python*, train it in *Java* and get its predictions in *JavaScript*!

#### It offers visualizations

Get an overview of a neural network by taking a brief look at its **svg drawing**.

![Network Drawing](https://github.com/mrdimosthenis/Synapses/blob/master/network-drawing.png?raw=true)

#### It's customizable

You can specify the **activation function** and the **weight distribution** for the neurons of each layer.
If this is not enough, edit the [json instance](https://raw.githubusercontent.com/mrdimosthenis/Synapses/master/network.json) of a network to be exactly what you have in mind.

#### It's efficient

The implementation is based on *lazy list*.
The information flows smoothly.
Everything is obtained at a single pass.

#### Data preprocessing is simple

By annotating the *discrete* and *continuous attributes*,
you can create a *preprocessor* that **encodes** and **decodes** the datapoints.

#### Works for huge datasets

The functions that process big volumes of data, have an *Iterable/Stream* as argument.
RAM should not get full!

#### It's well tested

Every function is tested for every language.
Please take a look at the test projects.

* [JavaScript](https://github.com/mrdimosthenis/Synapses/tree/master/test-projects/remote-deps/JavaScriptTest/test)
* [Python](https://github.com/mrdimosthenis/Synapses/tree/master/test-projects/remote-deps/PythonTest/test)
* [Java](https://github.com/mrdimosthenis/Synapses/tree/master/test-projects/remote-deps/JavaTest/src/test/java)
* [C#](https://github.com/mrdimosthenis/Synapses/tree/master/test-projects/remote-deps/CSharpTest)
* [Scala](https://github.com/mrdimosthenis/Synapses/tree/master/test-projects/remote-deps/ScalaTest/src/test/scala)
* [F#](https://github.com/mrdimosthenis/Synapses/tree/master/test-projects/remote-deps/FSharpTest)

### Dependencies

* [circe](https://github.com/circe/circe)
* [FSharpx.Collections](https://github.com/fsprojects/FSharpx.Collections)
* [FSharp.SystemTextJson](https://github.com/Tarmil/FSharp.SystemTextJson)
* [PyFunctional](https://github.com/EntilZha/PyFunctional)

<a name="myfootnote1">1</a>: Your Honour should be aware that I mean *almost* anywhere.
