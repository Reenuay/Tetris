namespace Tetris.Core.Types

open FSharpPlus


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
    /// Creates a new game board with the specified width and height with all cells initialized to empty state.
    /// Does not validate the width or height.
    /// </summary>
    /// <param name="width">The width of the game board.</param>
    /// <param name="height">The height of the game board.</param>
    /// <returns>A new game board with the specified width and height and all cells initialized to empty state.</returns>
    let private create width height =
        Cell.Empty
        |> konst
        |> konst
        |> Array2D.init width height
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
    /// A standard 10x20 game board.
    /// </summary>
    let standard = create 10 20

/// <summary>
/// Contains tetromino pieces for each type and functions for manipulating them.
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
        array2D [
            [ o; x; o; o ]
            [ o; x; o; o ]
            [ o; x; o; o ]
            [ o; x; o; o ]
        ]
        |> TetrominoPiece

    /// <summary>
    /// Represents J tetromino piece in it's standard orientation.
    /// </summary>
    let J =
        array2D [
            [o; x; o]
            [o; x; o]
            [x; x; o]
        ]
         |> TetrominoPiece

    /// <summary>
    /// Represents L tetromino piece in it's standard orientation.
    /// </summary>
    let L =
        array2D [
            [o; x; o]
            [o; x; o]
            [o; x; x]
        ]
        |> TetrominoPiece

    /// <summary>
    /// Represents O tetromino piece in it's standard orientation.
    /// </summary>
    let O =
        array2D [
            [x; x]
            [x; x]
        ]
        |> TetrominoPiece

    /// <summary>
    /// Represents S tetromino piece in it's standard orientation.
    /// </summary>
    let S =
        array2D [
            [o; x; x]
            [x; x; o]
            [o; o; o]
        ]
        |> TetrominoPiece

    /// <summary>
    /// Represents T tetromino piece in it's standard orientation.
    /// </summary>
    let T =
        array2D [
            [o; x; o]
            [x; x; x]
            [o; o; o]
        ]
        |> TetrominoPiece

    /// <summary>
    /// Represents Z tetromino piece in it's standard orientation.
    /// </summary>
    let Z =
        array2D [
            [x; x; o]
            [o; x; x]
            [o; o; o]
        ]
        |> TetrominoPiece

    /// <summary>
    /// Rotates a tetromino piece clockwise.
    /// </summary>
    /// <param name="piece">The tetromino piece to rotate.</param>
    /// <returns>A new tetromino piece that is rotated clockwise.</returns>
    let rotate (TetrominoPiece cells) =
        let width = Array2D.length2 cells
        let height = Array2D.length1 cells
        let rotated = Array2D.create width height Cell.Empty

        for i in 0 .. height - 1 do
            for j in 0 .. width - 1 do
                rotated[j, width - 1 - i] <- cells[i, j]

        TetrominoPiece rotated
