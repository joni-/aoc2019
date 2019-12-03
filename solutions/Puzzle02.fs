namespace Solutions

open System

module Puzzle02 =
    type Position =
        { Left: int
          Right: int
          Output: int }

    type Operation =
        | Add of Position
        | Multiply of Position
        | Halt

    let parse (input: int seq) =
        let rec apply (i: int) (acc: int array) =
            if i >= acc.Length then
                acc
            else
                let curr = acc.[i]

                let op =
                    match curr with
                    | 1 ->
                        Add
                            { Left = acc.[i + 1]
                              Right = acc.[i + 2]
                              Output = acc.[i + 3] }
                    | 2 ->
                        Multiply
                            { Left = acc.[i + 1]
                              Right = acc.[i + 2]
                              Output = acc.[i + 3] }
                    | 99 -> Halt
                    | _ -> raise (Exception "Invalid number...")

                match op with
                | Add p ->
                    let a = acc.[p.Left]
                    let b = acc.[p.Right]
                    let output = a + b
                    acc.[p.Output] <- output
                    apply (i + 4) acc
                | Multiply p ->
                    let a = acc.[p.Left]
                    let b = acc.[p.Right]
                    let output = a * b
                    acc.[p.Output] <- output
                    apply (i + 4) acc
                | Halt -> acc
        apply 0 (Seq.toArray input)

    let parseInput (input: string list) =
        let actualInput = input |> Seq.head
        actualInput.Split [| ',' |] |> Seq.map int

    let updateInitialMemory (v1: int) (v2: int) (input: int seq) =
        Seq.append (Seq.append [ Seq.head input ] [ v1; v2 ]) (Seq.skip 3 input)

    let solveANoReplace (input: string list) =
        let memory = input |> parseInput
        String.Join(",", parse memory)

    let solveA (input: string list) =
        let memory =
            input
            |> parseInput
            |> updateInitialMemory 12 2
        String.Join(",", parse memory)

    let solveB (input: string list) =
        let initialMemory = input |> parseInput
        let target = 19690720

        let candidates =
            seq {
                for noun in 0 .. 1 .. 99 do
                    for verb in 0 .. 1 .. 99 do
                        yield (noun, verb)
            }

        let (noun, verb) =
            candidates
            |> Seq.find (fun (noun, verb) ->
                let memory = initialMemory |> updateInitialMemory noun verb
                let result = parse memory
                result.[0] = target)

        100 * noun + verb
