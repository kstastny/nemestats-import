open System

open System.Threading

open FSharp.Control.Tasks.V2
open FSharp.Control.Rop.TaskResult

open NemeStats.Import
open NemeStats.Import.BoardGameGeek
open NemeStats.Import.Date
open NemeStats.Import.NemeStats

[<EntryPoint>]
let main argv =
    
    let tr =
        taskResult {
//            let! result =
//                { Id = 14711 }
//                |>  NemeStats.Api.getPlayersInGamingGroup CancellationToken.None
//            result
//            |> List.iter (printfn "%A")

            let! bggPlays =
                BoardGameGeek.Api.getPlays "aredhel" (Date.fromDateTime(DateTime(2018,1,1))) (Date.Today)

//            bggPlays |> List.iter (fun x -> printfn "%s: %s" (x.Date |> Date.value) x.GameName)
//            printfn "TOTAL LENGTH: %i" (bggPlays |> List.length)
//            printfn "TOTAL PLAY QUANTITY: %i" (bggPlays |> List.sumBy (fun x -> x.Quantity))

            bggPlays
            |> Functions.getUniquePlayers
            |> List.iter (fun (userId, userName, name) -> printfn "%A" name)
        }
    
    tr.Wait ()
    
    printfn "%s"
        (match tr.Result with
         | Ok _ -> "OK"
         | Error e -> sprintf "%A" e)
    
    0
