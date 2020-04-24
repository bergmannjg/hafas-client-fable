// DumpGenerator 0.0.0
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
 
let dumpLocation (x: Location) =
    [
    "type",JString (escapeString x.``type``)
    "id",
    match x.id with
        | Some v -> JString (escapeString v)
        | None -> JNull
    "name",
    match x.name with
        | Some v -> JString (escapeString v)
        | None -> JNull
    "poi",
    match x.poi with
        | Some v -> JBool  v
        | None -> JNull
    "address",
    match x.address with
        | Some v -> JString (escapeString v)
        | None -> JNull
    "longitude",
    match x.longitude with
        | Some v -> JNumber  v
        | None -> JNull
    "latitude",
    match x.latitude with
        | Some v -> JNumber  v
        | None -> JNull
    "altitude",
    match x.altitude with
        | Some v -> JNumber  v
        | None -> JNull
    ]
    |> Map.ofList
    |> JObject

let dumpProducts (x: Products) =
    [
    "nationalExpress",
    match x.nationalExpress with
        | Some v -> JBool  v
        | None -> JNull
    "national",
    match x.national with
        | Some v -> JBool  v
        | None -> JNull
    "regionalExp",
    match x.regionalExp with
        | Some v -> JBool  v
        | None -> JNull
    "regional",
    match x.regional with
        | Some v -> JBool  v
        | None -> JNull
    "suburban",
    match x.suburban with
        | Some v -> JBool  v
        | None -> JNull
    "bus",
    match x.bus with
        | Some v -> JBool  v
        | None -> JNull
    "express",
    match x.express with
        | Some v -> JBool  v
        | None -> JNull
    "ferry",
    match x.ferry with
        | Some v -> JBool  v
        | None -> JNull
    "subway",
    match x.subway with
        | Some v -> JBool  v
        | None -> JNull
    "tram",
    match x.tram with
        | Some v -> JBool  v
        | None -> JNull
    "taxi",
    match x.taxi with
        | Some v -> JBool  v
        | None -> JNull
    "metro",
    match x.metro with
        | Some v -> JBool  v
        | None -> JNull
    "interregional",
    match x.interregional with
        | Some v -> JBool  v
        | None -> JNull
    "onCall",
    match x.onCall with
        | Some v -> JBool  v
        | None -> JNull
    "high-speed-train",
    match x?``high-speed-train`` with
        | Some v -> JBool  v
        | None -> JNull
    "intercity-p",
    match x?``intercity-p`` with
        | Some v -> JBool  v
        | None -> JNull
    "local-train",
    match x?``local-train`` with
        | Some v -> JBool  v
        | None -> JNull
    "s-train",
    match x?``s-train`` with
        | Some v -> JBool  v
        | None -> JNull
    "long-distance-train",
    match x?``long-distance-train`` with
        | Some v -> JBool  v
        | None -> JNull
    "regional-train",
    match x?``regional-train`` with
        | Some v -> JBool  v
        | None -> JNull
    ]
    |> Map.ofList
    |> JObject

let rec dumpStation (x: Station) =
    [
    "type",JString (escapeString x.``type``)
    "id",JString (escapeString x.id)
    "name",JString (escapeString x.name)
    "station",
    match x.station with
        | Some v -> dumpStation v
        | None -> JNull
    "location",
    match x.location with
        | Some v -> dumpLocation v
        | None -> JNull
    "products",
    match x.products with
        | Some v -> dumpProducts v
        | None -> JNull
    "isMeta",
    match x.isMeta with
        | Some v -> JBool  v
        | None -> JNull
    "regions",
    match x.regions with
        | Some v -> JArray [ for e in v do yield JString e]
        | None -> JNull
    ]
    |> Map.ofList
    |> JObject

let dumpStop (x: Stop) =
    [
    "type",JString (escapeString x.``type``)
    "id",JString (escapeString x.id)
    "station",
    match x.station with
        | Some v -> dumpStation v
        | None -> JNull
    "name",JString (escapeString x.name)
    "location",
    match x.location with
        | Some v -> dumpLocation v
        | None -> JNull
    "products",dumpProducts x.products
    "isMeta",
    match x.isMeta with
        | Some v -> JBool  v
        | None -> JNull
    ]
    |> Map.ofList
    |> JObject

