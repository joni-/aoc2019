namespace Solutions

open System

module Puzzle09 =
    let solveA (input: string list) =
        input
        |> Shared.simpleIntcodeRun [ 1L ]
        |> List.head

    let solveB (input: string list) =
        input
        |> Shared.simpleIntcodeRun [ 2L ]
        |> List.head
