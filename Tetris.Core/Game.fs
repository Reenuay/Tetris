[<RequireQualifiedAccess>]
module Tetris.Core.Game


type Playfield = Playfield.Playfield

/// <summary>
/// Represents the state of the game.
/// </summary>
type Game =
    private
        { Playfield: Playfield
          Current: Piece option
          Next: Tetromino
          TetrominoGenerator: unit -> Tetromino }

/// <summary>
/// Creates a standard tetromino generator using the given seed.
/// </summary>
let standardTetrominoGenerator () =
    let rnd = System.Random()
    fun () -> Tetromino.all.[rnd.Next(Tetromino.all.Length)]

/// <summary>
/// Creates a new game using the given playfield and tetromino generator.
/// </summary>
/// <param name="playfield">The playfield to use.</param>
/// <param name="tetrominoGenerator">The tetromino generator to use.</param>
/// <returns>A new game.</returns>
let create playfield tetrominoGenerator =
    { Playfield = playfield
      Current = None
      Next = tetrominoGenerator ()
      TetrominoGenerator = tetrominoGenerator }
