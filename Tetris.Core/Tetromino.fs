// This file is added to .fantomasignore to prevent it from being formatted by Fantomas so block definitions stay pretty.
namespace Tetris.Core


/// <summary>
/// Represents the tetrominoes that can be used in the game.
/// - I: Line
/// - J: Reverse L-shape
/// - L: L-shape
/// - O: Square
/// - S: S-shape
/// - T: T-shape
/// - Z: Z-shape
/// </summary>
type Tetromino =
    | I
    | J
    | L
    | O
    | S
    | T
    | Z

[<RequireQualifiedAccess>]
module Tetromino =
    type Orientation = Direction

    let private o = Tile.Empty

    let private x = Tile.Occupied

    /// All tetrominoes.
    let all = [ I; J; L; O; S; T; Z ]

    let private I =
        array2D [
            [ o; x; o; o ]
            [ o; x; o; o ]
            [ o; x; o; o ]
            [ o; x; o; o ]
        ]
        |> Block.create

    let private J =
        array2D [
            [ o; x; o ]
            [ o; x; o ]
            [ x; x; o ]
        ]
        |> Block.create

    let private L =
        array2D [
            [ o; x; o ]
            [ o; x; o ]
            [ o; x; x ]
        ]
        |> Block.create

    let private O =
        array2D [
            [ x; x ]
            [ x; x ]
        ]
        |> Block.create

    let private S =
        array2D [
            [ o; x; x ]
            [ x; x; o ]
            [ o; o; o ]
        ]
        |> Block.create

    let private T =
        array2D [
            [ o; x; o ]
            [ x; x; x ]
            [ o; o; o ]
        ]
        |> Block.create

    let private Z =
        array2D [
            [ x; x; o ]
            [ o; x; x ]
            [ o; o; o ]
        ]
        |> Block.create

    let private rotateByOrientation orientation =
        match orientation with
        | Orientation.Up -> id
        | Orientation.Right -> Block.rotateClockwise
        | Orientation.Down -> Block.rotateClockwise >> Block.rotateClockwise
        | Orientation.Left -> Block.rotateClockwise >> Block.rotateClockwise >> Block.rotateClockwise

    // A lazy cache of all possible block representations of all tetrominoes in all orientations.
    let private blockCache =
        lazy
            [ Tetromino.I, I
              Tetromino.J, J
              Tetromino.L, L
              Tetromino.O, O
              Tetromino.S, S
              Tetromino.T, T
              Tetromino.Z, Z ]
            |> List.collect (fun (tetrominoType, blockInInitialOrientation) ->
                Direction.all
                |> List.map (fun orientation ->
                    (tetrominoType, orientation), rotateByOrientation orientation blockInInitialOrientation))
            |> Map.ofList

    /// <summary>
    /// Gets the block representation of a tetromino in the given orientation.
    /// </summary>
    /// <param name="orientation">The orientation of the tetromino.</param>
    /// <param name="tetromino">The tetromino.</param>
    /// <returns>The block representation of the tetromino in the given orientation.</returns>
    let toBlock orientation tetromino =
        // This will never throw because the cache is initialized with all possible combinations.
        blockCache.Force()[(tetromino, orientation)]
