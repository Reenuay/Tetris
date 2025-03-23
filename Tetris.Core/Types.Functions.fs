namespace Tetris.Core.Types

open FSharpPlus
open FSharpPlus.Data


/// <summary>
/// Contains functions for creating and manipulating game boards.
/// </summary>
[<RequireQualifiedAccess>]
module GameBoard =
    /// <summary>
    /// Minimal width of a game board.
    /// </summary>
    let minWidth = 8

    /// <summary>
    /// Minimal height of a game board.
    /// </summary>
    let minHeight = 10

    /// <summary>
    /// Initializes a new game board with the specified width and height.
    /// </summary>
    /// <param name="width">The width of the game board.</param>
    /// <param name="height">The height of the game board.</param>
    let private createCells<'a> (width: int) (height: int) =
        konst BoardCell<'a>.Empty
        |> List.init width
        |> konst
        |> List.init height
        |> GameBoard

    /// <summary>
    /// Validates the width of a game board.
    /// </summary>
    /// <param name="width">The width of the game board.</param>
    /// <returns>A list of errors if the width is invalid.</returns>
    let private validateWidth width =
        if width < minWidth then
            Failure [ WidthTooSmall(minWidth, width) ]
        else
            Success width

    /// <summary>
    /// Validates the height of a game board.
    /// </summary>
    /// <param name="height">The height of the game board.</param>
    /// <returns>A list of errors if the height is invalid.</returns>
    let private validateHeight height =
        if height < minHeight then
            Failure [ HeightTooSmall(minHeight, height) ]
        else
            Success height

    /// <summary>
    /// Tries to create a game board with the specified width and height.
    /// Returns a list of errors if the creation fails.
    /// </summary>
    /// <param name="width">The width of the game board.</param>
    /// <param name="height">The height of the game board.</param>
    /// <returns>A game board with the specified width and height and all cells initialized to empty state.</returns>
    let tryCreate<'a> width height =
        createCells<'a> <!> validateWidth width <*> validateHeight height

    /// <summary>
    /// Represents a standard 10x20 game board.
    /// </summary>
    let standard<'a> =
        tryCreate<'a> 10 20 |> Validation.defaultWith (fun _ -> failwith "Unreachable")

/// <summary>
/// Contains functions for working with tetromino types.
/// </summary>
[<RequireQualifiedAccess>]
module TetrominoType =
    /// <summary>
    /// Creates a figure from the given tetromino type.
    /// </summary>
    /// <param name="type'">The tetromino type.</param>
    let toFigure type' =
        match type' with
        | TetrominoType.I -> [ [ X ]; [ X ]; [ X ]; [ X ] ]
        | TetrominoType.J -> [ [ O; X ]; [ O; X ]; [ X; X ] ]
        | TetrominoType.L -> [ [ X; O ]; [ X; O ]; [ X; X ] ]
        | TetrominoType.O -> [ [ X; X ]; [ X; X ] ]
        | TetrominoType.S -> [ [ O; X; X ]; [ X; X; O ] ]
        | TetrominoType.T -> [ [ X; X; X ]; [ O; X; O ] ]
        | TetrominoType.Z -> [ [ X; X; O ]; [ O; X; X ] ]
        |> TetrominoFigure
