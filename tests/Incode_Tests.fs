namespace Tests

open System
open Xunit

open Solutions

module IntcodeTest =
    [<Fact>]
    let ``Parse returns correct output``() =
        Assert.Equal<int list>
            ([ 1 ],
             "3,0,4,0,99"
             |> Intcode.parseOutput [1]
             |> snd)
        Assert.Equal<int list>
            ([ 2 ],
             "3,0,4,0,99"
             |> Intcode.parseOutput [2]
             |> snd)
