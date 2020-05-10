namespace WeihnachtsgeschenkePlaner

module Types =
    type PersonName = PersonName of string

    let getPersonNameValue (PersonName personName) = personName

    type MaybePerson = {
        name: PersonName option
        plannedExpenses: float option
    }

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
