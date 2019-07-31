namespace NemeStats.Import.BoardGameGeek

open NemeStats.Import.Date

type Player =
    {
        UserId: int option
        Username: string option
        Name: string option
        Score: int option
        Win: bool
    }


type Play =
    {
        Id: int
        Date: Date
        Quantity: int
        Incomplete: bool
        DontTrackWinStats: bool
        Location: string
        GameName: string
        GameId: int
        Comments: string option
        Players: Player list
    }

