[<AutoOpen>]
module Tetris.Core.Common


/// <summary>
/// Convenience prefix operator to create sets.
/// </summary>
/// <param name="elements">The elements to add to the set.</param>
/// <returns>The set.</returns>
let inline (!) elements = set elements

/// <summary>
/// A validation operator that converts a condition into an error when the condition is met.
/// </summary>
/// <remarks>
/// The operator |--> should be read as "condition leads to error", meaning when the condition
/// is true, it results in the specified error.
/// </remarks>
/// <param name="condition">The condition that triggers the error.</param>
/// <param name="error">The error to return when the condition is met.</param>
/// <returns>
/// Error with the specified error value when the condition is met;
/// Ok otherwise.
/// </returns>
let inline (|-->) condition error = if condition then Error error else Ok()

module Result =
    /// <summary>
    /// Ignores the Ok value of a result.
    /// </summary>
    /// <param name="result">The result to ignore.</param>
    /// <returns>A result with unit as the Ok value.</returns>
    let inline ignore result = Result.map ignore result

    let replaceOk newValue result =
        match result with
        | Ok _ -> Ok newValue
        | Error e -> Error e

    /// <summary>
    /// Combines multiple validation results into a single result, collecting all errors if present.
    /// </summary>
    /// <remarks>
    /// This function is particularly useful when you need to aggregate multiple validation results
    /// into a single outcome. Instead of failing fast on the first error, it continues processing
    /// all results to provide a comprehensive set of validation errors.
    /// </remarks>
    /// <param name="results">A sequence of Result values to merge.</param>
    /// <returns>A single Result combining all successes or collecting all errors.</returns>
    let inline mergeErrors results =
        results
        |> Seq.fold
            (fun acc result ->
                match acc, result with
                | Ok(), Ok() -> Ok()
                | Ok(), Error e -> Error ![ e ]
                | Error e, Ok() -> Error e
                | Error e1, Error e2 -> Error(Set.add e2 e1))
            (Ok())

    let inline replaceError sourceResult targetResult =
        match sourceResult, targetResult with
        | _, Ok v -> Ok v
        | Ok(), Error e -> Error e
        | Error e, Error _ -> Error e


module Fail =
    /// <summary>
    /// Throws an ArgumentNullException if the given argument is null.
    /// </summary>
    /// <param name="argName">The name of the argument.</param>
    /// <param name="arg">The argument to check for null.</param>
    /// <exception cref="ArgumentNullException">Thrown if the argument is null.</exception>
    let inline ifNullArg argName arg =
        if isNull arg then
            nullArg argName
