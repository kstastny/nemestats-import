module NemeStats.Import.BoardGameGeek.Functions


let getUniquePlayers (plays: Play list) =
    plays
    |> List.collect (fun x ->
        x.Players |> List.map (fun p -> (p.UserId, p.Username, p.Name))
        )
    |> List.distinct