[<RequireQualifiedAccess>]
module Tetris.Core.Game


type Grid = Grid.Grid
type Tetromino = Tetromino.Tetromino

/// <summary>
/// Represents the state of the game.
/// </summary>
type Game = { Playfield: Grid; Next: Tetromino }
