[<RequireQualifiedAccess>]
module Tetris.Core.Block

open FsToolkit.ErrorHandling


/// <summary>
/// Represents a block of tiles.
/// </summary>
type Block = private { Tiles: Tile[,] }

/// <summary>
/// Represents possible errors that can occur when creating a block.
/// </summary>
type BlockCreationError =
    | ZeroWidth
    | ZeroHeight

let private failIfNull tiles =
    if isNull tiles then
        nullArg "Tiles cannot be null"

let private validateWidth tiles =
    if tiles |> Array2D.length2 < 1 then
        Error [ ZeroWidth ]
    else
        Ok()

let private validateHeight tiles =
    if tiles |> Array2D.length1 < 1 then
        Error [ ZeroHeight ]
    else
        Ok()

/// <summary>
/// Creates a new block from the given tiles.
/// </summary>
/// <param name="tiles">The tiles to create the block from.</param>
/// <returns>The created block if successful, otherwise an error.</returns>
/// <exception cref="System.ArgumentNullException">Thrown when tiles is null.</exception>
let tryCreate tiles =
    validation {
        do failIfNull tiles
        let! _ = validateWidth tiles
        and! _ = validateHeight tiles

        return { Tiles = tiles |> Array2D.copy }
    }

/// <summary>
/// Gets the width of the block.
/// </summary>
/// <param name="block">The block to get the width of.</param>
/// <returns>The width of the block.</returns>
let width block = Array2D.length2 block.Tiles

/// <summary>
/// Gets the height of the block.
/// </summary>
/// <param name="block">The block to get the height of.</param>
/// <returns>The height of the block.</returns>
let height block = Array2D.length1 block.Tiles

/// <summary>
/// Rotates a block clockwise.
/// </summary>
/// <param name="block">The block to rotate.</param>
/// <returns>The rotated block.</returns>
let rotateClockwise block =
    let width = width block
    let height = height block
    let rotated = Array2D.create width height Tile.Empty // Swap width and height

    for i in 0 .. width - 1 do
        for j in 0 .. height - 1 do
            rotated[i, height - 1 - j] <- block.Tiles[j, i]

    { Tiles = rotated }

/// <summary>
/// Gets the tile at the given position in the block.
/// </summary>
/// <param name="x">The x-coordinate of the tile.</param>
/// <param name="y">The y-coordinate of the tile.</param>
/// <param name="block">The block to get the tile from.</param>
let getTile x y block = block.Tiles[y, x]

/// <summary>
/// Converts a block to an array of tiles.
/// </summary>
/// <param name="block">The block to convert.</param>
/// <returns>An array of tiles.</returns>
/// <remarks>
/// The returned array is a copy of the original block's tiles.
/// </remarks>
let toArray block = block.Tiles |> Array2D.copy
