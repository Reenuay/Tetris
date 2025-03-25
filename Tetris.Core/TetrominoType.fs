/// <summary>
/// Contains the TetrominoType type and its associated functions.
/// </summary>
[<RequireQualifiedAccess>]
module Tetris.Core.TetrominoType


/// <summary>
/// Represents the tetrominoes that can be used in the game.
/// - I: Line
/// - J: Reverse L-shape
/// - L: L-shape
/// - O: Square
/// - S: S-shape
/// - T: T-shape
/// - Z: Z-shape
/// </summary>
type TetrominoType =
    | I
    | J
    | L
    | O
    | S
    | T
    | Z

/// <summary>
/// Associates each tetromino type with its corresponding shape.
/// </summary>
/// <param name="tetrominoType">The tetromino type to convert to a shape.</param>
/// <returns>The shape corresponding to the given tetromino type.</returns>
let toShape tetrominoType =
    match tetrominoType with
    | I -> Shape.I
    | J -> Shape.J
    | L -> Shape.L
    | O -> Shape.O
    | S -> Shape.S
    | T -> Shape.T
    | Z -> Shape.Z
