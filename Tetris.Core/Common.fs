namespace Tetris.Core


/// <summary>
/// Represents a tile that can be either empty or occupied.
/// </summary>
[<Struct>]
[<RequireQualifiedAccess>]
type Tile =
    | Empty
    | Occupied

[<Struct>]
[<RequireQualifiedAccess>]
type Rotation =
    | Clockwise
    | CounterClockwise

/// <summary>
/// Represents the direction of orientation or movement.
/// </summary>
[<Struct>]
[<RequireQualifiedAccess>]
type Direction =
    | Up
    | Right
    | Down
    | Left

/// <summary>
/// Represents the position in a two-dimensional space.
/// </summary>
[<Struct>]
type Position = { x: int; y: int }

[<RequireQualifiedAccess>]
module Direction =
    /// All possible directions in the clockwise order.
    let all = [ Direction.Up; Direction.Right; Direction.Down; Direction.Left ]

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
