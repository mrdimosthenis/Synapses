module Model.NetElems.LayerTest

open FSharpx.Collections
open Synapses.Model
open Synapses.Model.NetElems
open Xunit
open FsUnit.Xunit

let layerEq
        (realized: List<string * List<string>>)
        (layer: Layer.Layer)
        : unit =
        layer
        |> Utilities.realizedLayer
        |> should equal realized

type ``layer tests``() =

    let layer = Layer.init
                    3
                    4
                    Activation.sigmoid
                    (fun () () -> -0.2)

    let input = LazyList.ofList
                    [ 0.0; 1.1; 2.2; ]

    let output = LazyList.ofList
                    [ 0.297339
                      0.297339
                      0.297339
                      0.297339 ]

    [<Fact>]
    let initialize() =
        layer
        |> layerEq
            [ ("sigmoid", [ "-0.200000"; "-0.200000"; "-0.200000"; "-0.200000" ]);
              ("sigmoid", [ "-0.200000"; "-0.200000"; "-0.200000"; "-0.200000" ]);
              ("sigmoid", [ "-0.200000"; "-0.200000"; "-0.200000"; "-0.200000" ]);
              ("sigmoid", [ "-0.200000"; "-0.200000"; "-0.200000"; "-0.200000" ]) ]

    [<Fact>]
    let ``get output``() =
        Layer.output input layer
        |> LazyList.map (sprintf "%f")
        |> LazyList.toList
        |> should equal
                [ "0.297339"
                  "0.297339"
                  "0.297339"
                  "0.297339" ]

    [<Fact>]
    let ``back propagated layer of large errors``() =
        let learnRate = 0.2
        let outWithErrors = [ 0.9; -0.7; 0.8; -0.9 ]
                           |> LazyList.ofList
                           |> LazyList.zip output
        let (_, newLayer) = Layer.backPropagated
                                learnRate
                                input
                                outWithErrors
                                layer
        newLayer
        |> layerEq
                [ ("sigmoid", [ "-0.237607"; "-0.200000"; "-0.241368"; "-0.282736" ]);
                  ("sigmoid", [ "-0.170750"; "-0.200000"; "-0.167825"; "-0.135650" ]);
                  ("sigmoid", [ "-0.233429"; "-0.200000"; "-0.236771"; "-0.273543" ]);
                  ("sigmoid", [ "-0.162393"; "-0.200000"; "-0.158632"; "-0.117264" ]) ]

    [<Fact>]
    let ``back propagated layer of small errors``() =
        let learnRate = 0.2
        let outWithErrors = [ 0.01; -0.02; 0.03; -0.04 ]
                           |> LazyList.ofList
                           |> LazyList.zip output
        let (_, newLayer) = Layer.backPropagated
                                learnRate
                                input
                                outWithErrors
                                layer
        newLayer
        |> layerEq
                [ ("sigmoid", [ "-0.200418"; "-0.200000"; "-0.200460"; "-0.200919" ]);
                  ("sigmoid", [ "-0.199164"; "-0.200000"; "-0.199081"; "-0.198161" ]);
                  ("sigmoid", [ "-0.201254"; "-0.200000"; "-0.201379"; "-0.202758" ]);
                  ("sigmoid", [ "-0.198329"; "-0.200000"; "-0.198161"; "-0.196323" ]) ]

    [<Fact>]
    let ``back propagated layer of zero errors``() =
        let learnRate = 0.2
        let outWithErrors = [ 0.0; 0.0; 0.0; 0.0 ]
                           |> LazyList.ofList
                           |> LazyList.zip output
        let (_, newLayer) = Layer.backPropagated
                                learnRate
                                input
                                outWithErrors
                                layer
        newLayer
        |> layerEq
                [ ("sigmoid", [ "-0.200000"; "-0.200000"; "-0.200000"; "-0.200000" ]);
                  ("sigmoid", [ "-0.200000"; "-0.200000"; "-0.200000"; "-0.200000" ]);
                  ("sigmoid", [ "-0.200000"; "-0.200000"; "-0.200000"; "-0.200000" ]);
                  ("sigmoid", [ "-0.200000"; "-0.200000"; "-0.200000"; "-0.200000" ]) ]

    [<Fact>]
    let ``back propagated layer of large learning rate``() =
        let learnRate = 0.8
        let outWithErrors = [ 0.01; -0.02; 0.03; -0.04 ]
                           |> LazyList.ofList
                           |> LazyList.zip output
        let (_, newLayer) = Layer.backPropagated
                                learnRate
                                input
                                outWithErrors
                                layer
        newLayer
        |> layerEq
                [ ("sigmoid", [ "-0.201671"; "-0.200000"; "-0.201839"; "-0.203677" ]);
                  ("sigmoid", [ "-0.196657"; "-0.200000"; "-0.196323"; "-0.192646" ]);
                  ("sigmoid", [ "-0.205014"; "-0.200000"; "-0.205516"; "-0.211031" ]);
                  ("sigmoid", [ "-0.193314"; "-0.200000"; "-0.192646"; "-0.185291" ]) ]

    [<Fact>]
    let ``back propagated layer of zero learning rate``() =
        let learnRate = 0.0
        let outWithErrors = [ 0.01; -0.02; 0.03; -0.04 ]
                           |> LazyList.ofList
                           |> LazyList.zip output
        let (_, newLayer) = Layer.backPropagated
                                learnRate
                                input
                                outWithErrors
                                layer
        newLayer
        |> layerEq
                [ ("sigmoid", [ "-0.200000"; "-0.200000"; "-0.200000"; "-0.200000" ]);
                  ("sigmoid", [ "-0.200000"; "-0.200000"; "-0.200000"; "-0.200000" ]);
                  ("sigmoid", [ "-0.200000"; "-0.200000"; "-0.200000"; "-0.200000" ]);
                  ("sigmoid", [ "-0.200000"; "-0.200000"; "-0.200000"; "-0.200000" ]) ]

    [<Fact>]
    let ``back propagated errors of large errors``() =
        let learnRate = 0.2
        let outWithErrors = [ 0.9; -0.7; 0.8; -0.9 ]
                           |> LazyList.ofList
                           |> LazyList.zip output
        let (errors, _) = Layer.backPropagated
                                learnRate
                                input
                                outWithErrors
                                layer
        errors
        |> LazyList.map (sprintf "%f")
        |> LazyList.toList
        |> should equal [ "0.000000"; "0.022982"; "0.045964" ]

    [<Fact>]
    let ``back propagated errors of small errors``() =
        let learnRate = 0.2
        let outWithErrors = [ 0.01; -0.02; 0.03; -0.04 ]
                           |> LazyList.ofList
                           |> LazyList.zip output
        let (errors, _) = Layer.backPropagated
                                learnRate
                                input
                                outWithErrors
                                layer
        errors
        |> LazyList.map (sprintf "%f")
        |> LazyList.toList
        |> should equal [ "0.000000"; "-0.004596"; "-0.009193" ]

    [<Fact>]
    let ``back propagated errors of zero errors``() =
        let learnRate = 0.2
        let outWithErrors = [ 0.0; 0.0; 0.0; 0.0 ]
                           |> LazyList.ofList
                           |> LazyList.zip output
        let (errors, _) = Layer.backPropagated
                                learnRate
                                input
                                outWithErrors
                                layer
        errors
        |> LazyList.map (sprintf "%f")
        |> LazyList.toList
        |> should equal [ "0.000000"; "0.000000"; "0.000000" ]
