module Model.NetElems.NeuronTest

open FSharpx.Collections
open Synapses.Model
open Synapses.Model.NetElems
open Xunit
open FsUnit.Xunit

let neuronEq
        (realized: string * List<string>)
        (neuron: Neuron.Neuron)
        : unit =
        neuron
        |> Utilities.realizedNeuron
        |> should equal realized

type ``neuron tests``() =
    
    let neuron = Neuron.init
                    3
                    Activation.sigmoid
                    (fun () -> -0.2)
                    
    let input = LazyList.ofList
                    [ 0.0; 1.1; 2.2; ]
                    
    let output = 0.2973393456552685
    
    [<Fact>]
    let initialize() =
         neuron
         |> neuronEq
               (
                   "sigmoid",
                   [ "-0.200000"
                     "-0.200000"
                     "-0.200000"
                     "-0.200000" ]
               )


    [<Fact>]
    let ``get output``() =
        Neuron.output input neuron
        |> should equal output

    [<Fact>]
    let ``back propagated network of large diff``() =
        let learnRate = 0.2
        let outWithDiff = (output, 0.9)
        let (_, newNeuron) =
                Neuron.backPropagated
                        learnRate
                        input
                        outWithDiff
                        neuron
        newNeuron
        |> neuronEq
               (
                   "sigmoid",
                   [ "-0.237607"
                     "-0.200000"
                     "-0.241368"
                     "-0.282736" ]
               )

    [<Fact>]
    let ``back propagated network of small diff``() =
        let learnRate = 0.2
        let outWithDiff = (output, 0.001)
        let (_, newNeuron) =
                Neuron.backPropagated
                        learnRate
                        input
                        outWithDiff
                        neuron
        newNeuron
        |> neuronEq
               (
                   "sigmoid",
                   [ "-0.200042"
                     "-0.200000"
                     "-0.200046"
                     "-0.200092" ]
               )

    [<Fact>]
    let ``back propagated network of zero diff``() =
        let learnRate = 0.2
        let outWithDiff = (output, 0.0)
        let (_, newNeuron) =
                Neuron.backPropagated
                        learnRate
                        input
                        outWithDiff
                        neuron
        newNeuron
        |> neuronEq
               (
                   "sigmoid",
                   [ "-0.200000"
                     "-0.200000"
                     "-0.200000"
                     "-0.200000" ]
               )

    [<Fact>]
    let ``back propagated network of negative diff``() =
        let learnRate = 0.2
        let outWithDiff = (output, -0.15)
        let (_, newNeuron) =
                Neuron.backPropagated
                        learnRate
                        input
                        outWithDiff
                        neuron
        newNeuron
        |> neuronEq
               (
                   "sigmoid",
                   [ "-0.193732"
                     "-0.200000"
                     "-0.193105"
                     "-0.186211" ]
               )

    [<Fact>]
    let ``back propagated network of large learning rate``() =
        let learnRate = 0.8
        let outWithDiff = (output, 0.9)
        let (_, newNeuron) =
                Neuron.backPropagated
                        learnRate
                        input
                        outWithDiff
                        neuron
        newNeuron
        |> neuronEq
               (
                   "sigmoid",
                   [ "-0.350429"
                     "-0.200000"
                     "-0.365471"
                     "-0.530943" ]
               )

    [<Fact>]
    let ``back propagated network of zero learning rate``() =
        let learnRate = 0.0
        let outWithDiff = (output, 0.9)
        let (_, newNeuron) =
                Neuron.backPropagated
                        learnRate
                        input
                        outWithDiff
                        neuron
        newNeuron
        |> neuronEq
               (
                   "sigmoid",
                   [ "-0.200000"
                     "-0.200000"
                     "-0.200000"
                     "-0.200000" ]
               )

    [<Fact>]
    let ``back propagated errors of large diff``() =
        let learnRate = 0.2
        let outWithDiff = (output, 0.9)
        let (errors, _) =
                Neuron.backPropagated
                        learnRate
                        input
                        outWithDiff
                        neuron
        errors
        |> LazyList.map (sprintf "%f")
        |> LazyList.toList
        |> should equal [ "0.000000"; "0.206839"; "0.413679" ]

    [<Fact>]
    let ``back propagated errors of small diff``() =
        let learnRate = 0.2
        let outWithDiff = (output, 0.001)
        let (errors, _) =
                Neuron.backPropagated
                        learnRate
                        input
                        outWithDiff
                        neuron
        errors
        |> LazyList.map (sprintf "%f")
        |> LazyList.toList
        |> should equal [ "0.000000"; "0.000230"; "0.000460" ]

    [<Fact>]
    let ``back propagated errors of zero diff``() =
        let learnRate = 0.2
        let outWithDiff = (output, 0.0)
        let (errors, _) =
                Neuron.backPropagated
                        learnRate
                        input
                        outWithDiff
                        neuron
        errors
        |> LazyList.map (sprintf "%f")
        |> LazyList.toList
        |> should equal [ "0.000000"; "0.000000"; "0.000000" ]

    [<Fact>]
    let ``back propagated errors of negative diff``() =
        let learnRate = 0.2
        let outWithDiff = (output, -0.15)
        let (errors, _) =
                Neuron.backPropagated
                        learnRate
                        input
                        outWithDiff
                        neuron
        errors
        |> LazyList.map (sprintf "%f")
        |> LazyList.toList
        |> should equal [ "-0.000000"; "-0.034473"; "-0.068946" ]
