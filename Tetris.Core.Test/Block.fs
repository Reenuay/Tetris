module Tetris.Core.Test.Block

open Tetris.Core
open Xunit
open FsUnitTyped


let private o = Tile.Empty

let private x = Tile.Occupied

[<Fact>]
let ``create creates a new block`` () =
    [ [ x ] ] |> array2D |> Block.tryCreate |> Result.assertOk ignore
    [ [ o; x ] ] |> array2D |> Block.tryCreate |> Result.assertOk ignore
    [ [ o; x ]; [ o; o ] ] |> array2D |> Block.tryCreate |> Result.assertOk ignore

[<Fact>]
let ``width returns the width of the block`` () =
    let block = [ [ o; o ] ] |> array2D |> Block.tryCreate

    block |> Result.assertOk (Block.width >> shouldEqual 2)

[<Fact>]
let ``height returns the height of the block`` () =
    [ [ o; o ] ]
    |> array2D
    |> Block.tryCreate
    |> Result.assertOk (Block.height >> shouldEqual 1)

// Input:     Output:
// [o x o]    [x o]
// [x x x] -> [x x]
//            [x o]
[<Theory>]
[<InlineData(1)>]
[<InlineData(2)>]
[<InlineData(3)>]
[<InlineData(4)>]
[<InlineData(5)>]
let ``rotateClockwise correctly rotates`` rotations =
    let expected =
        match rotations % 4 with
        | 0 -> [ [ o; x; o ]; [ x; x; x ] ] |> array2D // 0°
        | 1 -> [ [ x; o ]; [ x; x ]; [ x; o ] ] |> array2D // 90°
        | 2 -> [ [ x; x; x ]; [ o; x; o ] ] |> array2D // 180°
        | 3 -> [ [ o; x ]; [ x; x ]; [ o; x ] ] |> array2D // 270°
        | _ -> failwith "Impossible"

    [ [ o; x; o ]; [ x; x; x ] ]
    |> array2D
    |> Block.tryCreate
    |> Result.assertOk (fun block ->
        let rotatedBlock =
            [ 1..rotations ]
            |> List.fold (fun b _ -> Block.rotateClockwise b) block
            |> Block.toArray

        Array2D.shouldEqual expected rotatedBlock)

[<Theory>]
[<InlineData(0, 0, "Empty")>]
[<InlineData(1, 0, "Occupied")>]
[<InlineData(2, 0, "Empty")>]
[<InlineData(0, 1, "Occupied")>]
[<InlineData(1, 1, "Occupied")>]
[<InlineData(2, 1, "Empty")>]
let ``getTile returns the tile at the given position`` i j value =
    [ [ o; x; o ]; [ x; x; o ] ]
    |> array2D
    |> Block.tryCreate
    |> Result.assertOk (Block.getTile i j >> string >> shouldEqual value)

[<Theory>]
[<InlineData(0, 0)>]
[<InlineData(1, 0)>]
[<InlineData(2, 0)>]
[<InlineData(0, 1)>]
[<InlineData(1, 1)>]
[<InlineData(2, 1)>]
let ``toArray returns the copy of the block's tiles`` i j =
    Array2D.create 2 3 o
    |> Block.tryCreate
    |> Result.assertOk (fun b ->
        let newArray = Block.toArray b
        newArray[j, i] <- x
        Block.getTile i j b |> shouldEqual o)
