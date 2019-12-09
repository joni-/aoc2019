namespace Solutions

open System

module Puzzle07 =
    let amplify (phase: int) (inputSignal: int) (memory: string) =
        memory
        |> Intcode.parseOutput [ phase; inputSignal ]
        |> snd
        |> List.last

    let trusterSignal (memory: string) (phaseSettings: int list) =
        phaseSettings |> List.fold (fun output phase -> amplify phase output memory) 0

    let solveA (input: string list) =
        let memory = input |> List.head
        let l = [ 0; 1; 2; 3; 4 ] // phase settings

        let combinations =
            List.collect
                (fun a ->
                List.collect
                    (fun b ->
                    List.collect (fun c -> List.collect (fun d -> List.map (fun e -> [ a; b; c; d; e ]) l) l) l) l) l
            |> List.filter (fun l ->
                l
                |> Set.ofList
                |> Set.count = 5)
        combinations
        |> List.map (trusterSignal memory)
        |> List.max


    let solveB (input: string list) = 1
