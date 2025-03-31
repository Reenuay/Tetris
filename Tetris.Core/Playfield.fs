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

let private failIfNull tiles =
    if isNull tiles then
        nullArg "Tiles cannot be null"

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
/// /// <exception cref="System.ArgumentNullException">Thrown when tiles is null.</exception>
let tryCreate tiles =
    validation {
        do failIfNull tiles
        let! _ = validateWidth tiles
        and! _ = validateHeight tiles
        return { Tiles = tiles |> Array2D.copy }
    }

/// <summary>
/// Gets the tile at the given position on the playfield.
/// </summary>
/// <param name="x">The x position of the tile.</param>
/// <param name="y">The y position of the tile.</param>
/// <param name="playfield">The playfield to get the tile from.</param>
/// <returns>The tile at the given position on the playfield.</returns>
let getTile x y playfield = playfield.Tiles[y, x]

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
/// Tries to fix the given piece on the playfield.
/// </summary>
/// <param name="piece">The piece to fix.</param>
/// <param name="playfield">The playfield to fix the piece on.</param>
/// <returns>A result containing the new playfield or the original playfield if the piece could not be fixed.</returns>
let fixPiece piece playfield =
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
                newPlayfield.Tiles[position.y + j, position.x + i] <- Block.getTile i j block

                j <- j + 1

            i <- i + 1

        Ok newPlayfield
    else
        Error playfield

let clearLines playfield =
    let width = width playfield
    let height = height playfield
    let newTiles = Array2D.create height width Tile.Empty
    let mutable clearedLines = []
    let mutable i = height - 1 // Source index (old array)
    let mutable j = height - 1 // Destination index (new array)

    while i >= 0 do
        let mutable shouldClear = true
        let mutable x = 0

        // Check if line needs to be cleared
        while shouldClear && x < width do
            shouldClear <- playfield.Tiles[i, x] = Tile.Occupied
            x <- x + 1

        if shouldClear then
            // Line should be cleared - add to cleared lines and skip copying
            clearedLines <- i :: clearedLines
            i <- i - 1
        else
            // Copy line from old array to new array
            for x in 0 .. width - 1 do
                newTiles[j, x] <- playfield.Tiles[i, x]

            i <- i - 1
            j <- j - 1

    { Tiles = newTiles }, clearedLines
