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
    /// <summary>
    /// Combines two errors.
    /// </summary>
    /// <param name="result1">The first result.</param>
    /// <param name="result2">The second result.</param>
    /// <returns>The combined result.</returns>
    let collect result1 result2 =
        match result1, result2 with
        | Ok(), Ok() -> Ok()
        | Ok(), Error e -> Error e
        | Error e, Ok() -> Error e
        | Error e1, Error e2 -> Error(NonEmptySet.union e1 e2)

/// <summary>
/// A convenience operator to create an error if a condition is true.
/// </summary>
/// <param name="condition">The condition to check.</param>
/// <param name="error">The error to create.</param>
/// <returns>Ok () if the condition is false, otherwise an error.</returns>
let inline (|-->) condition error =
    if condition then Error !![ error ] else Ok()
