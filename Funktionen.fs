namespace WeihnachtsgeschenkePlaner


open FSharp.Json
open System.IO

open WeihnachtsgeschenkePlaner.Types

module FileManagement =
    let private filePath = "gift.json"

    let saveAGiftList filePathP (giftList: PlannedGift list) =
        let json =
            giftList
            |> Json.serialize
        File.WriteAllText (filePathP, json)

    let saveGiftList = saveAGiftList filePath

module GiftManagement =
    let (|Float|_|) (str: string) =
        match System.Double.TryParse(str) with
        | (true,number) -> Some(number)
        | _ -> None

    let newGift (maybePlannedGift:MaybePlannedGift) =
        let checkTuple = (
            maybePlannedGift.person.name,
            maybePlannedGift.gift.description,
            maybePlannedGift.gift.totalCosts
        )
        match checkTuple with
        | Some name, Some description, Some totalCosts ->
            let person = createPerson name maybePlannedGift.person.plannedExpenses
            let gift = createGift description totalCosts
            let splitList =
                maybePlannedGift.split
                |> List.fold (fun splitList maybeSplit ->
                    let checkTuple = (
                        maybeSplit.involvedPerson,
                        maybeSplit.splitAmount
                    )
                    match checkTuple with
                    | Some involvedPerson, Some splitAmount ->
                        splitList
                        |> List.append [createSplit involvedPerson splitAmount]
                    | _ ->
                        splitList
                ) []
            let purchaseStatus =
                match maybePlannedGift.purchaseStatus.whoBuys with
                | Some whoBuys ->
                    createPurchaseStatus whoBuys maybePlannedGift.purchaseStatus.alreadyBought
                    |> Some
                | _ ->
                    None

            createPlannedGift person gift splitList purchaseStatus
            |> Some
        | _ ->
            None

    let createEmptyMaybePlannedGift () = {
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
            alreadyBought = false
        }
    }