let dumpRegion (x: Region) =
    [
    "type",JString (escapeString x.``type``)
    "id",JString (escapeString x.id)
    "name",JString (escapeString x.name)
    "stations",JArray [ for e in x.stations do yield JString e]
    ]
    |> Map.ofList
    |> JObject

let dumpOperator (x: Operator) =
    [
    "type",JString (escapeString x.``type``)
    "id",JString (escapeString x.id)
    "name",JString (escapeString x.name)
    ]
    |> Map.ofList
    |> JObject

let dumpLine (x: Line) =
    [
    "type",JString (escapeString x.``type``)
    "id",
    match x.id with
        | Some v -> JString (escapeString v)
        | None -> JNull
    "name",
    match x.name with
        | Some v -> JString (escapeString v)
        | None -> JNull
    "adminCode",
    match x.adminCode with
        | Some v -> JString (escapeString v)
        | None -> JNull
    "fahrtNr",
    match x.fahrtNr with
        | Some v -> JString (escapeString v)
        | None -> JNull
    "additionalName",
    match x.additionalName with
        | Some v -> JString (escapeString v)
        | None -> JNull
    "product",
    match x.product with
        | Some v -> JString (escapeString v)
        | None -> JNull
    "public",
    match x.``public`` with
        | Some v -> JBool  v
        | None -> JNull
    "mode",JString (x.mode.ToString())
    "routes",
    match x.routes with
        | Some v -> JArray [ for e in v do yield JString e]
        | None -> JNull
    "operator",
    match x.operator with
        | Some v -> dumpOperator v
        | None -> JNull
    "express",
    match x.express with
        | Some v -> JBool  v
        | None -> JNull
    "metro",
    match x.metro with
        | Some v -> JBool  v
        | None -> JNull
    "night",
    match x.night with
        | Some v -> JBool  v
        | None -> JNull
    "nr",
    match x.nr with
        | Some v -> JNumber  v
        | None -> JNull
    "symbol",
    match x.symbol with
        | Some v -> JString (escapeString v)
        | None -> JNull
    ]
    |> Map.ofList
    |> JObject

let dumpRoute (x: Route) =
    [
    "type",JString (escapeString x.``type``)
    "id",JString (escapeString x.id)
    "line",JString (escapeString x.line)
    "mode",JString (x.mode.ToString())
    "stops",JArray [ for e in x.stops do yield JString e]
    ]
    |> Map.ofList
    |> JObject

let dumpCycle (x: Cycle) =
    [
    "min",
    match x.min with
        | Some v -> JNumber  v
        | None -> JNull
    "max",
    match x.max with
        | Some v -> JNumber  v
        | None -> JNull
    "nr",
    match x.nr with
        | Some v -> JNumber  v
        | None -> JNull
    ]
    |> Map.ofList
    |> JObject

let dumpArrivalDeparture (x: ArrivalDeparture) =
    [
    "arrival",
    match x.arrival with
        | Some v -> JNumber  v
        | None -> JNull
    "departure",
    match x.departure with
        | Some v -> JNumber  v
        | None -> JNull
    ]
    |> Map.ofList
    |> JObject

let dumpSchedule (x: Schedule) =
    [
    "type",JString (escapeString x.``type``)
    "id",JString (escapeString x.id)
    "route",JString (escapeString x.route)
    "mode",JString (x.mode.ToString())
    "sequence",JArray [ for e in x.sequence do yield dumpArrivalDeparture e]
    "starts",JArray [ for e in x.starts do yield JString e]
    ]
    |> Map.ofList
    |> JObject

let dumpHint (x: Hint) =
    [
    "type",JString (escapeString x.``type``)
    "code",JString (escapeString x.code)
    "summary",
    match x.summary with
        | Some v -> JString (escapeString v)
        | None -> JNull
    "text",JString (escapeString x.text)
    ]
    |> Map.ofList
    |> JObject

