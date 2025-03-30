namespace Tetris.Core


/// <summary>
/// Represents a tile that can be either empty or occupied.
/// </summary>
[<Struct>]
[<RequireQualifiedAccess>]
type Tile =
    | Empty
    | Occupied


/// <summary>
/// Represents the position in a two-dimensional space.
/// </summary>
[<Struct>]
type Position = { x: int; y: int }
