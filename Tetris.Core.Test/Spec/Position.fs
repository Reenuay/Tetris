module Tetris.Core.Test.Spec.Position

open Tetris.Core
open Tetris.Core.Test
open FsCheck.FSharp
open FsCheck.Xunit


let zero = { X = 0; Y = 0 }

[<Property>]
let ``add returns position with summed coordinates`` (p1: Position) (p2: Position) =
    let result = Position.add p2 p1
    result ===> { X = p1.X + p2.X; Y = p1.Y + p2.Y }

[<Property>]
let ``add is commutative: p1 ⊕ p2 = p2 ⊕ p1`` (p1: Position) (p2: Position) =
    Position.add p1 p2 ===> Position.add p2 p1

[<Property>]
let ``add is associative: p1 ⊕ (p2 ⊕ p3) = (p1 ⊕ p2) ⊕ p3`` (p1: Position) (p2: Position) (p3: Position) =
    Position.add p1 (Position.add p2 p3) ===> Position.add (Position.add p1 p2) p3

[<Property>]
let ``add has identity element — { X: 0; Y: 0 }`` (p: Position) =
    Position.add zero p ===> p .&. (Position.add p zero ===> p)

[<Property>]
let ``add is invertable (each Position has it's negative counterpart)`` (p: Position) =
    let negated = { X = -p.X; Y = -p.Y }

    Position.add p negated ===> zero .&. (Position.add negated p ===> zero)
