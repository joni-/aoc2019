namespace Tests

open System
open Xunit

open Solutions

module Puzzle01Test =
    [<Fact>]
    let ``Puzzle01a returns correct result``() =
        let result = [ "1"; "2" ] |> Puzzle01a.solve
        Assert.Equal(result, 3)
