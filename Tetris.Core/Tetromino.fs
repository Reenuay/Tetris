// This file is added to .fantomasignore to prevent it from being formatted by Fantomas so block definitions stay pretty.
namespace Tetris.Core


module Orientation = Direction

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

    let private o = false

    let private x = true

    /// All tetrominoes.
    let all = [ I; J; L; O; S; T; Z ]

    let private createBlockUnsafe tiles =
        tiles |> Block.tryCreate |> Result.defaultWith (fun _ -> failwith "Invalid block")

    let private I =
        array2D [
            [ o; x; o; o ]
            [ o; x; o; o ]
            [ o; x; o; o ]
            [ o; x; o; o ]
        ]
        |> createBlockUnsafe

    let private J =
        array2D [
            [ o; x; o ]
            [ o; x; o ]
            [ x; x; o ]
        ]
        |> createBlockUnsafe

    let private L =
        array2D [
            [ o; x; o ]
            [ o; x; o ]
            [ o; x; x ]
        ]
        |> createBlockUnsafe

    let private O =
        array2D [
            [ x; x ]
            [ x; x ]
        ]
        |> createBlockUnsafe

    let private S =
        array2D [
            [ o; x; x ]
            [ x; x; o ]
            [ o; o; o ]
        ]
        |> createBlockUnsafe

    let private T =
        array2D [
            [ o; x; o ]
            [ x; x; x ]
            [ o; o; o ]
        ]
        |> createBlockUnsafe

    let private Z =
        array2D [
            [ x; x; o ]
            [ o; x; x ]
            [ o; o; o ]
        ]
        |> createBlockUnsafe

    let private rotateTimes times block =
        [ 1 ..times ]
        |> List.fold (fun block _ -> Block.rotateClockwise block) block

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
                Orientation.all
                |> List.mapi (fun i orientation ->
                    (tetrominoType, orientation), rotateTimes (i + 1) blockInInitialOrientation))
            |> Map.ofList

    /// <summary>
    /// Gets the block representation of the tetromino in the given orientation.
    /// </summary>
    /// <param name="orientation">The orientation of the tetromino.</param>
    /// <param name="tetromino">The tetromino.</param>
    /// <returns>The block representation of the tetromino in the given orientation.</returns>
    let toBlock orientation tetromino =
        // This will never throw because the cache is initialized with all possible combinations.
        blockCache.Force()[(tetromino, orientation)]
