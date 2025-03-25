// This file is added to .fantomasignore to prevent it from being formatted by Fantomas so shape definitions stay pretty.
/// <summary>
/// Contains all the possible shapes of tetromino pieces and functions to manipulate them.
/// </summary>
[<RequireQualifiedAccess>]
module Tetris.Core.Shape


type Cell = Cell.Cell

/// <summary>
/// Represents a shape of a tetromino piece.
/// </summary>
type Shape = private Shape of Cell[,]

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
let I =
    array2D [
        [ o; x; o; o ]
        [ o; x; o; o ]
        [ o; x; o; o ]
        [ o; x; o; o ]
    ]
    |> Shape

/// <summary>
/// Represents J tetromino piece.
/// </summary>
let J =
    array2D [
        [o; x; o]
        [o; x; o]
        [x; x; o]
    ]
    |> Shape

/// <summary>
/// Represents L tetromino piece.
/// </summary>
let L =
    array2D [
        [o; x; o]
        [o; x; o]
        [o; x; x]
    ]
    |> Shape

/// <summary>
/// Represents O tetromino piece.
/// </summary>
let O =
    array2D [
        [x; x]
        [x; x]
    ]
    |> Shape

/// <summary>
/// Represents S tetromino piece.
/// </summary>
let S =
    array2D [
        [o; x; x]
        [x; x; o]
        [o; o; o]
    ]
    |> Shape

/// <summary>
/// Represents T tetromino piece.
/// </summary>
let T =
    array2D [
        [o; x; o]
        [x; x; x]
        [o; o; o]
    ]
    |> Shape

/// <summary>
/// Represents Z tetromino
/// </summary>
let Z =
    array2D [
        [x; x; o]
        [o; x; x]
        [o; o; o]
    ]
    |> Shape

/// <summary>
/// Rotates a tetromino shape clockwise.
/// </summary>
/// <param name="shape">The tetromino shape to rotate.</param>
/// <returns>The rotated tetromino shape.</returns>
let rotate (Shape shape) =
    let width = Array2D.length2 shape
    let height = Array2D.length1 shape
    let rotated = Array2D.create width height Cell.Empty

    for i in 0 .. height - 1 do
        for j in 0 .. width - 1 do
            rotated[j, width - 1 - i] <- shape[i, j]

    Shape rotated
