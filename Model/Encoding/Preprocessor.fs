module Synapses.Model.Encoding.Preprocessor

open FSharpx.Collections
open Synapses
open Synapses.Model.Encoding
open Synapses.Model.Encoding.Discrete
open Synapses.Model.Encoding.Continuous
open System.Text.Json
open System.Text.Json.Serialization

type Attribute =
        | Discrete of Discrete.DiscreteAttribute
        | Continuous of Continuous.ContinuousAttribute

[<JsonFSharpConverter>]
type SerializableAttribute =
        | SerializableDiscrete
            of Discrete.SerializableDiscreteAttribute
        | SerializableContinuous
            of Continuous.SerializableContinuousAttribute

type Preprocessor = LazyList<Attribute>

[<JsonFSharpConverter>]
type SerializablePreprocessor = List<SerializableAttribute>

let updated (preprocessor: Preprocessor)
            (datapoint: Map<string, string>)
            : Preprocessor =
    LazyList.map
        (fun attr ->
            match attr with
            | Discrete att ->
                Discrete.updated att datapoint
                |> Discrete
            | Continuous att ->
                Continuous.updated att datapoint
                |> Continuous
        )
        preprocessor

let init (keysWithFlags: LazyList<string * bool>)
         (dataset: LazyList<Map<string, string>>)
         : Preprocessor =
    let (datasetHead, datasetTail) =
            LazyList.uncons dataset
    let initPreprocessor =
            LazyList.map
                (fun (key, isDiscrete) ->
                    match isDiscrete with
                    | true ->
                        let att: Discrete.DiscreteAttribute =
                            { key = key
                              values = LazyList.ofList
                                        [ datasetHead.[key] ] }
                        Discrete att
                    | false ->
                        let v = Continuous.parse
                                    datasetHead.[key]
                        { key = key
                          min = v
                          max = v }
                        |> Continuous
                )
                keysWithFlags
    LazyList.fold
        updated
        initPreprocessor
        datasetTail

let encode (preprocessor: Preprocessor)
           (datapoint: Map<string, string>)
           : LazyList<float> =
    preprocessor
    |> LazyList.map
        (fun attr ->
            match attr with
            | Discrete att ->
                Discrete.encode
                    att
                    datapoint.[att.key]
            | Continuous att ->
                Continuous.encode
                    att
                    datapoint.[att.key]
        )
    |> LazyList.concat

let decodeAccF
        (acc: LazyList<float> * LazyList<string * string>)
        (attr: Attribute)
        : LazyList<float> * LazyList<string * string> =
    let (unprocessedFloats, processedKsVs) = acc
    let (key, splitIndex) =
                match attr with
                | Discrete att ->
                    (
                        att.key,
                        LazyList.length
                            att.values
                    )
                | Continuous att ->
                    (att.key, 1)
    let (encodedValues, nextFloats) =
            Utilities.lazySplitAt
                splitIndex
                unprocessedFloats
    let decodedValue =
            match attr with
            | Discrete att ->
                Discrete.decode
                    att
                    encodedValues
            | Continuous att ->
                Continuous.decode
                    att
                    encodedValues
    let nextKsVs =
            LazyList.cons
                (key, decodedValue)
                processedKsVs
    (nextFloats, nextKsVs)

let decode (preprocessor: Preprocessor)
           (encodedDatapoint: LazyList<float>)
           : Map<string, string> =
    let (_, kvs) =
        LazyList.fold
            decodeAccF
            (encodedDatapoint, LazyList.empty)
            preprocessor
    kvs
    |> LazyList.toList
    |> Map.ofList

let serialized
        (preprocessor: Preprocessor):
        SerializablePreprocessor =
    preprocessor
    |> LazyList.map
        (fun attr ->
            match attr with
            | Discrete att ->
                att
                |> Discrete.serialized
                |> SerializableDiscrete
            | Continuous att ->
                att
                |> Continuous.serialized
                |> SerializableContinuous
        )
    |> LazyList.toList

let deserialized
        (s: SerializablePreprocessor):
        Preprocessor =
    s
    |> LazyList.ofList
    |> LazyList.map
        (fun attr ->
            match attr with
            | SerializableDiscrete att ->
                att
                |> Discrete.deserialized
                |> Discrete
            | SerializableContinuous att ->
                att
                |> Continuous.deserialized
                |> Continuous
        )

let toJson (preprocessor: Preprocessor): string =
    JsonSerializer.Serialize
        (serialized preprocessor, Utilities.jsonOptions)

let fromJson (json: string): Preprocessor =
    (json, Utilities.jsonOptions)
    |> JsonSerializer.Deserialize<SerializablePreprocessor>
    |> deserialized
