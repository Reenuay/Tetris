[<RequireQualifiedAccess>]
module Tetris.Core.Game


type Playfield = Playfield.Playfield

/// <summary>
/// Represents the state of the game.
/// </summary>
type Game =
    private
        { Playfield: Playfield
          CurrentPiece: Piece option
          NextTetromino: Tetromino
          TetrominoGenerator: unit -> Tetromino }

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
      CurrentPiece = None
      NextTetromino = tetrominoGenerator ()
      TetrominoGenerator = tetrominoGenerator }

/// <summary>
/// Gets the next tetromino in the game.
/// </summary>
/// <param name="game">The game to get the next tetromino from.</param>
/// <returns>The next tetromino in the game.</returns>
let nextTetromino game = game.NextTetromino

/// <summary>
/// Gets the current piece in the game.
/// </summary>
/// <param name="game">The game to get the current piece from.</param>
/// <returns>The current piece in the game.</returns>
let currentPiece game = game.CurrentPiece

/// <summary>
/// Gets the playfield in the game.
/// </summary>
/// <param name="game">The game to get the playfield from.</param>
/// <returns>The playfield in the game.</returns>
let playfield game = game.Playfield
