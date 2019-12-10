namespace Solutions

open System

module Intcode =
    type Parameter =
        | Position of int
        | Immediate of int64
        | Relative of int

    type InputParameter =
        | Position of int
        | Relative of int

    type Parameters =
        { Left: Parameter
          Right: Parameter
          Output: InputParameter }


    type Input =
        { Value: int64
          Position: InputParameter }

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
        | AdjustRelativeBase of Parameter
        | Halt

    type GetInput = unit -> int64

    let parseParameter (memory: int64 array) (index: int) (paramMode: int) =
        match paramMode with
        | 0 -> Parameter.Position(int memory.[index])
        | 1 -> Immediate memory.[index]
        | 2 -> Parameter.Relative(int memory.[index])
        | _ ->
            paramMode
            |> sprintf "Invalid parameter mode %A"
            |> Exception
            |> raise

    let parseParameters (memory: int64 array) (index: int) (instruction: int) =
        let left = instruction / 100 % 10 |> parseParameter memory (index + 1)
        let right = instruction / 1000 % 10 |> parseParameter memory (index + 2)
        left, right

    let parseOutputPosition (memory: int64 array) (index: int) (instructionList: int list) =
        let p = int memory.[index + 3]
        if instructionList.Length = 5 then
            if instructionList.Head = 0 then Position p
            elif instructionList.Head = 2 then Relative p
            else Position p
        else
            Position p

    let parseOperation (getInput: GetInput) (memory: int64 array) (index: int) (instruction: int) =
        let instructionList =
            instruction.ToString().ToCharArray()
            |> Array.toList
            |> List.map (string >> int)
        match instructionList with
        | [ 3 ] ->
            Save
                { Value = getInput()
                  Position = Position(int memory.[index + 1]) }
        | [ 2; 0; 3 ] ->
            Save
                { Value = getInput()
                  Position = Relative(int memory.[index + 1]) }

        | [ 4 ] -> Output { Position = Parameter.Position(int memory.[index + 1]) }
        | [ 1; 0; 4 ] -> Output { Position = Immediate memory.[index + 1] }
        | [ 2; 0; 4 ] -> Output { Position = Parameter.Relative(int memory.[index + 1]) }

        | [ 9 ] -> AdjustRelativeBase(Parameter.Position(int memory.[index + 1]))
        | [ 1; 0; 9 ] -> AdjustRelativeBase(Immediate memory.[index + 1])
        | [ 2; 0; 9 ] -> AdjustRelativeBase(Parameter.Relative(int memory.[index + 1]))

        | [ 9; 9 ] -> Halt

        | [ 1 ]
        | [ _; _; 1 ]
        | [ _; _; _; 1 ]
        | [ _; _; _; _; 1 ] ->
            let left, right = parseParameters memory index instruction
            let outputPosition = parseOutputPosition memory index instructionList
            Add
                { Left = left
                  Right = right
                  Output = outputPosition }
        | [ 2 ]
        | [ _; _; 2 ]
        | [ _; _; _; 2 ]
        | [ _; _; _; _; 2 ] ->
            let left, right = parseParameters memory index instruction
            let outputPosition = parseOutputPosition memory index instructionList
            Multiply
                { Left = left
                  Right = right
                  Output = outputPosition }
        | [ 5 ]
        | [ _; _; 5 ]
        | [ _; _; _; 5 ]
        | [ _; _; _; _; 5 ] ->
            let left, right = parseParameters memory index instruction
            JumpIfTrue
                { Predicate = left
                  To = right }
        | [ 6 ]
        | [ _; _; 6 ]
        | [ _; _; _; 6 ]
        | [ _; _; _; _; 6 ] ->
            let left, right = parseParameters memory index instruction
            JumpIfFalse
                { Predicate = left
                  To = right }
        | [ 7 ]
        | [ _; _; 7 ]
        | [ _; _; _; 7 ]
        | [ _; _; _; _; 7 ] ->
            let left, right = parseParameters memory index instruction
            let outputPosition = parseOutputPosition memory index instructionList
            LessThan
                { Left = left
                  Right = right
                  Output = outputPosition }
        | [ 8 ]
        | [ _; _; 8 ]
        | [ _; _; _; 8 ]
        | [ _; _; _; _; 8 ] ->
            let left, right = parseParameters memory index instruction
            let outputPosition = parseOutputPosition memory index instructionList
            Equals
                { Left = left
                  Right = right
                  Output = outputPosition }
        | _ ->
            instruction
            |> sprintf "Invalid operation number: %A"
            |> Exception
            |> raise

    let getValue (memory: int64 array) (extraMemory: int64 array) (relativeBase: int) (p: Parameter) =
        match p with
        | Immediate v -> v
        | _ ->
            let position =
                match p with
                | Parameter.Position i -> i
                | Parameter.Relative i -> relativeBase + i
                | _ ->
                    "Should never happen :))"
                    |> Exception
                    |> raise
                |> int
            if position >= memory.Length then extraMemory.[position]
            else memory.[position]


    let getPosition (relativeBase: int) (p: InputParameter) =
        match p with
        | Position i -> int i
        | Relative i -> relativeBase + int i

    let saveIntoMemory (memory: int64 array) (extraMemory: int64 array) (relativeBase: int) (position: InputParameter) (value: int64) =
        // printfn "  Saving value %A to position %A" value position
        let p = getPosition relativeBase position
        if p >= memory.Length then extraMemory.[p] <- value
        else memory.[p] <- value

    let parse (getInput: GetInput) (input: int64 seq) =
        let rec apply (i: int) (acc: int64 array) (extraMemory: int64 array) (outputs: int64 list) (relativeBase: int) =
            // printfn "%A, %A" acc
            // printfn "  -- Outputs: %A" outputs

            if i >= acc.Length then
                acc, outputs
            else
                let op = parseOperation getInput acc i (int acc.[i])
                // printfn "  -- Next Operation (%A): %A" acc.[i] op
                let getFromMemory = getValue acc extraMemory relativeBase
                let save = saveIntoMemory acc extraMemory relativeBase
                match op with
                | Add p ->
                    let a = getFromMemory p.Left
                    let b = getFromMemory p.Right
                    let output = a + b
                    save p.Output output
                    apply (i + 4) acc extraMemory outputs relativeBase
                | Multiply p ->
                    let a = getFromMemory p.Left
                    let b = getFromMemory p.Right
                    let output = a * b
                    save p.Output output
                    apply (i + 4) acc extraMemory outputs relativeBase
                | Save input ->
                    save input.Position input.Value
                    apply (i + 2) acc extraMemory outputs relativeBase
                | JumpIfTrue cmd ->
                    if getFromMemory cmd.Predicate <> 0L then
                        let jumpTo = getFromMemory cmd.To |> int
                        apply jumpTo acc extraMemory outputs relativeBase
                    else
                        apply (i + 3) acc extraMemory outputs relativeBase
                | JumpIfFalse cmd ->
                    if getFromMemory cmd.Predicate = 0L then
                        let jumpTo = getFromMemory cmd.To |> int
                        apply jumpTo acc extraMemory outputs relativeBase
                    else
                        apply (i + 3) acc extraMemory outputs relativeBase
                | LessThan cmd ->
                    let v =
                        if getFromMemory cmd.Left < getFromMemory cmd.Right then 1L
                        else 0L
                    save cmd.Output v
                    apply (i + 4) acc extraMemory outputs relativeBase
                | Equals cmd ->
                    let v =
                        if getFromMemory cmd.Left = getFromMemory cmd.Right then 1L
                        else 0L
                    save cmd.Output v
                    apply (i + 4) acc extraMemory outputs relativeBase
                | Output output ->
                    let v = getFromMemory output.Position
                    apply (i + 2) acc extraMemory (List.append outputs [ v ]) relativeBase
                | AdjustRelativeBase p ->
                    let v = getFromMemory p |> int
                    apply (i + 2) acc extraMemory outputs (relativeBase + v)
                | Halt -> acc, outputs
        apply 0 (Seq.toArray input) (Array.zeroCreate 10000000) List.empty 0


    let parseOutput (inputs: int64 list) (input: string) =
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
        |> Seq.map int64
        |> parse (inputReader())
