namespace Tetris.Core.Types

open FSharpPlus
open FSharpx.Collections


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
    let private create initial width height =
        (Cell.Empty, initial)
        |> konst
        |> PersistentVector.init (width * height)
        |> GameBoard

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
    /// Tries to create a game board with the specified width and height.
    /// Returns a list of errors if the creation fails.
    /// </summary>
    /// <param name="width">The width of the game board.</param>
    /// <param name="height">The height of the game board.</param>
    /// <returns>A game board with the specified width and height and all cells initialized to empty state.</returns>
    let tryCreate initial width height =
        create initial <!> validateWidth width <*> validateHeight height

    /// <summary>
    /// Creates a standard 10x20 game board.
    /// </summary>
    /// <returns>A standard 10x20 game board with all cells initialized to empty state.</returns>
    let createStandard initial = create initial 10 20

/// <summary>
/// Contains tetromino pieces and functions for manipulating them.
/// </summary>
[<RequireQualifiedAccess>]
module TetrominoPiece =
    /// <summary>
    /// Convenient alias for the empty cell.
    /// </summary>
    let private o = Cell.Empty

    /// <summary>
    /// Convenient alias for the occupied cell.
    /// </summary>
    let private x = Cell.Occupied

    /// <summary>
    /// Represents I tetromino piece in it's standard orientation.
    /// </summary>
    let I =
        [
            o; x; o; o;
            o; x; o; o;
            o; x; o; o;
            o; x; o; o;
        ]
        |> PersistentVector.ofSeq
        |> TetrominoPiece

    /// <summary>
    /// Represents J tetromino piece in it's standard orientation.
    /// </summary>
    let J =
        [
            o; x; o;
            o; x; o;
            x; x; o;
        ]
        |> PersistentVector.ofSeq
        |> TetrominoPiece

    /// <summary>
    /// Represents L tetromino piece in it's standard orientation.
    /// </summary>
    let L =
        [
            o; x; o;
            o; x; o;
            o; x; x;
        ]
        |> PersistentVector.ofSeq
        |> TetrominoPiece

    /// <summary>
    /// Represents O tetromino piece in it's standard orientation.
    /// </summary>
    let O =
        [
            x; x;
            x; x;
        ]
        |> PersistentVector.ofSeq
        |> TetrominoPiece

    /// <summary>
    /// Represents S tetromino piece in it's standard orientation.
    /// </summary>
    let S =
        [
            o; x; x;
            x; x; o;
            o; o; o;
        ]
        |> PersistentVector.ofSeq
        |> TetrominoPiece

    /// <summary>
    /// Represents T tetromino piece in it's standard orientation.
    /// </summary>
    let T =
        [
            o; x; o;
            x; x; x;
            o; o; o;
        ]
        |> PersistentVector.ofSeq
        |> TetrominoPiece

    /// <summary>
    /// Represents Z tetromino piece in it's standard orientation.
    /// </summary>
    let Z =
        [
            x; x; o;
            o; x; x;
            o; o; o;
        ]
        |> PersistentVector.ofSeq
        |> TetrominoPiece
