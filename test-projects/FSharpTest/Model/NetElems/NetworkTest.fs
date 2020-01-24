module Model.NetElems.NetworkTest

open FSharpx.Collections
open Synapses.Model
open Synapses.Model.NetElems
open Xunit
open FsUnit.Xunit
open Synapses

let networkEq
        (realized: List<List<string * List<string>>>)
        (network: Network.Network)
        : unit =
        network
        |> Utilities.realizedNetwork
        |> should equal realized

type ``neuron tests``() =

    let network = Network.init
                    (LazyList.ofList [ 3; 4; 5; 2 ])
                    (fun _ -> Activation.sigmoid)
                    (fun i -> 0.1 + (float i) / 10.0)

    let input = LazyList.ofList
                    [ 0.0; 1.1; 2.2; ]

    let output = LazyList.ofList
                    [ 0.7853599931; 0.7853599931 ]

    [<Fact>]
    let initialize() =
        network
        |> networkEq
            [ [ ("sigmoid", [ "0.100000"; "0.100000"; "0.100000"; "0.100000" ]);
                ("sigmoid", [ "0.100000"; "0.100000"; "0.100000"; "0.100000" ]);
                ("sigmoid", [ "0.100000"; "0.100000"; "0.100000"; "0.100000" ]);
                ("sigmoid", [ "0.100000"; "0.100000"; "0.100000"; "0.100000" ]) ];
             [ ("sigmoid", [ "0.200000"; "0.200000"; "0.200000"; "0.200000"; "0.200000" ]);
               ("sigmoid", [ "0.200000"; "0.200000"; "0.200000"; "0.200000"; "0.200000" ]);
               ("sigmoid", [ "0.200000"; "0.200000"; "0.200000"; "0.200000"; "0.200000" ]);
               ("sigmoid", [ "0.200000"; "0.200000"; "0.200000"; "0.200000"; "0.200000" ]);
               ("sigmoid", [ "0.200000"; "0.200000"; "0.200000"; "0.200000"; "0.200000" ]) ];
             [ ("sigmoid", [ "0.300000"; "0.300000"; "0.300000"; "0.300000"; "0.300000"; "0.300000" ]);
               ("sigmoid", [ "0.300000"; "0.300000"; "0.300000"; "0.300000"; "0.300000"; "0.300000" ]) ] ]

    [<Fact>]
    let ``get output``() =
        Network.output input network
        |> LazyList.map (sprintf "%f")
        |> LazyList.toList
        |> should equal
                [ "0.785360"; "0.785360" ]

    [<Fact>]
    let ``get fed forward network``() =
        let (_, fedNetwork) =
                    Network.fedForward input network
                    |> Utilities.lazyUnzip
        fedNetwork
        |> networkEq
            [ [ ("sigmoid", [ "0.300000"; "0.300000"; "0.300000"; "0.300000"; "0.300000"; "0.300000" ]);
                ("sigmoid", [ "0.300000"; "0.300000"; "0.300000"; "0.300000"; "0.300000"; "0.300000" ]) ];
              [ ("sigmoid", [ "0.200000"; "0.200000"; "0.200000"; "0.200000"; "0.200000" ]);
                ("sigmoid", [ "0.200000"; "0.200000"; "0.200000"; "0.200000"; "0.200000" ]);
                ("sigmoid", [ "0.200000"; "0.200000"; "0.200000"; "0.200000"; "0.200000" ]);
                ("sigmoid", [ "0.200000"; "0.200000"; "0.200000"; "0.200000"; "0.200000" ]);
                ("sigmoid", [ "0.200000"; "0.200000"; "0.200000"; "0.200000"; "0.200000" ]) ];
              [ ("sigmoid", [ "0.100000"; "0.100000"; "0.100000"; "0.100000" ]);
                ("sigmoid", [ "0.100000"; "0.100000"; "0.100000"; "0.100000" ]);
                ("sigmoid", [ "0.100000"; "0.100000"; "0.100000"; "0.100000" ]);
                ("sigmoid", [ "0.100000"; "0.100000"; "0.100000"; "0.100000" ]) ] ]

    [<Fact>]
    let ``get fed forward inputs``() =
        let (inputs, _) =
                    Network.fedForward input network
                    |> Utilities.lazyUnzip
        inputs
        |> LazyList.map
               (fun input ->
                         input
                         |> LazyList.map (sprintf "%f")
                         |> LazyList.toList
               )
        |> LazyList.toList
        |> should equal
                    [ [ "0.664787"; "0.664787"; "0.664787"; "0.664787"; "0.664787" ];
                      [ "0.605874"; "0.605874"; "0.605874"; "0.605874" ];
                      [ "0.000000"; "1.100000"; "2.200000" ] ]

    [<Fact>]
    let ``get back propagated network of large errors``() =
        let learnRate = 0.2
        let netExpOutput = LazyList.ofList [ 0.01; 0.99 ]
        let revInputsWithLayers = Network.fedForward input network
        Network.backPropagated
                    learnRate
                    netExpOutput
                    revInputsWithLayers
        |> networkEq
                [ [ ("sigmoid", [ "0.097938"; "0.100000"; "0.097732"; "0.095464" ]);
                    ("sigmoid", [ "0.097938"; "0.100000"; "0.097732"; "0.095464" ]);
                    ("sigmoid", [ "0.097938"; "0.100000"; "0.097732"; "0.095464" ]);
                    ("sigmoid", [ "0.097938"; "0.100000"; "0.097732"; "0.095464" ]) ];
                  [ ("sigmoid", [ "0.197150"; "0.198273"; "0.198273"; "0.198273"; "0.198273" ]);
                    ("sigmoid", [ "0.197150"; "0.198273"; "0.198273"; "0.198273"; "0.198273" ]);
                    ("sigmoid", [ "0.197150"; "0.198273"; "0.198273"; "0.198273"; "0.198273" ]);
                    ("sigmoid", [ "0.197150"; "0.198273"; "0.198273"; "0.198273"; "0.198273" ]);
                    ("sigmoid", [ "0.197150"; "0.198273"; "0.198273"; "0.198273"; "0.198273" ]) ];
                  [ ("sigmoid", [ "0.273860"; "0.282622"; "0.282622"; "0.282622"; "0.282622"; "0.282622" ]);
                    ("sigmoid", [ "0.306899"; "0.304587"; "0.304587"; "0.304587"; "0.304587"; "0.304587" ]) ] ]

    [<Fact>]
    let ``get back propagated network of small errors``() =
        let learnRate = 0.2
        let netExpOutput = LazyList.ofList [ 0.786; 0.785 ]
        let revInputsWithLayers = Network.fedForward input network
        Network.backPropagated
                    learnRate
                    netExpOutput
                    revInputsWithLayers
        |> networkEq
                [ [ ("sigmoid", [ "0.100001"; "0.100000"; "0.100001"; "0.100002" ]);
                    ("sigmoid", [ "0.100001"; "0.100000"; "0.100001"; "0.100002" ]);
                    ("sigmoid", [ "0.100001"; "0.100000"; "0.100001"; "0.100002" ]);
                    ("sigmoid", [ "0.100001"; "0.100000"; "0.100001"; "0.100002" ]) ];
                  [ ("sigmoid", [ "0.200001"; "0.200001"; "0.200001"; "0.200001"; "0.200001" ]);
                    ("sigmoid", [ "0.200001"; "0.200001"; "0.200001"; "0.200001"; "0.200001" ]);
                    ("sigmoid", [ "0.200001"; "0.200001"; "0.200001"; "0.200001"; "0.200001" ]);
                    ("sigmoid", [ "0.200001"; "0.200001"; "0.200001"; "0.200001"; "0.200001" ]);
                    ("sigmoid", [ "0.200001"; "0.200001"; "0.200001"; "0.200001"; "0.200001" ]) ];
                  [ ("sigmoid", [ "0.300022"; "0.300014"; "0.300014"; "0.300014"; "0.300014"; "0.300014" ]);
                    ("sigmoid", [ "0.299988"; "0.299992"; "0.299992"; "0.299992"; "0.299992"; "0.299992" ]) ] ]

    [<Fact>]
    let ``get back propagated network of zero errors``() =
        let learnRate = 0.2
        let netExpOutput = output
        let revInputsWithLayers = Network.fedForward input network
        Network.backPropagated
                    learnRate
                    netExpOutput
                    revInputsWithLayers
        |> networkEq
                [ [ ("sigmoid", [ "0.100000"; "0.100000"; "0.100000"; "0.100000" ]);
                     ("sigmoid", [ "0.100000"; "0.100000"; "0.100000"; "0.100000" ]);
                     ("sigmoid", [ "0.100000"; "0.100000"; "0.100000"; "0.100000" ]);
                     ("sigmoid", [ "0.100000"; "0.100000"; "0.100000"; "0.100000" ]) ];
                  [ ("sigmoid", [ "0.200000"; "0.200000"; "0.200000"; "0.200000"; "0.200000" ]);
                    ("sigmoid", [ "0.200000"; "0.200000"; "0.200000"; "0.200000"; "0.200000" ]);
                    ("sigmoid", [ "0.200000"; "0.200000"; "0.200000"; "0.200000"; "0.200000" ]);
                    ("sigmoid", [ "0.200000"; "0.200000"; "0.200000"; "0.200000"; "0.200000" ]);
                    ("sigmoid", [ "0.200000"; "0.200000"; "0.200000"; "0.200000"; "0.200000" ]) ];
                  [ ("sigmoid", [ "0.300000"; "0.300000"; "0.300000"; "0.300000"; "0.300000"; "0.300000" ]);
                    ("sigmoid", [ "0.300000"; "0.300000"; "0.300000"; "0.300000"; "0.300000"; "0.300000" ]) ] ]

    [<Fact>]
    let ``get back propagated network of large learning rate``() =
        let learnRate = 0.8
        let netExpOutput = LazyList.ofList [ 0.786; 0.785 ]
        let revInputsWithLayers = Network.fedForward input network
        Network.backPropagated
                    learnRate
                    netExpOutput
                    revInputsWithLayers
        |> networkEq
                [ [ ("sigmoid", [ "0.100004"; "0.100000"; "0.100004"; "0.100009" ]);
                    ("sigmoid", [ "0.100004"; "0.100000"; "0.100004"; "0.100009" ]);
                    ("sigmoid", [ "0.100004"; "0.100000"; "0.100004"; "0.100009" ]);
                    ("sigmoid", [ "0.100004"; "0.100000"; "0.100004"; "0.100009" ]) ];
                  [ ("sigmoid", [ "0.200006"; "0.200003"; "0.200003"; "0.200003"; "0.200003" ]);
                    ("sigmoid", [ "0.200006"; "0.200003"; "0.200003"; "0.200003"; "0.200003" ]);
                    ("sigmoid", [ "0.200006"; "0.200003"; "0.200003"; "0.200003"; "0.200003" ]);
                    ("sigmoid", [ "0.200006"; "0.200003"; "0.200003"; "0.200003"; "0.200003" ]);
                    ("sigmoid", [ "0.200006"; "0.200003"; "0.200003"; "0.200003"; "0.200003" ]) ];
                  [ ("sigmoid", [ "0.300086"; "0.300057"; "0.300057"; "0.300057"; "0.300057"; "0.300057" ]);
                    ("sigmoid", [ "0.299951"; "0.299968"; "0.299968"; "0.299968"; "0.299968"; "0.299968" ]) ] ]

    [<Fact>]
    let ``get back propagated network of zero learning rate``() =
        let learnRate = 0.0
        let netExpOutput = LazyList.ofList [ 0.786; 0.785 ]
        let revInputsWithLayers = Network.fedForward input network
        Network.backPropagated
                    learnRate
                    netExpOutput
                    revInputsWithLayers
        |> networkEq
                [ [ ("sigmoid", [ "0.100000"; "0.100000"; "0.100000"; "0.100000" ]);
                     ("sigmoid", [ "0.100000"; "0.100000"; "0.100000"; "0.100000" ]);
                     ("sigmoid", [ "0.100000"; "0.100000"; "0.100000"; "0.100000" ]);
                     ("sigmoid", [ "0.100000"; "0.100000"; "0.100000"; "0.100000" ]) ];
                  [ ("sigmoid", [ "0.200000"; "0.200000"; "0.200000"; "0.200000"; "0.200000" ]);
                    ("sigmoid", [ "0.200000"; "0.200000"; "0.200000"; "0.200000"; "0.200000" ]);
                    ("sigmoid", [ "0.200000"; "0.200000"; "0.200000"; "0.200000"; "0.200000" ]);
                    ("sigmoid", [ "0.200000"; "0.200000"; "0.200000"; "0.200000"; "0.200000" ]);
                    ("sigmoid", [ "0.200000"; "0.200000"; "0.200000"; "0.200000"; "0.200000" ]) ];
                  [ ("sigmoid", [ "0.300000"; "0.300000"; "0.300000"; "0.300000"; "0.300000"; "0.300000" ]);
                    ("sigmoid", [ "0.300000"; "0.300000"; "0.300000"; "0.300000"; "0.300000"; "0.300000" ]) ] ]

    [<Fact>]
    let ``get fitted network of large errors``() =
        let learnRate = 0.2
        let expOutput = LazyList.ofList [ 0.01; 0.99 ]
        Network.fitted
                    learnRate
                    input
                    expOutput
                    network
        |> networkEq
                [ [ ("sigmoid", [ "0.097938"; "0.100000"; "0.097732"; "0.095464" ]);
                    ("sigmoid", [ "0.097938"; "0.100000"; "0.097732"; "0.095464" ]);
                    ("sigmoid", [ "0.097938"; "0.100000"; "0.097732"; "0.095464" ]);
                    ("sigmoid", [ "0.097938"; "0.100000"; "0.097732"; "0.095464" ]) ];
                  [ ("sigmoid", [ "0.197150"; "0.198273"; "0.198273"; "0.198273"; "0.198273" ]);
                    ("sigmoid", [ "0.197150"; "0.198273"; "0.198273"; "0.198273"; "0.198273" ]);
                    ("sigmoid", [ "0.197150"; "0.198273"; "0.198273"; "0.198273"; "0.198273" ]);
                    ("sigmoid", [ "0.197150"; "0.198273"; "0.198273"; "0.198273"; "0.198273" ]);
                    ("sigmoid", [ "0.197150"; "0.198273"; "0.198273"; "0.198273"; "0.198273" ]) ];
                  [ ("sigmoid", [ "0.273860"; "0.282622"; "0.282622"; "0.282622"; "0.282622"; "0.282622" ]);
                    ("sigmoid", [ "0.306899"; "0.304587"; "0.304587"; "0.304587"; "0.304587"; "0.304587" ]) ] ]

    [<Fact>]
    let ``get fitted network of small errors``() =
        let learnRate = 0.2
        let expOutput = LazyList.ofList [ 0.786; 0.785 ]
        Network.fitted
                    learnRate
                    input
                    expOutput
                    network
        |> networkEq
                [ [ ("sigmoid", [ "0.100001"; "0.100000"; "0.100001"; "0.100002" ]);
                    ("sigmoid", [ "0.100001"; "0.100000"; "0.100001"; "0.100002" ]);
                    ("sigmoid", [ "0.100001"; "0.100000"; "0.100001"; "0.100002" ]);
                    ("sigmoid", [ "0.100001"; "0.100000"; "0.100001"; "0.100002" ]) ];
                  [ ("sigmoid", [ "0.200001"; "0.200001"; "0.200001"; "0.200001"; "0.200001" ]);
                    ("sigmoid", [ "0.200001"; "0.200001"; "0.200001"; "0.200001"; "0.200001" ]);
                    ("sigmoid", [ "0.200001"; "0.200001"; "0.200001"; "0.200001"; "0.200001" ]);
                    ("sigmoid", [ "0.200001"; "0.200001"; "0.200001"; "0.200001"; "0.200001" ]);
                    ("sigmoid", [ "0.200001"; "0.200001"; "0.200001"; "0.200001"; "0.200001" ]) ];
                  [ ("sigmoid", [ "0.300022"; "0.300014"; "0.300014"; "0.300014"; "0.300014"; "0.300014" ]);
                    ("sigmoid", [ "0.299988"; "0.299992"; "0.299992"; "0.299992"; "0.299992"; "0.299992" ]) ] ]

    [<Fact>]
    let ``get fitted network of zero errors``() =
        let learnRate = 0.2
        let expOutput = output
        Network.fitted
                    learnRate
                    input
                    expOutput
                    network
        |> networkEq
                [ [ ("sigmoid", [ "0.100000"; "0.100000"; "0.100000"; "0.100000" ]);
                    ("sigmoid", [ "0.100000"; "0.100000"; "0.100000"; "0.100000" ]);
                    ("sigmoid", [ "0.100000"; "0.100000"; "0.100000"; "0.100000" ]);
                    ("sigmoid", [ "0.100000"; "0.100000"; "0.100000"; "0.100000" ]) ];
                  [ ("sigmoid", [ "0.200000"; "0.200000"; "0.200000"; "0.200000"; "0.200000" ]);
                    ("sigmoid", [ "0.200000"; "0.200000"; "0.200000"; "0.200000"; "0.200000" ]);
                    ("sigmoid", [ "0.200000"; "0.200000"; "0.200000"; "0.200000"; "0.200000" ]);
                    ("sigmoid", [ "0.200000"; "0.200000"; "0.200000"; "0.200000"; "0.200000" ]);
                    ("sigmoid", [ "0.200000"; "0.200000"; "0.200000"; "0.200000"; "0.200000" ]) ];
                  [ ("sigmoid", [ "0.300000"; "0.300000"; "0.300000"; "0.300000"; "0.300000"; "0.300000" ]);
                    ("sigmoid", [ "0.300000"; "0.300000"; "0.300000"; "0.300000"; "0.300000"; "0.300000" ]) ] ]

    let json = """[[{"activationF":"sigmoid","weights":[0.1,0.1,0.1,0.1]},{"activationF":"sigmoid","weights":[0.1,0.1,0.1,0.1]},{"activationF":"sigmoid","weights":[0.1,0.1,0.1,0.1]},{"activationF":"sigmoid","weights":[0.1,0.1,0.1,0.1]}],[{"activationF":"sigmoid","weights":[0.2,0.2,0.2,0.2,0.2]},{"activationF":"sigmoid","weights":[0.2,0.2,0.2,0.2,0.2]},{"activationF":"sigmoid","weights":[0.2,0.2,0.2,0.2,0.2]},{"activationF":"sigmoid","weights":[0.2,0.2,0.2,0.2,0.2]},{"activationF":"sigmoid","weights":[0.2,0.2,0.2,0.2,0.2]}],[{"activationF":"sigmoid","weights":[0.30000000000000004,0.30000000000000004,0.30000000000000004,0.30000000000000004,0.30000000000000004,0.30000000000000004]},{"activationF":"sigmoid","weights":[0.30000000000000004,0.30000000000000004,0.30000000000000004,0.30000000000000004,0.30000000000000004,0.30000000000000004]}]]"""

    [<Fact>]
    let ``get json of network``() =
        Network.toJson network
        |> should equal json

    [<Fact>]
    let ``get network from json``() =
        Network.fromJson json
        |> networkEq
            [ [ ("sigmoid", [ "0.100000"; "0.100000"; "0.100000"; "0.100000" ]);
                ("sigmoid", [ "0.100000"; "0.100000"; "0.100000"; "0.100000" ]);
                ("sigmoid", [ "0.100000"; "0.100000"; "0.100000"; "0.100000" ]);
                ("sigmoid", [ "0.100000"; "0.100000"; "0.100000"; "0.100000" ]) ];
             [ ("sigmoid", [ "0.200000"; "0.200000"; "0.200000"; "0.200000"; "0.200000" ]);
               ("sigmoid", [ "0.200000"; "0.200000"; "0.200000"; "0.200000"; "0.200000" ]);
               ("sigmoid", [ "0.200000"; "0.200000"; "0.200000"; "0.200000"; "0.200000" ]);
               ("sigmoid", [ "0.200000"; "0.200000"; "0.200000"; "0.200000"; "0.200000" ]);
               ("sigmoid", [ "0.200000"; "0.200000"; "0.200000"; "0.200000"; "0.200000" ]) ];
             [ ("sigmoid", [ "0.300000"; "0.300000"; "0.300000"; "0.300000"; "0.300000"; "0.300000" ]);
               ("sigmoid", [ "0.300000"; "0.300000"; "0.300000"; "0.300000"; "0.300000"; "0.300000" ]) ] ]