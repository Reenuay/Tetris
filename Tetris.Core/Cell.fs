[<RequireQualifiedAccess>]
module Tetris.Core.Cell


/// <summary>
/// Represents a single cell in the game board that can be either empty or occupied.
/// </summary>
type Cell =
    | Empty
    | Occupied
