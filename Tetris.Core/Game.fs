[<RequireQualifiedAccess>]
module Tetris.Core.Game


type Grid = Grid.Grid

/// <summary>
/// Represents the state of the game.
/// </summary>
type Game = { Playfield: Grid; Next: Tetromino }
