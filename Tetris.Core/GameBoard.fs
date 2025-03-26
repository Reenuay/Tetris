/// <summary>
/// Contains the definition of the GameBoard type and related functions.
/// </summary>
[<RequireQualifiedAccess>]
module Tetris.Core.GameBoard

open FSharpPlus


type Cell = Cell.Cell
type TetrominoPiece = TetrominoPiece.TetrominoPiece

/// <summary>
/// Represents a 2D game board that consists of cells.
/// </summary>
type GameBoard =
    private
        { Cells: Cell[,]
          ActivePiece: TetrominoPiece option }

/// <summary>
/// Represents the possible errors that can occur during the creation of a game board.
/// </summary>
type GameBoardCreationError =
    | WidthTooSmall of minimalWidth: int * actualWidth: int
    | HeightTooSmall of minimalHeight: int * actualHeight: int

let minWidth = 8

let minHeight = 10

/// <summary>
/// Creates a new game board with the specified width and height with all cells initialized to empty state.
/// Does not validate the width or height.
/// </summary>
/// <param name="width">The width of the game board.</param>
/// <param name="height">The height of the game board.</param>
/// <returns>A new game board with the specified width and height and all cells initialized to empty state.</returns>
let private create width height =
    let cells = Cell.Empty |> konst |> konst |> Array2D.init width height

    { Cells = cells; ActivePiece = None }

/// <summary>
/// Validates the width of a game board.
/// </summary>
/// <param name="width">The width of the game board.</param>
/// <returns>A list of errors if the width is invalid.</returns>
let private validateWidth width =
    if width < minWidth then
        Error [ WidthTooSmall(minWidth, width) ]
    else
        Ok width

/// <summary>
/// Validates the height of a game board.
/// </summary>
/// <param name="height">The height of the game board.</param>
/// <returns>A list of errors if the height is invalid.</returns>
let private validateHeight height =
    if height < minHeight then
        Error [ HeightTooSmall(minHeight, height) ]
    else
        Ok height

/// <summary>
/// Tries to create a game board with the specified width and height with all cells initialized to empty state.
/// Validates the width and height and returns an error list.
/// </summary>
/// <param name="width">The width of the game board.</param>
/// <param name="height">The height of the game board.</param>
/// <returns>
/// A new game board with the specified width and height and all cells initialized to empty state if the width and height are valid.
/// Otherwise, returns an error list with the validation errors.
/// </returns>
let tryCreate width height =
    create <!> validateWidth width <*> validateHeight height

/// <summary>
/// Checks if the piece can be placed on the game board.
/// False if the piece is out of bounds or intersects with any non-empty cells on the game board.
/// </summary>
/// <param name="piece">The piece to check for collision.</param>
/// <param name="board">The game board to check for collision.</param>
/// <returns>True if the piece can be placed on the game board; otherwise, false.</returns>
let canPlacePiece piece board =
    let { Cells = boardCells } = board
    let pieceCells = piece |> TetrominoPiece.getShape |> TetrominoShape.getCells
    let position = piece.Position
    let pieceWidth = pieceCells |> Array2D.length1
    let pieceHeight = pieceCells |> Array2D.length2
    let boardWidth = boardCells |> Array2D.length1
    let boardHeight = boardCells |> Array2D.length2

    let isWithinBounds =
        position.X >= 0
        && position.X + pieceWidth <= boardWidth
        && position.Y >= 0
        && position.Y + pieceHeight <= boardHeight

    // Check for collision with non-empty cells on the game board
    // This has to be a function because the computation has to be deferred
    // Firstly because isWithinBounds has to be checked first so no index out of bounds exception is thrown
    // Secondly because it avoids unnecessary computation if isWithinBounds is false
    let hasNoCollision _ =
        seq {
            for i in 0 .. pieceWidth - 1 do
                for j in 0 .. pieceHeight - 1 do
                    yield
                        pieceCells[i, j] = Cell.Empty
                        || boardCells[position.X + i, position.Y + j] = Cell.Empty
        }
        |> Seq.forall id

    isWithinBounds && hasNoCollision ()
