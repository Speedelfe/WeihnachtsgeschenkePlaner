namespace WeihnachtsgeschenkePlaner

module Geschenk =
    type Person = {
        name: string
        geplanteAusgabe: float
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

    type Planer = GeschenkEmpfänger List
