namespace Solutions

open System

module Intcode =
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
        { Position: Parameter }

    type Jump =
        { Predicate: Parameter
          To: Parameter }

    type Operation =
        | Add of Parameters
        | Multiply of Parameters
        | Save of Input
        | Output of Output
        | JumpIfTrue of Jump
        | JumpIfFalse of Jump
        | LessThan of Parameters
        | Equals of Parameters
        | Halt

    type GetInput = unit -> int

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


    let parseOperation (getInput: GetInput) (memory: int array) (index: int) (instruction: int) =
        match instruction with
        | 3 ->
            Save
                { Value = getInput()
                  Position = memory.[index + 1] }
        | 4 -> Output { Position = Position memory.[index + 1] }
        | 104 -> Output { Position = Immediate memory.[index + 1] }
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

        | 5
        | 105
        | 1005
        | 1015
        | 1105
        | 1115 ->
            let left, right = parseParameters memory index instruction
            JumpIfTrue
                { Predicate = left
                  To = right }

        | 6
        | 106
        | 1006
        | 1016
        | 1106
        | 1116 ->
            let left, right = parseParameters memory index instruction
            JumpIfFalse
                { Predicate = left
                  To = right }

        | 7
        | 107
        | 1007
        | 1017
        | 1107
        | 1117 ->
            let left, right = parseParameters memory index instruction
            LessThan
                { Left = left
                  Right = right
                  Output = memory.[index + 3] }

        | 8
        | 108
        | 1008
        | 1018
        | 1108
        | 1118 ->
            let left, right = parseParameters memory index instruction
            Equals
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

    let parse (getInput: GetInput) (input: int seq) =
        let rec apply (i: int) (acc: int array) (outputs: int list) =
            if i >= acc.Length then
                acc, outputs
            else
                let op = parseOperation getInput acc i acc.[i]
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
                | JumpIfTrue cmd ->
                    if getFromMemory cmd.Predicate <> 0 then
                        let jumpTo = getFromMemory cmd.To
                        apply jumpTo acc outputs
                    else
                        apply (i + 3) acc outputs
                | JumpIfFalse cmd ->
                    if getFromMemory cmd.Predicate = 0 then
                        let jumpTo = getFromMemory cmd.To
                        apply jumpTo acc outputs
                    else
                        apply (i + 3) acc outputs
                | LessThan cmd ->
                    if getFromMemory cmd.Left < getFromMemory cmd.Right then acc.[cmd.Output] <- 1
                    else acc.[cmd.Output] <- 0
                    apply (i + 4) acc outputs
                | Equals cmd ->
                    if getFromMemory cmd.Left = getFromMemory cmd.Right then acc.[cmd.Output] <- 1
                    else acc.[cmd.Output] <- 0
                    apply (i + 4) acc outputs
                | Output output ->
                    let v = getFromMemory output.Position
                    apply (i + 2) acc (List.append outputs [ v ])
                | Halt -> acc, outputs
        apply 0 (Seq.toArray input) List.empty


    let parseOutput (inputs: int list) (input: string) =
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

        input.Split [| ',' |]
        |> Seq.map int
        |> parse (inputReader())
