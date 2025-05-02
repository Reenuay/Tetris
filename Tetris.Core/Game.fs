[<RequireQualifiedAccess>]
module Tetris.Core.Game


/// <summary>
/// Represents the state of the game.
/// </summary>
type Game =
    private
        { Playfield: Choice<Playfield.Frozen, Playfield.Live>
          NextTetromino: Tetromino
          TetrominoGenerator: unit -> Tetromino }

type PlayerAction =
    | MoveLeft
    | MoveRight
    | RotateClockwise
    | RotateCounterClockwise
    | SoftDrop

type LoopEvent =
    | Input of PlayerAction
    | Tick

type StateChange =
    | Unchanged
    | PieceSpawned
    | PieceUpdated
    | PieceFixed of Piece
    | LinesCleared of int list
    | GameOver

/// <summary>
/// Creates a standard tetromino generator using System.Random.
/// </summary>
let createStandardTetrominoGenerator () =
    let rnd = System.Random()
    fun () -> Tetromino.all[rnd.Next Tetromino.all.Length]

/// <summary>
/// Creates a new game using the given playfield and tetromino generator.
/// </summary>
/// <param name="playfield">The playfield to use.</param>
/// <param name="tetrominoGenerator">The tetromino generator to use.</param>
/// <returns>A new game.</returns>
let create playfield tetrominoGenerator =
    { Playfield = playfield
      NextTetromino = tetrominoGenerator ()
      TetrominoGenerator = tetrominoGenerator }
