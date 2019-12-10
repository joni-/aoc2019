namespace Tests

open System
open Xunit

open Solutions

module Puzzle09Test =
    // [<Fact>]
    let ``Puzzle09A returns correct result``() =
        Assert.Equal(1125899906842624L, [ "104,1125899906842624,99" ] |> Puzzle09.solveA)
        Assert.Equal(1219070632396864L, [ "1102,34915192,34915192,7,4,7,99,0" ] |> Puzzle09.solveA)

    [<Fact>]
    let ``Puzzle09B returns correct result``() =
        let result = [ ] |> Puzzle09.solveB
        Assert.Equal(1, result)
