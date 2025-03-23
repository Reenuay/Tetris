namespace Tetris.Core.Types


/// <summary>
/// Represents a single cell in the game board that can be either empty or occupied.
/// Additional data like color can be stored in the occupied cell.
/// </summary>
/// <typeparam name="'cellData">The type of data stored in the occupied cell.</typeparam>
type BoardCell<'cellData> =
    | Empty
    | Occupied of 'cellData

/// <summary>
/// Represents a 2D game board that consists of cells.
/// </summary>
/// <typeparam name="'cellData">The type of addtitional data that can be stored in each occupied cell of the board.</typeparam>
type GameBoard<'cellData> =
    private
        { Cells: BoardCell<'cellData> list list }

/// <summary>
/// Represents the tetrominoes that can be used in the game.
/// </summary>
type TetrominoType =
    | I
    | J
    | L
    | O
    | S
    | T
    | Z
