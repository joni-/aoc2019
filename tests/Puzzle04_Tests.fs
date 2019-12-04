namespace Tests

open System
open Xunit

open Solutions

module Puzzle04Test =
    // [<Fact>]
    // let ``Puzzle04A returns correct result``() =
    //     let result = [ ] |> Puzzle04.solveA
    //     Assert.Equal(1, result)

    [<Fact>]
    let ``Puzzle04B returns correct result``() =
        let result = [ ] |> Puzzle04.solveB
        Assert.Equal(1, result)
