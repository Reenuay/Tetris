[<AutoOpen>]
module Tetris.Core.Common


/// <summary>
/// Constantly returns the first argument.
/// </summary>
/// <param name="x">The first argument.</param>
/// <param name="_">The second argument.</param>
/// <returns>The first argument.</returns>
let inline always x _ = x

/// <summary>
/// Convenience prefix operator to create sets.
/// </summary>
/// <param name="elements">The elements to add to the set.</param>
/// <returns>The set.</returns>
let inline (!) elements = set elements

/// <summary>
/// Convenience infix operator to create validations from predicates.
/// </summary>
/// <param name="predicate">The predicate to validate.</param>
/// <param name="error">The error to return if the predicate fails.</param>
/// <param name="value">The value to validate.</param>
/// <returns>A validation.</returns>
/// <remarks>
/// Example:
/// <code>
/// let validateLength =
///     (fun s -> String.length s >= 5) --> "Length must be at least 5"
/// </code>
/// </remarks>
let inline (-->) predicate error value =
    if predicate value then Ok value else Error ![ error ]

module Result =
    /// <summary>
    /// Ignores the Ok value of a result.
    /// </summary>
    /// <param name="result">The result to ignore.</param>
    /// <returns>A result with unit as the Ok value.</returns>
    let inline ignore result = Result.map ignore result

    /// <summary>
    /// Validates a value against a list of validations.
    /// </summary>
    /// <param name="validations">The validations to apply.</param>
    /// <param name="value">The value to validate.</param>
    /// <returns>A result that is Ok if all validations pass, or Error with a set of all errors.</returns>
    let inline validateAll validations value =
        validations
        |> Seq.map (fun validate -> validate value)
        |> Seq.fold
            (fun state current ->
                match state, current with
                | Ok _, Ok _ -> current
                | Ok _, Error e -> Error e
                | Error e, Ok _ -> Error e
                | Error e1, Error e2 -> Error(Set.union e1 e2))
            (Ok value)
