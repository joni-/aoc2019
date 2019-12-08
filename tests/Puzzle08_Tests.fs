namespace Tests

open System
open Xunit

open Solutions

module Puzzle08Test =
    [<Fact>]
    let ``Create single layer``() =
        let expected =
            [ [ 1; 2; 3 ]
              [ 4; 5; 6 ] ]

        let layer, leftOver = [ 1; 2; 3; 4; 5; 6; 7 ] |> Puzzle08.createLayer 3 2
        Assert.Equal<int list list>(expected, layer)
        Assert.Equal<int list>([ 7 ], leftOver)

    [<Fact>]
    let ``Converting input into layers``() =
        let result = "123456789012" |> Puzzle08.createLayers 3 2

        let expectedLayer1 =
            [ [ 1; 2; 3 ]
              [ 4; 5; 6 ] ]

        let expectedLayer2 =
            [ [ 7; 8; 9 ]
              [ 0; 1; 2 ] ]

        let expected = [ expectedLayer1; expectedLayer2 ]

        Assert.Equal<int list list list>(expected, result)
