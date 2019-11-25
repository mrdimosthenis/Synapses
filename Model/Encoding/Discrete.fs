module Synapses.Model.Encoding.Discrete

open FSharpx.Collections
open System.Text.Json.Serialization

type DiscreteAttribute =
        { key: string
          values: LazyList<string> }

[<JsonFSharpConverter>]
type SerializableDiscreteAttribute =
        { key: string
          values: List<string> }

let updated (attribute: DiscreteAttribute)
            (datapoint: Map<string, string>)
            : DiscreteAttribute =
    let updatedValues =
               let exists = attribute.values
                            |> LazyList.tryFind
                                   ((=) datapoint.[attribute.key])
                            |> Option.isSome
               match exists with
               | true -> attribute.values
               | false -> LazyList.cons
                            datapoint.[attribute.key]
                            attribute.values
    { key = attribute.key
      values = updatedValues }

let init (key: string)
         (dataset: LazyList<Map<string, string>>)
         : DiscreteAttribute =
    let values = dataset
                 |> LazyList.fold
                    (fun acc datapoint ->
                        Set.add datapoint.[key] acc
                    )
                    Set.empty
                 |> Set.toList
                 |> LazyList.ofList
    { key = key
      values = values }

let encode (attribute: DiscreteAttribute)
           (value: string)
           : LazyList<float> =
    attribute.values
    |> LazyList.map
        (fun v ->
            match v = value with
            | true -> 1.0
            | false -> 0.0
        )

let decode (attribute: DiscreteAttribute)
           (encodedValues: LazyList<float>)
           : string =
    let zippedValues = LazyList.zip
                            attribute.values
                            encodedValues
    let (zippedHead, zippedTail) =
            LazyList.uncons zippedValues
    let (maxV, _) =
            LazyList.fold
                (fun (accV, accX) (v, x) ->
                    match x > accX with
                    | true -> (v, x)
                    | false -> (accV, accX)
                )
                zippedHead
                zippedTail
    maxV

let serialized
        (discreteAttribute: DiscreteAttribute):
        SerializableDiscreteAttribute =
    { key =
        discreteAttribute.key
      values =
          LazyList.toList discreteAttribute.values }

let deserialized
    (s: SerializableDiscreteAttribute)
    : DiscreteAttribute =
    { key =
        s.key
      values =
          LazyList.ofList s.values }
 