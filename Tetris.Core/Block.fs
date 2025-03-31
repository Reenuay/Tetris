[<RequireQualifiedAccess>]
module Tetris.Core.Block


/// <summary>
/// Represents a block of tiles.
/// </summary>
type Block =
    private
        { Tiles: Tile[,] }

    /// <summary>
    /// Gets the tile at the specified position in the block.
    /// </summary>
    /// <param name="x">The x-coordinate of the tile.</param>
    /// <param name="y">The y-coordinate of the tile.</param>
    /// <returns>The tile at the specified position in the block.</returns>
    member this.Item
        with get (x, y) = this.Tiles[x, y]

/// <summary>
/// Creates a new block from the given tiles.
/// </summary>
/// <param name="tiles">The tiles that make up the block.</param>
/// <returns>A new block.</returns>
let create tiles = { Tiles = tiles |> Array2D.copy }

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
    let width = Array2D.length2 block.Tiles
    let height = Array2D.length1 block.Tiles
    let rotated = Array2D.create width height Tile.Empty

    for i in 0 .. height - 1 do
        for j in 0 .. width - 1 do
            rotated[j, width - 1 - i] <- block[i, j]

    { Tiles = rotated }
