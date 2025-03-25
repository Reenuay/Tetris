/// <summary>
/// Contains the definition of the TetrominoOrientation type and related functions.
/// </summary>
[<RequireQualifiedAccess>]
module Tetris.Core.TetrominoOrientation


/// <summary>
/// Represents the orientations of a tetromino piece on the game board.
/// </summary>
type TetrominoOrientation =
    | Up
    | Right
    | Down
    | Left

/// All possible tetromino orientations.
let all = [ Up; Right; Down; Left ]

/// <summary>
/// Rotates a tetromino orientation clockwise.
/// </summary>
/// <param name="orientation">The tetromino orientation to rotate.</param>
/// <returns>A new tetromino orientation rotated clockwise.</returns>
let rotate orientation =
    match orientation with
    | Up -> Right
    | Right -> Down
    | Down -> Left
    | Left -> Up
