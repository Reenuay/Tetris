namespace Tetris.Core.Types


/// <summary>
/// Represents a single cell in the game board that can be either empty or occupied.
/// </summary>
[<RequireQualifiedAccess>]
type Cell =
    | Empty
    | Occupied

/// <summary>
/// Represents a 2D game board that consists of cells.
/// </summary>
type GameBoard = private GameBoard of Cell[,]

/// <summary>
/// Represents the tetrominoes that can be used in the game.
/// - I: Line
/// - J: Reverse L-shape
/// - L: L-shape
/// - O: Square
/// - S: S-shape
/// - T: T-shape
/// - Z: Z-shape
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
/// Represents a tetromino piece that consists of cells.
/// </summary>
type TetrominoPiece = private TetrominoPiece of Cell[,]
