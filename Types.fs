namespace WeihnachtsgeschenkePlaner

module Types =
    type Person = {
        name: string
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
        involvedPerson: string
        splitAmount: float
    }

    let createSplit personName amount =
        {
            involvedPerson = personName
            splitAmount = amount
        }

    type PurchaseStatus = {
        whoBuys: string
        alreadyBought: bool
    }

    let createPurchaseStatus whoBuys alreadyBought =
        {
            whoBuys = whoBuys
            alreadyBought = alreadyBought
        }


    type PlannedGift = {
        person: Person
        gift: Gift
        isGiftSplitted: bool
        splitList: Split List
        purchaseStatus: PurchaseStatus option
    }

    let createPlannedGift person gift isSplit splitList purchaseStatus =
        {
            person = person
            gift = gift
            isGiftSplitted = isSplit
            splitList = splitList
            purchaseStatus = purchaseStatus
        }


    type MaybePerson = {
        name: string option
        plannedExpenses: float option
    }

    type MaybeGift = {
        description: string option
        totalCosts: float option
    }

    type MaybeSplit = {
        involvedPerson: string option
        splitAmount: float option
    }

    type MaybePurchaseStatus = {
        whoBuys: string option
        alreadyBought: bool
    }

    type MaybePlannedGift = {
        person: MaybePerson
        gift: MaybeGift
        isGiftSplitted: bool
        split: MaybeSplit list
        purchaseStatus: MaybePurchaseStatus
    }