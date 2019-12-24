namespace Solutions

open System

module Puzzle24 =
    type TileState =
        | Empty
        | Bug

    type Tile =
        { Coordinate: Coordinate
          State: TileState }

    let parseTiles (input: string list) =
        input
        |> List.mapi (fun y row ->
            row.ToCharArray()
            |> Array.toList
            |> List.mapi (fun x c ->
                { Coordinate =
                      { x = x
                        y = y }
                  State =
                      if c = '#' then Bug else Empty }))
        |> Shared.flatten

    let findAdjacent (tiles: Tile list) =
        let tilesByCoordinate =
            tiles
            |> List.groupBy (fun t -> t.Coordinate)
            |> List.map (fun (c, t) -> (c, t |> List.head))
            |> Map.ofList

        tiles
        |> List.fold (fun acc t ->
            let up =
                tilesByCoordinate
                |> Map.tryFind
                    { x = t.Coordinate.x
                      y = t.Coordinate.y - 1 }

            let right =
                tilesByCoordinate
                |> Map.tryFind
                    { x = t.Coordinate.x + 1
                      y = t.Coordinate.y }

            let down =
                tilesByCoordinate
                |> Map.tryFind
                    { x = t.Coordinate.x
                      y = t.Coordinate.y + 1 }

            let left =
                tilesByCoordinate
                |> Map.tryFind
                    { x = t.Coordinate.x - 1
                      y = t.Coordinate.y }

            let neighbors = [ up; right; down; left ] |> List.choose id
            Map.add t neighbors acc) Map.empty

    let printTiles (tiles: Tile list) =
        let cellsPerRow =
            tiles
            |> List.sortBy (fun t -> t.Coordinate.y, t.Coordinate.x)
            |> List.groupBy (fun t -> t.Coordinate.y)
            |> List.map snd

        let mark =
            (fun ds ->
                match ds with
                | Empty -> "."
                | Bug -> "#")

        for cells in cellsPerRow do
            for cell in cells do
                printf "%s" (cell.State |> mark)
            printfn ""

        tiles

    let nextState (tiles: Tile list) =
        let neighbors = tiles |> findAdjacent
        tiles
        |> List.map (fun t ->
            let bugs =
                neighbors
                |> Map.find t
                |> List.filter (fun t -> t.State = Bug)
                |> List.length

            match t.State with
            | Bug ->
                if bugs <> 1 then { t with State = Empty } else t
            | Empty ->
                if bugs = 1 || bugs = 2 then { t with State = Bug } else t)

    let findFirstDuplicateGrid (initialGrid: Tile list) =
        let rec helper (state: Tile list) (seen: Tile list Set) =
            let next = state |> nextState
            if Set.contains next seen then next else helper next (Set.add state seen)
        helper initialGrid Set.empty

    let biodiversityRating (tiles: Tile list) =
        tiles
        |> List.mapi (fun i t ->
            if t.State = Bug then Math.Pow(2.0, float i) else 0.0)
        |> List.sum
        |> int64

    let solveA (input: string list) =
        input
        |> parseTiles
        |> findFirstDuplicateGrid
        |> biodiversityRating

    let solveB (input: string list) = 1
