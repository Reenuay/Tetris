[<RequireQualifiedAccess>]
module Tetris.Core.Playfield


/// <summary>
/// Represents a 2D playfield.
/// </summary>
type Playfield =
    private
        { TilePositions: Set<Position>
          Width: int
          Height: int }

/// <summary>
/// Represents the possible errors that can occur during the creation of a playfield.
/// </summary>
type PlayfieldCreationError =
    | WidthTooSmall of minimalWidth: int * actualWidth: int
    | HeightTooSmall of minimalHeight: int * actualHeight: int

/// <summary>
/// Represents the possible errors that can occur during the placement of a piece on a playfield.
/// </summary>
type PlacementError =
    | OutOfBounds
    | Collision

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

/// <summary>
/// Tries to create an empty playfield with the given width and height.
/// </summary>
/// <param name="width">The width of the playfield.</param>
/// <param name="height">The height of the playfield.</param>
/// <returns>A result containing the playfield or an error.</returns>
let tryCreate width height =
    { TilePositions = Set.empty
      Width = width
      Height = height }
    |> Result.validateAll
        [ (fun p -> p.Width >= minWidth) --> WidthTooSmall(minWidth, width)
          (fun p -> p.Height >= minHeight) --> HeightTooSmall(minHeight, height) ]

let private checkBounds playfield tiles =
    let isWithinBounds tile =
        tile.X >= 0
        && tile.X < playfield.Width
        && tile.Y >= 0
        && tile.Y < playfield.Height

    tiles |> Set.forall isWithinBounds

let private checkCollisions playfield tiles =
    tiles |> Set.intersect playfield.TilePositions |> Set.isEmpty

let private validatePlacement piece playfield =
    piece
    |> Piece.toBlock
    |> Block.tilePositions
    |> Set.map (Position.add piece.Position)
    |> Result.validateAll
        [ checkBounds playfield --> OutOfBounds
          checkCollisions playfield --> Collision ]

/// <summary>
/// Checks if a piece can be placed on the playfield.
/// </summary>
/// <param name="piece">The piece to check.</param>
/// <param name="playfield">The playfield to check on.</param>
/// <returns>An Ok () if the piece can be placed, or an error if it cannot.</returns>
let canPlace piece playfield =
    playfield |> validatePlacement piece |> Result.ignore

/// <summary>
/// Tries to place a piece on the playfield.
/// </summary>
/// <param name="piece">The piece to place.</param>
/// <param name="playfield">The playfield to place the piece on.</param>
/// <returns>An Ok playfield if the piece was placed successfully, or an error if it could not be placed.</returns>
let tryPlace piece playfield =
    playfield
    |> validatePlacement piece
    |> Result.map (fun tiles ->
        { playfield with
            TilePositions = playfield.TilePositions |> Set.union tiles })
