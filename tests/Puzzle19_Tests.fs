namespace Tests

open System
open Xunit

open Solutions

module Puzzle19Test =
    [<Fact>]
    let ``Puzzle19A returns correct result``() =
        let result = [ ] |> Puzzle19.solveA
        Assert.Equal(1, result)

    [<Fact>]
    let ``Puzzle19B returns correct result``() =
        let result = [ ] |> Puzzle19.solveB
        Assert.Equal(1, result)
