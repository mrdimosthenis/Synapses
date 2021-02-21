module Model.Encoding.PreprocessorTest

open FSharpx.Collections
open Xunit
open FsUnit.Xunit
open Synapses.Model.Encoding

type ``preprocessor tests``() =

    let datapoint1 = Map.ofList
                        [ ("a", "a1");
                          ("b", "b1");
                          ("c", "-8.0");
                          ("d", "3.0") ]

    let datapoint2 = Map.add "b" "b2" datapoint1
    let datapoint3 = datapoint1
                     |> Map.add "b" "b3"
    let datapoint4 = datapoint1
                     |> Map.add "b" "b4"
                     |> Map.add "d" "5.0"
    let datapoint5 = datapoint1
                     |> Map.add "b" "b5"
                     |> Map.add "d" "4.0"

    let dataset = LazyList.ofList
                    [ datapoint1;
                      datapoint2;
                      datapoint3;
                      datapoint4;
                      datapoint5 ]

    let keysWithFlags = LazyList.ofList
                            [ ("a", true);
                              ("b", true);
                              ("c", false);
                              ("d", false) ]

    let preprocessor = Preprocessor.init keysWithFlags dataset

    let encodedDataset = LazyList.map
                            (Preprocessor.encode preprocessor)
                            dataset

    let decodedDataset = LazyList.map
                            (Preprocessor.decode preprocessor)
                            encodedDataset

    [<Fact>]
    let ``encode dataset``() =
        encodedDataset
        |> LazyList.map LazyList.toList
        |> LazyList.toList
        |> should equal
                    [ [ 1.0; 0.0; 0.0; 0.0; 0.0; 1.0; 0.5; 0.0 ]
                      [ 1.0; 0.0; 0.0; 0.0; 1.0; 0.0; 0.5; 0.0 ]
                      [ 1.0; 0.0; 0.0; 1.0; 0.0; 0.0; 0.5; 0.0 ]
                      [ 1.0; 0.0; 1.0; 0.0; 0.0; 0.0; 0.5; 1.0 ]
                      [ 1.0; 1.0; 0.0; 0.0; 0.0; 0.0; 0.5; 0.5 ] ]

    [<Fact>]
    let ``decode dataset``() =
        decodedDataset
        |> LazyList.toList
        |> should equal
                    [ Map.ofList [ ("a", "a1"); ("b", "b1"); ("c", "-8.000000"); ("d", "3.000000") ]
                      Map.ofList [ ("a", "a1"); ("b", "b2"); ("c", "-8.000000"); ("d", "3.000000") ]
                      Map.ofList [ ("a", "a1"); ("b", "b3"); ("c", "-8.000000"); ("d", "3.000000") ]
                      Map.ofList [ ("a", "a1"); ("b", "b4"); ("c", "-8.000000"); ("d", "5.000000") ]
                      Map.ofList [ ("a", "a1"); ("b", "b5"); ("c", "-8.000000"); ("d", "4.000000") ] ]

    let json = """[{"Case":"SerializableDiscrete","Fields":[{"key":"a","values":["a1"]}]},{"Case":"SerializableDiscrete","Fields":[{"key":"b","values":["b5","b4","b3","b2","b1"]}]},{"Case":"SerializableContinuous","Fields":[{"key":"c","min":-8.0,"max":-8.0}]},{"Case":"SerializableContinuous","Fields":[{"key":"d","min":3.0,"max":5.0}]}]"""

    [<Fact>]
    let ``get json of preprocessor``() =
        Preprocessor.toJson preprocessor
        |> should equal json
    
    [<Fact>]
    let ``get serialized preprocessor from json``() =
        Preprocessor.fromJson json
        |> Preprocessor.serialized
        |> should equal
              [ Preprocessor.SerializableDiscrete
                  { key = "a"
                    values = ["a1"] };
                Preprocessor.SerializableDiscrete
                  { key = "b"
                    values = ["b5"; "b4"; "b3"; "b2"; "b1"] };
                Preprocessor.SerializableContinuous
                  { key = "c"
                    min = -8.0
                    max = -8.0 };
                Preprocessor.SerializableContinuous
                  { key = "d"
                    min = 3.0
                    max = 5.0 } ]
