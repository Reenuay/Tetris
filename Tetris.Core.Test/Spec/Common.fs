module Tetris.Core.Test.Spec.Common

open Tetris.Core
open Tetris.Core.Test
open FsCheck.Xunit


[<Property>]
let ``Failure.collect returns Nothing if both are Nothing`` () =
    Failure.collect Nothing Nothing ===> Nothing

[<Property>]
let ``Failure.collect returns Failure if left is Failure`` (failure: string) =
    Failure.collect (Failure !![ failure ]) Nothing ===> Failure !![ failure ]

[<Property>]
let ``Failure.collect returns Failure if right is Failure`` (failure: string) =
    Failure.collect Nothing (Failure !![ failure ]) ===> Failure !![ failure ]

[<Property>]
let ``Failure.collect returns Failure if both are Failure`` (failure1: string) (failure2: string) =
    Failure.collect (Failure !![ failure1 ]) (Failure !![ failure2 ])
    ===> Failure !![ failure1; failure2 ]

[<Property>]
let ``Failure.toResult returns Result.Error when Failure`` (failure: string) =
    Failure !![ failure ] |> Failure.toResult () ===> Error !![ failure ]

[<Property>]
let ``Failure.toResult returns Result.Ok with provided value when Nothing`` (value: int) =
    Nothing |> Failure.toResult value ===> Ok value

[<Property>]
let ``|--> operator returns Nothing when condition is false`` (failure: string) = false |--> failure ===> Nothing

[<Property>]
let ``|--> operator returns Failure when condition is true`` (failure: string) =
    true |--> failure ===> Failure !![ failure ]
