[<AutoOpen>]
module Tetris.Core.Common

open FSharpPlus.Data


/// <summary>
/// Convenience prefix operator to create sets.
/// </summary>
/// <param name="elements">The elements to add to the set.</param>
/// <returns>The set.</returns>
let inline (!) elements = set elements

/// <summary>
/// Convenience prefix operator to create non-empty sets.
/// </summary>
/// <param name="elements">The elements to add to the set.</param>
/// <returns>The set.</returns>
/// <exception cref="System.ArgumentException">Thrown when the set is empty.</exception>
let inline (!!) elements = NonEmptySet.ofSeq elements

module Error =
    let nothing = Ok()

    let collect result1 result2 =
        match result1, result2 with
        | Ok(), Ok() -> Ok()
        | Ok(), Error e -> Error e
        | Error e, Ok() -> Error e
        | Error e1, Error e2 -> Error(NonEmptySet.union e1 e2)

let inline (|-->) condition failure =
    if condition then Error !![ failure ] else Ok()
