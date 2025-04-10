[<RequireQualifiedAccess>]
module Tetris.Core.Playfield

open FsToolkit.ErrorHandling


/// <summary>
/// Represents a 2D playfield.
/// </summary>
type Playfield =
    private
        { Tiles: Set<Position>
          Width: int
          Height: int }

/// <summary>
/// Represents the possible errors that can occur during the creation of a playfield.
/// </summary>
type PlayfieldCreationFailure =
    | WidthTooSmall of minimalWidth: int * actualWidth: int
    | HeightTooSmall of minimalHeight: int * actualHeight: int

/// Minimal width of a playfield.
let minWidth = 10

/// Minimal height of a playfield.
let minHeight = 20

/// <summary>
/// Gets the width of the playfield.
/// </summary>
/// <param name="playfield">The playfield to get the width from.</param>
/// <returns>The width of the playfield.</returns>
let width playfield = playfield.Width

/// <summary>
/// Gets the height of the playfield.
/// </summary>
/// <param name="playfield">The playfield to get the height from.</param>
/// <returns>The height of the playfield.</returns>
let height playfield = playfield.Height

let private validateWidth width =
    if width < minWidth then
        Error(WidthTooSmall(minWidth, width))
    else
        Ok width

let private validateHeight height =
    if height < minHeight then
        Error(HeightTooSmall(minHeight, height))
    else
        Ok height

/// <summary>
/// Tries to create an empty playfield with the given width and height.
/// </summary>
/// <param name="width">The width of the playfield.</param>
/// <param name="height">The height of the playfield.</param>
/// <returns>A result containing the playfield or an error.</returns>
let tryCreate width height =
    validation {
        let! width = validateWidth width
        and! height = validateHeight height

        return
            { Tiles = Set.empty
              Width = width
              Height = height }
    }

/// <summary>
/// Checks if a piece can be placed at its current position in the playfield.
/// </summary>
/// <param name="piece">The piece to check.</param>
/// <param name="playfield">The playfield to check against.</param>
/// <returns>True if the piece can be placed, false otherwise.</returns>
let canPlace piece playfield =
    let pieceTiles =
        piece
        |> Piece.toBlock
        |> Block.tilePositions
        |> Set.map (Position.add piece.Position)

    let isWithinBoundary tile =
        let withinWidth = tile.X >= 0 && tile.X < playfield.Width
        let withinHeight = tile.Y >= 0 && tile.Y < playfield.Height
        withinWidth && withinHeight

    let isWithinBoundary = pieceTiles |> Set.forall isWithinBoundary

    let hasNoCollision = Set.intersect pieceTiles playfield.Tiles |> Set.isEmpty

    isWithinBoundary && hasNoCollision
