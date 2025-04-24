[<RequireQualifiedAccess>]
module Tetris.Core.Playfield

open FSharpPlus.Data


/// <summary>
/// Represents a 2D playfield.
/// </summary>
type Playfield =
    private
        { TilePositions: Set<Position>
          Width: uint16
          Height: uint16 }

/// <summary>
/// Represents the possible errors that can occur during the creation of a playfield.
/// </summary>
type PlayfieldCreationError =
    | SmallWidth of minimalWidth: uint16 * actualWidth: uint16
    | SmallHeight of minimalHeight: uint16 * actualHeight: uint16

/// <summary>
/// Represents the possible errors that can occur during the placement of a piece on a playfield.
/// </summary>
type PlacementError =
    | OutOfBounds
    | Collision

/// Minimal width of a playfield.
let minWidth = 20us

/// Minimal height of a playfield.
let minHeight = 20us

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

/// <summary>
/// Tries to create an empty playfield with the given width and height.
/// </summary>
/// <param name="width">The width of the playfield.</param>
/// <param name="height">The height of the playfield.</param>
/// <returns>A result containing the playfield if dimensions are valid, or error if they are too small.</returns>
let tryCreate width height =
    [ width < minWidth |--> SmallWidth(minWidth, width)
      height < minHeight |--> SmallHeight(minHeight, height) ]
    |> List.reduce Error.collect
    |> Result.map (fun _ ->
        { TilePositions = Set.empty
          Width = width
          Height = height })

let private isOutOfBounds playfield tiles =
    let isOutOfBounds tile =
        tile.X >= playfield.Width || tile.Y >= playfield.Height

    tiles |> Set.exists isOutOfBounds

let private hasCollisions playfield tiles =
    tiles |> Set.intersect playfield.TilePositions |> Set.isEmpty |> not

let private validatePlacement piece playfield =
    let tiles =
        piece
        |> Piece.toBlock
        |> Block.tiles
        |> NonEmptySet.toSet
        |> Set.map (Position.add piece.Position)

    [ tiles |> isOutOfBounds playfield |--> OutOfBounds
      tiles |> hasCollisions playfield |--> Collision ]
    |> List.reduce Error.collect
    |> Result.map (fun _ -> tiles)

/// <summary>
/// Checks if a piece can be placed on the playfield.
/// </summary>
/// <param name="piece">The piece to check.</param>
/// <param name="playfield">The playfield to check on.</param>
/// <returns>A result containing unit if the piece can be placed, or an error.</returns>
let canPlace piece playfield =
    playfield |> validatePlacement piece |> Result.map (fun _ -> ())

/// <summary>
/// Tries to place a piece on the playfield.
/// </summary>
/// <param name="piece">The piece to place.</param>
/// <param name="playfield">The playfield to place the piece on.</param>
/// <returns>A result containing the updated playfield or an error.</returns>
let tryPlace piece playfield =
    playfield
    |> validatePlacement piece
    |> Result.map (fun tiles ->
        { playfield with
            TilePositions = playfield.TilePositions |> Set.union tiles })
