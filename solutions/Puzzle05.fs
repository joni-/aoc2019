namespace Solutions

open System

module Puzzle05 =
    let solveA (input: string list) =
        input
        |> List.head
        |> Intcode.parseOutput [ 1L ]
        |> snd

    let solveB (input: string list) =
        input
        |> List.head
        |> Intcode.parseOutput [ 5L ]
        |> snd
