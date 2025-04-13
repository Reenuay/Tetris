[<FsCheck.Xunit.Properties(Arbitrary = [| typeof<Tetris.Core.Test.Arbitrary.Playfield.Extension> |])>]
module Tetris.Core.Test.Spec.Playfield

open Tetris.Core
open Tetris.Core.Test
open Tetris.Core.Test.Arbitrary
open FsCheck.Xunit


[<Property>]
let ``tryCreate returns Error when width is too small`` (Playfield.InvalidWidth width) (Playfield.ValidHeight height) =
    Playfield.tryCreate width height
    <=> Error ![ Playfield.WidthTooSmall(Playfield.minWidth, width) ]

[<Property>]
let ``tryCreate returns Error when height is too small`` (Playfield.ValidWidth width) (Playfield.InvalidHeight height) =
    Playfield.tryCreate width height
    <=> Error ![ Playfield.HeightTooSmall(Playfield.minHeight, height) ]

[<Property>]
let ``tryCreate returns Error when both dimensions are too small``
    (Playfield.InvalidWidth width)
    (Playfield.InvalidHeight height)
    =
    Playfield.tryCreate width height
    <=> Error
            ![ Playfield.WidthTooSmall(Playfield.minWidth, width)
               Playfield.HeightTooSmall(Playfield.minHeight, height) ]

[<Property>]
let ``tryCreate returns Ok when both dimensions are valid``
    (Playfield.ValidWidth width)
    (Playfield.ValidHeight height)
    =
    Playfield.tryCreate width height |> Result.isOk <=> true
