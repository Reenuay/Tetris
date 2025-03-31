module Tetris.Core.Test.Playfield

open Tetris.Core
open Xunit


[<Fact>]
let ``tryCreate fails when tiles is null`` () =
    let tiles = null

    Playfield.tryCreate tiles |> Result.shouldBeError [ Playfield.NullTiles ]

[<Fact>]
let ``tryCreate fails when width is too small`` () =
    let tiles = Array2D.create Playfield.minHeight 0 Tile.Empty

    Playfield.tryCreate tiles
    |> Result.shouldBeError [ Playfield.WidthTooSmall(Playfield.minWidth, 0) ]

[<Fact>]
let ``tryCreate fails when height is too small`` () =
    let tiles = Array2D.create 0 Playfield.minWidth Tile.Empty

    Playfield.tryCreate tiles
    |> Result.shouldBeError [ Playfield.HeightTooSmall(Playfield.minHeight, 0) ]

[<Fact>]
let ``tryCreate fails when width and height are too small`` () =
    let tiles = Array2D.create 0 0 Tile.Empty

    Playfield.tryCreate tiles
    |> Result.shouldBeError
        [ Playfield.WidthTooSmall(Playfield.minWidth, 0)
          Playfield.HeightTooSmall(Playfield.minHeight, 0) ]

[<Fact>]
let ``tryCreate succeeds when given tiles are valid`` () =
    let tiles = Array2D.create Playfield.minHeight Playfield.minWidth Tile.Empty

    Playfield.tryCreate tiles |> Result.assertOk ignore
