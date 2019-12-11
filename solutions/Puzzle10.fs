namespace Solutions

open System

module Puzzle10 =
    let flatten (l: 'a list list) = l |> List.collect (fun a -> a |> List.map (fun b -> b))

    let parseAsteroidCoordinates (input: string list) =
        input
        |> List.mapi (fun row s ->
            s.ToCharArray()
            |> Array.toList
            |> List.mapi (fun col c -> row, col, c)
            |> List.filter (fun (_, _, c) -> c = '#')
            |> List.map (fun (r, c, _) ->
                { x = c
                  y = r }))
        |> flatten

    let solveA (input: string list) = 1

    let solveB (input: string list) = 1
