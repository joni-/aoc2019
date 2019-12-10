namespace Solutions

open System

module Puzzle09 =
    let solveA (input: string list) =
        input
        |> List.head
        |> Intcode.parseOutput [ 1L ]
        |> snd
        |> List.head


    let solveB (input: string list) =
        input
        |> List.head
        |> Intcode.parseOutput [ 2L ]
        |> snd
        |> List.head
