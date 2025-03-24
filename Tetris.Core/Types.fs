namespace Tetris.Core.Types

open FSharpx.Collections


type Board<'a> = PersistentVector<PersistentVector<'a>>

/// <summary>
/// Represents a point in a 2D space.
/// </summary>
type Point = (int * int)

/// <summary>
/// Represents a single cell in the game board that can be either empty or occupied.
/// Additional data like color can be stored in the occupied cell.
/// </summary>
/// <typeparam name="'cellData">The type of data stored in the occupied cell.</typeparam>
[<RequireQualifiedAccess>]
type BoardCell<'cellData> =
    | Empty
    | Occupied of 'cellData

/// <summary>
/// Represents a 2D game board that consists of cells.
/// </summary>
/// <typeparam name="'cellData">The type of addtitional data that can be stored in each occupied cell of the board.</typeparam>
type GameBoard<'cellData> = private GameBoard of Board<BoardCell<'cellData>>

/// <summary>
/// Represents the tetrominoes that can be used in the game.
/// </summary>
[<RequireQualifiedAccess>]
type TetrominoType =
    | I
    | J
    | L
    | O
    | S
    | T
    | Z

/// <summary>
/// Represents the rotation of a tetromino.
/// </summary>
[<RequireQualifiedAccess>]
type TetrominoRotation =
    /// <summary>
    /// Represents the clockwise rotation of a tetromino.
    /// </summary>
    | Clockwise
    /// <summary>
    /// Represents the counterclockwise rotation of a tetromino.
    /// </summary>
    | CounterClockwise

/// <summary>
/// Represents a tetromino figure that consists of a list of points.
/// Each point represents a cell in the tetromino figure and is relative to the tetromino's origin.
/// </summary>
type TetrominoFigure = private TetrominoFigure of Point list
