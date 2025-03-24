namespace Tetris.Core


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
/// Represents the orientations of a tetromino piece on the game board.
/// Default orientation is Up.
/// </summary>
[<RequireQualifiedAccess>]
type TetrominoOrientation =
    | Up
    | Right
    | Down
    | Left

/// <summary>
/// Represents the position of a tetromino piece on the game board.
/// </summary>
type TetrominoPosition = TetrominoPosition of x: int * y: int

/// <summary>
/// Represents a tetromino piece that consists of cells.
/// </summary>
type TetrominoPiece =
    { Type: TetrominoType
      Orientation: TetrominoOrientation
      Position: TetrominoPosition }
