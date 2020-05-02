namespace WeihnachtsgeschenkePlaner


open FSharp.Json
open System.IO

open WeihnachtsgeschenkePlaner.Geschenk

module FileManagement =
    let private filePath = "gift.json"

    let saveGiftList (giftList: PlanningGift list) =
        let json =
            giftList
            |> Json.serialize
        File.WriteAllText (filePath, json)

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