namespace Tests

open System
open Xunit

open Solutions

module Puzzle24Test =
    [<Fact>]
    let ``Puzzle24A returns correct result``() =
        Assert.Equal(2129920L, [ "....#"; "#..#."; "#..##"; "..#.."; "#...." ] |> Puzzle24.solveA)

    [<Fact>]
    let ``Puzzle24B returns correct result``() =
        let result = [] |> Puzzle24.solveB
        Assert.Equal(1, result)
