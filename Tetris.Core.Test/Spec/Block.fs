module Tetris.Core.Test.Spec.Block

open Tetris.Core
open Tetris.Core.Test
open System
open FsCheck
open FsCheck.FSharp
open FsCheck.Xunit


[<Property>]
let ``tryCreate throws on null pattern`` () =
    Prop.throws<ArgumentNullException, _> (lazy Block.tryCreate null)

[<Property>]
let ``tryCreate returns Error when pattern width is 0`` (PositiveInt height) =
    (fun _ _ -> true) |> Array2D.init height 0 |> Block.tryCreate
    ===> Error ![ Block.ZeroWidth ]

[<Property>]
let ``tryCreate returns Error when pattern height is 0`` (PositiveInt width) =
    (fun _ _ -> true) |> Array2D.init 0 width |> Block.tryCreate
    ===> Error ![ Block.ZeroHeight ]

[<Property>]
let ``tryCreate returns Ok for valid dimensions`` (PositiveInt width) (PositiveInt height) =
    (fun _ _ -> false)
    |> Array2D.init height width
    |> Block.tryCreate
    |> Result.isOk
    ===> true

[<Property>]
let ``tryCreate creates empty block for all-false pattern`` (PositiveInt width) (PositiveInt height) =
    Array2D.init height width (fun _ _ -> false)
    |> Block.tryCreate
    |> Result.map Block.tilePositions
    ===> Ok Set.empty

[<Property>]
let ``tryCreate creates full block for all-true pattern`` (PositiveInt width) (PositiveInt height) =
    let expected =
        ![ for y in 0 .. (height - 1) do
               for x in 0 .. (width - 1) do
                   yield { X = x; Y = y } ]

    Array2D.init height width (fun _ _ -> true)
    |> Block.tryCreate
    |> Result.map Block.tilePositions
    ===> Ok expected
