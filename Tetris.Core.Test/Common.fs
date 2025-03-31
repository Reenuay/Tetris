namespace Tetris.Core.Test

open FsUnitTyped


module Result =
    /// <summary>
    /// Asserts that the given result is an error with the given value.
    /// </summary>
    /// <param name="x">The expected error value.</param>
    /// <param name="result">The result to assert.</param>
    let shouldBeError (x: 'a) (result: Result<'b, 'a>) =
        match result with
        | Ok _ -> failwith "Expected Error"
        | Error error -> shouldEqual error x

    /// <summary>
    /// Asserts that the given result is an ok.
    /// </summary>
    /// <param name="result">The result to assert.</param>
    let shouldBeOk (result: Result<'a, 'b>) =
        match result with
        | Ok _ -> ()
        | Error _ -> failwith "Expected Ok"
