namespace Tetris.Core.Test.Generator

open Tetris.Core
open FsCheck.FSharp


module Tile =
    let tile = Gen.elements [ Tile.Empty; Tile.Occupied ]

module Playfield =
    let private generateDimension isValid minValue =
        if isValid then
            Gen.choose (minValue, minValue * 2)
        else
            Gen.choose (0, minValue - 1)

    let tileArray isValidWidth isValidHeight =
        gen {
            let! width = generateDimension isValidWidth Playfield.minWidth
            let! height = generateDimension isValidHeight Playfield.minHeight
            return! Gen.array2DOfDim height width Tile.tile
        }
