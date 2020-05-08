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

module View =
    type Modus =
        | EasyView
        | NotEasyView
        | NewPresent
        | Expenses
        | Stats

    type State = {
        planer: PlannedGift list
        modus: Modus
        newGiftState: NewGiftView.State
        easyViewState: EasyViewView.State
    }

    type Msg =
        | ChangeMode of Modus
        | NewGiftMsg of NewGiftView.Msg
        | SaveNewGift

    let init () =
        { planer = FileManagement.loadGiftList ()
          modus = NewPresent
          newGiftState = NewGiftView.init ()
          easyViewState = EasyViewView.init()
        }

    let update (msg: Msg) state =
        match msg with
        | ChangeMode newMode ->
            { state with modus = newMode }
        | NewGiftMsg newGiftMsg ->
            let newState = NewGiftView.update newGiftMsg state.newGiftState
            { state with newGiftState = newState }
        | SaveNewGift ->
            match newGift state.newGiftState.newPlannedGift with
            | Some plannedGift ->
                let newGiftList = state.planer |> List.append [plannedGift]
                saveGiftList newGiftList
                {state with
                    planer = newGiftList
                    newGiftState = { newPlannedGift = createEmptyMaybePlannedGift() } }
            | _ ->
                printfn "Gift konnte nicht gespeichert werden\n%A" state.newGiftState.newPlannedGift
                state

    let menu (state: State) dispatch =
        StackPanel.create [
            StackPanel.classes [ "menu" ]
            StackPanel.orientation Orientation.Vertical
            StackPanel.spacing 100.0
            StackPanel.children [
                StackPanel.create [
                    StackPanel.spacing 10.0
                    StackPanel.orientation Orientation.Vertical
                    StackPanel.children [
                        Button.create [
                            Button.content "Planung (einfache Ansicht)"
                            Button.onClick (fun _ -> ChangeMode EasyView |> dispatch)
                        ]
                        Button.create [
                            Button.content "Planung (ausführliche Ansicht)"
                            Button.onClick (fun _ -> ChangeMode NotEasyView |> dispatch)
                        ]
                        Button.create [
                            Button.content "Neues Geschenk planen"
                            Button.onClick (fun _ -> ChangeMode NewPresent |> dispatch)
                        ]
                        Button.create [
                            Button.content "Ausgaben Planung"
                            Button.onClick (fun _ -> ChangeMode Expenses |> dispatch)
                        ]
                    ]
                ]
                StackPanel.create [
                    StackPanel.spacing 10.0
                    StackPanel.orientation Orientation.Vertical
                    StackPanel.children [
                        Button.create [
                            Button.content "Statistiken"
                        ]
                    ]
                ]
            ]
        ]


    let notEasyView (state: State) dispatch =
        DockPanel.create [
            DockPanel.margin 5.0
            DockPanel.children [
                TextBlock.create [
                    TextBlock.text "Ausführliche Planungs Ansicht"
                ]
            ]
        ]

    let expensesView (state: State) dispatch =
        DockPanel.create [
            DockPanel.margin 5.0
            DockPanel.children [
                TextBlock.create [
                    TextBlock.text "Übersicht über Ausgaben"
                ]
            ]
        ]

    let modeView (state: State) dispatch =
        match state.modus with
        | EasyView -> EasyViewView.easyView state.easyViewState dispatch
        | NotEasyView -> notEasyView state dispatch
        | NewPresent -> NewGiftView.view state.newGiftState (NewGiftMsg >> dispatch) (fun () -> SaveNewGift |> dispatch)
        | Expenses -> expensesView state dispatch
        | _ -> DockPanel.create []

    let view (state: State) dispatch =
        DockPanel.create [
            DockPanel.children [
                Border.create [
                    Border.borderThickness (0.0, 0.0, 2.0, 0.0)
                    Border.borderBrush "black"
                    Border.dock Dock.Left
                    Border.child (menu state dispatch)
                ]
                modeView state dispatch
            ]
        ]
