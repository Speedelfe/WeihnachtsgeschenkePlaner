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
          modus = EasyView }

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
                    TextBlock.text "Lege ein neues Geschenk an"
                ]
                StackPanel.create [
                    StackPanel.orientation Orientation.Vertical
                    StackPanel.children [
                        StackPanel.create [
                            StackPanel.orientation Orientation.Horizontal
                            StackPanel.children [
                                StackPanel.create [
                                    StackPanel.name "Present Info"

                                ]
                                StackPanel.create [
                                    StackPanel.name "Max Ausgabe"
                                    StackPanel.children [
                                        TextBox.create [
                                            TextBox.watermark " Was soll höchstens für die Person ausgegeben werden?"
                                        ]
                                    ]
                                ]
                            ]
                        ]
                        StackPanel.create [
                            StackPanel.orientation Orientation.Horizontal
                            StackPanel.children [
                                StackPanel.create [
                                    StackPanel.name "Orderd?"

                                ]
                                StackPanel.create [
                                    StackPanel.name "Anteil Info"
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
