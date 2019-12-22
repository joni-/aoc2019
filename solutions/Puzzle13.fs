namespace Solutions

open System

module Puzzle13 =
    let solveA (input: string list) =
        input
        |> Shared.simpleIntcodeRun List.empty
        |> List.chunkBySize 3
        |> List.filter (fun v ->
            v
            |> List.last = 2L)
        |> List.length

    let solveB (input: string list) = 1
