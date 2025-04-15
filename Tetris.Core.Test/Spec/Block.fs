[<FsCheck.Xunit.Properties(Arbitrary = [| typeof<Tetris.Core.Test.Arbitrary.Block.Extension> |])>]
module Tetris.Core.Test.Spec.Block

open Tetris.Core
open Tetris.Core.Test
open Tetris.Core.Test.Arbitrary
open System
open FsCheck
open FsCheck.FSharp
open FsCheck.Xunit


[<Property>]
let ``tryCreate throws on null pattern`` () =
    Prop.throws<ArgumentNullException, _> (lazy Block.tryCreate null)

[<Property>]
let ``tryCreate returns Error when pattern width is 0`` (PositiveInt height) =
    (fun _ _ -> true) |> Array2D.init height 0 |> Block.tryCreate
    ===> Error ![ Block.ZeroWidth ]

[<Property>]
let ``tryCreate returns Error when pattern height is 0`` (PositiveInt width) =
    (fun _ _ -> true) |> Array2D.init 0 width |> Block.tryCreate
    ===> Error ![ Block.ZeroHeight ]

[<Property>]
let ``tryCreate returns Ok for valid dimensions`` (PositiveInt width) (PositiveInt height) =
    (fun _ _ -> false)
    |> Array2D.init height width
    |> Block.tryCreate
    |> Result.isOk
    ===> true

[<Property>]
let ``tryCreate converts all true values to tiles and skips false ones`` (Block.NonEmptyPattern pattern) =
    let trueCount =
        [ for y in 0 .. Array2D.length1 pattern - 1 do
              for x in 0 .. Array2D.length2 pattern - 1 do
                  if pattern[y, x] then 1 else 0 ]
        |> List.sum

    pattern |> Block.tryCreate |> Result.map (Block.tilePositions >> Set.count)
    ===> Ok trueCount

[<Property>]
let ``tryCreate preserves tile positions`` (Block.NonEmptyPattern pattern) =
    pattern
    |> Block.tryCreate
    |> Result.map (fun block ->
        let tiles = Block.tilePositions block
        tiles |> Set.forall (fun pos -> pattern[pos.Y, pos.X]))
    |> Result.defaultValue false
    ===> true
