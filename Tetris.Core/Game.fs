/// <summary>
/// Contains the definition of the Game type and related functions.
/// </summary>
[<RequireQualifiedAccess>]
module Tetris.Core.Game


type GameBoard = GameBoard.GameBoard
type TetrominoType = TetrominoType.TetrominoType

/// <summary>
/// Represents the state of a the Game.
/// </summary>
type Game =
    { Board: GameBoard
      NextType: TetrominoType }
