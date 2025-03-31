module Tetris.Core.Test.Block

open Tetris.Core
open Xunit
open FsUnitTyped


let private o = Tile.Empty

let private x = Tile.Occupied

[<Fact>]
let ``tryCreate fails when tiles is null`` () =
    shouldFail (fun _ -> Block.tryCreate null |> ignore)

[<Fact>]
let ``tryCreate returns error when width is zero`` () =
    Array2D.create 2 0 o
    |> Block.tryCreate
    |> Result.assertError (isExactly [ Block.ZeroWidth ])

[<Fact>]
let ``tryCreate returns error when height is zero`` () =
    Array2D.create 0 2 o
    |> Block.tryCreate
    |> Result.assertError (isExactly [ Block.ZeroHeight ])

[<Fact>]
let ``tryCreate returns error when width and height are zero`` () =
    Array2D.create 0 0 o
    |> Block.tryCreate
    |> Result.assertError (isExactly [ Block.ZeroWidth; Block.ZeroHeight ])

[<Theory>]
[<InlineData(1, 1)>]
[<InlineData(1, 2)>]
[<InlineData(2, 1)>]
[<InlineData(2, 2)>]
[<InlineData(10, 20)>]
[<InlineData(20, 10)>]
let ``tryCreate creates a new block`` width height =
    Array2D.create height width o |> Block.tryCreate |> Result.assertOk ignore

[<Fact>]
let ``tryCreate creates a new block with the correct tiles`` () =
    let tiles = [ [ o; x; o ]; [ x; x; o ] ] |> array2D

    Block.tryCreate tiles
    |> Result.assertOk (fun block ->
        let createdTiles = Block.toArray block
        Array2D.shouldEqual tiles createdTiles)

[<Fact>]
let ``tryCreate does not modify original tiles`` () =
    let tiles = [ [ o; x ]; [ x; o ] ] |> array2D

    Block.tryCreate tiles
    |> Result.assertOk (fun block ->
        tiles[0, 0] <- x
        Block.getTile 0 0 block |> shouldEqual o)

[<Theory>]
[<InlineData(1, 1)>]
[<InlineData(1, 2)>]
[<InlineData(2, 1)>]
[<InlineData(2, 2)>]
[<InlineData(10, 20)>]
[<InlineData(20, 10)>]
let ``width returns the width of the block`` width height =
    Array2D.create height width o
    |> Block.tryCreate
    |> Result.assertOk (Block.width >> shouldEqual width)

[<Theory>]
[<InlineData(1, 1)>]
[<InlineData(1, 2)>]
[<InlineData(2, 1)>]
[<InlineData(2, 2)>]
[<InlineData(10, 20)>]
[<InlineData(20, 10)>]
let ``height returns the height of the block`` width height =
    Array2D.create height width o
    |> Block.tryCreate
    |> Result.assertOk (Block.height >> shouldEqual height)

// Input:     Output:
// [o x o]    [x o]
// [x x x] -> [x x]
//            [x o]
[<Theory>]
[<InlineData(1, 1)>]
[<InlineData(2, 1)>]
[<InlineData(3, 1)>]
[<InlineData(4, 1)>]
[<InlineData(5, 1)>]
[<InlineData(1, 2)>]
[<InlineData(2, 2)>]
[<InlineData(3, 2)>]
[<InlineData(4, 2)>]
[<InlineData(5, 2)>]
[<InlineData(1, 3)>]
[<InlineData(2, 3)>]
[<InlineData(3, 3)>]
[<InlineData(4, 3)>]
[<InlineData(5, 3)>]
let ``rotateClockwise correctly rotates block`` rotations blockId =
    let expected =
        match rotations % 4, blockId with
        | 0, 1 -> [ [ o; x; o ]; [ x; x; x ] ] |> array2D
        | 1, 1 -> [ [ x; o ]; [ x; x ]; [ x; o ] ] |> array2D
        | 2, 1 -> [ [ x; x; x ]; [ o; x; o ] ] |> array2D
        | 3, 1 -> [ [ o; x ]; [ x; x ]; [ o; x ] ] |> array2D
        | 0, 2 -> [ [ x; x ]; [ x; x ] ] |> array2D
        | 1, 2 -> [ [ x; x ]; [ x; x ] ] |> array2D
        | 2, 2 -> [ [ x; x ]; [ x; x ] ] |> array2D
        | 3, 2 -> [ [ x; x ]; [ x; x ] ] |> array2D
        | 0, 3 -> [ [ x ] ] |> array2D
        | 1, 3 -> [ [ x ] ] |> array2D
        | 2, 3 -> [ [ x ] ] |> array2D
        | 3, 3 -> [ [ x ] ] |> array2D
        | _ -> failwith "Impossible"

    let initial =
        if blockId = 1 then [ [ o; x; o ]; [ x; x; x ] ]
        else if blockId = 2 then [ [ x; x ]; [ x; x ] ]
        else [ [ x ] ]

    initial
    |> array2D
    |> Block.tryCreate
    |> Result.assertOk (fun block ->
        let rotatedBlock =
            [ 1..rotations ]
            |> List.fold (fun b _ -> Block.rotateClockwise b) block
            |> Block.toArray

        Array2D.shouldEqual expected rotatedBlock)

[<Theory>]
[<InlineData(-1, 0, 10, 10)>]
[<InlineData(0, -1, 10, 10)>]
[<InlineData(-100, 1, 10, 10)>]
[<InlineData(1, -100, 10, 10)>]
[<InlineData(3, 0, 2, 2)>]
[<InlineData(0, 3, 2, 2)>]
[<InlineData(100, 1, 4, 4)>]
[<InlineData(1, 100, 4, 4)>]
let ``getTile fails when x or y is out of bounds`` x y width height =
    shouldFail (fun _ ->
        Array2D.create height width o
        |> Block.tryCreate
        |> Result.assertOk (Block.getTile x y)
        |> ignore)

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
