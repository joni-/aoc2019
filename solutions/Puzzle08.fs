namespace Solutions

open System

module Puzzle08 =
    let createLayer (width: int) (height: int) (digits: int list) =
        { 0 .. height - 1 }
        |> Seq.fold (fun (layer, digitsLeft) _ ->
            let row = digitsLeft |> List.take width
            let leftOver = digitsLeft |> List.skip width
            [ row ] |> List.append layer, leftOver) (List.empty, digits)

    let createLayers (width: int) (height: int) (input: string) =
        let layers = input.Length / (width * height)
        let createLayer = createLayer width height

        let digits =
            input.ToCharArray()
            |> Array.toList
            |> List.map (string >> int)

        { 0 .. layers - 1 }
        |> Seq.fold (fun (layers, digitsLeft) _ ->
            let layer, leftOver = createLayer digitsLeft
            [ layer ] |> List.append layers, leftOver) (List.empty, digits)
        |> fst

    let flatten (l: int list list) = l |> List.collect (fun a -> a |> List.map (fun b -> b))

    let countDigits (digit: int) (digits: int list) =
        let count =
            digits
            |> List.countBy id
            |> Map.ofList
            |> Map.tryFind digit
        match count with
        | Some v -> v
        | None -> 0

    let countLayerDigits (digit: int) (layer: int list list) =
        layer
        |> flatten
        |> countDigits digit

    let layerWithFewestDigits (digit: int) (layers: int list list list) =
        let digitCountByLayer = layers |> List.map (countLayerDigits digit)
        let minDigitCount = digitCountByLayer |> List.min
        let layerIndexWithFewestDigits = digitCountByLayer |> List.findIndex (fun count -> count = minDigitCount)
        layers |> List.item layerIndexWithFewestDigits

    let firstNonTransparentPixel (row: int) (col: int) (layers: int array array array) =
        let layer = layers |> Array.find (fun layer -> layer.[row].[col] <> 2)
        layer.[row].[col]

    let solveA (input: string list) =
        let layer =
            input
            |> List.head
            |> createLayers 25 6
            |> layerWithFewestDigits 0
            |> flatten

        let ones = layer |> countDigits 1
        let twos = layer |> countDigits 2
        ones * twos

    let solveB (input: string list) =
        let width, height = 25, 6

        let layers =
            input
            |> List.head
            |> createLayers width height
            |> List.map (fun layer ->
                layer
                |> List.map (fun row -> row |> List.toArray)
                |> List.toArray)
            |> List.toArray


        let pixels =
            seq {
                for row in 0 .. height - 1 do
                    for col in 0 .. width - 1 -> row, col
            }

        let result =
            pixels
            |> Seq.map
                ((fun (row, col) -> layers |> firstNonTransparentPixel row col)
                 >> string
                 >> (fun s ->
                 // Replace zeros with space for better visualisation of the result
                 if s = "0" then " "
                 else s))
            |> Seq.chunkBySize width
            |> Seq.map String.Concat

        String.Join("\n", result)
