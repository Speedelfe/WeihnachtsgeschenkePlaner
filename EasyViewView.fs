namespace WeihnachtsgeschenkePlaner

open Avalonia
open Avalonia.Controls
open Avalonia.Input
open Avalonia.Layout
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Elmish
open System

open WeihnachtsgeschenkePlaner.Types
open WeihnachtsgeschenkePlaner.GiftManagement
open WeihnachtsgeschenkePlaner.FileManagement


module EasyViewView =
    type State = {
        planer: PlannedGift list
    }

    type Msg =
        | Update of string

    let init () =
        {
            planer = FileManagement.loadGiftList ()
        }

    let update (msg: Msg) state =
        match msg with
        | Update _ -> ()


    let easyView (state: State) dispatch =
        DockPanel.create [
            DockPanel.margin 5.0
            DockPanel.children [
                TextBlock.create [
                    TextBlock.text "Einfache Planungs Ansicht"
                ]
            ]
        ]
