namespace Tetris.Core.Test.Arbitrary

open Tetris.Core
open Tetris.Core.Test
open FsCheck.FSharp


[<RequireQualifiedAccess>]
module Common =
    /// Represents a non-empty list with at least one error case.
    type NonEmptyListWithAtLeastOneError = NonEmptyListWithAtLeastOneError of Result<unit, int> list

    type Extension =
        static member NonEmptyListWithAtLeastOneError() =
            Arb.fromGen (
                Generator.Result.nonEmptyListWithAtLeastOneError
                |> Gen.map NonEmptyListWithAtLeastOneError
            )

[<RequireQualifiedAccess>]
module Rotation =
    /// Represents a balanced sequence of rotations that have an equal number of clockwise and counter-clockwise rotations.
    type BalancedSequence = BalancedSequence of Rotation list

    type Extension =
        static member BalancedSequence() =
            Arb.fromGen (Generator.Rotation.balancedRotationSequence |> Gen.map BalancedSequence)

[<RequireQualifiedAccess>]
module Block =
    /// Represents a non-empty pattern for block creation.
    type NonEmptyPattern = NonEmptyPattern of bool[,]

    /// Represents a block for testing purposes.
    type Block = Block of Block.Block

    type Extension =
        static member NonEmptyPattern() =
            Arb.fromGen (Generator.Block.nonEmptyPattern |> Gen.map NonEmptyPattern)

        static member Block() =
            Arb.fromGen (Generator.Block.block |> Gen.map Block)

[<RequireQualifiedAccess>]
module Playfield =
    /// Represents a valid width for a playfield.
    type ValidWidth = ValidWidth of int

    /// Represents a valid height for a playfield.
    type ValidHeight = ValidHeight of int

    /// Represents an invalid width for a playfield.
    type InvalidWidth = InvalidWidth of int

    /// Represents an invalid height for a playfield.
    type InvalidHeight = InvalidHeight of int

    /// Extension methods for the Arbitrary module.
    type Extension =
        static member ValidWidth() =
            Arb.fromGen (Generator.Playfield.validWidth |> Gen.map ValidWidth)

        static member ValidHeight() =
            Arb.fromGen (Generator.Playfield.validHeight |> Gen.map ValidHeight)

        static member InvalidWidth() =
            Arb.fromGen (Generator.Playfield.invalidWidth |> Gen.map InvalidWidth)

        static member InvalidHeight() =
            Arb.fromGen (Generator.Playfield.invalidHeight |> Gen.map InvalidHeight)
