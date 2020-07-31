namespace WeihnachtsgeschenkePlaner

module Types =
    type PersonName = PersonName of string

    let getPersonNameValue (PersonName personName) = personName

    type MaybePerson = {
        name: PersonName option
        plannedExpenses: float option
    }

    module MaybePersonLenses =
        let name = Lens((fun p -> p.name), (fun p n -> { p with name = n }))
        let plannedExpenses = Lens((fun p -> p.plannedExpenses), (fun p pe -> { p with plannedExpenses = pe }))

    type MaybeGift = {
        description: string option
        totalCosts: float option
    }

    type MaybeSplit = {
        involvedPerson: PersonName option
        splitAmount: float option
    }

    type MaybePurchaseStatus = {
        whoBuys: PersonName option
        alreadyBought: bool
    }

    type MaybePlannedGift = {
        receiver: PersonName option
        gift: MaybeGift
        isGiftSplitted: bool
        split: MaybeSplit list
        purchaseStatus: MaybePurchaseStatus
    }

    module MaybePlannedGiftLenses =
        let receiver = Lens((fun p -> p.receiver), (fun p n -> { p with receiver = n }))

    type Person = {
        name: PersonName
        plannedExpenses: float option
    }

    let createPerson name expense =
        {
            name = name
            plannedExpenses = expense
        }

    type Gift = {
        description: string
        totalCost: float
    }

    let createGift description cost =
        {
            description = description
            totalCost = cost
        }

    type Split = {
        involvedPerson: PersonName
        splitAmount: float
    }

    let createSplit personName amount =
        {
            involvedPerson = personName
            splitAmount = amount
        }

    type PurchaseStatus = {
        whoBuys: PersonName
        alreadyBought: bool
    }

    let createPurchaseStatus whoBuys alreadyBought =
        {
            whoBuys = whoBuys
            alreadyBought = alreadyBought
        }


    type PlannedGift = {
        receiver: PersonName
        gift: Gift
        isGiftSplitted: bool
        splitList: Split List
        purchaseStatus: PurchaseStatus option
    }

    let createPlannedGift receiver gift isSplit splitList purchaseStatus =
        {
            receiver = receiver
            gift = gift
            isGiftSplitted = isSplit
            splitList = splitList
            purchaseStatus = purchaseStatus
        }

 module Settings =
    type Settings = {
        year: int option
        filePath: string option
    }
    type Year = int option
    type Filepath = string option

