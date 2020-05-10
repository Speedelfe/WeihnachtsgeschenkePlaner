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


module NotEasyViewView =
    type State = {
        nix: bool
    }

    type Msg =
        | Update of string

    let init () =
        {
            nix = false
        }

    let update (msg: Msg) state =
        match msg with
        | Update _ -> ()

    let borderMe (view:IView) =
        Border.create [
            Border.child view
        ]

    let getHeadline () : IView list =
        [
            TextBlock.create [
                TextBlock.column 0
                TextBlock.row 0
                TextBlock.text "Für"
            ]
            TextBlock.create [
                TextBlock.column 1
                TextBlock.row 0
                TextBlock.text "gepl. Ausgabe"
            ]
            TextBlock.create [
                TextBlock.column 2
                TextBlock.row 0
                TextBlock.text "Geschenk"
            ]
            TextBlock.create [
                TextBlock.column 3
                TextBlock.row 0
                TextBlock.text "Kosten"
            ]
            TextBlock.create [
                TextBlock.column 4
                TextBlock.row 0
                TextBlock.text "Besorgt?"
            ]
            TextBlock.create [
                TextBlock.column 5
                TextBlock.row 0
                TextBlock.text "Besorgt von"
            ]
            TextBlock.create [
                TextBlock.column 6
                TextBlock.row 0
                TextBlock.text "eigener Anteil"
            ]
            TextBlock.create [
                TextBlock.column 7
                TextBlock.row 0
                TextBlock.text "Anteil von"
            ]
            TextBlock.create [
                TextBlock.column 8
                TextBlock.row 0
                TextBlock.text "Anteil von"
            ]
        ]

    let giftEmpfaengerView personList (index:int, plannedGift:PlannedGift): IView list =
        [
            TextBlock.create [
                TextBlock.column 0
                TextBlock.row (index + 1)
                TextBlock.text (getPersonNameValue plannedGift.receiver)
            ]
            TextBlock.create [
                TextBlock.column 1
                TextBlock.row (index + 1)

                let euroSymbol = "€"

                personList
                |> List.tryFind (fun person -> person.name = plannedGift.receiver)
                |> function
                    | Some { plannedExpenses = Some expenses } -> (expenses |> string) + " " + euroSymbol
                    | Some { plannedExpenses = None }
                    | None -> ""
                |> TextBlock.text
            ]
            TextBlock.create [
                TextBlock.column 2
                TextBlock.row (index + 1)
                TextBlock.text plannedGift.gift.description
            ]
            TextBlock.create [
                TextBlock.column 3
                TextBlock.row (index + 1)
                let euroSymbol = "€"
                TextBlock.text ((plannedGift.gift.totalCost |> string) + " " + euroSymbol)
            ]
            CheckBox.create [
                CheckBox.column 4
                CheckBox.row (index + 1)
                CheckBox.isEnabled false
                CheckBox.isChecked (
                    match plannedGift.purchaseStatus with
                    | Some state -> state.alreadyBought
                    | None -> false
                )

            ]
            TextBlock.create [
                TextBlock.column 5
                TextBlock.row (index + 1)
                TextBlock.text (
                    match plannedGift.purchaseStatus with
                    | Some buyer -> (getPersonNameValue buyer.whoBuys)
                    | None ->  "")
            ]
            TextBlock.create [
                TextBlock.column 6
                TextBlock.row (index + 1)
                let euroSymbol = "€"
                match plannedGift.isGiftSplitted with
                | false -> TextBlock.text ((plannedGift.gift.totalCost |> string) + " " + euroSymbol)
                | true ->
                    plannedGift.splitList
                    |> List.sumBy (fun split -> split.splitAmount)
                    |> (fun sumOtherAmounts -> plannedGift.gift.totalCost - sumOtherAmounts |> string)
                    |> (fun selfAmount -> selfAmount + " " + euroSymbol)
                    |> TextBlock.text
            ]
        ]

    let notEasyView (giftList: PlannedGift list) personList dispatch =
        DockPanel.create [
            DockPanel.margin 5.0
            DockPanel.children [
                TextBlock.create [
                    TextBlock.dock Dock.Top
                    TextBlock.text "Einfache Planungs Ansicht"
                ]
                Grid.create [
                    Grid.dock Dock.Left
                    Grid.columnDefinitions "1*, 1*, 1*, 1*, 1*, 1*, 1*, 1*, 1*, 1*"
                    Grid.rowDefinitions ("Auto," +
                        (
                            giftList
                            |> List.map (fun _ -> "Auto")
                            |> List.reduce (fun a b -> a + "," + b)
                        )
                    )
                    Grid.children (List.concat [
                        getHeadline()
                        giftList
                        |> List.indexed
                        |> List.collect (giftEmpfaengerView personList)
                    ])
                ]
            ]
        ]