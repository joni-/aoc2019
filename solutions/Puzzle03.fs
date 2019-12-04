namespace Solutions

open System

module Puzzle03 =
    let parseDirection (item: string) =
        let direction =
            match item.[0] with
            | 'U' -> UP
            | 'D' -> DOWN
            | 'L' -> LEFT
            | 'R' -> RIGHT
            | _ -> raise (Exception "Invalid direction")
        let count = item.Substring(1) |> int
        direction, count

    let appendCoordinates (acc: Coordinate list) (direction: Direction) (amount: int) =
        let last = List.last acc
        // TODO: Append coordinates
        1

    let parseCoordinates (input: string list) =
        [ { x = 0
            y = 0 } ]

    let solveA (input: string list) = 1

    let solveB (input: string list) = 1
