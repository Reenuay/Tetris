module Tetris.Core.Test.Grid

open Tetris.Core
open Xunit
open FsUnitTyped


[<Fact>]
let ``Grid.tryCreate fails when tiles is null`` () =
    let tiles = null

    Grid.tryCreate tiles
    |> Result.map (fun _ -> failwith "Expected Error")
    |> Result.defaultWith (fun errors -> errors |> shouldEqual [ Grid.NullTiles ])

[<Fact>]
let ``Grid.tryCreate fails when width is too small`` () =
    let tiles = always [ Tile.Empty ] |> List.init Grid.minHeight |> array2D

    Grid.tryCreate tiles
    |> Result.map (fun _ -> failwith "Expected Error")
    |> Result.defaultWith (fun errors -> errors |> shouldEqual [ Grid.WidthTooSmall(Grid.minWidth, 1) ])


[<Fact>]
let ``Grid.tryCreate fails when height is too small`` () =
    let tiles =
        always Tile.Empty |> List.init Grid.minWidth |> List.singleton |> array2D

    Grid.tryCreate tiles
    |> Result.map (fun _ -> failwith "Expected Error")
    |> Result.defaultWith (fun errors -> errors |> shouldEqual [ Grid.HeightTooSmall(Grid.minHeight, 1) ])

[<Fact>]
let ``Grid.tryCreate fails when width and height are too small`` () =
    let tiles = always Tile.Empty |> List.init 1 |> List.singleton |> array2D

    Grid.tryCreate tiles
    |> Result.map (fun _ -> failwith "Expected Error")
    |> Result.defaultWith (fun errors ->
        errors
        |> shouldEqual [ Grid.WidthTooSmall(Grid.minWidth, 1); Grid.HeightTooSmall(Grid.minHeight, 1) ])

[<Fact>]
let ``Grid.tryCreate succeeds when given tiles are valid`` () =
    let tiles =
        always Tile.Empty
        |> List.init Grid.minWidth
        |> always
        |> List.init Grid.minHeight
        |> array2D

    Grid.tryCreate tiles |> Result.isOk |> shouldEqual true
