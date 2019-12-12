namespace Tests

open System
open Xunit

open Solutions

module IntcodeTest =
    [<Fact>]
    let ``Parse returns correct output``() =
        Assert.Equal<int64 list>([ 1L ], [ "3,0,4,0,99" ] |> Shared.simpleIntcodeRun [ 1L ])
        Assert.Equal<int64 list>([ 2L ], [ "3,0,4,0,99" ] |> Shared.simpleIntcodeRun [ 2L ])
