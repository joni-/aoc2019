namespace Solutions

open System

module Puzzle02 =
    let updateInitialMemory (v1: int64) (v2: int64) (input: int64 seq) =
        Seq.append (Seq.append [ Seq.head input ] [ v1; v2 ]) (Seq.skip 3 input)

    let solveANoReplace (input: string list) =
        let memory =
            input
            |> Shared.parseIntcodeInput
            |> Array.toSeq

        let output = [ String.Join(",", memory) ] |> Shared.simpleIntcodeRunReturnMemory []
        String.Join(",", output)

    let solveA (input: string list) =
        let memory =
            input
            |> Shared.parseIntcodeInput
            |> Array.toSeq
            |> updateInitialMemory 12L 2L

        let output = [ String.Join(",", memory) ] |> Shared.simpleIntcodeRunReturnMemory []
        String.Join(",", output)

    let solveB (input: string list) =
        let initialMemory =
            input
            |> Shared.parseIntcodeInput
            |> Array.toSeq

        let target = 19690720L

        let candidates =
            seq {
                for noun in 0L .. 1L .. 99L do
                    for verb in 0L .. 1L .. 99L do
                        yield (noun, verb)
            }

        let (noun, verb) =
            candidates
            |> Seq.find (fun (noun, verb) ->
                let memory = initialMemory |> updateInitialMemory noun verb
                let result = [ String.Join(",", memory) ] |> Shared.simpleIntcodeRunReturnMemory []
                result.[0] = target)

        100L * noun + verb
