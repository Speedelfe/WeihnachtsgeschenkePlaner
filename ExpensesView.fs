namespace WeihnachtsgeschenkePlaner

open Avalonia
open Avalonia.Controls
open Avalonia.FuncUI.Components
open Avalonia.FuncUI.Components.Hosts
open Avalonia.Input
open Avalonia.Layout
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Elmish
open Avalonia.FuncUI.Types
open System


open WeihnachtsgeschenkePlaner.Types
open WeihnachtsgeschenkePlaner.GiftManagement
open WeihnachtsgeschenkePlaner.FileManagement

module ExpensesView =

    let headlineGifts  () : IView list =
        [
            TextBlock.create [
                TextBlock.column 0
                TextBlock.row 0
                TextBlock.text "Person"
            ]
            TextBlock.create [
                TextBlock.column 1
                TextBlock.row 0
                TextBlock.text "geplante Ausgabe"
            ]
            TextBlock.create [
                TextBlock.column 2
                TextBlock.row 0
                TextBlock.text "Ausgabe(n) für Geschenk(e)"
            ]
            TextBlock.create [
                TextBlock.column 3
                TextBlock.row 0
                TextBlock.text "Differenz"
            ]
        ]

    let headlineTotalExpenses () : IView list =
        [
            TextBlock.create [
                TextBlock.column 0
                TextBlock.row 0
                TextBlock.text "Gesamte geplante Ausgaben"
            ]
            TextBlock.create [
                TextBlock.column 1
                TextBlock.row 0
                TextBlock.text "Gesamte Ausgaben aus Geschenken"
            ]
            TextBlock.create [
                TextBlock.column 2
                TextBlock.row 0
                TextBlock.text "Differenz"
            ]
        ]

    let getTotalPlannedExpenses personList =
            personList
            |> List.sumBy (fun person ->
                match person.plannedExpenses with
                | Some expenses -> expenses
                | None -> 0.0)

    let getTotalRealExpenses  giftList =
        giftList
        |> List.sumBy (fun gift -> gift.gift.totalCost)

    let getPersonExpensePerPerson (personName: PersonName) (giftList: PlannedGift List) =
        giftList
        |> List.filter (fun gift -> gift.receiver = personName )
        |> List.sumBy (fun gift -> gift.gift.totalCost)

    let getExpensesPerPersonList index person (giftList: PlannedGift List): IView list =
        [
            TextBlock.create [
                TextBlock.column 0
                TextBlock.row (index + 1)
                TextBlock.text (getPersonNameValue person.name)
            ]
            TextBlock.create [
                TextBlock.column 1
                TextBlock.row (index + 1)
                TextBlock.text (match person.plannedExpenses with
                    |Some expense -> expense |> string
                    |None -> 0.0 |> string)
            ]
            TextBlock.create [
                TextBlock.column 2
                TextBlock.row (index + 1)
                TextBlock.text (getPersonExpensePerPerson person.name giftList |> string)
            ]
            TextBlock.create [
                TextBlock.column 3
                TextBlock.row (index + 1)
                TextBlock.text (
                    match person.plannedExpenses with
                    |Some expenses-> expenses - getPersonExpensePerPerson person.name giftList|> string
                    |None -> - getPersonExpensePerPerson person.name giftList|> string)
            ]
        ]

    let getTotalValues personList giftList: IView list =
        [
            TextBlock.create [
                TextBlock.column 0
                TextBlock.row 1
                TextBlock.text (getTotalPlannedExpenses personList |> string)
            ]
            TextBlock.create [
                TextBlock.column 1
                TextBlock.row 1
                TextBlock.text (getTotalRealExpenses giftList |> string)
            ]
            TextBlock.create [
                TextBlock.column 2
                TextBlock.row 1
                TextBlock.text (getTotalPlannedExpenses personList - getTotalRealExpenses giftList|> string)
            ]
        ]

    let expensesView (giftList: PlannedGift list) (personList: Person list) dispatch =
        DockPanel.create [
            DockPanel.children [
                StackPanel.create [
                    StackPanel.orientation Orientation.Vertical
                    StackPanel.margin 25.0
                    StackPanel.dock Dock.Top
                    StackPanel.spacing 45.0
                    StackPanel.name "Expenses Info"
                    StackPanel.children [
                        TextBlock.create [
                            TextBlock.dock Dock.Top
                            TextBlock.text "Übersicht über die Ausgaben Planung"
                        ]
                        Grid.create [
                            Grid.columnDefinitions "1*, 1*, 1*, 1*"
                            Grid.rowDefinitions ("Auto" +
                                (
                                    (personList.Length, ",Auto")
                                    ||> String.replicate
                                )
                            )
                            Grid.children (List.concat [
                                headlineGifts()
                                personList
                                |> List.indexed
                                |> List.collect (fun (index, person) -> getExpensesPerPersonList index person giftList)
                            ])
                        ]
                        Grid.create [
                            Grid.columnDefinitions "1*, 1*, 1*"
                            Grid.rowDefinitions "Auto, Auto"
                            Grid.children ( List.concat [
                                headlineTotalExpenses()
                                getTotalValues personList giftList
                            ]
                            )
                        ]
                    ]
                ]
            ]
        ]



