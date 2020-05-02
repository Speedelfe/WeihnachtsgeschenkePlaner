// Learn more about F# at http://fsharp.org
namespace WeihnachtsgeschenkePlaner

open Avalonia
open Elmish
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Input
open Avalonia.FuncUI
open Avalonia.FuncUI.Components.Hosts
open Avalonia.FuncUI.Elmish

open WeihnachtsgeschenkePlaner.View

module Program =
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
        (*let person1 = createPerson "Horst" 50.0
        let geschenk1 = {Geschenk.name = "Blumen"; kosten = 5.80 }
        let anteil1 = { beteiligtePerson = person1; anteilPerson = geschenk1.kosten }
        let empfaenger = {person = person1; geschenk = geschenk1; betrag = [anteil1]}

        saveGiftList [empfaenger]*)

        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
            .StartWithClassicDesktopLifetime(args)
