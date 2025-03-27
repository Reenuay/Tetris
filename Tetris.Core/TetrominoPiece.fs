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
let getShape piece =
    TetrominoShape.get piece.Type piece.Orientation
