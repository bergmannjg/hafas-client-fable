module HafasClientTypesUtils

open Fable.Core
open Fable.Core.JsInterop
open HafasClientTypes.CreateClient

let (|Station|Stop|) (obj: U2<Station, Stop>): Choice<Station, Stop> =
    match obj?``type`` with
    | "station" -> Station(unbox obj)
    | "stop" -> Stop(unbox obj)
    | unkown -> failwithf "Unkown ``type`` value: `%s`" unkown

let (|Station|Stop|Location|) (obj: U3<Station, Stop, Location>): Choice<Station, Stop, Location> =
    match obj?``type`` with
    | "location" -> Location(unbox obj)
    | "stop" -> Stop(unbox obj)
    | "station" -> Station(unbox obj)
    | unkown -> failwithf "Unkown ``type`` value: `%s`" unkown

let locationOptions results = jsOptions<LocationsOptions> (fun x -> x.results <- Some(float results))

let defaultLocationOptions = locationOptions 3

let journeyOptions results departure =
    jsOptions<JourneysOptions> (fun x ->
        x.results <- Some(float results)
        x.departure <- Some(departure)
        x.stopovers <- Some(false)
        x.scheduledDays <- Some(false)
        x.polylines <- Some(false)
        x.stopovers <- Some(false))

let defaultJourneyOptions = journeyOptions 3 System.DateTime.Now

let createBoundingBox n w s e = 
    jsOptions<BoundingBox> (fun x ->
        x.north <- n
        x.west <- w
        x.south <- s
        x.east <- e)

let radarOptions results = jsOptions<RadarOptions> (fun x -> x.results <- Some(float results))

let defaultRadarOptions = radarOptions 2
