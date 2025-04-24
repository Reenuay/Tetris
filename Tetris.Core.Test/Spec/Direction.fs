[<FsCheck.Xunit.Properties(Arbitrary = [| typeof<Tetris.Core.Test.Arbitrary.Rotation.Extension> |])>]
module Tetris.Core.Test.Spec.Direction

open Tetris.Core
open Tetris.Core.Test
open Tetris.Core.Test.Arbitrary
open FsCheck
open FsCheck.Xunit


[<Property>]
let ``rotate follows correct sequence`` (direction: Direction) (rotation: Rotation) =
    let currentIndex = Direction.all |> List.findIndex ((=) direction)

    let nextIndex =
        match rotation with
        | Rotation.Clockwise -> (currentIndex + 1) % 4
        | Rotation.CounterClockwise -> (currentIndex + 3) % 4

    let expected = List.item nextIndex Direction.all

    Direction.rotate rotation direction ===> expected

[<Property>]
let ``rotate 4n times returns to original direction`` (direction: Direction) (rotation: Rotation) (PositiveInt n) =
    let rotated =
        [ 1 .. n * 4 ]
        |> List.fold (fun acc _ -> Direction.rotate rotation acc) direction

    rotated ===> direction

[<Property>]
let ``rotate will return original direction with equal number of clockwise and counterclockwise rotations``
    (direction: Direction)
    (Rotation.BalancedSequence rotations)
    =
    let result =
        rotations |> List.fold (fun dir rot -> Direction.rotate rot dir) direction

    result ===> direction