let dumpStopOver (x: StopOver) =
    [
    "stop", match x.stop with | Station station -> dumpStation station | Stop stop -> dumpStop stop | _ -> JNull 
    "departure",
    match x.departure with
        | Some v -> JString (escapeString v)
        | None -> JNull
    "departureDelay",
    match x.departureDelay with
        | Some v -> JNumber  v
        | None -> JNull
    "plannedDeparture",
    match x.plannedDeparture with
        | Some v -> JString (escapeString v)
        | None -> JNull
    "departurePlatform",
    match x.departurePlatform with
        | Some v -> JString (escapeString v)
        | None -> JNull
    "plannedDeparturePlatform",
    match x.plannedDeparturePlatform with
        | Some v -> JString (escapeString v)
        | None -> JNull
    "arrival",
    match x.arrival with
        | Some v -> JString (escapeString v)
        | None -> JNull
    "arrivalDelay",
    match x.arrivalDelay with
        | Some v -> JNumber  v
        | None -> JNull
    "plannedArrival",
    match x.plannedArrival with
        | Some v -> JString (escapeString v)
        | None -> JNull
    "arrivalPlatform",
    match x.arrivalPlatform with
        | Some v -> JString (escapeString v)
        | None -> JNull
    "plannedArrivalPlatform",
    match x.plannedArrivalPlatform with
        | Some v -> JString (escapeString v)
        | None -> JNull
    "remarks",
    match x.remarks with
        | Some v -> JArray [ for e in v do yield dumpHint e]
        | None -> JNull
    ]
    |> Map.ofList
    |> JObject

let dumpTrip (x: Trip) =
    [
    "id",JString (escapeString x.id)
    "origin",dumpStop x.origin
    "departure",JString (escapeString x.departure)
    "departurePlatform",
    match x.departurePlatform with
        | Some v -> JString (escapeString v)
        | None -> JNull
    "plannedDeparture",JString (escapeString x.plannedDeparture)
    "plannedDeparturePlatform",
    match x.plannedDeparturePlatform with
        | Some v -> JString (escapeString v)
        | None -> JNull
    "departureDelay",
    match x.departureDelay with
        | Some v -> JNumber  v
        | None -> JNull
    "destination",dumpStop x.destination
    "arrival",JString (escapeString x.arrival)
    "arrivalPlatform",
    match x.arrivalPlatform with
        | Some v -> JString (escapeString v)
        | None -> JNull
    "plannedArrival",JString (escapeString x.plannedArrival)
    "plannedArrivalPlatform",
    match x.plannedArrivalPlatform with
        | Some v -> JString (escapeString v)
        | None -> JNull
    "arrivalDelay",
    match x.arrivalDelay with
        | Some v -> JNumber  v
        | None -> JNull
    "stopovers",JArray [ for e in x.stopovers do yield dumpStopOver e]
    "remarks",
    match x.remarks with
        | Some v -> JArray [ for e in v do yield dumpHint e]
        | None -> JNull
    "line",
    match x.line with
        | Some v -> dumpLine v
        | None -> JNull
    "direction",
    match x.direction with
        | Some v -> JString (escapeString v)
        | None -> JNull
    "reachable",
    match x.reachable with
        | Some v -> JBool  v
        | None -> JNull
    ]
    |> Map.ofList
    |> JObject

let dumpPrice (x: Price) =
    [
    "amount",JNumber  x.amount
    "currency",JString (escapeString x.currency)
    "hint",
    match x.hint with
        | Some v -> JString (escapeString v)
        | None -> JNull
    ]
    |> Map.ofList
    |> JObject

let dumpAlternative (x: Alternative) =
    [
    "direction",
    match x.direction with
        | Some v -> JString (escapeString v)
        | None -> JNull
    "line",
    match x.line with
        | Some v -> dumpLine v
        | None -> JNull
    "plannedWhen",
    match x.plannedWhen with
        | Some v -> JString (escapeString v)
        | None -> JNull
    "tripId",JString (escapeString x.tripId)
    "when",
    match x.``when`` with
        | Some v -> JString (escapeString v)
        | None -> JNull
    ]
    |> Map.ofList
    |> JObject

