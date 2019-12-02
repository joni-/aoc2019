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

    let solveA (input: string list) =
        let realInput = input |> Seq.head
        let ints =
            realInput.Split [| ',' |]
            |> Seq.map int
            |> Seq.toList
        let x = Seq.append (Seq.append [Seq.head ints] [12; 2]) (Seq.skip 3 ints)
        String.Join(",", parse x)

    let solveB (input: string list) = 1
