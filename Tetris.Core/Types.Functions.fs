namespace Tetris.Core.Types

open FSharpPlus
open FSharpPlus.Data


/// <summary>
/// Contains functions for creating and manipulating game boards.
/// </summary>
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
    let private createCells<'a> (width: int) (height: int) =
        { Cells = konst BoardCell<'a>.Empty |> List.init width |> konst |> List.init height }

    let private validateWidth width =
        if width < minWidth then
            Failure [ WidthTooSmall(minWidth, width) ]
        else
            Success width

    let private validateHeight height =
        if height < minHeight then
            Failure [ HeightTooSmall(minHeight, height) ]
        else
            Success height

    /// <summary>
    /// Tries to create a game board with the specified width and height.
    /// Returns a list of errors if the creation fails.
    /// </summary>
    /// <param name="width">The width of the game board.</param>
    /// <param name="height">The height of the game board.</param>
    /// <returns>A game board with the specified width and height and all cells initialized to empty state.</returns>
    let tryCreate<'a> width height =
        createCells<'a> <!> validateWidth width <*> validateHeight height

    /// <summary>
    /// Represents a standard 10x20 game board.
    /// </summary>
    let standard<'a> =
        tryCreate<'a> 10 20 |> Validation.defaultWith (fun _ -> failwith "Unreachable")
