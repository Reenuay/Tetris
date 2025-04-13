namespace Tetris.Core.Test.Generator

open Tetris.Core
open FsCheck.FSharp


module Result =
    let nonEmptyListWithAtLeastOneError =
        gen {
            let ok = Gen.constant (Ok())
            let error = Gen.choose (0, 10) |> Gen.map Error

            let! listLength = Gen.choose (0, 10)
            let! list = Gen.listOfLength listLength (Gen.oneof [ ok; error ])
            let! randomIndex = Gen.choose (0, listLength)
            let! randomError = error

            return List.insertAt randomIndex randomError list
        }

module Playfield =
    let validWidth = Gen.choose (Playfield.minWidth, Playfield.minWidth + 10)

    let validHeight = Gen.choose (Playfield.minHeight, Playfield.minHeight + 10)

    let invalidWidth = Gen.choose (0, Playfield.minWidth - 1)

    let invalidHeight = Gen.choose (0, Playfield.minHeight - 1)
