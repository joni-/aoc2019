namespace Tests

open System
open Xunit

open Solutions

module Puzzle12Test =

    [<Fact>]
    let ``parseCoordinate returns correct result``() =
        Assert.Equal
            ({ x = -19
               y = -4
               z = 2 }, "<x=-19, y=-4, z=2>" |> Puzzle12.parseCoordinate)

    [<Fact>]
    let ``Puzzle12A returns correct result``() =
        Assert.Equal
            (179,
             [ "<x=-1, y=0, z=2>"; "<x=2, y=-10, z=-7>"; "<x=4, y=-8, z=8>"; "<x=3, y=5, z=-1>" ]
             |> Puzzle12.energyAfter 10)
        Assert.Equal
            (1940,
             [ "<x=-8, y=-10, z=0>"; "<x=5, y=5, z=10>"; "<x=2, y=-7, z=3>"; "<x=9, y=-8, z=-3>" ]
             |> Puzzle12.energyAfter 100)

    [<Fact>]
    let ``Puzzle12B returns correct result``() =
        let result = [] |> Puzzle12.solveB
        Assert.Equal(1, result)
