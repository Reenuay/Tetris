namespace Tetris.Core.Test.Arbitrary

open Tetris.Core.Test
open FsCheck.FSharp


[<RequireQualifiedAccess>]
module Common =
    type NonEmptyListWithAtLeastOneError = NonEmptyListWithAtLeastOneError of Result<unit, int> list

    type Extension =
        static member NonEmptyListWithAtLeastOneError() =
            Arb.fromGen (
                Generator.Result.nonEmptyListWithAtLeastOneError
                |> Gen.map NonEmptyListWithAtLeastOneError
            )


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
