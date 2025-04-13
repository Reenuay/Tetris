[<AutoOpen>]
module Tetris.Core.Test.Common

open FsCheck.FSharp


/// <summary>
/// Asserts that two values are equal printing both of them in the error message.
/// </summary>
/// <param name="left">The first value to compare.</param>
/// <param name="right">The second value to compare.</param>
/// <returns>Property that indicates whether the assertion passed or failed.</returns>
let inline (===>) left right =
    left = right |> Prop.label (sprintf "%A = %A" left right)
