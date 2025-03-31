namespace Tetris.Core


type Orientation = Direction

type Piece =
    { Tetromino: Tetromino
      Position: Position
      Orientation: Orientation }
