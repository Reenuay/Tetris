[<RequireQualifiedAccess>]
module Tetris.Core.Grid

open FSharpPlus


/// <summary>
/// Represents a 2D grid of tiles.
/// </summary>
type Grid = private { Tiles: Tile[,] }

/// <summary>
/// Represents the possible errors that can occur during the creation of a grid.
/// </summary>
type GridCreationError =
    | NullTiles
    | WidthTooSmall of minimalWidth: int * actualWidth: int
    | HeightTooSmall of minimalHeight: int * actualHeight: int

/// Minimal width of the grid.
let minWidth = 10

/// Minimal height of the grid.
let minHeight = 20

let private create tiles = { Tiles = tiles }

let private validateNull tiles =
    if isNull tiles then Error [ NullTiles ] else Ok()

let private validateWidth tiles =
    if tiles |> Array2D.length2 < minWidth then
        Error [ WidthTooSmall(minWidth, tiles |> Array2D.length1) ]
    else
        Ok()

let private validateHeight tiles =
    if tiles |> Array2D.length1 < minHeight then
        Error [ HeightTooSmall(minHeight, tiles |> Array2D.length2) ]
    else
        Ok()

/// <summary>
/// Tries to create a new grid with the given width and height.
/// </summary>
/// <param name="width">The width of the grid.</param>
/// <param name="height">The height of the grid.</param>
/// <returns>A result containing the grid or an error.</returns>
let tryCreate tiles =
    monad {
        do! validateNull tiles
        do! validateWidth tiles
        do! validateHeight tiles
        return { Tiles = Array2D.copy tiles }
    }

/// <summary>
/// Checks if the given block can be placed at the given position on the given grid.
/// </summary>
/// <param name="block">The block to check.</param>
/// <param name="position">The position to check.</param>
/// <param name="grid">The grid to check.</param>
/// <returns>True if the block can be placed at the given position on the given grid, false otherwise.</returns>
let canPlace block position grid =
    let blockWidth = block |> Block.width
    let blockHeight = block |> Block.height
    let gridWidth = grid.Tiles |> Array2D.length1
    let gridHeight = grid.Tiles |> Array2D.length2

    let isWithinBounds =
        position.x >= 0
        && position.x + blockWidth <= gridWidth
        && position.y >= 0
        && position.y + blockHeight <= gridHeight

    // Check for collision with non-empty tiles on the grid
    // This has to be a function because the computation has to be deferred
    // Firstly because isWithinBounds has to be checked first so no index out of bounds exception is thrown
    // Secondly because it avoids unnecessary computation if isWithinBounds is false
    let hasNoCollision _ =
        let mutable hasCollision = false
        let mutable i = 0

        while not hasCollision && i < blockWidth do
            let mutable j = 0

            while not hasCollision && j < blockHeight do
                if
                    block[i, j] <> Tile.Empty
                    && grid.Tiles[position.x + i, position.y + j] <> Tile.Empty
                then
                    hasCollision <- true

                j <- j + 1

            i <- i + 1

        not hasCollision

    isWithinBounds && hasNoCollision ()
