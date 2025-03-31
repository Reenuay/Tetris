module Tetris.Core.Test.Playfield

open Tetris.Core
open Xunit
open FsUnitTyped


[<Fact>]
let ``tryCreate fails when tiles is null`` () =
    shouldFail (fun _ -> Playfield.tryCreate null |> ignore)

[<Fact>]
let ``tryCreate returns error when width is too small`` () =
    Array2D.create Playfield.minHeight 0 Tile.Empty
    |> Playfield.tryCreate
    |> Result.assertError (isExactly [ Playfield.WidthTooSmall(Playfield.minWidth, 0) ])

[<Fact>]
let ``tryCreate returns error when height is too small`` () =
    Array2D.create 0 Playfield.minWidth Tile.Empty
    |> Playfield.tryCreate
    |> Result.assertError (isExactly [ Playfield.HeightTooSmall(Playfield.minHeight, 0) ])

[<Fact>]
let ``tryCreate returns error when width and height are too small`` () =
    Array2D.create 0 0 Tile.Empty
    |> Playfield.tryCreate
    |> Result.assertError (
        isExactly
            [ Playfield.WidthTooSmall(Playfield.minWidth, 0)
              Playfield.HeightTooSmall(Playfield.minHeight, 0) ]
    )

[<Fact>]
let ``tryCreate succeeds when given tiles are valid`` () =
    Array2D.create Playfield.minHeight Playfield.minWidth Tile.Empty
    |> Playfield.tryCreate
    |> Result.assertOk ignore
