namespace Tetris.Core


[<RequireQualifiedAccess>]
module Place =
    /// <summary>
    /// Represents an abstract place that can be either empty or occupied.
    /// </summary>
    [<Struct>]
    type Place =
        | Empty
        | Occupied

[<RequireQualifiedAccess>]
module Rotation =
    /// <summary>
    /// Represents the rotation direction.
    /// </summary>
    [<Struct>]
    type Rotation =
        | Clockwise
        | CounterClockwise

[<RequireQualifiedAccess>]
module Direction =
    /// <summary>
    /// Represents the direction of orientation or movement.
    /// </summary>
    [<Struct>]
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
    /// <param name="direction">The current direction.</param>
    /// <returns>The new direction after rotation.</returns>
    let rotate rotation direction =
        let currentIndex = List.findIndex ((=) direction) all
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
    [<Struct>]
    type Position = { X: int; Y: int }

    /// <summary>
    /// Creates a new position with the given coordinates.
    /// </summary>
    /// <param name="x">The x-coordinate.</param>
    /// <param name="y">The y-coordinate.</param>
    /// <returns>The new position.</returns>
    let create x y = { X = x; Y = y }

    /// <summary>
    /// Moves the position in the given direction.
    /// </summary>
    /// <param name="direction">The direction of movement.</param>
    /// <param name="position">The current position.</param>
    /// <returns>The new position after movement.</returns>
    let move direction position =
        match direction with
        | Direction.Up -> { position with Y = position.Y - 1 }
        | Direction.Right -> { position with X = position.X + 1 }
        | Direction.Down -> { position with Y = position.Y + 1 }
        | Direction.Left -> { position with X = position.X - 1 }
