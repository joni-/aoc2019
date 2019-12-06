namespace Tests

open System
open Xunit

open Solutions

module Puzzle06Test =
    [<Fact>]
    let ``Puzzle06A returns correct result``() =
        let input = [ "COM)B"; "B)C"; "C)D"; "D)E"; "E)F"; "B)G"; "G)H"; "D)I"; "E)J"; "J)K"; "K)L" ]
        let result = input |> Puzzle06.solveA
        Assert.Equal(42, result)

    [<Fact>]
    let ``Puzzle06B returns correct result``() =
        let result = [] |> Puzzle06.solveB
        Assert.Equal(1, result)
