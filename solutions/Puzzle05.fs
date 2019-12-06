namespace Solutions

open System

module Puzzle05 =
    type Parameter =
        | Position of int
        | Immediate of int

    type Parameters =
        { Left: Parameter
          Right: Parameter
          Output: int }

    type Input =
        { Value: int
          Position: int }

    type Output =
        { Position: int }

    type Operation =
        | Add of Parameters
        | Multiply of Parameters
        | Save of Input
        | Output of Output
        | Halt

    let parseParameters (memory: int array) (index: int) (instruction: int) =
        let left =
            match instruction / 100 % 10 with
            | 0 -> Position memory.[index + 1]
            | 1 -> Immediate memory.[index + 1]
            | _ ->
                instruction
                |> sprintf "Invalid left parameter mode instruction %A"
                |> Exception
                |> raise

        let right =
            match instruction / 1000 % 10 with
            | 0 -> Position memory.[index + 2]
            | 1 -> Immediate memory.[index + 2]
            | _ ->
                instruction
                |> sprintf "Invalid right parameter mode instruction %A"
                |> Exception
                |> raise

        left, right


    let parseOperation (inputOperationValue: int) (memory: int array) (index: int) (instruction: int) =
        match instruction with
        | 3 ->
            Save
                { Value = inputOperationValue
                  Position = memory.[index + 1] }
        | 4
        | 104 -> Output { Position = memory.[index + 1] }
        | 99 -> Halt

        | 1
        | 101
        | 1001
        | 1011
        | 1101
        | 1111 ->
            let left, right = parseParameters memory index instruction
            Add
                { Left = left
                  Right = right
                  Output = memory.[index + 3] }

        | 2
        | 102
        | 1002
        | 1012
        | 1102
        | 1112 ->
            let left, right = parseParameters memory index instruction
            Multiply
                { Left = left
                  Right = right
                  Output = memory.[index + 3] }

        | _ ->
            instruction
            |> sprintf "Invalid operation number: %A"
            |> Exception
            |> raise

    let getValue (memory: int array) (p: Parameter) =
        match p with
        | Position i -> memory.[i]
        | Immediate v -> v

    let parse (inputOperationValue: int) (input: int seq) =
        let rec apply (i: int) (acc: int array) (outputs: int list) =
            if i >= acc.Length then
                acc, outputs
            else
                let op = parseOperation inputOperationValue acc i acc.[i]
                let getFromMemory = getValue acc
                match op with
                | Add p ->
                    let a = getFromMemory p.Left
                    let b = getFromMemory p.Right
                    let output = a + b
                    acc.[p.Output] <- output
                    apply (i + 4) acc outputs
                | Multiply p ->
                    let a = getFromMemory p.Left
                    let b = getFromMemory p.Right
                    let output = a * b
                    acc.[p.Output] <- output
                    apply (i + 4) acc outputs
                | Save input ->
                    acc.[input.Position] <- input.Value
                    apply (i + 2) acc outputs
                | Output output ->
                    let outputValue = acc.[output.Position]
                    apply (i + 2) acc (List.append outputs [ outputValue ])
                | Halt -> acc, outputs
        apply 0 (Seq.toArray input) List.empty


    let parseOutput (inputOperationValue: int) (input: string) =
        input.Split [| ',' |]
        |> Seq.map int
        |> parse inputOperationValue

    let solveA (input: string list) =
        input
        |> List.head
        |> parseOutput 1
        |> snd

    let solveB (input: string list) = 1
