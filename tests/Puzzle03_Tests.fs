namespace Tests

open System
open Xunit

open Solutions

module Puzzle03Test =
    [<Fact>]
    let ``Parses coordinates``() =
        let result = "R2,U1,D1" |> Puzzle03.parseCoordinates
        Assert.Equal<(Coordinate * int) list>
            ([ ({ x = 0
                  y = 0 }, 0)
               ({ x = 1
                  y = 0 }, 1)
               ({ x = 2
                  y = 0 }, 2)
               ({ x = 2
                  y = 1 }, 3)
               ({ x = 2
                  y = 0 }, 4) ], result)

    [<Fact>]
    let ``Puzzle03A returns correct result``() =
        Assert.Equal
            ([ "R75,D30,R83,U83,L12,D49,R71,U7,L72"; "U62,R66,U55,R34,D71,R55,D58,R83" ] |> Puzzle03.solveA, 159)
        Assert.Equal
            ([ "R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51"; "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7" ]
             |> Puzzle03.solveA, 135)

    [<Fact>]
    let ``Puzzle03B returns correct result``() =
        Assert.Equal
            ([ "R75,D30,R83,U83,L12,D49,R71,U7,L72"; "U62,R66,U55,R34,D71,R55,D58,R83" ] |> Puzzle03.solveB, 610)
        Assert.Equal
            ([ "R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51"; "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7" ]
             |> Puzzle03.solveB, 410)
