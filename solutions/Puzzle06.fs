namespace Solutions

open System

module Puzzle06 =
    let solve (input: Map<string, string list>) =
        let rec helper (parentCount: int) (name: string) =
            match input.TryFind name with
            | Some children ->
                children
                |> List.sumBy (fun node ->
                    let count = parentCount + 1
                    count + helper count node)
            | None -> 0
        helper 0 "COM"

    let buildMap (input: string list) =
        input
        |> List.map (fun entry ->
            let parts = entry.Split [| ')' |]
            parts.[0], parts.[1])
        |> List.groupBy (fun (a, b) -> a)
        |> List.map (fun (a, b) -> a, b |> List.map snd)
        |> Map.ofList

    let solveA (input: string list) =
        input
        |> buildMap
        |> solve

    let solveB (input: string list) = 1
