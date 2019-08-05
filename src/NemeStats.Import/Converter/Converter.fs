module NemeStats.Import.Converter



open FSharp.Data

type PlayerSynonyms =
        JsonProvider<"""Converter/PlayerSynonyms.json"""
        ,EmbeddedResource = """NemeStats.Import, NemeStats.Import.Converter.PlayerSynonyms.json""">


let getCanonicalPlayerName (canonicalNameByName: Map<string, string>) name =
    
    canonicalNameByName |> Map.tryFind name |> Option.defaultValue name
    
    
    
//let bggPlayerToNemePlayer (bggPlayer: BoardGameGeek.PlayerIdentification) : NemeStats.Player =
    
       