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

/// <summary>
/// Represents a failure that may occur during validation.
/// </summary>
/// <typeparam name="e">The type of the failure.</typeparam>
type Failure<'e when 'e: comparison> =
    | Nothing
    | Failure of NonEmptySet<'e>

module Failure =
    let nothing = Nothing

    let collect failure1 failure2 =
        match failure1, failure2 with
        | Nothing, Nothing -> Nothing
        | Nothing, Failure e -> Failure e
        | Failure e, Nothing -> Failure e
        | Failure e1, Failure e2 -> Failure(NonEmptySet.union e1 e2)

    let toResult v failure =
        match failure with
        | Nothing -> Ok v
        | Failure e -> Error e

let inline (|-->) condition failure =
    if condition then Failure !![ failure ] else Nothing
