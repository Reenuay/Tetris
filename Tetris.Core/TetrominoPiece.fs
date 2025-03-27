[<RequireQualifiedAccess>]
module Tetris.Core.TetrominoPiece


type TetrominoType = TetrominoType.TetrominoType
type Direction = Direction.Direction
type Position = Position.Position

/// <summary>
/// Represents a tetromino piece on the board.
/// </summary>
type TetrominoPiece =
    { Type: TetrominoType
      Orientation: Direction
      Position: Position }

/// <summary>
/// Gets the position of a tetromino piece.
/// </summary>
/// <param name="piece">The tetromino piece.</param>
/// <returns>The position of the tetromino piece.</returns>
let position piece = piece.Position

/// <summary>
/// Gets the shape of a tetromino piece by its type and orientation.
/// </summary>
/// <param name="piece">The tetromino piece.</param>
/// <returns>A cell array representing the geometrical shape of the tetromino piece.</returns>
let getShape piece =
    TetrominoShape.get piece.Type piece.Orientation

/// <summary>
/// Moves a tetromino piece in a given direction.
/// </summary>
/// <param name="direction">The direction of movement.</param>
/// <param name="piece">The tetromino piece to move.</param>
/// <returns>The moved tetromino piece.</returns>
let move direction piece =
    { piece with
        Position = Position.move direction piece.Position }

/// <summary>
/// Rotates a tetromino piece by a given rotation.
/// </summary>
/// <param name="rotation">The rotation direction.</param>
/// <param name="piece">The tetromino piece to rotate.</param>
/// <returns>The rotated tetromino piece.</returns>
let rotate rotation piece =
    { piece with
        Orientation = Direction.rotate rotation piece.Orientation }
