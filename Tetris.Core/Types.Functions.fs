namespace Tetris.Core.Types

open FSharpPlus
open FSharpPlus.Data
open FSharpx.Collections


/// <summary>
/// Contains functions for creating and manipulating game boards.
/// </summary>
[<RequireQualifiedAccess>]
module GameBoard =
    /// <summary>
    /// Minimal width of a game board.
    /// </summary>
    let minWidth = 8

    /// <summary>
    /// Minimal height of a game board.
    /// </summary>
    let minHeight = 10

    /// <summary>
    /// Initializes a new game board with the specified width and height.
    /// </summary>
    /// <param name="width">The width of the game board.</param>
    /// <param name="height">The height of the game board.</param>
    let private createBoard<'a> (width: int) (height: int) =
        BoardCell<'a>.Empty
        |> konst
        |> PersistentVector.init width
        |> konst
        |> PersistentVector.init height
        |> GameBoard

    /// <summary>
    /// Validates the width of a game board.
    /// </summary>
    /// <param name="width">The width of the game board.</param>
    /// <returns>A list of errors if the width is invalid.</returns>
    let private validateWidth width =
        if width < minWidth then
            Error [ WidthTooSmall(minWidth, width) ]
        else
            Ok width

    /// <summary>
    /// Validates the height of a game board.
    /// </summary>
    /// <param name="height">The height of the game board.</param>
    /// <returns>A list of errors if the height is invalid.</returns>
    let private validateHeight height =
        if height < minHeight then
            Error [ HeightTooSmall(minHeight, height) ]
        else
            Ok height

    /// <summary>
    /// Tries to create a game board with the specified width and height.
    /// Returns a list of errors if the creation fails.
    /// </summary>
    /// <param name="width">The width of the game board.</param>
    /// <param name="height">The height of the game board.</param>
    /// <returns>A game board with the specified width and height and all cells initialized to empty state.</returns>
    let tryCreate<'a> width height =
        createBoard<'a> <!> validateWidth width <*> validateHeight height

    /// <summary>
    /// Represents a standard 10x20 game board.
    /// </summary>
    let standard<'a> =
        tryCreate<'a> 10 20 |> Result.defaultWith (fun _ -> failwith "Unreachable")
