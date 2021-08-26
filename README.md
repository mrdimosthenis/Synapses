# Synapses

A group of **neural-network** libraries for functional and mainstream languages!

Choose a programming language:

* [Clojure](https://github.com/mrdimosthenis/clj-synapses)
* [C#](https://github.com/mrdimosthenis/SynapsesCSharp)
* [Elixir](https://github.com/mrdimosthenis/elixir_synapses)
* [F#](https://github.com/mrdimosthenis/FSharp.Synapses)
* [Gleam](https://github.com/mrdimosthenis/gleam_synapses)
* [Java](https://github.com/mrdimosthenis/synapses-java)
* [JavaScript](https://github.com/mrdimosthenis/synapses.js)
* [Python](https://github.com/mrdimosthenis/synapses-py)
* [Scala](https://github.com/mrdimosthenis/scala-synapses)

# Why Sypapses?

## It's efficient

The implementation is based on *lazy list*.
The information flows smoothly.
Everything is obtained at a single pass.

## It's customizable

You can specify the **activation function** and the **weight distribution** for the neurons of each layer.
If this is not enough, edit the json of a network to be exactly what you have in mind.

## It offers visualizations

Get an overview of a neural network by taking a brief look at its **svg drawing**.

![Network Drawing](https://github.com/mrdimosthenis/Synapses/blob/master/network-drawing.png?raw=true)

## Data preprocessing is simple

By annotating the *discrete* and *continuous attributes*,
you can create a *preprocessor* that **encodes** and **decodes** the datapoints.

## Works for huge datasets

The functions that process big volumes of data, have an *Iterable/Stream* as argument.
RAM should not get full!

## It's well tested

Every function is tested for every language.
Take a look at the test projects.

## It's compatible across languages

The interface is similar across languages.
You can transfer a network from one platform to another via its **json instance**.
Create a neural network in *Python*, train it in *Java* and get its predictions in *JavaScript*!
