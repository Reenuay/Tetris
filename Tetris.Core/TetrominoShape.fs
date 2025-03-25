// This file is added to .fantomasignore to prevent it from being formatted by Fantomas so shape definitions stay pretty.
/// <summary>
/// Contains all the possible shapes of tetromino pieces and functions to manipulate them.
/// </summary>
[<RequireQualifiedAccess>]
module Tetris.Core.TetrominoShape


type Cell = Cell.Cell

/// <summary>
/// Represents a shape of a tetromino piece.
/// </summary>
type TetrominoShape = private TetrominoShape of Cell[,]

/// <summary>
/// Convenient alias for the empty cell.
/// </summary>
let private o = Cell.Empty

/// <summary>
/// Convenient alias for the occupied cell.
/// </summary>
let private x = Cell.Occupied

/// <summary>
/// Represents I tetromino piece.
/// </summary>
let private I =
    array2D [
        [ o; x; o; o ]
        [ o; x; o; o ]
        [ o; x; o; o ]
        [ o; x; o; o ]
    ]
    |> TetrominoShape

/// <summary>
/// Represents J tetromino piece.
/// </summary>
let private J =
    array2D [
        [o; x; o]
        [o; x; o]
        [x; x; o]
    ]
    |> TetrominoShape

/// <summary>
/// Represents L tetromino piece.
/// </summary>
let private L =
    array2D [
        [o; x; o]
        [o; x; o]
        [o; x; x]
    ]
    |> TetrominoShape

/// <summary>
/// Represents O tetromino piece.
/// </summary>
let private O =
    array2D [
        [x; x]
        [x; x]
    ]
    |> TetrominoShape

/// <summary>
/// Represents S tetromino piece.
/// </summary>
let private S =
    array2D [
        [o; x; x]
        [x; x; o]
        [o; o; o]
    ]
    |> TetrominoShape

/// <summary>
/// Represents T tetromino piece.
/// </summary>
let private T =
    array2D [
        [o; x; o]
        [x; x; x]
        [o; o; o]
    ]
    |> TetrominoShape

/// <summary>
/// Represents Z tetromino piece.
/// </summary>
let private Z =
    array2D [
        [x; x; o]
        [o; x; x]
        [o; o; o]
    ]
    |> TetrominoShape

/// <summary>
/// Rotates a tetromino shape clockwise.
/// </summary>
/// <param name="shape">The tetromino shape to rotate.</param>
/// <returns>The rotated tetromino shape.</returns>
let private rotate (TetrominoShape shape) =
    let width = Array2D.length2 shape
    let height = Array2D.length1 shape
    let rotated = Array2D.create width height Cell.Empty

    for i in 0 .. height - 1 do
        for j in 0 .. width - 1 do
            rotated[j, width - 1 - i] <- shape[i, j]

    TetrominoShape rotated

/// <summary>
/// Caches all possible shapes of all tetromino pieces in all orientations.
/// </summary>
let private shapeCache =
    [ TetrominoType.I, I
      TetrominoType.J, J
      TetrominoType.L, L
      TetrominoType.O, O
      TetrominoType.S, S
      TetrominoType.T, T
      TetrominoType.Z, Z ]
    |> List.collect (fun (type', shapeInInitialOrientation) ->
        let mutable orientation = TetrominoOrientation.initial
        let mutable shape = shapeInInitialOrientation

        [ for _ in 1 .. TetrominoOrientation.totalCount ->
              orientation <- TetrominoOrientation.rotate orientation
              shape <- rotate shape
              (type', orientation), shape ])
    |> Map.ofList

/// <summary>
/// Gets the shape of a tetromino piece in a given type and orientation.
/// </summary>
/// <param name="type'"">The type of the tetromino piece.</param>
/// <param name="orientation">The orientation of the tetromino piece.</param>
/// <returns>The shape of the tetromino piece.</returns>
let get type' orientation =
    // This will never throw because the cache is initialized with all possible combinations.
    shapeCache[(type', orientation)]
