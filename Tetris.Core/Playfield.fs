[<RequireQualifiedAccess>]
module Tetris.Core.Playfield

open FsToolkit.ErrorHandling


/// <summary>
/// Represents a 2D playfield.
/// </summary>
type Playfield = private { Tiles: Tile[,] }

/// <summary>
/// Represents the possible errors that can occur during the creation of a playfield.
/// </summary>
type PlayfieldCreationFailure =
    | NullTiles
    | WidthTooSmall of minimalWidth: int * actualWidth: int
    | HeightTooSmall of minimalHeight: int * actualHeight: int

/// Minimal width of a playfield.
let minWidth = 10

/// Minimal height of a playfield.
let minHeight = 20

/// <summary>
/// Gets the width of the playfield.
/// </summary>
/// <param name="playfield">The playfield to get the width from.</param>
/// <returns>The width of the playfield.</returns>
let width playfield = playfield.Tiles |> Array2D.length2

/// <summary>
/// Gets the height of the playfield.
/// </summary>
/// <param name="playfield">The playfield to get the height from.</param>
/// <returns>The height of the playfield.</returns>
let height playfield = playfield.Tiles |> Array2D.length1

let private create tiles = { Tiles = tiles |> Array2D.copy }

let private validateNull tiles =
    if isNull tiles then Error [ NullTiles ] else Ok()

let private validateWidth tiles =
    if tiles |> Array2D.length2 < minWidth then
        Error [ WidthTooSmall(minWidth, tiles |> Array2D.length2) ]
    else
        Ok()

let private validateHeight tiles =
    if tiles |> Array2D.length1 < minHeight then
        Error [ HeightTooSmall(minHeight, tiles |> Array2D.length1) ]
    else
        Ok()

/// <summary>
/// Creates a new playfield from the given tiles.
/// </summary>
/// <param name="tiles">The tiles to create the playfield from.</param>
/// <returns>A result containing the playfield or the errors that occurred during the creation.</returns>
let tryCreate tiles =
    validation {
        let! _ = validateNull tiles // short curcuits here
        let! _ = validateWidth tiles
        and! _ = validateHeight tiles
        return create tiles
    }

/// <summary>
/// Gets the tile at the given position on the playfield.
/// </summary>
/// <param name="x">The x position of the tile.</param>
/// <param name="y">The y position of the tile.</param>
/// <param name="playfield">The playfield to get the tile from.</param>
/// <returns>The tile at the given position on the playfield.</returns>
let getTile x y playfield = playfield.Tiles[x, y]

/// <summary>
/// Checks if the given playfield can place the given piece.
/// </summary>
/// <param name="piece">The piece to check.</param>
/// <param name="playfield">The playfield to check.</param>
/// <returns>True if the piece can be placed on the playfield, false otherwise.</returns>
let canPlace piece playfield =
    let block = piece |> Piece.toBlock
    let position = piece.Position
    let blockWidth = block |> Block.width
    let blockHeight = block |> Block.height
    let playfieldWidth = playfield.Tiles |> Array2D.length1
    let playfieldHeight = playfield.Tiles |> Array2D.length2

    let isWithinBounds =
        position.x >= 0
        && position.x + blockWidth <= playfieldWidth
        && position.y >= 0
        && position.y + blockHeight <= playfieldHeight

    // Check for collision with non-empty tiles on the playfield
    // This has to be a function because the computation has to be deferred
    // Firstly because isWithinBounds has to be checked first so no index out of bounds exception is thrown
    // Secondly because it avoids unnecessary computation if isWithinBounds is false
    let hasNoCollision _ =
        let mutable hasCollision = false
        let mutable i = 0

        while not hasCollision && i < blockWidth do
            let mutable j = 0

            while not hasCollision && j < blockHeight do
                let blockTile = Block.getTile i j block
                let playfieldTile = playfield.Tiles[position.x + i, position.y + j]

                hasCollision <- blockTile <> Tile.Empty && playfieldTile <> Tile.Empty

                j <- j + 1

            i <- i + 1

        not hasCollision

    isWithinBounds && hasNoCollision ()

/// <summary>
/// Fixes the given piece on the given playfield.
/// </summary>
/// <param name="piece">The piece to fix.</param>
/// <param name="playfield">The playfield to fix the piece on.</param>
/// <returns>An option containing the fixed playfield or None if the piece could not be placed.</returns>
let tryFix piece playfield =
    if canPlace piece playfield then
        let newPlayfield = { Tiles = playfield.Tiles |> Array2D.copy }
        let block = piece |> Piece.toBlock
        let position = piece.Position
        let blockWidth = block |> Block.width
        let blockHeight = block |> Block.height
        let mutable i = 0

        while i < blockWidth do
            let mutable j = 0

            while j < blockHeight do
                newPlayfield.Tiles[position.x + i, position.y + j] <- Block.getTile i j block

                j <- j + 1

            i <- i + 1

        Some newPlayfield
    else
        None
