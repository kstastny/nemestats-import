open System

open System.Threading

open FSharp.Control.Tasks.V2
open FSharp.Control.Rop.TaskResult

open NemeStats.Import
open NemeStats.Import.NemeStats

[<EntryPoint>]
let main argv =
    
    let tr =
        taskResult {
            let! result =
                { Id = 14711 }
                |>  NemeStats.Api.getPlayersInGamingGroup CancellationToken.None
            result
            |> List.iter (printfn "%A")
        }
    
    tr.Wait ()
    
    printfn "%s"
        (match tr.Result with
         | Ok _ -> "OK"
         | Error e -> sprintf "%A" e)
    
    0
