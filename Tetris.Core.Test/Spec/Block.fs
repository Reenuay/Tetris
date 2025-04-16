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
let ``tryCreate returns Error when both dimensions are 0`` () =
    (fun _ _ -> true) |> Array2D.init 0 0 |> Block.tryCreate
    ===> Error ![ Block.ZeroWidth; Block.ZeroHeight ]

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

    pattern |> Block.tryCreate |> Result.map (Block.tiles >> Set.count)
    ===> Ok trueCount

[<Property>]
let ``tryCreate preserves tile positions`` (Block.NonEmptyPattern pattern) =
    pattern
    |> Block.tryCreate
    |> Result.map (fun block ->
        let tiles = Block.tiles block
        tiles |> Set.forall (fun pos -> pattern[pos.Y, pos.X]))
    ===> Ok true

[<Property>]
let ``tryCreate sets correct extent`` (Block.NonEmptyPattern pattern) =
    let expectedExtent =
        pattern
        |> Array2D.mapi (fun y x value -> if value then max x y else 0)
        |> Seq.cast<int>
        |> Seq.max

    pattern |> Block.tryCreate |> Result.map Block.extent ===> Ok expectedExtent

[<Property>]
let ``rotateClockwise preserves tile count`` (Block.Block block) =
    let rotated = Block.rotateClockwise block

    block |> Block.tiles |> Set.count ===> (rotated |> Block.tiles |> Set.count)

[<Property>]
let ``rotateClockwise 4n times returns tile to original state`` (Block.Block block) (PositiveInt n) =
    let rotated = [ 1 .. n * 4 ] |> List.fold (fun b _ -> Block.rotateClockwise b) block

    Block.tiles block ===> Block.tiles rotated

[<Property>]
let ``rotateClockwise preserves extent`` (Block.Block block) (PositiveInt n) =
    let rotated = [ 1..n ] |> List.fold (fun b _ -> Block.rotateClockwise b) block

    Block.extent block ===> Block.extent rotated

[<Property>]
let ``rotateClockwise keeps coordinates non-negative`` (Block.Block block) =
    let rotated = Block.rotateClockwise block

    Block.tiles rotated |> Set.forall (fun pos -> pos.X >= 0 && pos.Y >= 0)
    ===> true

[<Property>]
let ``rotateClockwise changes relative positions for non-square blocks`` (Block.Block block) =
    let originalTiles = Block.tiles block

    let rotatedTiles = block |> Block.rotateClockwise |> Block.tiles

    let isSquare =
        let side = Block.extent block + 1
        Set.count originalTiles = side * side

    originalTiles = rotatedTiles ===> isSquare
