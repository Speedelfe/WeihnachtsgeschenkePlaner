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

module NewGiftView =
    type State = {
        newPlannedGift: MaybePlannedGift
        person: MaybePerson
    }

    module StateLenses =
        let person = Lens((fun s -> s.person), (fun s p -> { s with person = p }))
        let newPlannedGift = Lens((fun s -> s.newPlannedGift), (fun s p -> { s with newPlannedGift = p }))

        let personName = person << MaybePersonLenses.name
        let personPlannedExpenses = person << MaybePersonLenses.plannedExpenses
        let newPlannedGiftReceiver = newPlannedGift << MaybePlannedGiftLenses.receiver

    type LensValuePair<'v> = Lens<State, 'v> * 'v

    type Msg<'v> =
        | LensesMsg of LensValuePair<'v> list
        | SetMaybePersonName of PersonName option
        | SetMaybeGiftDescription of string option
        | SetMaybeGiftCosts of float option
        | SetMaybePurchaseStatusWhoBuys of PersonName option
        | SetMaybePurchaseStatusAlreadyBought of bool
        | ResetNewGift

    let init () =
        {
            newPlannedGift = createEmptyMaybePlannedGift ()
            person = { name = None; plannedExpenses = None }
        }

    let update msg state =
        match msg with
        | LensesMsg lensValuePairList ->
            lensValuePairList
            |> List.fold (fun state (lens, value) -> setl lens value state) state
        | SetMaybePersonName name ->
            state
            |> setl StateLenses.personName name
            |> setl StateLenses.newPlannedGiftReceiver name
        | SetMaybeGiftDescription description ->
            let gift = {state.newPlannedGift.gift with description = description}
            let geschenkEmpfänger = { state.newPlannedGift with gift = gift}
            { state with newPlannedGift = geschenkEmpfänger}
        | SetMaybeGiftCosts costs ->
            let gift = { state.newPlannedGift.gift with totalCosts = costs}
            let geschenkEmpfänger = { state.newPlannedGift with gift = gift}
            {state with newPlannedGift = geschenkEmpfänger}
        | SetMaybePurchaseStatusWhoBuys whoBuys ->
            let purchaseStatus = { state.newPlannedGift.purchaseStatus with whoBuys = whoBuys}
            let geschenkEmpfänger = { state.newPlannedGift with purchaseStatus = purchaseStatus}
            {state with newPlannedGift = geschenkEmpfänger}
        | SetMaybePurchaseStatusAlreadyBought alreadyBought ->
            let purchaseStatus = { state.newPlannedGift.purchaseStatus with alreadyBought = alreadyBought}
            let geschenkEmpfänger = { state.newPlannedGift with purchaseStatus = purchaseStatus}
            {state with newPlannedGift = geschenkEmpfänger}
        | ResetNewGift ->
            {state with newPlannedGift = createEmptyMaybePlannedGift()}

    let quadrant1View state dispatch =
        StackPanel.create [
            StackPanel.orientation Orientation.Vertical
            StackPanel.margin 25.0
            StackPanel.spacing 15.0
            StackPanel.name "Present Info"
            StackPanel.children [
                TextBox.create [
                    TextBox.text (match state.newPlannedGift.receiver with | Some (PersonName name) -> name | None -> "")
                    TextBox.watermark "Für wen soll das Geschenk sein?*"
                    TextBox.width 400.0
                    TextBox.onTextChanged (fun newName -> SetMaybePersonName (if newName = "" then None else newName |> PersonName |> Some) |> dispatch)
                ]
                TextBox.create [
                    TextBox.text (match state.newPlannedGift.gift.description with | None -> "" | Some name -> name)
                    TextBox.watermark "Was möchtest du verschenken?*"
                    TextBox.width 400.0
                    TextBox.onTextChanged (fun newGiftDescription -> SetMaybeGiftDescription (if newGiftDescription = "" then None else Some newGiftDescription) |> dispatch)
                ]
                TextBox.create [
                    TextBox.text (match state.newPlannedGift.gift.totalCosts with | None -> "" | Some costs -> (string costs))
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

    let quadrant2View personList state dispatch =
        StackPanel.create [
            StackPanel.orientation Orientation.Vertical
            StackPanel.margin 25.0
            StackPanel.name "Max Ausgabe"
            StackPanel.children [
                TextBox.create [
                    TextBox.text (
                        personList
                        |> List.tryFind (fun person -> Some person.name = state.newPlannedGift.receiver)
                        |> function
                            | Some { plannedExpenses = Some expenses } -> expenses |> string
                            | Some { plannedExpenses = None }
                            | None ->
                                match state.person.plannedExpenses with
                                | Some expense -> (string expense)
                                | None -> "")
                    TextBox.watermark "Was soll höchstens für die Person ausgegeben werden?"
                    TextBox.width 400.0
                    TextBox.onTextChanged (
                        function
                        | Float newExpense -> [(StateLenses.personPlannedExpenses, Some newExpense)] |> LensesMsg |> dispatch
                        | _ -> ()
                    )
                ]
            ]
        ]

    let quadrant3View state dispatch =
        StackPanel.create [
            StackPanel.orientation Orientation.Vertical
            StackPanel.margin 25.0
            StackPanel.spacing 15.0
            StackPanel.name "Orderd?"
            StackPanel.children [
                TextBox.create [
                    TextBox.watermark "Wer besorgt das Geschenk?"
                    TextBox.width 400.0
                    TextBox.onTextChanged (fun newWhoBuys -> SetMaybePurchaseStatusWhoBuys (if newWhoBuys = "" then None else newWhoBuys |> PersonName |> Some) |> dispatch)
                ]
                CheckBox.create [
                    CheckBox.content "Wurde das Geschenk schon besorgt?"
                    CheckBox.padding 25.0
                    CheckBox.onChecked (fun _ -> SetMaybePurchaseStatusAlreadyBought true |> dispatch)
                    CheckBox.onUnchecked (fun _ -> SetMaybePurchaseStatusAlreadyBought false |> dispatch)
                ]
            ]
        ]

    let quadrant4View state dispatch =
        StackPanel.create [
            StackPanel.orientation Orientation.Vertical
            StackPanel.margin 25.0
            StackPanel.spacing 15.0
            StackPanel.name "Anteil Info"
            StackPanel.children [
                CheckBox.create [
                    CheckBox.content "Ist es ein geteiltes Geschenk?"
                    CheckBox.padding 25.0
                ]
            ]
        ]

    let view (state: State) personList dispatch saveNewGift =
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
                            Button.onClick (fun _ -> saveNewGift())
                        ]
                        Button.create [
                            Button.content "Zurücksetzen"
                            Button.onClick (fun _ -> ResetNewGift |> dispatch)
                        ]
                    ]
                ]
                Grid.create [
                    Grid.columnDefinitions "1*, 1*"
                    Grid.rowDefinitions "1*, 1*"
                    Grid.children [
                        Border.create [
                            Border.borderBrush "black"
                            Border.borderThickness (0.0,0.0,1.0,1.0)
                            Border.column 0
                            Border.row 0
                            Border.child (quadrant1View state dispatch)
                        ]
                        Border.create [
                            Border.borderBrush "black"
                            Border.borderThickness (1.0,0.0,0.0,1.0)
                            Border.column 1
                            Border.row 0
                            Border.child (quadrant2View personList state dispatch)
                        ]
                        Border.create [
                            Border.borderBrush "black"
                            Border.borderThickness (0.0,1.0,1.0,0.0)
                            Border.column 0
                            Border.row 1
                            Border.child (quadrant3View state dispatch)
                        ]
                        Border.create [
                            Border.borderBrush "black"
                            Border.borderThickness (1.0,1.0,0.0,0.0)
                            Border.column 1
                            Border.row 1
                            Border.child (quadrant4View state dispatch)
                        ]
                    ]
                ]
            ]
        ]
