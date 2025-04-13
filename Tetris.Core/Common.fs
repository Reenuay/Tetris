[<AutoOpen>]
module Tetris.Core.Common


/// <summary>
/// Convenience prefix operator to create sets.
/// </summary>
/// <param name="elements">The elements to add to the set.</param>
/// <returns>The set.</returns>
let inline (!) elements = set elements

/// <summary>
/// A validation operator that converts a boolean condition into a Result type.
/// </summary>
/// <remarks>
/// This operator provides a concise way to perform validation checks and convert them into
/// the Result type pattern. It's particularly useful in validation pipelines where multiple
/// conditions need to be checked sequentially.
///
/// The operator name |--> visually represents the flow from a condition to a potential error.
/// </remarks>
/// <param name="condition">The boolean condition to check.</param>
/// <param name="error">The error value to return if the condition is false.</param>
/// <returns>A result that contains unit if the condition is true, or an error if it's false.</returns>
let inline (|-->) condition error = if condition then Ok() else Error error

module Result =
    /// <summary>
    /// Ignores the Ok value of a result.
    /// </summary>
    /// <param name="result">The result to ignore.</param>
    /// <returns>A result with unit as the Ok value.</returns>
    let inline ignore result = Result.map ignore result

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

module Fail =
    /// <summary>
    /// Throws an ArgumentNullException if the given argument is null.
    /// </summary>
    /// <param name="argName">The name of the argument.</param>
    /// <param name="arg">The argument to check for null.</param>
    /// <exception cref="ArgumentNullException">Thrown if the argument is null.</exception>
    let ifNullArg argName arg =
        if isNull arg then
            nullArg argName
