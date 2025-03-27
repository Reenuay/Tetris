namespace Tetris.Core


[<RequireQualifiedAccess>]
module Cell =
    /// <summary>
    /// Represents a single cell in the game board that can be either empty or occupied.
    /// </summary>
    type Cell =
        | Empty
        | Occupied

[<RequireQualifiedAccess>]
module Rotation =
    /// <summary>
    /// Represents the rotation direction.
    /// </summary>
    type Rotation =
        | Clockwise
        | CounterClockwise

[<RequireQualifiedAccess>]
module Direction =
    /// <summary>
    /// Represents the direction of orientation or movement.
    /// </summary>
    type Direction =
        | Up
        | Right
        | Down
        | Left

    /// All possible directions of rotation in a clockwise order.
    let all = [ Up; Right; Down; Left ]

    /// <summary>
    /// Rotates direction by the given rotation.
    /// </summary>
    /// <param name="rotation">The direction of rotation.</param>
    /// <param name="orientation">The current direction.</param>
    /// <returns>The new direction after rotation.</returns>
    let rotate rotation orientation =
        let currentIndex = List.findIndex ((=) orientation) all
        let length = List.length all

        let nextIndex =
            match rotation with
            | Rotation.Clockwise -> (currentIndex + 1) % length
            | Rotation.CounterClockwise -> (currentIndex + 3) % length

        List.item nextIndex all

[<RequireQualifiedAccess>]
module Position =
    /// <summary>
    /// Represents the position of a tetromino piece on the board.
    /// </summary>
    type Position = { X: int; Y: int }

    /// <summary>
    /// Creates a new position with the given coordinates.
    /// </summary>
    /// <param name="x">The x-coordinate.</param>
    /// <param name="y">The y-coordinate.</param>
    /// <returns>The new position.</returns>
    let create x y = { X = x; Y = y }
