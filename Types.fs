namespace WeihnachtsgeschenkePlaner

module Geschenk =
    type Person = {
        name: string
        geplanteAusgabe: float
    }

    let createPerson name ausgabe =
        {
            Person.name = name
            geplanteAusgabe = ausgabe
        }

    type Geschenk = {
        name: string
        kosten: float
    }

    type Anteil = {
        beteiligtePerson: Person
        anteilPerson: float
    }

    type GeschenkEmpfänger = {
        person: Person
        geschenk: Geschenk
        betrag: Anteil List
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