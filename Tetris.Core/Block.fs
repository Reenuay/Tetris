[<RequireQualifiedAccess>]
module Tetris.Core.Block


/// <summary>
/// Represents a block of tiles.
/// </summary>
type Block =
    private
        { Tiles: Set<Position>
          Extent: int }

/// <summary>
/// Represents possible errors that can occur when creating a block.
/// </summary>
type BlockCreationError =
    | ZeroWidth
    | ZeroHeight
    | EmptyPattern

let private calculateExtent tiles =
    tiles |> Set.fold (fun m t -> max m (max t.X t.Y)) 0

/// <summary>
/// Tries to create a new block from the given pattern.
/// </summary>
/// <param name="pattern">The pattern to create the block from.</param>
/// <returns>A result containing the new block or a list of errors.</returns>
/// <exception cref="ArgumentNullException">Thrown when the pattern is null.</exception>
/// <remarks>
/// The pattern is represented as a 2D array of booleans, where true indicates a tile and false indicates an empty space.
/// </remarks>
let tryCreate pattern =
    Fail.ifNullArg (nameof pattern) pattern

    let width = pattern |> Array2D.length2
    let height = pattern |> Array2D.length1

    let tiles =
        ![ for y in 0 .. (height - 1) do
               for x in 0 .. (width - 1) do
                   if pattern[y, x] then
                       yield { X = x; Y = y } ]

    [ width = 0 |--> ZeroWidth; height = 0 |--> ZeroHeight ]
    |> Result.mergeErrors
    |> Result.replaceError (Set.isEmpty tiles |--> ![ EmptyPattern ])
    |> Result.replaceOk
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
        |> Set.map (fun position ->
            { X = extent - position.Y
              Y = position.X })

    { block with
        Tiles = rotatedTilePositions }
