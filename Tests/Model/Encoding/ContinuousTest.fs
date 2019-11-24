module Synapses.Tests.Model.Encoding.ContinuousTest

open FSharpx.Collections
open Xunit
open FsUnit.Xunit
open Synapses.Model.Encoding
open Synapses.Model.Encoding.Continuous

type ``continuous tests``() =

    let datapoint1 = Map.ofList
                        [ ("a", "-13.0");
                          ("b", "30.5") ]

    let datapoint2 = Map.add "b" "-0.008" datapoint1
    let datapoint3 = Map.add "b" "12.9" datapoint1
    let datapoint4 = Map.add "b" "0.0" datapoint1

    let dataset = LazyList.ofList
                    [ datapoint1;
                      datapoint2;
                      datapoint3;
                      datapoint4 ]

    let contAttrA = Continuous.init "a" dataset
    let contAttrB = Continuous.init "b" dataset

    [<Fact>]
    let ``initialize A``() =
        contAttrA
        |> should equal
            { key = "a"
              min = -13.0
              max = -13.0 }

    [<Fact>]
    let ``initialize B``() =
        contAttrB
        |> should equal
            { key = "b"
              min = -0.008
              max = 30.5 }

    [<Fact>]
    let ``encode A for -13.0``() =
        Continuous.encode contAttrA "-13.0"
        |> LazyList.toList
        |> should equal [ 0.5 ]

    [<Fact>]
    let ``encode B for -0.008``() =
        Continuous.encode contAttrB "-0.008"
        |> LazyList.toList
        |> should equal [ 0.0 ]

    [<Fact>]
    let ``encode B for 30.5``() =
        Continuous.encode contAttrB "30.5"
        |> LazyList.toList
        |> should equal [ 1.0 ]

    [<Fact>]
    let ``encode B for 15.246``() =
        Continuous.encode contAttrB "15.246"
        |> LazyList.toList
        |> should equal [ 0.5 ]

    [<Fact>]
    let ``decode A to -13.0``() =
        let encodedValues = LazyList.ofList [ -13.0 ]
        Continuous.decode contAttrA encodedValues
        |> should equal "-13.000000"

    [<Fact>]
    let ``decode B to -0.008``() =
        let encodedValues = LazyList.ofList [ 0.0 ]
        Continuous.decode contAttrB encodedValues
        |> should equal "-0.008000"

    [<Fact>]
    let ``decode B to 30.5``() =
        let encodedValues = LazyList.ofList [ 1.0 ]
        Continuous.decode contAttrB encodedValues
        |> should equal "30.500000"

    [<Fact>]
    let ``decode B to 15.246``() =
        let encodedValues = LazyList.ofList [ 0.5 ]
        Continuous.decode contAttrB encodedValues
        |> should equal "15.246000"
