namespace Tetris.Core


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
