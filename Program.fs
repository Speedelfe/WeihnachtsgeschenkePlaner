// Learn more about F# at http://fsharp.org
namespace WeihnachtsgeschenkePlaner

open System

module Program =

    [<EntryPoint>]
    let main argv =
        printfn "Geschenkeplaner"
        printfn "Mögliche Optionen"
        printfn "1. aktuelle Planung ausgeben"
        printfn "2. neues Geschenk planen"
        printfn "Was möchtest du tun?"
        let key = Console.ReadKey()

        match key.KeyChar with
        | '1' -> printfn " aktuelle Planung"
            //TODO PLanung ausgeben
        | '2' -> printfn " neues Geschenk"

        | _ -> ()

        0 // return an integer exit code
