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

    let expensesView (giftList: PlannedGift list) personList dispatch =
        DockPanel.create [
            DockPanel.children [
                StackPanel.create [
                    StackPanel.orientation Orientation.Vertical
                    StackPanel.margin 25.0
                    StackPanel.spacing 15.0
                    StackPanel.name "Expenses Info"
                    StackPanel.children [
                        TextBlock.create [
                            TextBlock.dock Dock.Top
                            TextBlock.text "Übersicht über die Ausgaben Planung"
                        ]
                        Grid.create [
                            Grid.columnDefinitions "1*, 1*, 1*, 1*"
                        ]
                        Grid.create [
                            Grid.columnDefinitions "1*, 1*, 1*"
                            Grid.rowDefinitions "2"
                        ]
                    ]
                ]
            ]
        ]



