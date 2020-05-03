namespace WeihnachtsgeschenkePlaner


open FSharp.Json
open System.IO

open WeihnachtsgeschenkePlaner.Geschenk

module FileManagement =
    let private filePath = "gift.json"

    let saveAGiftList filePathP (giftList: PlannedGift list) =
        let json =
            giftList
            |> Json.serialize
        File.WriteAllText (filePathP, json)

    let saveGiftList = saveAGiftList filePath

module GiftManagement =
    let newGift () =
        ()

    let createEmptyMaybeGeschenkEmpfänger () = {
        person = {
            name = None
            plannedExpenses = None
        }
        gift = {
            description = None
            totalCosts = None
        }
        split = []
        purchaseStatus = {
            whoBuys = None
            alreadyBought = None
        }
    }