let dumpLeg (x: Leg) =
    [
    "tripId",
    match x.tripId with
        | Some v -> JString (escapeString v)
        | None -> JNull
    "origin", match x.origin with | Station station -> dumpStation station | Stop stop -> dumpStop stop | _ -> JNull 
    "destination", match x.destination with | Station station -> dumpStation station | Stop stop -> dumpStop stop | _ -> JNull 
    "departure",
    match x.departure with
        | Some v -> JString (escapeString v)
        | None -> JNull
    "plannedDeparture",JString (escapeString x.plannedDeparture)
    "departureDelay",
    match x.departureDelay with
        | Some v -> JNumber  v
        | None -> JNull
    "departurePlatform",
    match x.departurePlatform with
        | Some v -> JString (escapeString v)
        | None -> JNull
    "plannedDeparturePlatform",
    match x.plannedDeparturePlatform with
        | Some v -> JString (escapeString v)
        | None -> JNull
    "arrival",
    match x.arrival with
        | Some v -> JString (escapeString v)
        | None -> JNull
    "plannedArrival",JString (escapeString x.plannedArrival)
    "arrivalDelay",
    match x.arrivalDelay with
        | Some v -> JNumber  v
        | None -> JNull
    "arrivalPlatform",
    match x.arrivalPlatform with
        | Some v -> JString (escapeString v)
        | None -> JNull
    "plannedArrivalPlatform",
    match x.plannedArrivalPlatform with
        | Some v -> JString (escapeString v)
        | None -> JNull
    "stopovers",
    match x.stopovers with
        | Some v -> JArray [ for e in v do yield dumpStopOver e]
        | None -> JNull
    "schedule",
    match x.schedule with
        | Some v -> JNumber  v
        | None -> JNull
    "price",
    match x.price with
        | Some v -> dumpPrice v
        | None -> JNull
    "operator",
    match x.operator with
        | Some v -> JNumber  v
        | None -> JNull
    "direction",
    match x.direction with
        | Some v -> JString (escapeString v)
        | None -> JNull
    "line",
    match x.line with
        | Some v -> dumpLine v
        | None -> JNull
    "reachable",
    match x.reachable with
        | Some v -> JBool  v
        | None -> JNull
    "cancelled",
    match x.cancelled with
        | Some v -> JBool  v
        | None -> JNull
    "walking",
    match x.walking with
        | Some v -> JBool  v
        | None -> JNull
    "loadFactor",
    match x.loadFactor with
        | Some v -> JString (escapeString v)
        | None -> JNull
    "distance",
    match x.distance with
        | Some v -> JNumber  v
        | None -> JNull
    "public",
    match x.``public`` with
        | Some v -> JBool  v
        | None -> JNull
    "transfer",
    match x.transfer with
        | Some v -> JBool  v
        | None -> JNull
    "cycle",
    match x.cycle with
        | Some v -> dumpCycle v
        | None -> JNull
    "alternatives",
    match x.alternatives with
        | Some v -> JArray [ for e in v do yield dumpAlternative e]
        | None -> JNull
    ]
    |> Map.ofList
    |> JObject

let dumpJourney (x: Journey) =
    [
    "type",JString (escapeString x.``type``)
    "legs",JArray [ for e in x.legs do yield dumpLeg e]
    "refreshToken",
    match x.refreshToken with
        | Some v -> JString (escapeString v)
        | None -> JNull
    "remarks",
    match x.remarks with
        | Some v -> JArray [ for e in v do yield dumpHint e]
        | None -> JNull
    "price",
    match x.price with
        | Some v -> dumpPrice v
        | None -> JNull
    "cycle",
    match x.cycle with
        | Some v -> dumpCycle v
        | None -> JNull
    ]
    |> Map.ofList
    |> JObject

let dumpDuration (x: Duration) =
    [
    "duration",JNumber  x.duration
    "stations",JArray [ for e in x.stations do yield  match e with | Station station -> dumpStation station | Stop stop -> dumpStop stop | _ -> JNull ]
    ]
    |> Map.ofList
    |> JObject


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


