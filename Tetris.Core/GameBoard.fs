[<RequireQualifiedAccess>]
module Tetris.Core.GameBoard

open FSharpPlus


type Cell = Place.Place
type Direction = Direction.Direction
type Rotation = Rotation.Rotation
type Position = Position.Position
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
    let cells = Place.Empty |> konst |> konst |> Array2D.init width height

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
/// Checks if the shape can be placed at the given position on the game board.
/// </summary>
/// <param name="shape">The shape of the tetromino piece.</param>
/// <param name="position">The position on the game board where the shape should be placed.</param>
/// <param name="board">The game board.</param>
/// <returns>True if the shape can be placed at the given position on the game board; otherwise, false.</returns>
let private canPlaceShape shape position board =
    let { Cells = boardCells } = board
    let { Position.X = x; Position.Y = y } = position
    let cells = shape |> TetrominoShape.getCells
    let pieceWidth = cells |> Array2D.length1
    let pieceHeight = cells |> Array2D.length2
    let boardWidth = boardCells |> Array2D.length1
    let boardHeight = boardCells |> Array2D.length2

    let isWithinBounds =
        x >= 0
        && x + pieceWidth <= boardWidth
        && y >= 0
        && y + pieceHeight <= boardHeight

    // Check for collision with non-empty cells on the game board
    // This has to be a function because the computation has to be deferred
    // Firstly because isWithinBounds has to be checked first so no index out of bounds exception is thrown
    // Secondly because it avoids unnecessary computation if isWithinBounds is false
    let hasNoCollision _ =
        seq {
            for i in 0 .. pieceWidth - 1 do
                for j in 0 .. pieceHeight - 1 do
                    yield cells[i, j] = Place.Empty || boardCells[x + i, y + j] = Place.Empty
        }
        |> Seq.forall id

    isWithinBounds && hasNoCollision ()
