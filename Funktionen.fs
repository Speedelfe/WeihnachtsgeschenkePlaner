namespace WeihnachtsgeschenkePlaner


open FSharp.Json
open System.IO

open WeihnachtsgeschenkePlaner.Types

module FileManagement =
    let private filePath = "gift.json"

    type SaveStructure = {
        giftList: PlannedGift list
        personList: Person list
    }

    let save (giftList: PlannedGift list) (personList: Person list) =
        let json =
            { giftList = giftList; personList = personList }
            |> Json.serialize
        File.WriteAllText (filePath, json)

    let load () =
        match File.Exists filePath with
        | false -> ([], [])
        | true ->
            File.ReadAllText filePath
            |> Json.deserialize<SaveStructure>
            |> fun s -> s.giftList, s.personList


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
            // TODO: Handle new person
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