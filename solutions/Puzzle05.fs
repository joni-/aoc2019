namespace Solutions

open System

module Puzzle05 =
    let solveA (input: string list) = input |> Shared.simpleIntcodeRun [ 1L ]

    let solveB (input: string list) = input |> Shared.simpleIntcodeRun [ 5L ]
