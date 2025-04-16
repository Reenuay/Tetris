[<FsCheck.Xunit.Properties(Arbitrary = [| typeof<Tetris.Core.Test.Arbitrary.Common.Extension> |])>]
module Tetris.Core.Test.Spec.Common

open Tetris.Core
open Tetris.Core.Test
open Tetris.Core.Test.Arbitrary
open FsCheck
open FsCheck.Xunit


[<Property>]
let ``|--> operator returns Ok when condition is false`` (error: string) = false |--> error ===> Ok()

[<Property>]
let ``|--> operator returns Error when condition is true`` (error: string) = true |--> error ===> Error error

[<Property>]
let ``Result.ignore converts any Ok value to Ok()`` (x: int) = Ok x |> Result.ignore ===> Ok()

[<Property>]
let ``Result.mergeErrors returns Ok for empty list`` () =
    List.empty |> Result.mergeErrors ===> Ok()

[<Property>]
let ``Result.mergeErrors returns Ok when all results are Ok`` (PositiveInt count) =
    Ok() |> List.replicate count |> Result.mergeErrors ===> Ok()

[<Property>]
let ``Result.mergeErrors preserves all errors`` (Common.NonEmptyListWithAtLeastOneError results) =
    let expectedErrors =
        results
        |> List.choose (function
            | Ok() -> None
            | Error e -> Some e)
        |> set

    results |> Result.mergeErrors ===> Error expectedErrors
