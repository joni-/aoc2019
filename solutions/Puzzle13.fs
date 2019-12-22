namespace Solutions

open System

module Puzzle13 =
    type TileType =
        | Empty
        | Wall
        | Block
        | HorizontalPaddle
        | Ball
        | Score of int64

    type Tile =
        { Coordinate: Coordinate
          Type: TileType }

    type JoystickPosition =
        | Left
        | Right
        | Neutral

    let mark (tile: Tile) =
        match tile.Type with
        | Empty -> " "
        | Wall -> " "
        | Block -> "#"
        | HorizontalPaddle -> "_"
        | Ball -> "*"
        | Score v -> v.ToString()

    let printState (tiles: Tile list) =
        let tilesPerRow =
            tiles
            |> List.sortBy (fun t -> t.Coordinate.y, t.Coordinate.x)
            |> List.groupBy (fun t -> t.Coordinate.y)
            |> List.map snd

        for tiles in tilesPerRow do
            for tile in tiles do
                printf "%s" (tile |> mark)
            printfn ""


    let solveA (input: string list) =
        input
        |> Shared.simpleIntcodeRun List.empty
        |> List.chunkBySize 3
        |> List.filter (fun v ->
            v
            |> List.last = 2L)
        |> List.length

    let solveB (input: string list) =
        let inputReader (state: Intcode.State) =
            let numberOfBalls =
                state.Outputs
                |> List.chunkBySize 3
                |> List.map (fun t -> t |> List.last)
                |> List.filter (fun v -> v = 4L)
                |> List.length

            printfn "Reading input... Number of balls: %d" numberOfBalls

            let choices = [ -1L; 0L; 1L ]
            choices.[Random().Next(3)]

        let initialMemory = input |> Shared.parseIntcodeInput
        Array.set initialMemory 0 2L

        let initialState: Intcode.State =
            { Outputs = List.empty
              Memory = initialMemory
              ExtraMemory = (Array.zeroCreate 10000)
              CurrentIndex = 0
              RelativeBase = 0 }

        let stateAfterOneIteration =
            Intcode.run initialState inputReader (fun _ -> None)
            |> (fun v -> v.Outputs)
            |> List.chunkBySize 3
            |> List.map (fun t ->
                let x = t |> List.head

                let y =
                    t
                    |> List.tail
                    |> List.head

                let numericType = t |> List.last

                if x = -1L && y = 0L then
                    printfn "Read score tile from %A" t
                    { Coordinate =
                          { x = int x
                            y = int y }
                      Type = Score numericType }
                else
                    let tileType =
                        match List.last t with
                        | 0L -> Empty
                        | 1L -> Wall
                        | 2L -> Block
                        | 3L -> HorizontalPaddle
                        | 4L -> Ball
                        | _ ->
                            sprintf "Invalid tile type %A" t
                            |> Exception
                            |> raise
                    { Coordinate =
                          { x = int x
                            y = int y }
                      Type = tileType })

        stateAfterOneIteration |> printState
        1
1