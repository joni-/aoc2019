namespace Tests

open System
open Xunit

open Solutions

module Puzzle04Test =

    [<Fact>]
    let ``solveA returns correct result``() = Assert.Equal(530, [ "357253-892942" ] |> Puzzle04.solveA)

    [<Fact>]
    let ``solveB returns correct result``() = Assert.Equal(324, [ "357253-892942" ] |> Puzzle04.solveB)
