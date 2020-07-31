namespace WeihnachtsgeschenkePlaner


open FSharp.Json
open System.IO

open WeihnachtsgeschenkePlaner.Types
open WeihnachtsgeschenkePlaner.Settings

module FileManagement =
    let private giftFilePath = "gift.json"
    let private settingsFilePath = "settings.json"

    type GiftSaveStructure = {
        giftList: PlannedGift list
        personList: Person list
    }

    type SettingSaveStructure = {
        settingList: Settings list
    }

    let saveGift (giftList: PlannedGift list) (personList: Person list) =
        let json =
            { giftList = giftList; personList = personList }
            |> Json.serialize
        File.WriteAllText (giftFilePath, json)

    let saveSettings (settingList: Settings list) =
        let json =
            {settingList = settingList }
            |> Json.serialize
        File.WriteAllText (settingsFilePath, json)

    let loadGift () =
        match File.Exists giftFilePath with
        | false -> ([], [])
        | true ->
            File.ReadAllText giftFilePath
            |> Json.deserialize<GiftSaveStructure>
            |> fun s -> s.giftList, s.personList

    let loadSettings () =
        match File.Exists settingsFilePath with
        | false -> ([])
        | true ->
            File.ReadAllText settingsFilePath
            |> Json.deserialize<SettingSaveStructure>
            |> fun s -> s.settingList

module GiftManagement =
    let (|Float|_|) (str: string) =
        match System.Double.TryParse(str) with
        | (true,number) -> Some(number)
        | _ -> None

    let newGift (maybePlannedGift:MaybePlannedGift) =
        let checkTuple = (
            maybePlannedGift.receiver,
            maybePlannedGift.gift.description,
            maybePlannedGift.gift.totalCosts
        )
        match checkTuple with
        | Some name, Some description, Some totalCosts ->
            let gift = createGift description totalCosts
            let isSplit = maybePlannedGift.isGiftSplitted
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

            createPlannedGift name gift isSplit splitList purchaseStatus
            |> Some
        | _ ->
            None

    let newPerson (maybePerson: MaybePerson) =
        match maybePerson.name with
        | Some name ->
            createPerson name maybePerson.plannedExpenses
        | None ->
            failwith "Nope"

    let createEmptyMaybePlannedGift () = {
        receiver = None
        gift = {
            description = None
            totalCosts = None
        }
        isGiftSplitted = false
        split = []
        purchaseStatus = {
            whoBuys = None
            alreadyBought = false
        }
    }

    let getMostSplittedGift (plannedGift: PlannedGift List) =
        plannedGift
        |> List.map (fun gift -> gift.splitList.Length)
        |> List.max
