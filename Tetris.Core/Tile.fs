namespace Tetris.Core


/// <summary>
/// Represents a tile that can be either empty or occupied.
/// </summary>
[<Struct>]
[<RequireQualifiedAccess>]
type Tile =
    | Empty
    | Occupied
