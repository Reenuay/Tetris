/// <summary>
/// Contains the definition of the Game type and related functions.
/// </summary>
[<RequireQualifiedAccess>]
module Tetris.Core.Game


type GameBoard = GameBoard.GameBoard
type Tetromino = Tetromino.Tetromino

/// <summary>
/// Represents the state of the game.
/// </summary>
type Game = { Board: GameBoard; Next: Tetromino }
