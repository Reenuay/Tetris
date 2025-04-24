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

/// <summary>
/// Alias for <see cref="Direction"/>.
/// </summary>
type Orientation = Direction

[<RequireQualifiedAccess>]
module Direction =
    /// All directions in clockwise order starting from `Direction.Up`.
    let all = [ Direction.Up; Direction.Right; Direction.Down; Direction.Left ]

    /// <summary>
    /// Rotates direction by the given rotation.
    /// </summary>
    /// <param name="rotation">The direction of rotation.</param>
    /// <param name="direction">The current direction.</param>
    /// <returns>The new direction after rotation.</returns>
    let rotate rotation direction =
        match rotation, direction with
        | Rotation.Clockwise, Direction.Up -> Direction.Right
        | Rotation.Clockwise, Direction.Right -> Direction.Down
        | Rotation.Clockwise, Direction.Down -> Direction.Left
        | Rotation.Clockwise, Direction.Left -> Direction.Up
        | Rotation.CounterClockwise, Direction.Up -> Direction.Left
        | Rotation.CounterClockwise, Direction.Left -> Direction.Down
        | Rotation.CounterClockwise, Direction.Down -> Direction.Right
        | Rotation.CounterClockwise, Direction.Right -> Direction.Up
