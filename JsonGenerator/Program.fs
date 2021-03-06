
open System
open HafasClientTypes.CreateClient
open Fable.SimpleJson 

let intro =  """// JsonGenerator 0.0.0
module HafasClientTypesDump

open HafasClientTypes
open HafasClientTypes.CreateClient
open Fable.Core
open Fable.Core.JsInterop
open Fable.SimpleJson
open System.Text

//  obj?``type``
let (|Station|_|)  obj = 
    if obj?``type`` = "station" then Some (Station(unbox obj)) else None

let (|Stop|_|)  obj = 
    if obj?``type`` = "stop" then Some (Stop(unbox obj)) else None

let (|Location|_|)  obj = 
    if obj?``type`` = "location" then Some (Location(unbox obj)) else None

let (|Hint|_|)  obj = 
    if obj?``type`` = "hint" || obj?``type`` = "status" then Some (Hint(unbox obj)) else None

let (|Warning|_|)  obj = 
    if obj?``type`` = "warning" then Some (Warning(unbox obj)) else None

// adhoc way to resolve mutual recursion
let mutable dumpStopFunc : (Stop -> Json) Option = None

let escapeString (str : string) =
   if not (isNull str) && str.Contains "\"" then
       let buf = StringBuilder(str.Length)
       let replaceOrLeave c =
          match c with
          | '"' -> buf.Append "\\\""
          | _ -> buf.Append c
       str.ToCharArray() |> Array.iter (replaceOrLeave >> ignore)
       buf.ToString()
    else
        str

let inline objectKeys (o: obj) : string seq = upcast JS.Constructors.Object.keys(o)

let dumpProducts (x: Products) =
    let json = Seq.map (fun k -> (k, JBool (x.Item(k)))) (objectKeys x)
    Seq.toList json |> Map.ofList |> JObject

let dumpIds (x: Ids) =
    let json = Seq.map (fun k -> (k, JString (x.Item(k)))) (objectKeys x)
    Seq.toList json |> Map.ofList |> JObject

let dumpScheduledDays (x: ScheduledDays) =
    let json = Seq.map (fun k -> (k, JBool (x.Item(k)))) (objectKeys x)
    Seq.toList json |> Map.ofList |> JObject
   
let dumpFacilities (x: Facilities) =
    let json =
        Seq.map (fun k ->
            (k,
             match x.Item(k) with
             | U2.Case1 s -> JString s
             | U2.Case2 b -> JBool b)) (objectKeys x)
    Seq.toList json |> Map.ofList |> JObject
"""

let finale = """
let dumpStations (stations: ReadonlyArray<Station>) =
    JArray [ for e in stations do yield dumpStation e ]

let dumpStops (stops: ReadonlyArray<Stop>) =
    JArray [ for e in stops do yield dumpStop e ]

let dumpJourneys (journeys: ReadonlyArray<Journey>) =
    JArray [ for e in journeys do yield dumpJourney e ]

let dumpDurations (durations: ReadonlyArray<Duration>) =
    JArray [ for e in durations do yield dumpDuration e ]

let dumMovements (movements: ReadonlyArray<Movement>) =
    JArray [ for e in movements do yield dumpMovement e ]

let dumpAlternatives (alternatives: ReadonlyArray<Alternative>) =
    JArray [ for e in alternatives do yield dumpAlternative e ]

let dumpU2StopsLocations (stops: ReadonlyArray<U2<Stop, Location>>) =
    JArray
        [ for e in stops do
            yield match e with
                  | Location location -> dumpLocation location
                  | Stop stop -> dumpStop stop
                  | _ -> JNull ]

let dumpU3StationsStopsLocations (stops: ReadonlyArray<U3<Station, Stop, Location>>) =
    JArray
        [ for e in stops do
            yield match e with
                  | Location location -> dumpLocation location
                  | Stop stop -> dumpStop stop
                  | Station station -> dumpStation station
                  | _ -> JNull ]

// resolve mutual recursions
let init =
    dumpStopFunc <- Some dumpStop
"""

[<EntryPoint>]
let main argv =
    printfn "%s" intro
    // type Products has fixed implementation
    Generator.generateDumpFunction typeof<Geometry>
    Generator.generateDumpFunction typeof<Price>
    Generator.generateDumpFunction typeof<Operator>
    Generator.generateDumpFunction typeof<Location>
    Generator.generateDumpFunction typeof<ReisezentrumOpeningHours>
    Generator.generateDumpFunction typeof<Station>
    Generator.generateDumpFunction typeof<Line>
    Generator.generateDumpFunction typeof<Stop>
    Generator.generateDumpFunction typeof<Feature>
    Generator.generateDumpFunction typeof<FeatureCollection>
    Generator.generateDumpFunction typeof<Region>
    Generator.generateDumpFunction typeof<Route>
    Generator.generateDumpFunction typeof<Cycle>
    Generator.generateDumpFunction typeof<ArrivalDeparture>
    Generator.generateDumpFunction typeof<Schedule>
    Generator.generateDumpFunction typeof<Hint>
    Generator.generateDumpFunction typeof<Warning>
    Generator.generateDumpFunction typeof<StopOver>
    Generator.generateDumpFunction typeof<Alternative>
    Generator.generateDumpFunction typeof<Trip>
    Generator.generateDumpFunction typeof<Leg>
    Generator.generateDumpFunction typeof<Journey>
    Generator.generateDumpFunction typeof<Duration>
    Generator.generateDumpFunction typeof<Frame>
    Generator.generateDumpFunction typeof<Movement>
    printfn "%s" finale
    0 // return an integer exit code
