module StatisticsTest

open Xunit
open FsUnit.Xunit
open Synapses
open FSharp.Data
open FSharpx.Collections

type ``statistics tests``() =
    
    let expectedWithOutputValues =
            Seq.ofList [ ( [ 0.0; 0.0; 1.0], [ 0.0; 0.0; 1.0] )
                         ( [ 0.0; 0.0; 1.0], [ 0.0; 1.0; 1.0] ) ]
    
    [<Fact>]
    let ``root mean square error``() =
        Statistics.rootMeanSquareError expectedWithOutputValues
        |> should equal 0.7071067811865476
