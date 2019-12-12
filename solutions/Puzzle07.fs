namespace Solutions

open System

module Puzzle07 =
    let amplify (phase: int64) (inputSignal: int64) (memory: string) =
        [ memory ]
        |> Shared.simpleIntcodeRun [ phase; inputSignal ]
        |> List.last

    let trusterSignal (memory: string) (phaseSettings: int64 list) =
        phaseSettings |> List.fold (fun output phase -> amplify phase output memory) 0L

    let solveA (input: string list) =
        let memory = input |> List.head
        let l = [ 0L; 1L; 2L; 3L; 4L ] // phase settings

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
