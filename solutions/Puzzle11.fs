namespace Solutions

open System

module Puzzle11 =
    type Color =
        | BLACK
        | WHITE

    let move (coordinate: Coordinate) (amount: int) (direction: Direction) =
        match direction with
        | UP -> { coordinate with y = coordinate.y + amount }
        | DOWN -> { coordinate with y = coordinate.y - amount }
        | LEFT -> { coordinate with x = coordinate.x - amount }
        | RIGHT -> { coordinate with x = coordinate.x + amount }

    let run (initialMemory: int64 array) =
        let rec helper (memory: int64 array) (extraMemory: int64 array) (index: int) (relativeBase: int)
                (panelColors: Map<Coordinate, Color>) (instructions: (Coordinate * Color) list) (position: Coordinate)
                (direction: Direction) =
            let currentPanelColor =
                match panelColors |> Map.tryFind position with
                | Some c -> c
                | None -> BLACK

            let inputReader =
                (fun _ ->
                if currentPanelColor = BLACK then 0L
                else 1L)

            let intcodeResult = Intcode.parseOutput memory extraMemory index inputReader relativeBase

            if List.isEmpty intcodeResult.Outputs then
                instructions
            else
                let paintColor =
                    match intcodeResult.Outputs |> List.head with
                    | 0L -> BLACK
                    | 1L -> WHITE
                    | _ ->
                        sprintf "Invalid output color: %A" intcodeResult.Outputs
                        |> Exception
                        |> raise

                // Paint the current position
                let newInstructions = [ position, paintColor ] |> List.append instructions
                let newPanelColors = panelColors |> Map.add position paintColor

                // printfn "Paint %A with %A" position paintColor

                let nextDirection =
                    match intcodeResult.Outputs
                          |> List.tail
                          |> List.head with
                    | 0L ->
                        match direction with
                        | UP -> LEFT
                        | DOWN -> RIGHT
                        | LEFT -> DOWN
                        | RIGHT -> UP
                    | 1L ->
                        match direction with
                        | UP -> RIGHT
                        | DOWN -> LEFT
                        | LEFT -> UP
                        | RIGHT -> DOWN
                    | _ ->
                        sprintf "Invalid output turn direction: %A" intcodeResult.Outputs
                        |> Exception
                        |> raise

                let nextPosition = move position 1 nextDirection

                // // TODO: refactor so no need to concat back to str
                // let a = fst intcodeResult
                // let updatedMemory = String.Join(",", a)

                helper intcodeResult.Memory intcodeResult.ExtraMemory intcodeResult.CurrentIndex
                    intcodeResult.RelativeBase newPanelColors newInstructions nextPosition nextDirection

        let instructions =
            helper initialMemory (Array.zeroCreate 1000000) 0 0 Map.empty List.empty
                { x = 0
                  y = 0 } UP

        instructions

    // let paintInstructions = List.empty

    // let currentPosition =
    //     { x = 0
    //       y = 0 }

    // let currentDirection = UP
    // let currentMemory = fst intcodeResult

    // let initialTiles: Map<Coordinate, Color> = Map.empty
    // initialTiles

    let solveA (input: string list) =
        input
        |> List.head
        |> (fun s -> s.Split [| ',' |])
        |> Array.map int64
        |> run
        |> List.map (fun (c, _) -> c)
        |> Set.ofList
        |> Set.count

    let solveB (input: string list) =
        let coordinates =
            input
            |> List.head
            |> (fun s -> s.Split [| ',' |])
            |> Array.map int64
            |> run
            |> List.map (fun (c, _) -> c)

        let xs = coordinates |> List.map (fun c -> c.x)
        let ys = coordinates |> List.map (fun c -> c.y)

        let minX = xs |> List.min
        let maxX = xs |> List.max
        let minY = ys |> List.min
        let maxY = ys |> List.max

        { x = minX ; y = minY}, { x = maxX ; y = maxY }





