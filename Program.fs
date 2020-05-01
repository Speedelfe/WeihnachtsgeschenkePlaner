// Learn more about F# at http://fsharp.org
namespace WeihnachtsgeschenkePlaner

open Avalonia
open Elmish
open Avalonia.Controls
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Input
open Avalonia.Layout
open Avalonia.FuncUI
open Avalonia.FuncUI.Components.Hosts
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Elmish
open System

open WeihnachtsgeschenkePlaner.Geschenk

module Program =
    type Modus =
        | EasyView
        | NotEasyView
        | NewPresent
        | Expenses
        | Stats

    type State = {
        planer: Planer
        modus: Modus
    }

    type Msg =
        | ChangeMode of Modus

    let init () =
        { planer = []
          modus = NewPresent }

    let update (msg: Msg) state =
        match msg with
        | ChangeMode newMode ->
            { state with modus = newMode }

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
                                    TextBox.watermark "Für wen soll das Geschenk sein?"
                                    TextBox.width 400.0
                                ]
                                TextBox.create [
                                    TextBox.watermark "Was möchtest du verschenken?"
                                    TextBox.width 400.0
                                ]
                                TextBox.create [
                                    TextBox.watermark "Was kostet es?"
                                    TextBox.width 400.0
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
                                    TextBox.watermark "Was soll höchstens für die Person ausgegeben werden?"
                                    TextBox.width 400.0
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
                                ]
                                CheckBox.create [
                                    CheckBox.content "Wurde das Geschenk schon besorgt?"
                                    CheckBox.padding 25.0
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
                    Border.borderThickness 2.0
                    Border.borderBrush "red"
                    Border.dock Dock.Left
                    Border.child (menu state dispatch)
                ]
                modeView state dispatch
            ]
        ]

    type MainWindow() as this =
        inherit HostWindow()
        do
            base.Title <- "Weihnachtsgeschenke Planer"
            base.Height <- 600.0
            base.Width <- 1200.0

            this.AttachDevTools(KeyGesture(Key.F12))

            Program.mkSimple init update view
            |> Program.withHost this
            |> Program.withConsoleTrace
            |> Program.run

    type App() =
        inherit Application()

        override this.Initialize() =
            this.Styles.Load "avares://Citrus.Avalonia/Candy.xaml"
            this.Styles.Load "avares://WeihnachtsgeschenkePlaner/Styles.xaml"

        override this.OnFrameworkInitializationCompleted() =
            match this.ApplicationLifetime with
            | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
                let mainWindow = MainWindow()
                desktopLifetime.MainWindow <- mainWindow
            | _ -> ()

    [<EntryPoint>]
    let main (args: string[]) =
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
            .StartWithClassicDesktopLifetime(args)
