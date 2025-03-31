[<RequireQualifiedAccess>]
module Tetris.Core.Game


type Grid = Grid.Grid

/// <summary>
/// Represents the state of the game.
/// </summary>
type Game =
    private
        { Playfield: Grid
          Current: Piece option
          Next: Tetromino }
