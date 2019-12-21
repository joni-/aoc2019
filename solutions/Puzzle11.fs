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

    let run (initialPanelColors: Map<Coordinate, Color>) (initialMemory: int64 array) =
        let rec helper (state: Intcode.State) (panelColors: Map<Coordinate, Color>)
                (instructions: (Coordinate * Color) list) (position: Coordinate) (direction: Direction) =
            let currentPanelColor =
                match panelColors |> Map.tryFind position with
                | Some c -> c
                | None -> BLACK

            let inputReader =
                (fun _ -> if currentPanelColor = BLACK then 0L else 1L)

            let intcodeResult = Intcode.run state inputReader

            if List.isEmpty intcodeResult.Outputs then
                panelColors, instructions
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
                helper { intcodeResult with Outputs = List.empty } newPanelColors newInstructions nextPosition
                    nextDirection

        let initialState: Intcode.State =
            { Outputs = List.empty
              Memory = initialMemory
              ExtraMemory = (Array.zeroCreate 1000000)
              CurrentIndex = 0
              RelativeBase = 0 }

        helper initialState initialPanelColors List.empty
            { x = 0
              y = 0 } UP

    let solveA (input: string list) =
        input
        |> List.head
        |> (fun s -> s.Split [| ',' |])
        |> Array.map int64
        |> run Map.empty
        |> snd
        |> List.map (fun (c, _) -> c)
        |> Set.ofList
        |> Set.count

    let solveB (input: string list) =
        let result =
            input
            |> List.head
            |> (fun s -> s.Split [| ',' |])
            |> Array.map int64
            |> run
                (Map.ofList
                    [ { x = 0
                        y = 0 }, WHITE ])

        let coordinates =
            result
            |> snd
            |> List.map (fun (c, _) -> c)

        let colors = result |> fst

        let xs = coordinates |> List.map (fun c -> c.x)
        let ys = coordinates |> List.map (fun c -> c.y)

        let minX = xs |> List.min
        let maxX = xs |> List.max
        let minY = ys |> List.min
        let maxY = ys |> List.max

        let rows =
            { minY .. maxY }
            |> Seq.map (fun row ->
                { minX .. maxX }
                |> Seq.map (fun col ->
                    let coordinate =
                        { x = col
                          y = row }

                    let color =
                        match colors |> Map.tryFind coordinate with
                        | Some WHITE -> "#"
                        | Some BLACK -> " "
                        | None -> " "

                    color)
                |> (fun seq -> String.Join("", seq)))

        for r in rows |> Seq.rev do
            printfn "%A" r
