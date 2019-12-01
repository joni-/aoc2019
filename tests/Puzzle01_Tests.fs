namespace Tests

open System
open Xunit

open Solutions

module Puzzle01Test =
    [<Fact>]
    let ``Puzzle01a returns correct result``() =
        Assert.Equal ([ "12" ] |> Puzzle01a.solve, 2L)
        Assert.Equal ([ "14" ] |> Puzzle01a.solve, 2L)
        Assert.Equal ([ "1969" ] |> Puzzle01a.solve, 654L)
        Assert.Equal ([ "100756" ] |> Puzzle01a.solve, 33583L)
        Assert.Equal ([ "12"; "14" ] |> Puzzle01a.solve, 4L)
