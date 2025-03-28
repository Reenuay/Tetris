[<RequireQualifiedAccess>]
module Tetris.Core.Shape


type Tile = Place.Place

/// <summary>
/// Represents a 2D shape.
/// </summary>
type Shape =
    private
        { Tiles: Tile[,] }

    /// <summary>
    /// Gets the tile at the specified position in the shape.
    /// </summary>
    /// <param name="x">The x-coordinate of the tile.</param>
    /// <param name="y">The y-coordinate of the tile.</param>
    /// <returns>The tile at the specified position in the shape.</returns>
    member this.Item
        with get (x, y) = this.Tiles[x, y]

/// <summary>
/// Creates a new shape from the given tiles.
/// </summary>
/// <param name="tiles">The tiles that make up the shape.</param>
/// <returns>A new shape.</returns>
let create tiles = { Tiles = tiles |> Array2D.copy }

/// <summary>
/// Gets the width of a shape.
/// </summary>
/// <param name="shape">The shape to get the width of.</param>
/// <returns>The width of the shape.</returns>
let width shape = Array2D.length2 shape.Tiles

/// <summary>
/// Gets the height of a shape.
/// </summary>
/// <param name="shape">The shape to get the height of.</param>
/// <returns>The height of the shape.</returns>
let height shape = Array2D.length1 shape.Tiles

/// <summary>
/// Rotates a shape clockwise.
/// </summary>
/// <param name="shape">The shape to rotate.</param>
/// <returns>The rotated shape.</returns>
let rotateClockwise shape =
    let width = Array2D.length2 shape.Tiles
    let height = Array2D.length1 shape.Tiles
    let rotated = Array2D.create width height Place.Empty

    for i in 0 .. height - 1 do
        for j in 0 .. width - 1 do
            rotated[j, width - 1 - i] <- shape[i, j]

    { Tiles = rotated }
