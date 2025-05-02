[<RequireQualifiedAccess>]
module Tetris.Core.Block

open FSharpPlus.Data


/// <summary>
/// Represents a block of tiles regardless of the playfield they are on.
/// </summary>
type Block =
    private
        { Tiles: NonEmptySet<Position>
          Extent: uint16 }

let private calculateExtent tiles =
    tiles |> NonEmptySet.fold (fun m t -> max m (max t.X t.Y)) 0us

/// <summary>
/// Creates a new block.
/// </summary>
/// <param name="tiles">The tiles that make up the block.</param>
/// <returns>The new block.</returns>
/// <remarks>
/// The tiles are represented as a non-empty set of positions.
/// </remarks>
let create tiles =
    { Tiles = tiles
      Extent = calculateExtent tiles }

/// <summary>
/// Gets the coordinates of the occupied tiles of the block.
/// </summary>
/// <param name="block">The block to get the tile positions of.</param>
/// <returns>The coordinates of the occupied tiles of the block.</returns>
/// <remarks>
/// The tiles are represented as a set of positions.
/// </remarks>
let tiles block = block.Tiles

/// <summary>
/// Gets the size of the square that contains all the tiles of the block.
/// </summary>
/// <param name="block">The block to get the extent of.</param>
/// <returns>The size of the square that contains all the tiles of the block.</returns>
let extent block = block.Extent

/// <summary>
/// Rotates a block clockwise.
/// </summary>
/// <param name="block">The block to rotate.</param>
/// <returns>The rotated block.</returns>
let rotateClockwise block =
    let extent = extent block

    let rotatedTilePositions =
        block.Tiles
        |> NonEmptySet.map (fun position ->
            { X = extent - position.Y
              Y = position.X })

    { block with
        Tiles = rotatedTilePositions }
