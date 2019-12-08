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

    let addStep (c: Coordinate) (stepsTaken: int) (step: Step) =
        match step.Direction with
        | UP ->
            { 1 .. step.Amount }
            |> Seq.fold (fun coordinates i -> [ ({ c with y = c.y + i }, stepsTaken + i) ] |> List.append coordinates)
                   List.empty
        | DOWN ->
            { 1 .. step.Amount }
            |> Seq.fold (fun coordinates i -> [ ({ c with y = c.y - i }, stepsTaken + i) ] |> List.append coordinates)
                   List.empty
        | RIGHT ->
            { 1 .. step.Amount }
            |> Seq.fold (fun coordinates i -> [ ({ c with x = c.x + i }, stepsTaken + i) ] |> List.append coordinates)
                   List.empty
        | LEFT ->
            { 1 .. step.Amount }
            |> Seq.fold (fun coordinates i -> [ ({ c with x = c.x - i }, stepsTaken + i) ] |> List.append coordinates)
                   List.empty

    let parseCoordinates (wire: string) =
        let steps =
            wire.Split [| ',' |]
            |> Array.toList
            |> List.map parseStep

        steps
        |> List.fold (fun coordinates step ->
            let latestItem = coordinates |> List.last
            let previousCoordinate = latestItem |> fst
            let previousStepsTaken = latestItem |> snd
            let coords = step |> addStep previousCoordinate previousStepsTaken
            List.append coordinates coords)
               [ ({ x = 0
                    y = 0 }, 0) ]

    let solveA (input: string list) =
        let wire1Coordinates =
            input
            |> List.head
            |> parseCoordinates
            |> List.map fst
            |> List.skip 1
            |> Set.ofList
            |> Set.toList

        let wire2Coordinates =
            input
            |> List.tail
            |> List.head
            |> parseCoordinates
            |> List.map fst
            |> List.skip 1
            |> Set.ofList
            |> Set.toList

        List.append wire1Coordinates wire2Coordinates
        |> List.countBy id
        |> List.filter (fun (_, count) -> count = 2)
        |> List.map (fun (c, _) -> abs c.x + abs c.y)
        |> List.min

    let solveB (input: string list) =
        let wire1CoordinatesAndSteps =
            input
            |> List.head
            |> parseCoordinates
            |> List.skip 1

        let wire2CoordinatesAndSteps =
            input
            |> List.tail
            |> List.head
            |> parseCoordinates
            |> List.skip 1

        let wire1Coordinates =
            wire1CoordinatesAndSteps
            |> List.map fst
            |> Set.ofList
            |> Set.toList

        let wire2Coordinates =
            wire2CoordinatesAndSteps
            |> List.map fst
            |> Set.ofList
            |> Set.toList

        let intersections =
            List.append wire1Coordinates wire2Coordinates
            |> List.countBy id
            |> List.filter (fun (_, count) -> count = 2)
            |> List.map fst

        intersections
        |> List.map (fun target ->
            let wire1Steps =
                wire1CoordinatesAndSteps
                |> List.find (fun c -> fst c = target)
                |> snd

            let wire2Steps =
                wire2CoordinatesAndSteps
                |> List.find (fun c -> fst c = target)
                |> snd

            wire1Steps + wire2Steps)
        |> List.min
