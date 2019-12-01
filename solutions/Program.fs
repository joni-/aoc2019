// Learn more about F# at http://fsharp.org

open System
open Solutions

[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"
    let path = IO.Path.Combine(__SOURCE_DIRECTORY__, "inputs", "Puzzle01.input")
    let input = path |> IO.File.ReadAllLines |> Seq.toList
    printfn "%A" (Puzzle01a.solveB input)
    0 // return an integer exit code
