namespace WeihnachtsgeschenkePlaner

module Geschenk =
    type Person = {
        name: string
        plannedExpenses: float
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


    type PlannedGift = {
        person: Person
        gift: Gift
        splitList: Split List
        purchaseStatus: PurchaseStatus
    }

    let createPlannedGift person gift splitList purchaseStatus =
        {
            person = person
            gift = gift
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
        alreadyBought: bool option
    }

    type MaybePlannedGift = {
        person: MaybePerson
        gift: MaybeGift
        split: MaybeSplit list
        purchaseStatus: MaybePurchaseStatus
    }