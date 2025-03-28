// This file is added to .fantomasignore to prevent it from being formatted by Fantomas so shape definitions stay pretty.
[<RequireQualifiedAccess>]
module Tetris.Core.TetrominoShape


type Cell = Place.Place
type Shape = Shape.Shape

let private o = Place.Empty

let private x = Place.Occupied

let private I =
    array2D [
        [ o; x; o; o ]
        [ o; x; o; o ]
        [ o; x; o; o ]
        [ o; x; o; o ]
    ]
    |> Shape.create

let private J =
    array2D [
        [o; x; o]
        [o; x; o]
        [x; x; o]
    ]
    |> Shape.create

let private L =
    array2D [
        [o; x; o]
        [o; x; o]
        [o; x; x]
    ]
    |> Shape.create

let private O =
    array2D [
        [x; x]
        [x; x]
    ]
    |> Shape.create

let private S =
    array2D [
        [o; x; x]
        [x; x; o]
        [o; o; o]
    ]
    |> Shape.create

let private T =
    array2D [
        [o; x; o]
        [x; x; x]
        [o; o; o]
    ]
    |> Shape.create

let private Z =
    array2D [
        [x; x; o]
        [o; x; x]
        [o; o; o]
    ]
    |> Shape.create

/// <summary>
/// Rotates a tetromino shape clockwise till it matches the given orientation.
/// </summary>
/// <param name="orientation">The orientation to match.</param>
/// <param name="shape">The tetromino shape to rotate.</param>
/// <returns>The rotated tetromino shape.</returns>
let private rotateByOrientation orientation =
    match orientation with
    | Direction.Up -> id
    | Direction.Right -> Shape.rotateClockwise
    | Direction.Down -> Shape.rotateClockwise >> Shape.rotateClockwise
    | Direction.Left -> Shape.rotateClockwise >> Shape.rotateClockwise >> Shape.rotateClockwise

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
            Direction.all
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

