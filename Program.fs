// Learn more about F# at http://fsharp.org
namespace WeihnachtsgeschenkePlaner

open Avalonia
open Elmish
open Avalonia.Controls
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.FuncUI
open Avalonia.FuncUI.Components.Hosts
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Elmish
open System

open WeihnachtsgeschenkePlaner.Geschenk

module Program =
    type State = {
        planer: Planer
    }

    type Msg =
        | None

    let init () =
        { planer = [] }

    let update (msg: Msg) state =
        state

    let view (state: State) dispatch =
        DockPanel.create [
            DockPanel.background "red"
        ]

    type MainWindow() as this =
        inherit HostWindow()
        do
            base.Title <- "Weihnachtsgeschenke Planer"
            base.Height <- 400.0
            base.Width <- 600.0

            Program.mkSimple init update view
            |> Program.withHost this
            |> Program.withConsoleTrace
            |> Program.run

    type App() =
        inherit Application()

        override this.Initialize() =
            this.Styles.Load "avares://Citrus.Avalonia/Rust.xaml"

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
