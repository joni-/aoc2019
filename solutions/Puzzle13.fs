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

    let createTile (t: int64 list) =
        let x = t |> List.head

        let y =
            t
            |> List.tail
            |> List.head

        let numericType = t |> List.last

        if x = -1L && y = 0L then
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
              Type = tileType }


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

    // Answer was. 10292
    let solveB (input: string list) =
        let initialBlocks =
            input
            |> Shared.simpleIntcodeRun List.empty
            |> List.chunkBySize 3
            |> List.map createTile
            |> List.filter (fun t -> t.Type = Block)

        let mutable blockStatus =
            initialBlocks
            |> List.map (fun b -> b.Coordinate)
            |> Set.ofList

        let inputReader (state: Intcode.State) =
            let tiles =
                state.Outputs
                |> List.chunkBySize 3
                |> List.map createTile

            let lastBallTile =
                tiles
                |> List.filter (fun t -> t.Type = Ball)
                |> List.last

            let lastPaddleTile =
                tiles
                |> List.filter (fun t -> t.Type = HorizontalPaddle)
                |> List.last

            let lastScoreTile =
                tiles
                |> List.filter (fun t -> t.Coordinate.x = -1)
                |> List.last


            blockStatus <- blockStatus.Remove lastBallTile.Coordinate


            if blockStatus.Count % 10 = 0 then
                printfn "Number of blocks left: %d. Current score: %A" blockStatus.Count lastScoreTile.Type

            // printfn "Number of blocks left: %d" blockStatus.Count
            // printfn "Reading input... Ball(x=%d), Paddle(x=%d)" lastPaddleTile.Coordinate.x lastBallTile.Coordinate.x

            if lastPaddleTile.Coordinate.x < lastBallTile.Coordinate.x
            then 1L
            elif lastPaddleTile.Coordinate.x > lastBallTile.Coordinate.x
            then -1L
            else 0L

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
            |> List.map createTile

        stateAfterOneIteration |> printState
        1
