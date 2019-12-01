namespace Solutions

open System

module Puzzle01a =
    let fuelAmount (mass: int64) = mass / 3L - 2L

    let solve (input: string list) =
        input
        |> Seq.sumBy (Int64.Parse >> fuelAmount)
