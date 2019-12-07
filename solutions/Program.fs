// Learn more about F# at http://fsharp.org

open System
open Solutions

[<EntryPoint>]
let main argv =
    let input = match Array.isEmpty argv with
                | true ->
                    let path = IO.Path.Combine(__SOURCE_DIRECTORY__, "inputs", "Puzzle07.input")
                    path |> IO.File.ReadAllLines |> Seq.toList
                | false -> argv |> Array.toList
    printfn "%A" (Puzzle07.solveA input)
    0 // return an integer exit code
