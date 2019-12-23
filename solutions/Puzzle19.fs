namespace Solutions

open System

module Puzzle19 =
    type DroneState =
        | Stationary
        | Pulled

    type CellState =
        { Coordinate: Coordinate
          State: DroneState }

    let createGrid (size: int) =
        { 0 .. size - 1 }
        |> Seq.collect (fun y ->
            { 0 .. size - 1 }
            |> Seq.map (fun x ->
                { x = x
                  y = y }))
        |> Seq.toList

    let initCells (coordinates: Coordinate list) =
        coordinates
        |> List.map (fun c ->
            { Coordinate = c
              State = Stationary })

    let printState (tiles: CellState list) =
        let cellsPerRow =
            tiles
            |> List.sortBy (fun t -> t.Coordinate.y, t.Coordinate.x)
            |> List.groupBy (fun t -> t.Coordinate.y)
            |> List.map snd

        let mark =
            (fun ds ->
                match ds with
                | Stationary -> "."
                | Pulled -> "#")

        for cells in cellsPerRow do
            for cell in cells do
                printf "%s" (cell.State |> mark)
            printfn ""


    let solveA (input: string list) =
        let coordinates = 50 |> createGrid

        let rec helper (coordinatesLeft: Coordinate list) (outputs: int64 list) =
            if coordinatesLeft |> List.isEmpty then
                outputs
            else
                let initialState: Intcode.State =
                    { Outputs = List.empty
                      Memory = input |> Shared.parseIntcodeInput
                      ExtraMemory = (Array.zeroCreate 100000)
                      CurrentIndex = 0
                      RelativeBase = 0 }

                let c = coordinatesLeft |> List.head

                let inputReader _ =
                    let coords = [ c.x; c.y ]
                    let mutable next = 0

                    let read _ =
                        let v = coords.[next]
                        next <- next + 1
                        int64 v
                    read

                let nextState = Intcode.run initialState (inputReader()) (fun _ -> None)
                helper (coordinatesLeft |> List.tail) (List.append outputs nextState.Outputs)

        let outputs = helper coordinates List.empty |> List.toArray

        coordinates
        |> List.mapi (fun i c ->
            { Coordinate = c
              State =
                  match outputs.[i] with
                  | 0L -> Stationary
                  | 1L -> Pulled
                  | _ ->
                      sprintf "Invalid output %d" outputs.[i]
                      |> Exception
                      |> raise })
        |> List.filter (fun s -> s.State = Pulled)
        |> List.length

    let solveB (input: string list) = 1
