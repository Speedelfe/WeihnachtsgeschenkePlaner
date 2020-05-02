namespace WeihnachtsgeschenkePlaner


open FSharp.Json
open System.IO

open WeihnachtsgeschenkePlaner.Geschenk

module FileManagement =
    let private filePath = "gift.json"

    let saveAGiftList filePathP (giftList: PlanningGift list) =
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
            geplanteAusgabe = None
        }
        geschenk = {
            name = None
            kosten = None
        }
        betrag = []
    }