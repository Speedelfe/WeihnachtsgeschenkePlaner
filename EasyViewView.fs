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


module EasyViewView =
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

    let giftEmpfaengerView (index:int, plannedGift:PlannedGift): IView list =
        [
            TextBlock.create [
                TextBlock.column 0
                TextBlock.row index
                TextBlock.text plannedGift.person.name
            ]
            TextBlock.create [
                TextBlock.column 1
                TextBlock.row index
                let euroSymbol = "€"
                match plannedGift.person.plannedExpenses with
                | Some expenses -> TextBlock.text ((expenses |> string) + " " + euroSymbol)
                | None -> TextBlock.text ""
            ]
            TextBlock.create [
                TextBlock.column 2
                TextBlock.row index
                TextBlock.text plannedGift.gift.description
            ]
            TextBlock.create [
                TextBlock.column 3
                TextBlock.row index
                let euroSymbol = "€"
                TextBlock.text ((plannedGift.gift.totalCost |> string) + " " + euroSymbol)
            ]
            CheckBox.create [
                CheckBox.column 4
                CheckBox.row index
                match plannedGift.purchaseStatus with
                | Some state -> CheckBox.isChecked state.alreadyBought
                | None -> CheckBox.isChecked false
            ]
            TextBlock.create [
                TextBlock.column 5
                TextBlock.row index
                match plannedGift.purchaseStatus with
                | Some buyer -> (TextBlock.text buyer.whoBuys)
                | None -> (TextBlock.text "")
            ]
        ]

    let easyView (state: State) (giftList: PlannedGift list) dispatch =
        DockPanel.create [
            DockPanel.margin 5.0
            DockPanel.children [
                TextBlock.create [
                    TextBlock.dock Dock.Top
                    TextBlock.text "Einfache Planungs Ansicht"
                ]
                Grid.create [
                    Grid.dock Dock.Left
                    Grid.columnDefinitions "1*, 1*, 1*, 1*, 1*, 1*"
                    Grid.rowDefinitions (
                        giftList
                        |> List.map (fun _ -> "Auto")
                        |> List.reduce (fun a b -> a + "," + b))
                    Grid.children (
                        giftList
                        |> List.indexed
                        |> List.collect giftEmpfaengerView)
                ]
            ]
        ]