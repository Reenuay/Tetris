[<RequireQualifiedAccess>]
module Tetris.Core.Grid

open FSharpPlus


type Tile = Tile.Tile

/// <summary>
/// Represents a 2D grid of tiles.
/// </summary>
type Grid = private { Tiles: Tile[,] }

/// <summary>
/// Represents the possible errors that can occur during the creation of a grid.
/// </summary>
type GridCreationError =
    | WidthTooSmall of minimalWidth: int * actualWidth: int
    | HeightTooSmall of minimalHeight: int * actualHeight: int

let minWidth = 10

let minHeight = 10

let private create width height =
    let tiles = Tile.Empty |> konst |> konst |> Array2D.init width height

    { Tiles = tiles }

let private validateWidth width =
    if width < minWidth then
        Error [ WidthTooSmall(minWidth, width) ]
    else
        Ok width

let private validateHeight height =
    if height < minHeight then
        Error [ HeightTooSmall(minHeight, height) ]
    else
        Ok height

/// <summary>
/// Tries to create a new grid with the given width and height.
/// </summary>
/// <param name="width">The width of the grid.</param>
/// <param name="height">The height of the grid.</param>
/// <returns>A result containing the grid or an error.</returns>
let tryCreate width height =
    create <!> validateWidth width <*> validateHeight height

/// <summary>
/// Checks if the given block can be placed at the given position on the given grid.
/// </summary>
/// <param name="block">The block to check.</param>
/// <param name="position">The position to check.</param>
/// <param name="grid">The grid to check.</param>
/// <returns>True if the block can be placed at the given position on the given grid, false otherwise.</returns>
let canPlace block position grid =
    let { Position.X = x; Position.Y = y } = position
    let blockWidth = block |> Block.width
    let blockHeight = block |> Block.height
    let gridWidth = grid.Tiles |> Array2D.length1
    let gridHeight = grid.Tiles |> Array2D.length2

    let isWithinBounds =
        x >= 0 && x + blockWidth <= gridWidth && y >= 0 && y + blockHeight <= gridHeight

    // Check for collision with non-empty tiles on the grid
    // This has to be a function because the computation has to be deferred
    // Firstly because isWithinBounds has to be checked first so no index out of bounds exception is thrown
    // Secondly because it avoids unnecessary computation if isWithinBounds is false
    let hasNoCollision _ =
        seq {
            for i in 0 .. blockWidth - 1 do
                for j in 0 .. blockHeight - 1 do
                    yield block[i, j] = Tile.Empty || grid.Tiles[x + i, y + j] = Tile.Empty
        }
        |> Seq.forall id

    isWithinBounds && hasNoCollision ()
