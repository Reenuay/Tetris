module Tetris.Core.Test.Playfield

open Tetris.Core
open Xunit
open FsUnitTyped
open FsCheck.Xunit
open FsCheck.FSharp
open FsToolkit.ErrorHandling


[<Fact>]
let ``tryCreate fails when tiles is null`` () =
    shouldFail (fun _ -> Playfield.tryCreate null |> ignore)

[<Property>]
let ``tryCreate returns error when width is too small`` (tiles: Tile array2d) =
    let width = Array2D.length2 tiles

    width < Playfield.minWidth
    ==> (Playfield.tryCreate tiles
         |> Result.mapError (List.contains (Playfield.WidthTooSmall(Playfield.minWidth, width)))
         |> Result.defaultError false)

[<Property>]
let ``tryCreate returns error when height is too small`` (tiles: Tile array2d) =
    let height = Array2D.length1 tiles

    height < Playfield.minHeight
    ==> (Playfield.tryCreate tiles
         |> Result.mapError (List.contains (Playfield.HeightTooSmall(Playfield.minHeight, height)))
         |> Result.defaultError false)

[<Property>]
let ``tryCreate succeeds with valid dimensions`` (tiles: Tile array2d) =
    let width = Array2D.length2 tiles
    let height = Array2D.length1 tiles

    (width >= Playfield.minWidth && height >= Playfield.minHeight)
    ==> (Playfield.tryCreate tiles |> Result.isOk)

[<Property>]
let ``tryCreate copies the input array`` (tiles: Tile array2d) =
    let width = Array2D.length2 tiles
    let height = Array2D.length1 tiles

    (width >= Playfield.minWidth
     && height >= Playfield.minHeight
     && width > 0
     && height > 0)
    ==> (Playfield.tryCreate tiles
         |> Result.map (fun playfield ->
             let originalTile = Playfield.getTile 0 0 playfield

             tiles[0, 0] <-
                 if originalTile = Tile.Empty then
                     Tile.Occupied
                 else
                     Tile.Empty

             let tileAfterChange = Playfield.getTile 0 0 playfield
             tileAfterChange = originalTile)
         |> Result.defaultValue false)
