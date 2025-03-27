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

/// All possible tetromino orientations in the clockwise order.
let all = [ Up; Right; Down; Left ]

/// <summary>
/// Rotates the tetromino piece in the given direction.
/// </summary>
/// <param name="direction">The direction to rotate the tetromino piece.</param>
/// <param name="orientation">The current orientation of the tetromino piece.</param>
/// <returns>The new orientation of the tetromino piece.</returns>
let rotate direction orientation =
    let currentIndex = List.findIndex ((=) orientation) all
    let length = List.length all

    let nextIndex =
        match direction with
        | Rotation.Clockwise -> (currentIndex + 1) % length
        | Rotation.CounterClockwise -> (currentIndex + 3) % length

    List.item nextIndex all
