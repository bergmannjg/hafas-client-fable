
open System
open HafasClientTypes
open Fable.SimpleJson 

let intro =  """// DumpGenerator 0.0.0
module HafasClientTypesDump

open HafasClientTypes
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

let escapeString (str : string) =
   if str <> null && str.Contains "\"" then
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
    Seq.toList json
    |> Map.ofList
    |> JObject
 """

let finale = """
let dumpStations (stations: ResizeArray<Station>) =
    JArray [ for e in stations do yield dumpStation e ]

let dumpStops (stops: ResizeArray<Stop>) =
    JArray [ for e in stops do yield dumpStop e ]

let dumpJourneys (journeys: ResizeArray<Journey>) =
    JArray [ for e in journeys do yield dumpJourney e ]

let dumpDurations (durations: ResizeArray<Duration>) =
    JArray [ for e in durations do yield dumpDuration e ]

let dumpU2StopsLocations (stops: ResizeArray<U2<Stop, Location>>) =
    JArray
        [ for e in stops do
            yield match e with
                  | Location location -> dumpLocation location
                  | Stop stop -> dumpStop stop
                  | _ -> JNull ]

let dumpU3StationsStopsLocations (stops: ResizeArray<U3<Station, Stop, Location>>) =
    JArray
        [ for e in stops do
            yield match e with
                  | Location location -> dumpLocation location
                  | Stop stop -> dumpStop stop
                  | Station station -> dumpStation station
                  | _ -> JNull ]

"""

[<EntryPoint>]
let main argv =
    printfn "%s" intro
    // type Products has fixed implementation
    Generator.generateDumpFunction typeof<Location>
    Generator.generateDumpFunction typeof<Station>
    Generator.generateDumpFunction typeof<Stop>
    Generator.generateDumpFunction typeof<Region>
    Generator.generateDumpFunction typeof<Operator>
    Generator.generateDumpFunction typeof<Line>
    Generator.generateDumpFunction typeof<Route>
    Generator.generateDumpFunction typeof<Cycle>
    Generator.generateDumpFunction typeof<ArrivalDeparture>
    Generator.generateDumpFunction typeof<Schedule>
    Generator.generateDumpFunction typeof<Hint>
    Generator.generateDumpFunction typeof<StopOver>
    Generator.generateDumpFunction typeof<Trip>
    Generator.generateDumpFunction typeof<Price>
    Generator.generateDumpFunction typeof<Alternative>
    Generator.generateDumpFunction typeof<Leg>
    Generator.generateDumpFunction typeof<Journey>
    Generator.generateDumpFunction typeof<Duration>
    printfn "%s" finale
    0 // return an integer exit code
