[<FsCheck.Xunit.Properties(Arbitrary = [| typeof<Tetris.Core.Test.Arbitrary.Playfield.Extension> |])>]
module Tetris.Core.Test.Playfield

open Tetris.Core
open Tetris.Core.Test.Arbitrary
open FsCheck.Xunit
open FsCheck.FSharp
open FsToolkit.ErrorHandling
open System


[<Property>]
let ``tryCreate fails when tiles is null`` () =
    lazy Playfield.tryCreate null |> Prop.throws<ArgumentNullException, _>

[<Property>]
let ``tryCreate returns error when width is too small`` (Playfield.InvalidWidthArray tiles) =
    let width = Array2D.length2 tiles

    Playfield.tryCreate tiles
    <=> Error [ Playfield.WidthTooSmall(Playfield.minWidth, width) ]

[<Property>]
let ``tryCreate returns error when height is too small`` (Playfield.InvalidHeightArray tiles) =
    let height = Array2D.length1 tiles

    Playfield.tryCreate tiles
    <=> Error [ Playfield.HeightTooSmall(Playfield.minHeight, height) ]

[<Property>]
let ``tryCreate returns error when both dimensions are too small`` (Playfield.InvalidSizeTileArray tiles) =
    let width = Array2D.length2 tiles
    let height = Array2D.length1 tiles

    Playfield.tryCreate tiles
    <=> Error
            [ Playfield.WidthTooSmall(Playfield.minWidth, width)
              Playfield.HeightTooSmall(Playfield.minHeight, height) ]

[<Property>]
let ``tryCreate returns ok when both dimensions are valid`` (Playfield.ValidArray tiles) =
    Playfield.tryCreate tiles |> Result.isOk

[<Property>]
let ``tryCreate copies the input array`` (Playfield.ValidArray tiles) =
    Playfield.tryCreate tiles
    |> Result.map (fun playfield ->
        let originalTile = Playfield.getTile 0 0 playfield

        tiles[0, 0] <-
            if originalTile = Tile.Empty then
                Tile.Occupied
            else
                Tile.Empty

        let tileAfterChange = Playfield.getTile 0 0 playfield
        tileAfterChange = originalTile)
    |> Result.defaultValue false

[<Property>]
let ``getTile returns the correct tile`` (Playfield.ValidArrayWithCoordinate(tiles, x, y)) =
    let playfield =
        Playfield.tryCreate tiles
        |> Result.defaultWith (fun _ -> failwith "Invalid playfield")

    Playfield.getTile x y playfield <=> tiles[y, x]
