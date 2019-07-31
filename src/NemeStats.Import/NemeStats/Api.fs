module NemeStats.Import.NemeStats.Api

open System
open System.Threading
open System.Threading.Tasks


open FSharp.Data
open FSharp.Control.Rop.TaskResult


open NemeStats.Import.Http



let baseUri = "https://nemestats.com/api/v2"


type private PlayersInGamingGroupJson =
    JsonProvider<"""NemeStats/SampleData/GetPlayersInGamingGroup.json"""
    ,EmbeddedResource = """NemeStats.Import, NemeStats.Import.NemeStats.SampleData.GetPlayersInGamingGroup.json""">

let private getPlayersInGamingGroupUri groupId =
    sprintf "%s/Players/?gamingGroupId=%i" baseUri groupId
    |> Uri


let private jsonToPlayers (root: PlayersInGamingGroupJson.Root) =
    root.Players
    |> Array.map (fun x -> {
        Id = x.PlayerId
        Name = x.PlayerName
    })


//TODO error handling
let getPlayersInGamingGroup (cancellationToken: CancellationToken) (gamingGroup: GamingGroup) : Task<Result<Player list, HttpError>> =
    
    taskResult {
        let! response =
            getPlayersInGamingGroupUri gamingGroup.Id
            |> createRequest System.Net.Http.HttpMethod.Get
            |> getResponse cancellationToken
        
        //TODO tryParse
        return
            PlayersInGamingGroupJson.Parse response
            |> jsonToPlayers
            |> List.ofArray
    }
    