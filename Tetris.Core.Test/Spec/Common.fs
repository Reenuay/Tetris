[<FsCheck.Xunit.Properties(Arbitrary = [| typeof<Tetris.Core.Test.Arbitrary.Common.Extension> |])>]
module Tetris.Core.Test.Spec.Common

open Tetris.Core
open Tetris.Core.Test
open Tetris.Core.Test.Arbitrary
open System
open FsCheck
open FsCheck.FSharp
open FsCheck.Xunit


[<Property>]
let ``! operator creates same set as built-in set function`` (xs: int list) = !xs ===> set xs

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

[<Property>]
let ``Fail.ifNullArg throws ArgumentNullException when argument is null`` () =
    let argName = "arg"
    Prop.throws<ArgumentNullException, _> (lazy Fail.ifNullArg argName null)

[<Property>]
let ``Fail.ifNullArg does not throw when argument is not null`` (x: obj) =
    let argName = "arg"
    Fail.ifNullArg argName x ===> ()
