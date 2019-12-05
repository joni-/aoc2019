namespace Solutions

open System

module Puzzle04 =
    let isAscending (chars: char list) =
        chars
        |> List.pairwise
        |> List.tryFind (fun (a, b) -> a > b)
        |> Option.isNone

    let hasDuplicates (chars: char list) =
        let uniques =
            chars
            |> Set.ofList
            |> Set.count
        uniques < chars.Length

    let hasAtleastOneDigitExactlyTwice (chars: char list) =
        chars
        |> List.countBy id
        |> List.map (fun (_, count) -> count)
        |> Set.ofList
        |> Set.contains 2

    let isValid (testFn: char list -> bool) (candidate: int) =
        let chars = candidate.ToString().ToCharArray() |> Array.toList
        chars
        |> isAscending
        && chars |> testFn

    let parseRange (input: string) =
        let parts = input.Split [| '-' |]
        let rangeStart = parts.[0] |> int
        let rangeEnd = parts.[1] |> int
        rangeStart, rangeEnd

    let solve (testFn: char list -> bool) (input: string list) =
        let rangeStart, rangeEnd =
            input
            |> List.head
            |> parseRange

        let checkValidity = isValid testFn

        let mutable count = 0
        for candidate in rangeStart .. 1 .. rangeEnd do
            if candidate |> checkValidity then count <- count + 1
        count

    let solveA (input: string list) = input |> solve hasDuplicates

    let solveB (input: string list) = input |> solve hasAtleastOneDigitExactlyTwice
