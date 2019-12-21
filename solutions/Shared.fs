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

        let initialState: Intcode.State =
            { Outputs = List.empty
              Memory = memory
              ExtraMemory = (Array.zeroCreate 0)
              CurrentIndex = 0
              RelativeBase = 0 }
        Intcode.run initialState (inputReader()) |> (fun r -> r.Outputs)

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

        let initialState: Intcode.State =
            { Outputs = List.empty
              Memory = memory
              ExtraMemory = (Array.zeroCreate 0)
              CurrentIndex = 0
              RelativeBase = 0 }
        Intcode.run initialState (inputReader()) |> (fun r -> r.Memory)
