module HafasClientTypesUtils

open Fable.Core
open Fable.Core.JsInterop
open HafasClientTypes

let locationOptions results = jsOptions<LocationsOptions> (fun x -> x.results <- Some(float results))

let defaultLocationOptions = locationOptions 3

let journeyOptions results departure =
    jsOptions<JourneysOptions> (fun x ->
        x.results <- Some(float results)
        x.departure <- Some(departure)
        x.stopovers <- Some(false))

let defaultJourneyOptions = journeyOptions 3 System.DateTime.Now
