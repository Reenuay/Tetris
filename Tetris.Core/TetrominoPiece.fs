/// <summary>
/// Contains the definition of the TetrominoPiece type and related functions.
/// </summary>
[<RequireQualifiedAccess>]
module Tetris.Core.TetrominoPiece


type TetrominoType = TetrominoType.TetrominoType
type TetrominoOrientation = TetrominoOrientation.TetrominoOrientation

/// <summary>
/// Represents the position of a tetromino piece on the board.
/// </summary>
type TetrominoPosition = { X: int; Y: int }

/// <summary>
/// Represents a tetromino piece on the board.
/// </summary>
type TetrominoPiece =
    { Type: TetrominoType
      Orientation: TetrominoOrientation
      Position: TetrominoPosition }

/// <summary>
/// Gets the shape of a tetromino piece by its type and orientation.
/// </summary>
/// <param name="piece">The tetromino piece.</param>
/// <returns>A cell array representing the geometrical shape of the tetromino piece.</returns>
let toShape piece =
    TetrominoShape.get piece.Type piece.Orientation

/// <summary>
/// Rotates a tetromino piece by 90 degrees clockwise.
/// </summary>
/// <param name="piece">The tetromino piece.</param>
/// <returns>A new tetromino piece with the same type and position, but rotated by 90 degrees clockwise.</returns>
let rotate piece =
    { piece with
        Orientation = TetrominoOrientation.rotate piece.Orientation }

/// <summary>
/// Moves a tetromino piece to the left by one unit.
/// </summary>
let moveLeft piece =
    { piece with
        Position =
            { X = piece.Position.X - 1
              Y = piece.Position.Y } }

/// <summary>
/// Moves a tetromino piece to the right by one unit.
/// </summary>
let moveRight piece =
    { piece with
        Position =
            { X = piece.Position.X + 1
              Y = piece.Position.Y } }

/// <summary>
/// Moves a tetromino piece down by one unit.
/// </summary>
let moveDown piece =
    { piece with
        Position =
            { X = piece.Position.X
              Y = piece.Position.Y + 1 } }
