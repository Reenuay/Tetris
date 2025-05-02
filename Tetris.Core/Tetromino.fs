namespace Tetris.Core

open FSharpPlus.Data


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
    module Orientation = Direction

    type Block = Block.Block

    let private o = false

    let private x = true

    /// All tetrominoes.
    let all = [ I; J; L; O; S; T; Z ]

    let private I =
        Block.create
            !![ { X = 0us; Y = 1us }
                { X = 1us; Y = 1us }
                { X = 2us; Y = 1us }
                { X = 3us; Y = 1us } ]

    let private J =
        Block.create
            !![ { X = 1us; Y = 0us }
                { X = 1us; Y = 1us }
                { X = 1us; Y = 2us }
                { X = 0us; Y = 2us } ]

    let private L =
        Block.create
            !![ { X = 0us; Y = 0us }
                { X = 0us; Y = 1us }
                { X = 0us; Y = 2us }
                { X = 1us; Y = 2us } ]

    let private O =
        Block.create
            !![ { X = 0us; Y = 0us }
                { X = 1us; Y = 0us }
                { X = 0us; Y = 1us }
                { X = 1us; Y = 1us } ]

    let private S =
        Block.create
            !![ { X = 1us; Y = 0us }
                { X = 2us; Y = 0us }
                { X = 0us; Y = 1us }
                { X = 1us; Y = 1us } ]

    let private T =
        Block.create
            !![ { X = 0us; Y = 1us }
                { X = 1us; Y = 1us }
                { X = 2us; Y = 1us }
                { X = 1us; Y = 0us } ]

    let private Z =
        Block.create
            !![ { X = 0us; Y = 0us }
                { X = 1us; Y = 0us }
                { X = 1us; Y = 1us }
                { X = 2us; Y = 1us } ]

    let private rotateTimes times block =
        [ 1..times ] |> List.fold (fun block _ -> Block.rotateClockwise block) block

    // Сache of all possible block representations of all tetrominoes in all orientations.
    let private blockCache =
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
        blockCache[(tetromino, orientation)]
