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

    type PlanningGift = {
        person: Person
        gift: Gift
        splitList: Split List
    }

    let createPlannedGift person gift splitList =
        {
            person = person
            gift = gift
            splitList = splitList
        }


    type MaybePerson = {
        name: string option
        geplanteAusgabe: float option
    }

    type MaybeGeschenk = {
        name: string option
        kosten: float option
    }

    type MaybeAnteil = {
        beteiligtePerson: MaybePerson
        anteilPerson: float option
    }

    type MaybeGeschenkEmpfänger = {
        person: MaybePerson
        geschenk: MaybeGeschenk
        betrag: MaybeAnteil list
    }