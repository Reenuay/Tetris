/// <summary>
/// Contains the definition of the Cell type and related functions.
/// </summary>
[<RequireQualifiedAccess>]
module Tetris.Core.Cell


/// <summary>
/// Represents a single cell in the game board that can be either empty or occupied.
/// </summary>
type Cell =
    | Empty
    | Occupied
