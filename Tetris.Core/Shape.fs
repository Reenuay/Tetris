// This file is added to .fantomasignore to prevent it from being formatted by Fantomas so shape definitions stay pretty.
namespace Tetris.Core


/// <summary>
/// Contains all the possible shapes of tetromino pieces.
/// </summary>
[<RequireQualifiedAccess>]
module private Shape =
    /// <summary>
    /// Convenient alias for the empty cell.
    /// </summary>
    let private o = Cell.Empty

    /// <summary>
    /// Convenient alias for the occupied cell.
    /// </summary>
    let private x = Cell.Occupied

    /// <summary>
    /// Represents I tetromino piece.
    /// </summary>
    let I =
        array2D [
            [ o; x; o; o ]
            [ o; x; o; o ]
            [ o; x; o; o ]
            [ o; x; o; o ]
        ]

    /// <summary>
    /// Represents J tetromino piece.
    /// </summary>
    let J =
        array2D [
            [o; x; o]
            [o; x; o]
            [x; x; o]
        ]

    /// <summary>
    /// Represents L tetromino piece.
    /// </summary>
    let L =
        array2D [
            [o; x; o]
            [o; x; o]
            [o; x; x]
        ]

    /// <summary>
    /// Represents O tetromino piece.
    /// </summary>
    let O =
        array2D [
            [x; x]
            [x; x]
        ]

    /// <summary>
    /// Represents S tetromino piece.
    /// </summary>
    let S =
        array2D [
            [o; x; x]
            [x; x; o]
            [o; o; o]
        ]

    /// <summary>
    /// Represents T tetromino piece.
    /// </summary>
    let T =
        array2D [
            [o; x; o]
            [x; x; x]
            [o; o; o]
        ]

    /// <summary>
    /// Represents Z tetromino
    /// </summary>
    let Z =
        array2D [
            [x; x; o]
            [o; x; x]
            [o; o; o]
        ]
