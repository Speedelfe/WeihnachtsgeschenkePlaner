namespace WeihnachtsgeschenkePlaner

open Avalonia
open Avalonia.Controls
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

module View =
    type Modus =
        | EasyView
        | NotEasyView
        | NewPresent
        | Expenses
        | Stats
        | Settings

    type State = {
        planer: PlannedGift list
        persons: Person list
        modus: Modus
        newGiftState: NewGiftView.State
        easyViewState: EasyViewView.State
    }

    type Msg<'v> =
        | ChangeMode of Modus
        | NewGiftMsg of NewGiftView.Msg<'v>
        | SaveNewGift

    let init () =
        let settingList = loadSettings ()
        let giftList, personList = loadGift ()
        {
          planer = giftList
          persons = personList
          modus = EasyView
          newGiftState = NewGiftView.init ()
          easyViewState = EasyViewView.init()
        }

    let update msg state =
        match msg with
        | ChangeMode newMode ->
            { state with modus = newMode }
        | NewGiftMsg newGiftMsg ->
            let newState = NewGiftView.update newGiftMsg state.newGiftState
            { state with newGiftState = newState }
        | SaveNewGift ->
            match newGift state.newGiftState.newPlannedGift with
            | Some plannedGift ->
                let newGiftList = List.append state.planer [plannedGift]

                // Brauchen wir eine neue Person oder gibt es die schon?
                let personList =
                    state.persons
                    |> List.tryFind (fun person -> person.name = plannedGift.receiver)
                    |> function
                        | Some person ->
                            state.persons
                            |> List.map (fun p -> if p.name = person.name then { p with plannedExpenses = state.newGiftState.person.plannedExpenses } else p)
                        | None ->
                            List.append state.persons [ newPerson state.newGiftState.person ]

                saveGift newGiftList personList
                { state with
                    persons = personList
                    planer = newGiftList
                    newGiftState = NewGiftView.init () }
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

    let settingsView (state: State) dispatch =
        DockPanel.create [
            DockPanel.margin 5.0
            DockPanel.children [
                TextBlock.create [
                    TextBlock.text "Einstellungen erforderlich"
                ]
            ]
        ]

    let modeView (state: State) dispatch =
        match state.modus with
        | EasyView -> EasyViewView.easyView state.easyViewState state.planer dispatch
        | NotEasyView -> NotEasyViewView.notEasyView state.planer state.persons dispatch
        | NewPresent -> NewGiftView.view state.newGiftState state.persons (NewGiftMsg >> dispatch) (fun () -> SaveNewGift |> dispatch)
        | Expenses -> ExpensesView.expensesView state.planer state.persons dispatch
        | Settings -> settingsView state dispatch
        | _ -> DockPanel.create []


    let view (state: State) dispatch =
        match state.modus with
        | Settings ->
            DockPanel.create [
                DockPanel.children [
                    modeView state dispatch
                ]
            ]
        | _ ->
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
