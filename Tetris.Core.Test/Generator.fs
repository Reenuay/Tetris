namespace Tetris.Core.Test.Generator

open Tetris.Core
open FsCheck.FSharp

module Playfield =
    let validWidth = Gen.choose (Playfield.minWidth, Playfield.minWidth + 10)

    let validHeight = Gen.choose (Playfield.minHeight, Playfield.minHeight + 10)

    let invalidWidth = Gen.choose (0, Playfield.minWidth - 1)

    let invalidHeight = Gen.choose (0, Playfield.minHeight - 1)
