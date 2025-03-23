namespace Tetris.Core.Types


/// <summary>
/// Represents the possible errors that can occur during the creation of a game board.
/// </summary>
type GameBoardCreationError =
    | WidthTooSmall of minimalWidth: int * actualWidth: int
    | HeightTooSmall of minimalHeight: int * actualHeight: int
