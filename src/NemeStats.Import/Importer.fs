module NemeStats.Import.Importer

open System.Threading

open FSharp.Control.Rop.TaskResult

open FSharp.Control.Rop
open NemeStats.Import.BoardGameGeek
open NemeStats.Import.BoardGameGeek.Functions
open NemeStats.Import.NemeStats



/// Return BGG players that are not created in NemeStats
let getMissingPlayers cancellationToken bggUsername (gamingGroup: GamingGroup) () =
    taskResult {
        //TODO parameter - dateFrom, dateTo
        let! bggPlays = BoardGameGeek.Api.getPlays cancellationToken bggUsername (Date.create(2019,5,1)) Date.Today
        let! nemePlayers = NemeStats.Api.Players.getPlayersInGamingGroup cancellationToken gamingGroup
        
        let nemePlayerNameSet = nemePlayers |> List.map (fun x -> Some x.Name) |> Set.ofList
        
        return
            bggPlays
            |> getUniquePlayers
            |> List.where (fun x ->
                nemePlayerNameSet |> Set.contains x.Name |> not)
    }
    
    
    
    
//TODO handle UNKNOWN PLAYERS - some higher level import logic
//TODO support for canonical names!


let createPlayers cancellationToken authenticationToken gamingGroup (players: PlayerIdentification list) =
    taskResult {
        return!
            players
            |> List.choose (fun x -> x.Name)
            |> List.distinct 
            |> List.map (fun x -> Api.Players.createPlayer cancellationToken authenticationToken gamingGroup x)
            //TODO TEST THIS
            //TODO mb we should return all correct results and all errors? if we need the correct results even when something fails. but then we could not return Result
            |> TaskResult.bisequence
    }    
    