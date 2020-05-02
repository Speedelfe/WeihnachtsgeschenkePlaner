namespace WeihnachtsgeschenkePlaner

open WeihnachtsgeschenkePlaner.Geschenk
open FSharp.Json
open System.IO

module DateiVerwaltung =
    let private filePath = "gift.json"

    let saveGiftList (giftList: GeschenkEmpfänger list) =
        let json =
            giftList
            |> Json.serialize
        File.WriteAllText (filePath, json)

module GeschenkVerwaltung =
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