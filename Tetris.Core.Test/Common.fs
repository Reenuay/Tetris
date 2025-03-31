namespace Tetris.Core.Test

open System.Text
open FsUnitTyped


module Result =
    /// <summary>
    /// Asserts that the given result is an error with the given value.
    /// </summary>
    /// <param name="x">The expected error value.</param>
    /// <param name="result">The result to assert.</param>
    let shouldBeError x result =
        match result with
        | Ok _ -> failwith "Expected Error"
        | Error error -> shouldEqual error x

    /// <summary>
    /// Asserts that the given result is an ok with the given value.
    /// </summary>
    /// <param name="assertion">The assertion to apply to the ok value.</param>
    /// <param name="result">The result to assert.</param>
    let assertOk assertion result =
        match result with
        | Ok value -> assertion value
        | Error _ -> failwith "Expected Ok"

module Array2D =
    /// <summary>
    /// Asserts that two 2D arrays are equal in both dimensions and content.
    /// </summary>
    /// <param name="expected">The expected 2D array.</param>
    /// <param name="actual">The actual 2D array to compare against the expected array.</param>
    /// <exception cref="System.Exception">Thrown when arrays differ in dimensions or content.</exception>
    let shouldEqual (expected: 'a[,]) (actual: 'a[,]) =
        let expectedRows = Array2D.length1 expected
        let expectedCols = Array2D.length2 expected
        let actualRows = Array2D.length1 actual
        let actualCols = Array2D.length2 actual

        if expectedRows <> actualRows || expectedCols <> actualCols then
            failwithf
                "Array2D dimensions mismatch: expected %dx%d, but got %dx%d"
                expectedRows
                expectedCols
                actualRows
                actualCols

        for i in 0 .. expectedRows - 1 do
            for j in 0 .. expectedCols - 1 do
                if expected[i, j] <> actual[i, j] then
                    let sb = StringBuilder()

                    sprintf
                        "Array2D elements differ at [%d, %d]: expected %A, but got %A"
                        i
                        j
                        expected[i, j]
                        actual[i, j]
                    |> sb.AppendLine
                    |> ignore

                    "Expected:" |> sb.AppendLine |> ignore

                    for row in 0 .. expectedRows - 1 do
                        "  [ " |> sb.Append |> ignore

                        for col in 0 .. expectedCols - 1 do
                            sprintf "%A " expected[row, col] |> sb.Append |> ignore

                        "]" |> sb.AppendLine |> ignore

                    "Actual:" |> sb.AppendLine |> ignore

                    for row in 0 .. actualRows - 1 do
                        "  [ " |> sb.Append |> ignore

                        for col in 0 .. actualCols - 1 do
                            sprintf "%A " actual[row, col] |> sb.Append |> ignore

                        "]" |> sb.AppendLine |> ignore

                    failwith (sb.ToString())
