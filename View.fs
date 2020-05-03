namespace WeihnachtsgeschenkePlaner

open Avalonia
open Avalonia.Controls
open Avalonia.Input
open Avalonia.Layout
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Elmish
open System

open WeihnachtsgeschenkePlaner.Geschenk
open WeihnachtsgeschenkePlaner.GiftManagement

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
        newGeschenkEmpfaenger: MaybePlannedGift
    }

    type Msg =
        | ChangeMode of Modus
        | SetMaybePersonName of string option
        | SetMaybePersonPlannedExpenses of float option
        | SetMaybeGiftDescription of string option
        | SetMaybeGiftCosts of float option
        | SetMaybePurchaseStatusWhoBuys of string option
        | SetMaybePurchaseStatusAlreadyBought of bool option

    let (|Float|_|) (str: string) =
        match System.Double.TryParse(str) with
        | (true,number) -> Some(number)
        | _ -> None

    let init () =
        { planer = []
          modus = NewPresent
          newGeschenkEmpfaenger = createEmptyMaybeGeschenkEmpfänger () }

    let update (msg: Msg) state =
        match msg with
        | ChangeMode newMode ->
            { state with modus = newMode }
        | SetMaybePersonName name ->
            // TODO: Lenses
            let person = { state.newGeschenkEmpfaenger.person with name = name }
            let geschenkEmpfänger = { state.newGeschenkEmpfaenger with person = person }
            { state with newGeschenkEmpfaenger = geschenkEmpfänger}
        | SetMaybePersonPlannedExpenses expense ->
            let person = {state.newGeschenkEmpfaenger.person with plannedExpenses = expense}
            let geschenkEmpfänger = { state.newGeschenkEmpfaenger with person = person}
            {state with newGeschenkEmpfaenger = geschenkEmpfänger}
        | SetMaybeGiftDescription description ->
            let gift = {state.newGeschenkEmpfaenger.gift with description = description}
            let geschenkEmpfänger = { state.newGeschenkEmpfaenger with gift = gift}
            { state with newGeschenkEmpfaenger = geschenkEmpfänger}
        | SetMaybeGiftCosts costs ->
            let gift = { state.newGeschenkEmpfaenger.gift with totalCosts = costs}
            let geschenkEmpfänger = { state.newGeschenkEmpfaenger with gift = gift}
            {state with newGeschenkEmpfaenger = geschenkEmpfänger}
        | SetMaybePurchaseStatusWhoBuys whoBuys ->
            let purchaseStatus = { state.newGeschenkEmpfaenger.purchaseStatus with whoBuys = whoBuys}
            let geschenkEmpfänger = { state.newGeschenkEmpfaenger with purchaseStatus = purchaseStatus}
            {state with newGeschenkEmpfaenger = geschenkEmpfänger}
        | SetMaybePurchaseStatusAlreadyBought alreadyBought ->
            let purchaseStatus = { state.newGeschenkEmpfaenger.purchaseStatus with alreadyBought = alreadyBought}
            let geschenkEmpfänger = { state.newGeschenkEmpfaenger with purchaseStatus = purchaseStatus}
            {state with newGeschenkEmpfaenger = geschenkEmpfänger}

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

    let easyView (state: State) dispatch =
        DockPanel.create [
            DockPanel.margin 5.0
            DockPanel.children [
                TextBlock.create [
                    TextBlock.text "Einfache Planungs Ansicht"
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

    let newPresentView (state: State) dispatch =
        DockPanel.create [
            DockPanel.margin 5.0
            DockPanel.children [
                TextBlock.create [
                    TextBlock.dock Dock.Top
                    TextBlock.text "Lege ein neues Geschenk an"
                ]
                StackPanel.create [
                    StackPanel.dock Dock.Bottom
                    StackPanel.orientation Orientation.Horizontal
                    StackPanel.spacing 10.0
                    StackPanel.children [
                        Button.create [
                            Button.content "Speichern"
                        ]
                        Button.create [
                            Button.content "Zurücksetzen"
                        ]
                    ]
                ]
                Grid.create [
                    Grid.columnDefinitions "1*, 1*"
                    Grid.rowDefinitions "1*, 1*"
                    Grid.children [
                        StackPanel.create [
                            StackPanel.orientation Orientation.Vertical
                            StackPanel.margin 25.0
                            StackPanel.column 0
                            StackPanel.row 0
                            StackPanel.spacing 15.0
                            StackPanel.name "Present Info"
                            StackPanel.children [
                                TextBox.create [
                                    TextBox.text (match state.newGeschenkEmpfaenger.person.name with | None -> "" | Some name -> name)
                                    TextBox.watermark "Für wen soll das Geschenk sein?*"
                                    TextBox.width 400.0
                                    TextBox.onTextChanged (fun newName -> SetMaybePersonName (if newName = "" then None else Some newName) |> dispatch)
                                ]
                                TextBox.create [
                                    TextBox.text (match state.newGeschenkEmpfaenger.gift.description with | None -> "" | Some name -> name)
                                    TextBox.watermark "Was möchtest du verschenken?*"
                                    TextBox.width 400.0
                                    TextBox.onTextChanged (fun newGiftDescription -> SetMaybeGiftDescription (if newGiftDescription = "" then None else Some newGiftDescription) |> dispatch)
                                ]
                                TextBox.create [
                                    TextBox.text (match state.newGeschenkEmpfaenger.gift.totalCosts with | None -> "" | Some costs -> (string costs))
                                    TextBox.watermark "Was kostet es?*"
                                    TextBox.width 400.0
                                    TextBox.onTextChanged (
                                        function
                                        | Float newGiftCosts -> Some newGiftCosts |> SetMaybeGiftCosts |> dispatch
                                        | _ -> ()
                                    )
                                ]
                            ]
                        ]
                        StackPanel.create [
                            StackPanel.orientation Orientation.Vertical
                            StackPanel.margin 25.0
                            StackPanel.column 1
                            StackPanel.row 0
                            StackPanel.name "Max Ausgabe"
                            StackPanel.children [
                                TextBox.create [
                                    TextBox.text (match state.newGeschenkEmpfaenger.person.plannedExpenses with | None -> "" | Some expense -> (string expense))
                                    TextBox.watermark "Was soll höchstens für die Person ausgegeben werden?"
                                    TextBox.width 400.0
                                    TextBox.onTextChanged (
                                        function
                                        | Float newExpense -> Some newExpense |> SetMaybePersonPlannedExpenses |> dispatch
                                        | _ -> ()
                                    )
                                ]
                            ]
                        ]
                        StackPanel.create [
                            StackPanel.orientation Orientation.Vertical
                            StackPanel.margin 25.0
                            StackPanel.column 0
                            StackPanel.row 1
                            StackPanel.spacing 15.0
                            StackPanel.name "Orderd?"
                            StackPanel.children [
                                TextBox.create [
                                    TextBox.watermark "Wer besorgt das Geschenk?"
                                    TextBox.width 400.0
                                    TextBox.onTextChanged (fun newWhoBuys -> SetMaybePurchaseStatusWhoBuys (if newWhoBuys = "" then None else Some newWhoBuys) |> dispatch)
                                ]
                                CheckBox.create [
                                    CheckBox.content "Wurde das Geschenk schon besorgt?"
                                    CheckBox.padding 25.0
                                    // CheckBox.onClick (fun newAlreadyBought -> SetMaybePurchaseStatusAlreadyBought (if (not newAlreadyBought) then None else Some true) |> dispatch)
                                ]
                            ]
                        ]
                        StackPanel.create [
                            StackPanel.orientation Orientation.Vertical
                            StackPanel.margin 25.0
                            StackPanel.column 1
                            StackPanel.row 1
                            StackPanel.spacing 15.0
                            StackPanel.name "Anteil Info"
                            StackPanel.children [
                                CheckBox.create [
                                    CheckBox.content "Ist es ein geteiltes Geschenk?"
                                    CheckBox.padding 25.0
                                ]

                            ]
                        ]
                    ]
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
        | EasyView -> easyView state dispatch
        | NotEasyView -> notEasyView state dispatch
        | NewPresent -> newPresentView state dispatch
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