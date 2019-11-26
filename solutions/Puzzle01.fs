namespace Solutions

open System

module Puzzle01a =
    let solve (input: string list) =
        input
        |> Seq.sumBy Int32.Parse
        |> int
