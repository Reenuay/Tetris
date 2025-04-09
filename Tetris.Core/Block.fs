[<RequireQualifiedAccess>]
module Tetris.Core.Block

open FsToolkit.ErrorHandling


/// <summary>
/// Represents a block of tiles.
/// </summary>
type Block = private { Tiles: Set<Position> }

/// <summary>
/// Represents possible errors that can occur when creating a block.
/// </summary>
type BlockCreationError =
    | ZeroWidth
    | ZeroHeight

let private failIfNull pattern =
    if isNull pattern then
        nullArg "Pattern cannot be null"

let private validateWidth pattern =
    let width = pattern |> Array2D.length2
    if width < 1 then Error [ ZeroWidth ] else Ok width

let private validateHeight pattern =
    let height = pattern |> Array2D.length1
    if height < 1 then Error [ ZeroHeight ] else Ok height

/// <summary>
/// Tries to create a new block from the given pattern.
/// </summary>
/// <param name="pattern">The pattern to create the block from.</param>
/// <returns>A result containing the new block or a list of errors.</returns>
/// <exception cref="ArgumentNullException">Thrown when the pattern is null.</exception>
/// <remarks>
/// The pattern is represented as a 2D array of booleans, where true indicates a tile and false indicates an empty space.
/// </remarks>
let tryCreate (pattern: bool[,]) =
    validation {
        do failIfNull pattern

        let! width = validateWidth pattern
        and! height = validateHeight pattern

        let positions =
            [| for y in 0 .. (height - 1) do
                   for x in 0 .. (width - 1) do
                       if pattern[y, x] then
                           yield { X = x; Y = y } |]
            |> Set.ofArray

        return { Tiles = positions }
    }

/// <summary>
/// Get the maximum coordinate of the block.
/// </summary>
/// <param name="block">The block to get the maximum coordinate of.</param>
/// <returns>The maximum coordinate of the block.</returns>
let maxExtent block =
    block.Tiles |> Set.fold (fun m t -> max t.X t.Y |> max m) 0

/// <summary>
/// Rotates a block clockwise.
/// </summary>
/// <param name="block">The block to rotate.</param>
/// <returns>The rotated block.</returns>
let rotateClockwise block =
    let maxExtent = maxExtent block
    let tiles = block.Tiles |> Set.map (fun t -> { X = maxExtent - t.Y; Y = t.X })
    { Tiles = tiles }

/// <summary>
/// Gets the coordinates of the occupied tiles of the block.
/// </summary>
/// <param name="block">The block to get the tiles of.</param>
/// <returns>The coordinates of the occupied tiles of the block.</returns>
let getTiles block = block.Tiles
