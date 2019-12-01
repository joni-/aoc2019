namespace Tests

open System
open Xunit

open Solutions

module Puzzle01Test =
    [<Fact>]
    let ``Puzzle01a returns correct result``() =
        Assert.Equal (2L, [ "12" ] |> Puzzle01a.solveA)
        Assert.Equal (2L, [ "14" ] |> Puzzle01a.solveA)
        Assert.Equal (654L, [ "1969" ] |> Puzzle01a.solveA)
        Assert.Equal (33583L, [ "100756" ] |> Puzzle01a.solveA)
        Assert.Equal (4L, [ "12"; "14" ] |> Puzzle01a.solveA)

    [<Fact>]
    let ``Puzzle01b returns correct result``() =
        Assert.Equal (2L, [ "14" ] |> Puzzle01a.solveB)
        Assert.Equal (966L, [ "1969" ] |> Puzzle01a.solveB)
        Assert.Equal (50346L, [ "100756" ] |> Puzzle01a.solveB)
        Assert.Equal (968L, [ "14"; "1969" ] |> Puzzle01a.solveB)
