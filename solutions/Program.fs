// Learn more about F# at http://fsharp.org

open System
open Solutions

[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"
    let path = IO.Path.Combine(__SOURCE_DIRECTORY__, "inputs", "Puzzle02.input")
    let input = path |> IO.File.ReadAllLines |> Seq.toList
    // let input = ["1,1,1,4,99,5,6,0,99"]
    printfn "%A" (Puzzle02.solveA input)
    0 // return an integer exit code
