namespace Tetris.Core

open System
open FSharpPlus.Data


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
        // Adding UInt16.MaxValue works as minus 1
        let dx, dy =
            match direction with
            | Direction.Up -> 0us, UInt16.MaxValue
            | Direction.Right -> 1us, 0us
            | Direction.Down -> 0us, 1us
            | Direction.Left -> UInt16.MaxValue, 0us

        { piece with
            Position =
                { piece.Position with
                    X = piece.Position.X + dx
                    Y = piece.Position.Y + dy } }

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
    /// Returns the tiles that make up a piece.
    /// </summary>
    /// <param name="piece">The piece to get the tiles for.</param>
    /// <returns>The tiles that make up the piece.</returns>
    /// <remarks>
    /// The tiles are returned as a set of positions relative to the piece's position.
    /// </remarks>
    let tiles piece =
        Tetromino.toBlock piece.Orientation piece.Tetromino
        |> Block.tiles
        |> NonEmptySet.toSet
        |> Set.map (Position.add piece.Position)
