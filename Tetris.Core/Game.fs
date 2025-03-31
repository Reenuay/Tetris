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

// Step helpers
let private tryUpdatePiece f game =
    game.CurrentPiece
    |> Option.bind (fun piece ->
        let newPiece = f piece

        if Playfield.canPlace newPiece game.Playfield then
            Some
                { game with
                    CurrentPiece = Some newPiece }
        else
            None)
    |> Option.map (fun game -> game, PieceUpdated)
    |> Option.defaultValue (game, Unchanged)

let private spawnPiece game =
    let initialOrientation = Orientation.Up
    let nextBlock = Tetromino.toBlock initialOrientation game.NextTetromino
    let blockWidth = Block.width nextBlock
    let playfieldCenter = Playfield.width game.Playfield / 2

    let spawnPosition =
        { x = playfieldCenter - blockWidth / 2
          y = 0 }

    let newPiece =
        { Tetromino = game.NextTetromino
          Position = spawnPosition
          Orientation = initialOrientation }

    { game with
        CurrentPiece = Some newPiece
        NextTetromino = game.TetrominoGenerator() },
    PieceSpawned
