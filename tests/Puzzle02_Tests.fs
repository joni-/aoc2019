namespace Tests

open System
open Xunit

open Solutions

module Puzzle02Test =
    [<Fact>]
    let ``Puzzle02A returns correct result``() =
        Assert.Equal([ "1,0,0,0,99" ] |> Puzzle02.solveANoReplace, "2,0,0,0,99")
        Assert.Equal([ "2,3,0,3,99" ] |> Puzzle02.solveANoReplace, "2,3,0,6,99")
        Assert.Equal([ "2,4,4,5,99,0" ] |> Puzzle02.solveANoReplace, "2,4,4,5,99,9801")
        Assert.Equal([ "1,1,1,4,99,5,6,0,99" ] |> Puzzle02.solveANoReplace, "30,1,1,4,2,5,6,0,99")
