namespace Solutions

open System

module Puzzle01a =
    let fuelAmount (mass: int64) = mass / 3L - 2L

    let fuelAmountWithFuelIncluded (mass: int64) =
        let rec totalFuelRequired (fuelMass: int64) =
            if fuelMass <= 0L then 0L
            else fuelMass + (fuelMass |> fuelAmount |> totalFuelRequired)
        mass |> fuelAmount |> totalFuelRequired

    let solveA (input: string list) =
        input
        |> Seq.sumBy (Int64.Parse >> fuelAmount)

    let solveB (input: string list) =
        input
        |> Seq.sumBy (Int64.Parse >> fuelAmountWithFuelIncluded)
