namespace Tetris.Core


/// <summary>
/// Represents the position in a two-dimensional space.
/// </summary>
[<Struct>]
type Position = { X: uint16; Y: uint16 }

module Position =
    /// <summary>
    /// Returns a new position that is the result of adding the given offset to the given position.
    /// </summary>
    /// <param name="offset">The offset to add to the position.</param>
    /// <param name="position">The position to add the offset to.</param>
    /// <returns>A new position that is the result of adding the given offset to the given position.</returns>
    let add offset position =
        { X = position.X + offset.X
          Y = position.Y + offset.Y }
