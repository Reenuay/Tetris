module Tetris.Core.Test.Spec.Common

open Tetris.Core
open Tetris.Core.Test
open FsCheck.Xunit


[<Property>]
let ``Error.nothing returns Ok if both are Ok `` () =
    Error.collect Error.nothing Error.nothing ===> Error.nothing

[<Property>]
let ``Error.collect returns Error if left is Error`` (error: string) =
    Error.collect (Error !![ error ]) Error.nothing ===> Error !![ error ]

[<Property>]
let ``Error.collect returns Error if right is Error`` (error: string) =
    Error.collect Error.nothing (Error !![ error ]) ===> Error !![ error ]

[<Property>]
let ``Error.collect returns Error if both are Error`` (error1: string) (error2: string) =
    Error.collect (Error !![ error1 ]) (Error !![ error2 ])
    ===> Error !![ error1; error2 ]

[<Property>]
let ``|--> operator returns Ok when condition is false`` (error: string) = false |--> error ===> Ok()

[<Property>]
let ``|--> operator returns Error when condition is true`` (error: string) = true |--> error ===> Error !![ error ]
