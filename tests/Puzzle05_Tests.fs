namespace Tests

open System
open Xunit

open Solutions

module Puzzle05Test =
    [<Fact>]
    let ``Parse returns correct output``() =
        Assert.Equal<int list>
            ([ 1 ],
             "3,0,4,0,99"
             |> Puzzle05.parseOutput 1
             |> snd)
        Assert.Equal<int list>
            ([ 2 ],
             "3,0,4,0,99"
             |> Puzzle05.parseOutput 2
             |> snd)

// [<Fact>]
// let ``Puzzle05A returns correct result``() =
//     let result = [] |> Puzzle05.solveA
//     Assert.Equal(1, result)

// [<Fact>]
// let ``Puzzle05B returns correct result``() =
//     let result = [] |> Puzzle05.solveB
//     Assert.Equal(1, result)s
