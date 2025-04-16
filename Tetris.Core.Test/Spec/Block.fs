[<FsCheck.Xunit.Properties(Arbitrary = [| typeof<Tetris.Core.Test.Arbitrary.Block.Extension> |])>]
module Tetris.Core.Test.Spec.Block

open Tetris.Core
open Tetris.Core.Test
open Tetris.Core.Test.Arbitrary
open FSharpPlus.Data
open FsCheck
open FsCheck.Xunit


type NonEmptySet<'t when 't: comparison> = FSharpPlus.Data.NonEmptySet<'t>

[<Property>]
let ``create sets correct extent`` (tiles: NonEmptySet<Position>) =
    let expectedExtent = tiles |> NonEmptySet.fold (fun m t -> max m (max t.X t.Y)) 0us

    let block = Block.create tiles

    Block.extent block ===> expectedExtent

[<Property>]
let ``rotateClockwise preserves tile count`` (Block.Block block) =
    let rotated = Block.rotateClockwise block

    block |> Block.tiles |> NonEmptySet.count
    ===> (rotated |> Block.tiles |> NonEmptySet.count)

[<Property>]
let ``rotateClockwise 4n times returns tile to original state`` (Block.Block block) (PositiveInt n) =
    let rotated = [ 1 .. n * 4 ] |> List.fold (fun b _ -> Block.rotateClockwise b) block

    Block.tiles block ===> Block.tiles rotated

[<Property>]
let ``rotateClockwise preserves extent`` (Block.Block block) (PositiveInt n) =
    let rotated = [ 1..n ] |> List.fold (fun b _ -> Block.rotateClockwise b) block

    Block.extent block ===> Block.extent rotated

[<Property>]
let ``rotateClockwise changes relative positions for non-square blocks`` (Block.Block block) =
    let originalTiles = Block.tiles block

    let rotatedTiles = block |> Block.rotateClockwise |> Block.tiles

    let isSquare =
        let side = Block.extent block + 1us |> int
        NonEmptySet.count originalTiles = side * side

    originalTiles = rotatedTiles ===> isSquare
