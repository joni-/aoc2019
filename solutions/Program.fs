// Learn more about F# at http://fsharp.org

open System
open Solutions

[<EntryPoint>]
let main argv =
    let path = IO.Path.Combine(__SOURCE_DIRECTORY__, "inputs", "Puzzle06.input")
    let input = path |> IO.File.ReadAllLines |> Seq.toList
    // let input = [ "COM)B"; "B)C"; "C)D"; "D)E"; "E)F"; "B)G"; "G)H"; "D)I"; "E)J"; "J)K"; "K)L" ]
    printfn "%A" (Puzzle06.solveA input)
    0 // return an integer exit code
