module JourneyInfoApp

open FSharp.Core
open HafasClientTypes
open HafasClientTypesUtils
open Fable.Core
open Fable.SimpleJson

let printLocations (client: HafasClient) name =
    promise {
        let! stopsOrLocations = client.locations name (Some defaultLocationOptions)
        let json1 = Json.stringify stopsOrLocations
        printfn "%s" json1
        let json2 = SimpleJson.toString (HafasClientTypesDump.dumpU2StopsLocations stopsOrLocations)
        printfn "%s" json2
    }
    |> ignore

let printReachableFrom (client: HafasClient) name =
    promise {
        try
            let! stations = client.locations name (Some defaultLocationOptions)
            let location =
                match stations.[0] with
                | Location location -> Some location
                | Stop stop -> stop.location
            match location with
            | Some location ->
                location.address <- Some "dummy"
                let! durations = client.reachableFrom location None
                let json1 = Json.stringify durations
                printfn "%s" json1
                let json2 = SimpleJson.toString (HafasClientTypesDump.dumpDurations durations)
                printfn "%s" json2
            | None -> ()

        with ex -> printf "printJourneyInfos error: %s %s" ex.Message ex.StackTrace
    }
    |> ignore

let printStop (client: HafasClient) id =
    promise {
        let! stop = client.stop id None
        let json1 = Json.stringify stop
        printfn "%s" json1
        let json2 = SimpleJson.toString (HafasClientTypesDump.dumpStop stop)
        printfn "%s" json2
    }
    |> ignore

let journeys (client: HafasClient) fromStation toStation =
    promise {
        try
            let! stationsFrom = client.locations fromStation (Some(locationOptions 1))
            let! stationsTo = client.locations toStation (Some(locationOptions 1))
            match stationsFrom.[0], stationsTo.[0] with
            | Stop sFrom, Stop sTo ->
                let! res = client.journeys (U3.Case1 sFrom.id) (U3.Case1 sTo.id) (Some defaultJourneyOptions)
                return res.journeys
            | _ -> return new ResizeArray<Journey>()
        with ex ->
            printf "journeys error: %s" ex.Message
            return new ResizeArray<Journey>()
    }

let printJourneyInfos (client: HafasClient) fromStation toStation =
    promise {
        try
            let! journeys = journeys client fromStation toStation
            let json1 = Json.stringify journeys
            printfn "%s" json1
            let json2 = SimpleJson.toString (HafasClientTypesDump.dumpJourneys journeys)
            printfn "%s" json2

        with ex -> printf "printJourneyInfos error: %s %s" ex.Message ex.StackTrace
    }
    |> ignore

let printTrip (client: HafasClient) tripId =
    promise {
        try
            let! trip = client.trip tripId "dummy" None
            let json1 = Json.stringify trip
            printfn "%s" json1
            let json2 = SimpleJson.toString (HafasClientTypesDump.dumpTrip trip)
            printfn "%s" json2

        with ex -> printf "printTrip error: %s %s" ex.Message ex.StackTrace
    }
    |> ignore

[<EntryPoint>]
let main argv =
    let profile =
        match argv.[0] with
        | "Db" -> Some Profile.Db
        | "Cfl" -> Some Profile.Cfl
        | "Vbb" -> Some Profile.Vbb
        | "Sncb" -> Some Profile.Sncb
        | _ -> None

    match profile with
    | Some p ->
        let client = HafasClient.createHafasClient p "client"

        match argv.[1] with
        | "journeys" when argv.Length = 4 -> printJourneyInfos client argv.[2] argv.[3]
        | "trip" when argv.Length = 3 -> printTrip client argv.[2]
        | "locations" when argv.Length = 3 -> printLocations client argv.[2]
        | "reachableFrom" when argv.Length = 3 -> printReachableFrom client argv.[2]
        | _ -> printfn "expected 'journeys x y|trip x|locations x|reachableFrom x'"

    | None -> printfn "expected 'Db|Cfl|Vbb|Sncb'"
    0
