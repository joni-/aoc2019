// Learn more about F# at http://fsharp.org

open System
open Solutions

[<EntryPoint>]
let main argv =
    let path = IO.Path.Combine(__SOURCE_DIRECTORY__, "inputs", "Puzzle05.input")
    let input = path |> IO.File.ReadAllLines |> Seq.toList
    // let input = ["3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9"]
    printfn "%A" (Puzzle05.solveB input)
    0 // return an integer exit code
