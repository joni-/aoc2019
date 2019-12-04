namespace Solutions

open System

module Puzzle04 =
    let isAscending (chars: char list) =
        let pairs = List.pairwise chars
        let el = List.tryFind (fun (a, b) -> a > b) pairs
        el.IsNone

    let hasDuplicates (chars: char list) =
        chars |> Set.ofList |> Set.count < chars.Length

    let isValid (candidate: int) =
        let chars = candidate.ToString().ToCharArray() |> Array.toList
        chars |> isAscending && chars |> hasDuplicates

    let solveA (input: string list) =
        let realInput = input |> List.head
        let parts = realInput.Split [| '-' |]
        let rangeStart = parts.[0] |> int
        let rangeEnd = parts.[1] |> int

        let mutable count = 0
        for candidate in rangeStart .. 1 .. rangeEnd do
            if candidate |> isValid then
                count <- count + 1
        count

    let solveB (input: string list) = 1
