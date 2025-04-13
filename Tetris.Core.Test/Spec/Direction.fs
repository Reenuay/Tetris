[<FsCheck.Xunit.Properties(Arbitrary = [| typeof<Tetris.Core.Test.Arbitrary.Rotation.Extension> |])>]
module Tetris.Core.Test.Spec.Direction

open Tetris.Core
open Tetris.Core.Test
open Tetris.Core.Test.Arbitrary
open FsCheck
open FsCheck.Xunit


let rotationSequence =
    [ Direction.Up; Direction.Right; Direction.Down; Direction.Left ]

[<Property>]
let ``rotate follows correct sequence`` (direction: Direction) (rotation: Rotation) =
    let currentIndex = rotationSequence |> List.findIndex ((=) direction)

    let nextIndex =
        match rotation with
        | Rotation.Clockwise -> (currentIndex + 1) % 4
        | Rotation.CounterClockwise -> (currentIndex + 3) % 4

    let expected = List.item nextIndex rotationSequence

    Direction.rotate rotation direction ===> expected

[<Property>]
let ``rotate n*4 times returns to original direction`` (direction: Direction) (rotation: Rotation) (PositiveInt n) =
    let rotations = n * 4

    let rotateNTimes n dir =
        [ 1..n ] |> List.fold (fun acc _ -> Direction.rotate rotation acc) dir

    rotateNTimes rotations direction ===> direction

[<Property>]
let ``rotate will return original direction with equal number of clockwise and counterclockwise rotations``
    (direction: Direction)
    (Rotation.BalancedSequence rotations)
    =
    let result =
        rotations |> List.fold (fun dir rot -> Direction.rotate rot dir) direction

    result ===> direction
