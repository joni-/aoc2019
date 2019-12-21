namespace Solutions

open System

module Puzzle12 =
    type PositionVelocity = Coordinate3D * Coordinate3D

    type Moon =
        { Id: int
          Position: Coordinate3D
          Velocity: Coordinate3D }

    let rec pairs l =
        match l with
        | []
        | [ _ ] -> []
        | h :: t ->
            [ for x in t do
                yield h, x
                yield! pairs t ]

    let (|RegEx|_|) p i =
        let m = System.Text.RegularExpressions.Regex.Match(i, p)
        if m.Success
        then Some(List.tail [ for g in m.Groups -> g.Value ])
        else None

    let parseCoordinate (v: string) =
        match v with
        | RegEx @"<x=(-?\d+), y=(-?\d+), z=(-?\d+)>" [ x; y; z ] ->
            { x = int x
              y = int y
              z = int z }
        | _ ->
            sprintf "Invalid input coordinate %s" v
            |> Exception
            |> raise

    let applyGravity (a: Moon) (b: Moon) =
        let getDeltaX =
            (fun a b ->
                if a.x = b.x then 0
                elif a.x > b.x then -1
                else 1)

        let getDeltaY =
            (fun a b ->
                if a.y = b.y then 0
                elif a.y > b.y then -1
                else 1)

        let getDeltaZ =
            (fun a b ->
                if a.z = b.z then 0
                elif a.z > b.z then -1
                else 1)

        let nextVeloA =
            { x = a.Velocity.x + (getDeltaX a.Position b.Position)
              y = a.Velocity.y + (getDeltaY a.Position b.Position)
              z = a.Velocity.z + (getDeltaZ a.Position b.Position) }

        let nextVeloB =
            { x = b.Velocity.x + (getDeltaX b.Position a.Position)
              y = b.Velocity.y + (getDeltaY b.Position a.Position)
              z = b.Velocity.z + (getDeltaZ b.Position a.Position) }

        { a with Velocity = nextVeloA }, { b with Velocity = nextVeloB }


    let applyVelocity (moon: Moon) =
        { moon with
              Position =
                  { x = moon.Position.x + moon.Velocity.x
                    y = moon.Position.y + moon.Velocity.y
                    z = moon.Position.z + moon.Velocity.z } }

    let advanceOneStep (moons: Moon list) =
        let moonsById =
            moons
            |> List.groupBy (fun moon -> moon.Id)
            |> List.map (fun v -> fst v, (snd v |> List.head))
            |> Map.ofList

        let moonPairs =
            moons
            |> pairs
            |> Set.ofList
            |> Set.toList

        moonPairs
        |> List.fold (fun acc (a, b) ->
            match acc |> Map.tryFind a.Id with
            | Some aa ->
                match acc |> Map.tryFind b.Id with
                | Some bb ->
                    let newA, newB = applyGravity aa bb
                    acc
                    |> Map.add newA.Id newA
                    |> Map.add newB.Id newB
                | None _ -> acc
            | None -> acc) moonsById
        |> Map.toList
        |> List.map snd
        |> List.sortBy (fun v -> v.Id)
        |> List.map applyVelocity

    let potentialEnergy (moon: Moon) = abs moon.Position.x + abs moon.Position.y + abs moon.Position.z

    let kineticEnergy (moon: Moon) = abs moon.Velocity.x + abs moon.Velocity.y + abs moon.Velocity.z

    let totalEnergy (moon: Moon) = (moon |> potentialEnergy) * (moon |> kineticEnergy)

    let energyAfter (steps: int) (input: string list) =
        let initialState =
            input
            |> List.map (parseCoordinate)
            |> List.mapi (fun i c ->
                { Id = i
                  Position = c
                  Velocity =
                      { x = 0
                        y = 0
                        z = 0 } })

        let finalState = { 0 .. steps - 1 } |> Seq.fold (fun acc _ -> acc |> advanceOneStep) initialState
        finalState |> List.sumBy totalEnergy

    let solveA (input: string list) = input |> energyAfter 1000

    let solveB (input: string list) =
        let initialState =
            input
            |> List.map (parseCoordinate)
            |> List.mapi (fun i c ->
                { Id = i
                  Position = c
                  Velocity =
                      { x = 0
                        y = 0
                        z = 0 } })

        let rec helper (state: Moon list) (counter: int) (seen: Moon list Set) =
            // This needs to be calculated as brute force is too slow.
            let newState = state |> advanceOneStep
            if seen |> Set.contains newState
            then counter
            else helper newState (counter + 1) (seen |> Set.add newState)

        helper initialState 0 Set.empty
