/// <summary>
/// Contains the definition of the GameBoard type and related functions.
/// </summary>
[<RequireQualifiedAccess>]
module Tetris.Core.GameBoard

open FSharpPlus


type Cell = Cell.Cell

/// <summary>
/// Represents a 2D game board that consists of cells.
/// </summary>
type GameBoard = private GameBoard of Cell[,]

/// <summary>
/// Represents the possible errors that can occur during the creation of a game board.
/// </summary>
type GameBoardCreationError =
    | WidthTooSmall of minimalWidth: int * actualWidth: int
    | HeightTooSmall of minimalHeight: int * actualHeight: int

/// <summary>
/// Minimal width of a game board - 8.
/// </summary>
let minWidth = 8

/// <summary>
/// Minimal height of a game board - 10.
/// </summary>
let minHeight = 10

/// <summary>
/// Creates a new game board with the specified width and height with all cells initialized to empty state.
/// Does not validate the width or height.
/// </summary>
/// <param name="width">The width of the game board.</param>
/// <param name="height">The height of the game board.</param>
/// <returns>A new game board with the specified width and height and all cells initialized to empty state.</returns>
let private create width height =
    Cell.Empty |> konst |> konst |> Array2D.init width height |> GameBoard

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
