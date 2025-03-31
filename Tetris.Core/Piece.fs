namespace Tetris.Core


module Orientation = Direction

type Piece =
    { Tetromino: Tetromino
      Position: Position
      Orientation: Orientation }

module Piece =
    /// <summary>
    /// Moves a piece in a given direction by one unit.
    /// </summary>
    /// <param name="direction">The direction to move the piece in.</param>
    /// <param name="piece">The piece to move.</param>
    /// <returns>The moved piece.</returns>
    let move direction piece =
        let dx, dy =
            match direction with
            | Direction.Up -> 0, -1
            | Direction.Right -> 1, 0
            | Direction.Down -> 0, 1
            | Direction.Left -> -1, 0

        { piece with
            Position =
                { piece.Position with
                    x = piece.Position.x + dx
                    y = piece.Position.y + dy } }

    /// <summary>
    /// Rotates a piece in a given direction.
    /// </summary>
    /// <param name="direction">The direction to rotate the piece in.</param>
    /// <param name="piece">The piece to rotate.</param>
    /// <returns>The rotated piece.</returns>
    let rotate direction piece =
        { piece with
            Orientation = Orientation.rotate direction piece.Orientation }

    /// <summary>
    /// Converts a piece to a block.
    /// </summary>
    /// <param name="piece">The piece to convert.</param>
    /// <returns>The block representation of the piece.</returns>
    let toBlock piece =
        Tetromino.toBlock piece.Orientation piece.Tetromino
