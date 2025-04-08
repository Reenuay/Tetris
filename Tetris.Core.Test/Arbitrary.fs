namespace Tetris.Core.Test.Arbitrary

open Tetris.Core
open Tetris.Core.Test
open FsCheck.FSharp

[<RequireQualifiedAccess>]
module Playfield =
    /// Represents a 2D array of tiles with invalid width and valid height.
    type InvalidWidthArray = InvalidWidthArray of tiles: Tile array2d

    /// Represents a 2D array of tiles with valid width and invalid height.
    type InvalidHeightArray = InvalidHeightArray of tiles: Tile array2d

    /// Represents a 2D array of tiles with invalid width and height.
    type InvalidSizeTileArray = InvalidSizeTileArray of tiles: Tile array2d

    /// Represents a 2D array of tiles with valid width and height.
    type ValidArray = ValidArray of tiles: Tile array2d

    /// Represents a 2D array of tiles with valid width and height and a valid coordinate inside the array.
    type ValidArrayWithCoordinate = ValidArrayWithCoordinate of tiles: Tile array2d * x: int * y: int

    /// Extension methods for the Arbitrary module.
    type Extension =
        static member InvalidWidthArray() =
            Arb.fromGen (Generator.Playfield.tileArray false true |> Gen.map InvalidWidthArray)

        static member InvalidHeightArray() =
            Arb.fromGen (Generator.Playfield.tileArray true false |> Gen.map InvalidHeightArray)

        static member InvalidSizeTileArray() =
            Arb.fromGen (Generator.Playfield.tileArray false false |> Gen.map InvalidSizeTileArray)

        static member ValidArray() =
            Arb.fromGen (Generator.Playfield.tileArray true true |> Gen.map ValidArray)

        static member ValidArrayWithCoordinate() =
            Arb.fromGen (
                Generator.Playfield.tileArrayWithCoordinate true true
                |> Gen.map ValidArrayWithCoordinate
            )
