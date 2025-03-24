namespace Tetris.Core

open FSharpPlus


/// <summary>
/// Contains functions to manipulate tetromino shapes.
/// </summary>
[<RequireQualifiedAccess>]
module private Shape =
    /// <summary>
    /// Rotates a tetromino shape clockwise.
    /// </summary>
    /// <param name="shape">The tetromino shape to rotate.</param>
    let rotate shape =
        let width = Array2D.length2 shape
        let height = Array2D.length1 shape
        let rotated = Array2D.create width height Cell.Empty

        for i in 0 .. height - 1 do
            for j in 0 .. width - 1 do
                rotated[j, width - 1 - i] <- shape[i, j]

        rotated

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
    /// Standard width of a game board.
    /// </summary>
    let standardWidth = 10

    /// <summary>
    /// Standard height of a game board.
    /// </summary>
    let standardHeight = 20

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

    /// <summary>
    /// A standard 10x20 game board.
    /// </summary>
    let standard = create standardWidth standardHeight

/// <summary>
/// Contains functions for manipulating tetromino orientations.
/// </summary>
[<RequireQualifiedAccess>]
module TetrominoOrientation =
    /// <summary>
    /// The total number of tetromino orientations.
    /// </summary>
    let totalCount = 4

    /// <summary>
    /// Rotates a tetromino orientation clockwise.
    /// </summary>
    /// <param name="orientation">The tetromino orientation to rotate.</param>
    /// <returns>A new tetromino orientation rotated clockwise.</returns>
    let rotate orientation =
        match orientation with
        | TetrominoOrientation.Up -> TetrominoOrientation.Right
        | TetrominoOrientation.Right -> TetrominoOrientation.Down
        | TetrominoOrientation.Down -> TetrominoOrientation.Left
        | TetrominoOrientation.Left -> TetrominoOrientation.Up

/// <summary>
/// Contains tetromino pieces for each type and functions for manipulating them.
/// </summary>
[<RequireQualifiedAccess>]
module TetrominoPiece =
    /// <summary>
    /// Caches all possible shapes of all tetromino pieces in all orientations.
    /// </summary>
    let private shapeCache =
        [ TetrominoType.I, Shape.I
          TetrominoType.J, Shape.J
          TetrominoType.L, Shape.L
          TetrominoType.O, Shape.O
          TetrominoType.S, Shape.S
          TetrominoType.T, Shape.T
          TetrominoType.Z, Shape.Z ]
        |> List.collect (fun (type', initialShape) ->
            let mutable orientation = TetrominoOrientation.Up
            let mutable shape = initialShape

            [ for _ in 1 .. TetrominoOrientation.totalCount ->
                  orientation <- TetrominoOrientation.rotate orientation
                  shape <- Shape.rotate shape
                  (type', orientation), shape ])
        |> Map.ofList

    /// <summary>
    /// Gets the shape of a tetromino piece by its type and orientation.
    /// </summary>
    /// <param name="piece">The tetromino piece.</param>
    /// <returns>A cell array representing the geometrical shape of the tetromino piece.</returns>
    let toShape piece =
        shapeCache[(piece.Type, piece.Orientation)]
