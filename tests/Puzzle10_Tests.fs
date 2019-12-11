namespace Tests

open System
open Xunit

open Solutions

module Puzzle10Test =
    [<Fact>]
    let ``Puzzle10A returns correct result``() =
        let result = [ ] |> Puzzle10.solveA
        Assert.Equal(1, result)

    [<Fact>]
    let ``Puzzle10B returns correct result``() =
        let result = [ ] |> Puzzle10.solveB
        Assert.Equal(1, result)
