namespace Tests

open System
open Xunit

open Solutions

module Puzzle03Test =
    [<Fact>]
    let ``Parses coordinates``() =
        let result = [ "R2"; "U1"; "D1" ] |> Puzzle03.parseCoordinates
        Assert.Equal
            ({ x = 0
               y = 0 }, List.head result)
        Assert.Equal
            ({ x = 1
               y = 0 }, List.item 1 result)
        Assert.Equal
            ({ x = 2
               y = 0 }, List.item 2 result)
        Assert.Equal
            ({ x = 2
               y = 1 }, List.item 3 result)
        Assert.Equal
            ({ x = 2
               y = 0 }, List.item 4 result)

    [<Fact>]
    let ``Puzzle03A returns correct result``() =
        let result = [] |> Puzzle03.solveA
        Assert.Equal(1, result)

    [<Fact>]
    let ``Puzzle03B returns correct result``() =
        let result = [] |> Puzzle03.solveB
        Assert.Equal(1, result)
