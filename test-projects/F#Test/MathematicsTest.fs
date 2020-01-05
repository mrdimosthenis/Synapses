module MathematicsTest

open FSharpx.Collections
open Synapses
open Xunit
open FsUnit.Xunit

type ``mathematics tests``() =
    
    let v1 = LazyList.ofList [ 1.0; 0.0; 0.0]
    let v2 = LazyList.ofList [ 0.0; 1.0; 0.0]
    let v3 = LazyList.ofList [ 0.0; 0.0; 1.0]
    let v4 = LazyList.ofList [ 0.0; 1.0; 1.0]
    
    [<Fact>]
    let ``RMSE zero a``() =
        let expectedWithOutputValues = LazyList.ofList [ (v1, v1) ]
        Mathematics.rootMeanSquareError expectedWithOutputValues
        |> should equal 0.0

    [<Fact>]
    let ``RMSE zero b``() =
        let expectedWithOutputValues = LazyList.ofList [ (v1, v1)
                                                         (v2, v2)
                                                         (v3, v3)
                                                         (v4, v4) ]
        Mathematics.rootMeanSquareError expectedWithOutputValues
        |> should equal 0.0

    [<Fact>]
    let ``RMSE c``() =
        let expectedWithOutputValues = LazyList.ofList [ (v3, v3)
                                                         (v3, v4) ]
        Mathematics.rootMeanSquareError expectedWithOutputValues
        |> should equal 0.7071067811865476
    