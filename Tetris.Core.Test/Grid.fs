module Tetris.Core.Test.Grid

open Tetris.Core
open Xunit
open FsUnitTyped


[<Fact>]
let ``tryCreate fails when tiles is null`` () =
    let tiles = null

    Grid.tryCreate tiles |> Result.shouldBeError [ Grid.NullTiles ]

[<Fact>]
let ``tryCreate fails when width is too small`` () =
    let tiles = Array2D.create Grid.minHeight 0 Tile.Empty

    Grid.tryCreate tiles
    |> Result.shouldBeError [ Grid.WidthTooSmall(Grid.minWidth, 0) ]

[<Fact>]
let ``tryCreate fails when height is too small`` () =
    let tiles = Array2D.create 0 Grid.minWidth Tile.Empty

    Grid.tryCreate tiles
    |> Result.shouldBeError [ Grid.HeightTooSmall(Grid.minHeight, 0) ]

[<Fact>]
let ``tryCreate fails when width and height are too small`` () =
    let tiles = Array2D.create 0 0 Tile.Empty

    Grid.tryCreate tiles
    |> Result.shouldBeError [ Grid.WidthTooSmall(Grid.minWidth, 0); Grid.HeightTooSmall(Grid.minHeight, 0) ]

[<Fact>]
let ``tryCreate succeeds when given tiles are valid`` () =
    let tiles = Array2D.create Grid.minHeight Grid.minWidth Tile.Empty

    Grid.tryCreate tiles |> Result.shouldBeOk
