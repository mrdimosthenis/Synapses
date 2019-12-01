module Model.Encoding.DiscreteTest

open FSharpx.Collections
open Xunit
open FsUnit.Xunit
open Synapses.Model.Encoding

type ``discrete tests``() =

    let datapoint1 = Map.ofList
                        [ ("a", "a1");
                          ("b", "b1") ]

    let datapoint2 = Map.add "b" "b2" datapoint1
    let datapoint3 = Map.add "b" "b3" datapoint1
    let datapoint4 = Map.add "b" "b4" datapoint1

    let dataset = LazyList.ofList
                    [ datapoint1;
                      datapoint2;
                      datapoint3;
                      datapoint4 ]

    let discAttrA = Discrete.init "a" dataset
    let discAttrB = Discrete.init "b" dataset

    [<Fact>]
    let ``initialize A``() =
        discAttrA.values
        |> LazyList.toList
        |> should equal [ "a1" ]

    [<Fact>]
    let ``initialize B``() =
        discAttrB.values
        |> LazyList.toList
        |> should equal [ "b1"; "b2"; "b3"; "b4" ]

    [<Fact>]
    let ``encode A for a1``() =
        Discrete.encode discAttrA "a1"
        |> LazyList.toList
        |> should equal [ 1.0 ]

    [<Fact>]
    let ``encode B for b3``() =
        Discrete.encode discAttrB "b3"
        |> LazyList.toList
        |> should equal [ 0.0; 0.0; 1.0; 0.0 ]

    [<Fact>]
    let ``decode A to a1``() =
        let encodedValues = LazyList.ofList [ 1.0 ]
        Discrete.decode discAttrA encodedValues
        |> should equal "a1"

    [<Fact>]
    let ``decode B to b3``() =
        let encodedValues = LazyList.ofList [ 0.0; 0.78; 0.3; 0.77 ]
        Discrete.decode discAttrB encodedValues
        |> should equal "b2"
