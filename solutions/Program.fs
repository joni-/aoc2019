// Learn more about F# at http://fsharp.org

open System
open Solutions

[<EntryPoint>]
let main argv =
    let path = IO.Path.Combine(__SOURCE_DIRECTORY__, "inputs", "Puzzle04.input")
    let input = path |> IO.File.ReadAllLines |> Seq.toList
    // let input = ["357253-892942"]
    printfn "%A" (Puzzle04.solveA input)
    0 // return an integer exit code
