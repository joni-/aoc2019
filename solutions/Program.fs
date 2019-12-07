// Learn more about F# at http://fsharp.org

open System
open Solutions

[<EntryPoint>]
let main argv =
    let path = IO.Path.Combine(__SOURCE_DIRECTORY__, "inputs", "Puzzle05.input")
    let input = path |> IO.File.ReadAllLines |> Seq.toList
    // let input = [ "3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0" ]
    printfn "%A" (Puzzle05.solveB input)
    0 // return an integer exit code
