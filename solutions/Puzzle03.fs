namespace Solutions

open System

module Puzzle03 =
    type Step =
        { Direction: Direction
          Amount: int }

    let parseStep (item: string) =
        let direction =
            match item.[0] with
            | 'U' -> UP
            | 'D' -> DOWN
            | 'L' -> LEFT
            | 'R' -> RIGHT
            | _ ->
                sprintf "Invalid direction %A" item
                |> Exception
                |> raise

        let amount = item.Substring(1) |> int
        { Direction = direction
          Amount = amount }

    let addStep (c: Coordinate) (step: Step) =
        match step.Direction with
        | UP ->
            { 1 .. step.Amount }
            |> Seq.fold (fun coordinates i -> [ { c with y = c.y + i } ] |> List.append coordinates) List.empty
        | DOWN ->
            { 1 .. step.Amount }
            |> Seq.fold (fun coordinates i -> [ { c with y = c.y - i } ] |> List.append coordinates) List.empty
        | RIGHT ->
            { 1 .. step.Amount }
            |> Seq.fold (fun coordinates i -> [ { c with x = c.x + i } ] |> List.append coordinates) List.empty
        | LEFT ->
            { 1 .. step.Amount }
            |> Seq.fold (fun coordinates i -> [ { c with x = c.x - i } ] |> List.append coordinates) List.empty

    let parseCoordinates (wire: string) =
        let steps =
            wire.Split [| ',' |]
            |> Array.toList
            |> List.map parseStep

        steps
        |> List.fold (fun coordinates step ->
            let previous = coordinates |> List.last
            let coords = step |> addStep previous
            List.append coordinates coords)
               [ { x = 0
                   y = 0 } ]

    let solveA (input: string list) =
        let wire1Coordinates =
            input
            |> List.head
            |> parseCoordinates
            |> List.skip 1
            |> Set.ofList
            |> Set.toList

        let wire2Coordinates =
            input
            |> List.tail
            |> List.head
            |> parseCoordinates
            |> List.skip 1
            |> Set.ofList
            |> Set.toList

        List.append wire1Coordinates wire2Coordinates
        |> List.countBy id
        |> List.filter (fun (_, count) -> count = 2)
        |> List.map (fun (c, _) -> abs c.x + abs c.y)
        |> List.min

    let solveB (input: string list) = 1
