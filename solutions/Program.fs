// Learn more about F# at http://fsharp.org

open System
open System.Reflection
open Solutions

let invokePuzzleSolver (puzzleNumber: string) (part: string) (args: string list) =
    let asm = Assembly.GetExecutingAssembly()
    let fn = match asm.GetTypes() |> Array.tryFind (fun t -> t.FullName = sprintf "Solutions.Puzzle%s" puzzleNumber) with
                | Some t -> t.GetMethods() |> Array.tryFind (fun m -> m.IsStatic && m.Name = sprintf "solve%s" part)
                | None -> None
    match fn with
    | Some method -> method.Invoke(null, [| args |])
    | None -> "Could not find a solver function to invoke. Check the CLI params" |> Exception |> raise

[<EntryPoint>]
let main argv =
    if argv.Length < 2 then
        "Missing required params: <PuzzleNumber> <Part>" |> Exception |> raise

    let puzzleNumber = argv.[0]
    let part = argv.[1]

    let input = match argv.Length = 2 with
                | true ->
                    let path = IO.Path.Combine(__SOURCE_DIRECTORY__, "inputs", sprintf "Puzzle%s.input" puzzleNumber)
                    path |> IO.File.ReadAllLines |> Seq.toList
                | false -> argv |> Array.toList |> List.skip 2

    let result = invokePuzzleSolver puzzleNumber part input
    printfn "%A" result

    0 // return an integer exit code
