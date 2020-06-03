module JourneyInfoApp

open FSharp.Core
open FSharp.Reflection
open System.Collections.Generic
open HafasClientTypes
open HafasClientTypes.CreateClient
open HafasClientTypesProfile
open HafasClientTypesUtils
open Fable.Core
open Fable.SimpleJson

let execute action (o: obj) =
    match action with
    | Some a -> a o
    | None -> ()

let printRawJson o = printfn "%s" (JS.JSON.stringify o)

let printSimpleJson json = printfn "%s" (SimpleJson.toString json)

let printLocations (client: HafasClient) name action =
    promise {
        let! stopsOrLocations = client.locations name (Some defaultLocationOptions)
        execute action stopsOrLocations
        printSimpleJson (HafasClientTypesDump.dumpU3StationsStopsLocations stopsOrLocations)
    }
    |> ignore

let printDepartures (client: HafasClient) (station: string) action =
    promise {
        try
            let! alternatives = client.departures (U2.Case1 station) None
            execute action alternatives
            printSimpleJson (HafasClientTypesDump.dumpAlternatives alternatives)

        with ex -> eprintf "printDepartures error: %s %s" ex.Message ex.StackTrace
    }
    |> ignore

let printReachableFrom (client: HafasClient) name action =
    promise {
        try
            let! stations = client.locations name (Some defaultLocationOptions)

            let location =
                match stations.[0] with
                | Location location -> Some location
                | Stop stop -> stop.location
                | Station station -> station.location

            match location with
            | Some location ->
                location.address <- Some "dummy"
                let! durations = client.reachableFrom location None
                execute action durations
                printSimpleJson (HafasClientTypesDump.dumpDurations durations)
            | None -> ()

        with ex -> eprintf "printJourneyInfos error: %s %s" ex.Message ex.StackTrace
    }
    |> ignore

let printStop (client: HafasClient) id action =
    promise {
        let! stop = client.stop id None
        execute action stop
        printSimpleJson (HafasClientTypesDump.dumpStop stop)
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
            | _ -> return new System.Collections.Generic.List<Journey>() :> ReadonlyArray<Journey>
        with ex ->
            eprintf "journeys error: %s" ex.Message
            return new System.Collections.Generic.List<Journey>() :> ReadonlyArray<Journey>
    }

let printJourneys (client: HafasClient) fromStation toStation action =
    promise {
        try
            let! journeys = journeys client fromStation toStation
            execute action journeys
            printSimpleJson (HafasClientTypesDump.dumpJourneys journeys)

        with ex -> eprintf "printJourneyInfos error: %s %s" ex.Message ex.StackTrace
    }
    |> ignore

let printTrip (client: HafasClient) tripId action =
    promise {
        try
            let! trip = client.trip tripId "dummy" None
            execute action trip
            printSimpleJson (HafasClientTypesDump.dumpTrip trip)

        with ex -> eprintf "printTrip error: %s %s" ex.Message ex.StackTrace
    }
    |> ignore

let printRadar (client: HafasClient) n w s e action =
    promise {
        try
            let box = createBoundingBox (n |> float) (w |> float) (s |> float) (e |> float)
            let! movements = client.radar box (Some defaultRadarOptions)
            execute action movements
            printSimpleJson (HafasClientTypesDump.dumMovements movements)

        with ex -> eprintf "printRadar error: %s %s" ex.Message ex.StackTrace
    }
    |> ignore

let toProfile (s: string) =
    match FSharpType.GetUnionCases typedefof<Profile>
          |> Array.filter (fun case -> case.Name = s) with
    | [| case |] -> Some(FSharpValue.MakeUnion(case, [||]) :?> Profile)
    | _ -> None

[<EntryPoint>]
let main argv =
    if argv.Length > 2 then
        match toProfile argv.[0] with
        | Some p ->
            let client = HafasClient.createHafasClient p "client"
            let check = "check-"
            let stripped = argv.[1].Replace(check, "")

            let op =
                match stripped with
                | "journeys" when argv.Length = 4 -> printJourneys client argv.[2] argv.[3]
                | "trip" when argv.Length = 3 -> printTrip client argv.[2]
                | "locations" when argv.Length = 3 -> printLocations client argv.[2]
                | "locations" when argv.Length = 3 -> printLocations client argv.[2]
                | "departures" when argv.Length = 3 -> printDepartures client argv.[2]
                | "reachableFrom" when argv.Length = 3 -> printReachableFrom client argv.[2]
                | "radar" when argv.Length = 6 -> printRadar client argv.[2] argv.[3] argv.[4] argv.[5]
                | _ ->
                    eprintfn "expected 'journeys x y|trip x|locations x|departures x|reachableFrom x'"
                    ignore

            if argv.[1].StartsWith(check) then Some(printRawJson) else None
            |> op

        | None -> eprintfn "expected profile"
    0
