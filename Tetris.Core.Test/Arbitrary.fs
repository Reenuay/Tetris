namespace Tetris.Core.Test.Arbitrary

open Tetris.Core
open Tetris.Core.Test
open FsCheck.FSharp

[<RequireQualifiedAccess>]
module Playfield =
    /// <summary>
    /// Represents a 2D array of tiles with invalid width and valid height.
    /// </summary>
    type InvalidWidthArray = InvalidWidthArray of Tile array2d

    /// <summary>
    /// Represents a 2D array of tiles with valid width and invalid height.
    /// </summary>
    type InvalidHeightArray = InvalidHeightArray of Tile array2d

    /// <summary>
    /// Represents a 2D array of tiles with invalid width and height.
    /// </summary>
    type InvalidSizeTileArray = InvalidSizeTileArray of Tile array2d

    /// <summary>
    /// Represents a 2D array of tiles with valid width and height.
    /// </summary>
    type ValidArray = ValidArray of Tile array2d

    /// <summary>
    /// Extension methods for the Arbitrary module.
    /// </summary>
    type Extension =
        static member InvalidWidthArray() =
            Arb.fromGen (Generator.Playfield.tileArray false true |> Gen.map InvalidWidthArray)

        static member InvalidHeightArray() =
            Arb.fromGen (Generator.Playfield.tileArray true false |> Gen.map InvalidHeightArray)

        static member InvalidSizeTileArray() =
            Arb.fromGen (Generator.Playfield.tileArray false false |> Gen.map InvalidSizeTileArray)

        static member ValidArray() =
            Arb.fromGen (Generator.Playfield.tileArray true true |> Gen.map ValidArray)
