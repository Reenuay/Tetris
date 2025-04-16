namespace Tetris.Core.Test.Generator

open Tetris.Core
open FSharpPlus.Data
open FsCheck.FSharp


module Gen =
    let inline chooseUint16 a b =
        Gen.choose (int a, int b) |> Gen.map uint16

module Result =
    let nonEmptyListWithAtLeastOneError =
        gen {
            let ok = Gen.constant (Ok())
            let error = Gen.choose (0, 10) |> Gen.map Error

            let! listLength = Gen.choose (0, 10)
            let! list = Gen.listOfLength listLength (Gen.oneof [ ok; error ])
            let! randomIndex = Gen.choose (0, listLength)
            let! randomError = error

            return List.insertAt randomIndex randomError list
        }

module Position =
    let position =
        gen {
            let! x = Gen.chooseUint16 0us 10us
            let! y = Gen.chooseUint16 0us 10us
            return { X = x; Y = y }
        }

module Rotation =
    let balancedRotationSequence =
        gen {
            let! n = Gen.choose (1, 10)

            let rotations =
                List.replicate n Rotation.Clockwise @ List.replicate n Rotation.CounterClockwise

            return! Gen.shuffle rotations |> Gen.map List.ofArray
        }

module Block =
    let block =
        gen {
            let! tiles = Gen.nonEmptyListOf Position.position |> Gen.map NonEmptySet.ofList
            return tiles |> Block.create
        }

module Playfield =
    let validWidth = Gen.chooseUint16 Playfield.minWidth (Playfield.minWidth + 10us)

    let validHeight = Gen.chooseUint16 Playfield.minHeight (Playfield.minHeight + 10us)

    let invalidWidth = Gen.chooseUint16 0us (Playfield.minWidth - 1us)

    let invalidHeight = Gen.chooseUint16 0us (Playfield.minHeight - 1us)
