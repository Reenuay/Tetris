/// <summary>
/// Contains the definition of the TetrominoPiece type and related functions.
/// </summary>
[<RequireQualifiedAccess>]
module Tetris.Core.TetrominoPiece


type TetrominoType = TetrominoType.TetrominoType
type TetrominoOrientation = TetrominoOrientation.TetrominoOrientation

/// <summary>
/// Represents a tetromino piece on the board.
/// </summary>
type TetrominoPiece =
    private
        { Type: TetrominoType
          Orientation: TetrominoOrientation
          Position: int * int }

/// <summary>
/// Caches all possible shapes of all tetromino pieces in all orientations.
/// </summary>
let private shapeCache =
    [ TetrominoType.I
      TetrominoType.J
      TetrominoType.L
      TetrominoType.O
      TetrominoType.S
      TetrominoType.T
      TetrominoType.Z ]
    |> List.collect (fun type' ->
        let mutable orientation = TetrominoOrientation.initial
        let mutable shape = TetrominoType.toShape type'

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
