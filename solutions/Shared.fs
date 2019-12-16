namespace Solutions

open System

type Direction =
    | UP
    | DOWN
    | LEFT
    | RIGHT

type Coordinate =
    { x: int
      y: int }

module Shared =
    let parseIntcodeInput (input: string list) =
        input
        |> List.head
        |> (fun s -> s.Split [| ',' |])
        |> Array.map int64

    let simpleIntcodeRun (inputs: int64 list) (input: string list) =
        let inputReader =
            (fun _ ->
            let mutable inputsLeft = inputs
            (fun _ ->
            match inputsLeft |> List.tryHead with
            | Some v ->
                inputsLeft <- List.tail inputsLeft
                v
            | None ->
                "No more inputs left."
                |> Exception
                |> raise))

        let memory = input |> parseIntcodeInput
        Intcode.parseOutput memory (Array.zeroCreate 0) 0 (inputReader()) 0 |> (fun r -> r.Outputs)

    let simpleIntcodeRunReturnMemory (inputs: int64 list) (input: string list) =
        let inputReader =
            (fun _ ->
            let mutable inputsLeft = inputs
            (fun _ ->
            match inputsLeft |> List.tryHead with
            | Some v ->
                inputsLeft <- List.tail inputsLeft
                v
            | None ->
                "No more inputs left."
                |> Exception
                |> raise))

        let memory = input |> parseIntcodeInput
        Intcode.parseOutput memory (Array.zeroCreate 0) 0 (inputReader()) 0 |> (fun r -> r.Memory)
