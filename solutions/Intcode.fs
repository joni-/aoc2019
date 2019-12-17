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

    type State =
        { Outputs: int64 list
          Memory: int64 array
          ExtraMemory: int64 array
          CurrentIndex: int
          RelativeBase: int }

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

    let parseOperation (getInput: GetInput) (state: State) (instruction: int) =
        let instructionList =
            instruction.ToString().ToCharArray()
            |> Array.toList
            |> List.map (string >> int)

        let { Memory = memory; CurrentIndex = index } = state
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
            if position >= memory.Length then extraMemory.[position] else memory.[position]


    let getPosition (relativeBase: int) (p: InputParameter) =
        match p with
        | Position i -> int i
        | Relative i -> relativeBase + int i

    let saveIntoMemory (state: State) (position: InputParameter) (value: int64) =
        let p = getPosition state.RelativeBase position
        if p >= state.Memory.Length then state.ExtraMemory.[p] <- value else state.Memory.[p] <- value

    let runProgram (getInput: GetInput) (initialState: State) =
        let rec runNextInstruction (state: State) =
            let { CurrentIndex = i; Memory = acc; ExtraMemory = extraMemory; RelativeBase = relativeBase } = state

            if state.CurrentIndex >= initialState.Memory.Length then
                { state with CurrentIndex = state.CurrentIndex + 1 }
            elif state.Outputs.Length = 2 then // todo: fix this stop condition
                state
            else
                let op = parseOperation getInput state (int acc.[i])
                let getFromMemory = getValue acc extraMemory relativeBase
                let save = saveIntoMemory state
                match op with
                | Add p ->
                    let a = getFromMemory p.Left
                    let b = getFromMemory p.Right
                    let output = a + b
                    save p.Output output
                    runNextInstruction { state with CurrentIndex = state.CurrentIndex + 4 }
                | Multiply p ->
                    let a = getFromMemory p.Left
                    let b = getFromMemory p.Right
                    let output = a * b
                    save p.Output output
                    runNextInstruction { state with CurrentIndex = state.CurrentIndex + 4 }
                | Save input ->
                    save input.Position input.Value
                    runNextInstruction { state with CurrentIndex = state.CurrentIndex + 2 }
                | JumpIfTrue cmd ->
                    if getFromMemory cmd.Predicate <> 0L then
                        let jumpTo = getFromMemory cmd.To |> int
                        runNextInstruction { state with CurrentIndex = jumpTo }
                    else
                        runNextInstruction { state with CurrentIndex = state.CurrentIndex + 3 }
                | JumpIfFalse cmd ->
                    if getFromMemory cmd.Predicate = 0L then
                        let jumpTo = getFromMemory cmd.To |> int
                        runNextInstruction { state with CurrentIndex = jumpTo }
                    else
                        runNextInstruction { state with CurrentIndex = state.CurrentIndex + 3 }
                | LessThan cmd ->
                    let v =
                        if getFromMemory cmd.Left < getFromMemory cmd.Right
                        then 1L
                        else 0L
                    save cmd.Output v
                    runNextInstruction { state with CurrentIndex = state.CurrentIndex + 4 }
                | Equals cmd ->
                    let v =
                        if getFromMemory cmd.Left = getFromMemory cmd.Right
                        then 1L
                        else 0L
                    save cmd.Output v
                    runNextInstruction { state with CurrentIndex = state.CurrentIndex + 4 }
                | Output output ->
                    let v = getFromMemory output.Position
                    let outputs = List.append state.Outputs [ v ]
                    runNextInstruction
                        { state with
                              CurrentIndex = state.CurrentIndex + 2
                              Outputs = outputs }
                | AdjustRelativeBase p ->
                    let v = getFromMemory p |> int
                    runNextInstruction
                        { state with
                              CurrentIndex = state.CurrentIndex + 2
                              RelativeBase = state.RelativeBase + v }
                | Halt -> { state with CurrentIndex = state.CurrentIndex + 1 }

        initialState |> runNextInstruction


    let run (memory: int64 array) (extraMemory: int64 array) (index: int) (inputReader: GetInput) (relativeBase: int) =
        let initialState =
            { Outputs = List.empty
              Memory = memory
              ExtraMemory = extraMemory
              CurrentIndex = index
              RelativeBase = relativeBase }
        initialState |> runProgram inputReader
