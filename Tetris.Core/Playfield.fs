[<RequireQualifiedAccess>]
module Tetris.Core.Playfield

open FSharpPlus.Data


/// <summary>
/// Represents a 2D board with filled tiles and fixed boundaries.
/// </summary>
type private Board =
    { Tiles: Set<Position>
      Width: uint16
      Height: uint16 }

/// <summary>
/// Represents a frozen playfield that awaits a new piece to be spawned.
/// </summary>
type Frozen = private Frozen of Board

/// <summary>
/// Represents a live playfield with a piece on it that can be moved or rotated.
/// </summary>
type Live = private Live of Board * Piece

/// <summary>
/// Represents the possible errors that can occur during the creation of a playfield.
/// </summary>
type PlayfieldCreationError =
    | SmallWidth of minimalWidth: uint16 * actualWidth: uint16
    | SmallHeight of minimalHeight: uint16 * actualHeight: uint16

/// <summary>
/// Represents the possible errors that can occur during the spawning of a piece on a playfield.
/// </summary>
type SpawnError = Collision

/// Minimal width of a playfield.
let minWidth = 20us

/// Minimal height of a playfield.
let minHeight = 20us

/// <summary>
/// Tries to create an empty playfield with the given width and height.
/// </summary>
/// <param name="width">The width of the playfield.</param>
/// <param name="height">The height of the playfield.</param>
/// <returns>A result containing the playfield if dimensions are valid, or error if they are too small.</returns>
let tryCreate width height =
    [ width < minWidth |--> SmallWidth(minWidth, width)
      height < minHeight |--> SmallHeight(minHeight, height) ]
    |> List.reduce Error.collect
    |>? Frozen
            { Tiles = Set.empty
              Width = width
              Height = height }

let private hasCollision board piece =
    piece
    |> Piece.tiles
    |> NonEmptySet.toSet
    |> Set.intersect board.Tiles
    |> Set.isEmpty
    |> not

/// <summary>
/// Tries to spawn a tetromino piece on the playfield.
/// </summary>
/// <param name="tetromino">The tetromino to spawn.</param>
/// <param name="frozenPlayfield">The frozen playfield to spawn the piece on.</param>
/// <returns>A result containing the live playfield if the spawn is successful, or an error if it collides with existing tiles.</returns>
let trySpawn tetromino frozenPlayfield =
    match frozenPlayfield with
    | Frozen board ->
        let blockExtent = tetromino |> Tetromino.toBlock Orientation.Up |> Block.extent

        let spawnPosition =
            { X = (board.Width - blockExtent) / 2us
              Y = (board.Height - blockExtent) / 2us }

        let piece =
            { Tetromino = tetromino
              Orientation = Orientation.Up
              Position = spawnPosition }

        hasCollision board piece |--> Collision |>? Live(board, piece)
