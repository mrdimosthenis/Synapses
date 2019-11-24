module Synapses.Model.Encoding.Continuous

open FSharpx.Collections
open Microsoft.VisualBasic
open System.Text.Json.Serialization

type ContinuousAttribute =
        { key: string
          min: float
          max: float }

[<JsonFSharpConverter>]
type SerializableContinuousAttribute =
        ContinuousAttribute

let parse (s: string): float =
        s
        |> Strings.Trim
        |> float

let updated (attribute: ContinuousAttribute)
            (datapoint: Map<string, string>)
            : ContinuousAttribute =
    let v = parse datapoint.[attribute.key]
    let updatedMin =
            match v < attribute.min with
            | true -> v
            | false -> attribute.min
    let updatedMax =
            match v > attribute.max with
            | true -> v
            | false -> attribute.max
    { key = attribute.key
      min = updatedMin
      max = updatedMax }

let init (key: string)
         (dataset: LazyList<Map<string, string>>)
         : ContinuousAttribute =
    let (datasetHead, datasetTail) =
            LazyList.uncons dataset
    let initMinMax = parse datasetHead.[key]
    let initAttribute =
           { key = key
             min = initMinMax
             max = initMinMax }
    LazyList.fold
        updated
        initAttribute
        datasetTail

let encode (attribute: ContinuousAttribute)
           (value: string)
           : LazyList<float> =
    match attribute.min = attribute.max with
    | true ->
        LazyList.ofList [ 0.5 ]
    | false ->
        let v = parse value
        attribute.max - attribute.min
        |> (/) (v - attribute.min)
        |> List.singleton
        |> LazyList.ofList

let decode (attribute: ContinuousAttribute)
           (encodedValues: LazyList<float>)
           : string =
    match attribute.min = attribute.max with
    | true ->
        sprintf "%f" attribute.min
    | false ->
        attribute.max - attribute.min
        |> (*) (LazyList.head encodedValues)
        |> (+) attribute.min
        |> sprintf "%f"

let serialized
        (continuousAttribute: ContinuousAttribute):
        SerializableContinuousAttribute =
    continuousAttribute

let deserialized
    (s: SerializableContinuousAttribute)
    : ContinuousAttribute =
    s
