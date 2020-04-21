module HafasClientTypesUtils

open Fable.Core
open Fable.Core.JsInterop
open HafasClientTypes

let (|Station|Stop|) (obj: U2<Station, Stop>): Choice<Station, Stop> =
    match obj?``type`` with
    | "station" -> Station(unbox obj)
    | "stop" -> Stop(unbox obj)
    | unkown -> failwithf "Unkown ``type`` value: `%s`" unkown

let (|Stop|Location|) (obj: U2<Stop, Location>): Choice<Stop, Location> =
    match obj?``type`` with
    | "location" -> Location(unbox obj)
    | "stop" -> Stop(unbox obj)
    | unkown -> failwithf "Unkown ``type`` value: `%s`" unkown

let locationOptions results = jsOptions<LocationsOptions> (fun x -> x.results <- Some(float results))

let defaultLocationOptions = locationOptions 3

let journeyOptions results departure =
    jsOptions<JourneysOptions> (fun x ->
        x.results <- Some(float results)
        x.departure <- Some(departure)
        x.stopovers <- Some(false))

let defaultJourneyOptions = journeyOptions 3 System.DateTime.Now
