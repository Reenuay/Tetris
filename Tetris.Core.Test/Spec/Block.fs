module Tetris.Core.Test.Spec.Block

open Tetris.Core
open Tetris.Core.Test
open FsCheck
open FsCheck.FSharp
open FsCheck.Xunit


[<Property>]
let ``tryCreate returns Error when pattern width is 0`` (PositiveInt height) =
    (fun _ _ -> true) |> Array2D.init height 0 |> Block.tryCreate
    ===> Error ![ Block.ZeroWidth ]
