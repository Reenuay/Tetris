[<RequireQualifiedAccess>]
module Tetris.Core.Direction


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
