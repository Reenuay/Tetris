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

let private o = Cell.Empty

let private x = Cell.Occupied

let private I =
    array2D [
        [ o; x; o; o ]
        [ o; x; o; o ]
        [ o; x; o; o ]
        [ o; x; o; o ]
    ]
    |> TetrominoShape

let private J =
    array2D [
        [o; x; o]
        [o; x; o]
        [x; x; o]
    ]
    |> TetrominoShape

let private L =
    array2D [
        [o; x; o]
        [o; x; o]
        [o; x; x]
    ]
    |> TetrominoShape

let private O =
    array2D [
        [x; x]
        [x; x]
    ]
    |> TetrominoShape

let private S =
    array2D [
        [o; x; x]
        [x; x; o]
        [o; o; o]
    ]
    |> TetrominoShape

let private T =
    array2D [
        [o; x; o]
        [x; x; x]
        [o; o; o]
    ]
    |> TetrominoShape

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
/// Rotates a tetromino shape clockwise till it matches the given orientation.
/// </summary>
/// <param name="orientation">The orientation to match.</param>
/// <param name="shape">The tetromino shape to rotate.</param>
/// <returns>The rotated tetromino shape.</returns>
let private rotateByOrientation orientation =
    match orientation with
    | TetrominoOrientation.Up -> id
    | TetrominoOrientation.Right -> rotate
    | TetrominoOrientation.Down -> rotate >> rotate
    | TetrominoOrientation.Left -> rotate >> rotate >> rotate

/// Caches all possible shapes of all tetromino pieces in all orientations.
let private shapeCache =
    lazy
        [ TetrominoType.I, I
          TetrominoType.J, J
          TetrominoType.L, L
          TetrominoType.O, O
          TetrominoType.S, S
          TetrominoType.T, T
          TetrominoType.Z, Z ]
        |> List.collect (fun (tetrominoType, shapeInInitialOrientation) ->
            TetrominoOrientation.all
            |> List.map (fun orientation ->
                (tetrominoType, orientation), rotateByOrientation orientation shapeInInitialOrientation))
        |> Map.ofList

/// <summary>
/// Gets the shape of a tetromino piece in a given type and orientation.
/// </summary>
/// <param name="tetrominoType">The type of the tetromino piece.</param>
/// <param name="orientation">The orientation of the tetromino piece.</param>
/// <returns>The shape of the tetromino piece.</returns>
let get tetrominoType orientation =
    // This will never throw because the cache is initialized with all possible combinations.
    shapeCache.Force()[(tetrominoType, orientation)]
