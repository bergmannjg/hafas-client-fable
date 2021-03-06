// JsonGenerator 0.0.0
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

let dumpGeometry (x: Geometry) =
    [
    "type",JString (escapeString x.``type``) 
    "coordinates",JArray [ for e in x.coordinates do yield JNumber  e ] 
    ] |> Map.ofList |> JObject

let dumpPrice (x: Price) =
    [
    "amount",JNumber  x.amount 
    "currency",JString (escapeString x.currency) 
    "hint", match x.hint with  Some v -> JString (escapeString v)  | None -> JNull 
    ] |> Map.ofList |> JObject

let dumpOperator (x: Operator) =
    [
    "type",JString (escapeString x.``type``) 
    "id",JString (escapeString x.id) 
    "name",JString (escapeString x.name) 
    ] |> Map.ofList |> JObject

let dumpLocation (x: Location) =
    [
    "type",JString (escapeString x.``type``) 
    "id", match x.id with  Some v -> JString (escapeString v)  | None -> JNull 
    "name", match x.name with  Some v -> JString (escapeString v)  | None -> JNull 
    "poi", match x.poi with  Some v -> JBool  v  | None -> JNull 
    "address", match x.address with  Some v -> JString (escapeString v)  | None -> JNull 
    "longitude", match x.longitude with  Some v -> JNumber  v  | None -> JNull 
    "latitude", match x.latitude with  Some v -> JNumber  v  | None -> JNull 
    "altitude", match x.altitude with  Some v -> JNumber  v  | None -> JNull 
    "distance", match x.distance with  Some v -> JNumber  v  | None -> JNull 
    ] |> Map.ofList |> JObject

let dumpReisezentrumOpeningHours (x: ReisezentrumOpeningHours) =
    [
    "Mo", match x.Mo with  Some v -> JString (escapeString v)  | None -> JNull 
    "Di", match x.Di with  Some v -> JString (escapeString v)  | None -> JNull 
    "Mi", match x.Mi with  Some v -> JString (escapeString v)  | None -> JNull 
    "Do", match x.Do with  Some v -> JString (escapeString v)  | None -> JNull 
    "Fr", match x.Fr with  Some v -> JString (escapeString v)  | None -> JNull 
    "Sa", match x.Sa with  Some v -> JString (escapeString v)  | None -> JNull 
    "So", match x.So with  Some v -> JString (escapeString v)  | None -> JNull 
    ] |> Map.ofList |> JObject

let rec dumpStation (x: Station) =
    [
    "type",JString (escapeString x.``type``) 
    "id", match x.id with  Some v -> JString (escapeString v)  | None -> JNull 
    "name", match x.name with  Some v -> JString (escapeString v)  | None -> JNull 
    "station", match x.station with  Some v -> dumpStation v  | None -> JNull 
    "location", match x.location with  Some v -> dumpLocation v  | None -> JNull 
    "products", match x.products with  Some v -> dumpProducts v  | None -> JNull 
    "isMeta", match x.isMeta with  Some v -> JBool  v  | None -> JNull 
    "regions", match x.regions with  Some v -> JArray [ for e in v do yield JString (escapeString e) ]  | None -> JNull 
    "facilities", match x.facilities with  Some v -> dumpFacilities v  | None -> JNull 
    "reisezentrumOpeningHours", match x.reisezentrumOpeningHours with  Some v -> dumpReisezentrumOpeningHours v  | None -> JNull 
    "stops", match x.stops with  Some v -> JArray [ for e in v do yield ( match e with | Station s -> dumpStation  s  | Stop s -> (match dumpStopFunc with | Some f -> f  s  | None -> JNull)| Location s -> dumpLocation  s  | _ -> JNull )]  | None -> JNull 
    "entrances", match x.entrances with  Some v -> JArray [ for e in v do yield dumpLocation e ]  | None -> JNull 
    "transitAuthority", match x.transitAuthority with  Some v -> JString (escapeString v)  | None -> JNull 
    "distance", match x.distance with  Some v -> JNumber  v  | None -> JNull 
    ] |> Map.ofList |> JObject

let dumpLine (x: Line) =
    [
    "type",JString (escapeString x.``type``) 
    "id", match x.id with  Some v -> JString (escapeString v)  | None -> JNull 
    "name", match x.name with  Some v -> JString (escapeString v)  | None -> JNull 
    "adminCode", match x.adminCode with  Some v -> JString (escapeString v)  | None -> JNull 
    "fahrtNr", match x.fahrtNr with  Some v -> JString (escapeString v)  | None -> JNull 
    "additionalName", match x.additionalName with  Some v -> JString (escapeString v)  | None -> JNull 
    "product", match x.product with  Some v -> JString (escapeString v)  | None -> JNull 
    "public", match x.``public`` with  Some v -> JBool  v  | None -> JNull 
    "mode", match x.mode with  Some v -> JString (v.ToString())  | None -> JNull 
    "routes", match x.routes with  Some v -> JArray [ for e in v do yield JString (escapeString e) ]  | None -> JNull 
    "operator", match x.operator with  Some v -> dumpOperator v  | None -> JNull 
    "express", match x.express with  Some v -> JBool  v  | None -> JNull 
    "metro", match x.metro with  Some v -> JBool  v  | None -> JNull 
    "night", match x.night with  Some v -> JBool  v  | None -> JNull 
    "nr", match x.nr with  Some v -> JNumber  v  | None -> JNull 
    "symbol", match x.symbol with  Some v -> JString (escapeString v)  | None -> JNull 
    ] |> Map.ofList |> JObject

let dumpStop (x: Stop) =
    [
    "type",JString (escapeString x.``type``) 
    "id",JString (escapeString x.id) 
    "name", match x.name with  Some v -> JString (escapeString v)  | None -> JNull 
    "station", match x.station with  Some v -> dumpStation v  | None -> JNull 
    "location", match x.location with  Some v -> dumpLocation v  | None -> JNull 
    "products", match x.products with  Some v -> dumpProducts v  | None -> JNull 
    "lines", match x.lines with  Some v -> JArray [ for e in v do yield dumpLine e ]  | None -> JNull 
    "isMeta", match x.isMeta with  Some v -> JBool  v  | None -> JNull 
    "reisezentrumOpeningHours", match x.reisezentrumOpeningHours with  Some v -> dumpReisezentrumOpeningHours v  | None -> JNull 
    "ids", match x.ids with  Some v -> dumpIds v  | None -> JNull 
    "loadFactor", match x.loadFactor with  Some v -> JString (escapeString v)  | None -> JNull 
    "entrances", match x.entrances with  Some v -> JArray [ for e in v do yield dumpLocation e ]  | None -> JNull 
    "transitAuthority", match x.transitAuthority with  Some v -> JString (escapeString v)  | None -> JNull 
    "distance", match x.distance with  Some v -> JNumber  v  | None -> JNull 
    ] |> Map.ofList |> JObject

let dumpFeature (x: Feature) =
    [
    "type",JString (escapeString x.``type``) 
    "properties", match x.properties with  Some v -> ( match v with | Station s -> dumpStation  s  | Stop s -> (match dumpStopFunc with | Some f -> f  s  | None -> JNull)| U3.Case3 s -> JNull | _ -> JNull ) | None -> JNull 
    "geometry",dumpGeometry x.geometry 
    ] |> Map.ofList |> JObject

let dumpFeatureCollection (x: FeatureCollection) =
    [
    "type",JString (escapeString x.``type``) 
    "features",JArray [ for e in x.features do yield dumpFeature e ] 
    ] |> Map.ofList |> JObject

let dumpRegion (x: Region) =
    [
    "type",JString (escapeString x.``type``) 
    "id",JString (escapeString x.id) 
    "name",JString (escapeString x.name) 
    "stations",JArray [ for e in x.stations do yield JString (escapeString e) ] 
    ] |> Map.ofList |> JObject

let dumpRoute (x: Route) =
    [
    "type",JString (escapeString x.``type``) 
    "id",JString (escapeString x.id) 
    "line",JString (escapeString x.line) 
    "mode",JString (x.mode.ToString()) 
    "stops",JArray [ for e in x.stops do yield JString (escapeString e) ] 
    ] |> Map.ofList |> JObject

let dumpCycle (x: Cycle) =
    [
    "min", match x.min with  Some v -> JNumber  v  | None -> JNull 
    "max", match x.max with  Some v -> JNumber  v  | None -> JNull 
    "nr", match x.nr with  Some v -> JNumber  v  | None -> JNull 
    ] |> Map.ofList |> JObject

let dumpArrivalDeparture (x: ArrivalDeparture) =
    [
    "arrival", match x.arrival with  Some v -> JNumber  v  | None -> JNull 
    "departure", match x.departure with  Some v -> JNumber  v  | None -> JNull 
    ] |> Map.ofList |> JObject

let dumpSchedule (x: Schedule) =
    [
    "type",JString (escapeString x.``type``) 
    "id",JString (escapeString x.id) 
    "route",JString (escapeString x.route) 
    "mode",JString (x.mode.ToString()) 
    "sequence",JArray [ for e in x.sequence do yield dumpArrivalDeparture e ] 
    "starts",JArray [ for e in x.starts do yield JString (escapeString e) ] 
    ] |> Map.ofList |> JObject

let dumpHint (x: Hint) =
    [
    "type",JString (x.``type``.ToString()) 
    "code", match x.code with  Some v -> JString (escapeString v)  | None -> JNull 
    "summary", match x.summary with  Some v -> JString (escapeString v)  | None -> JNull 
    "text",JString (escapeString x.text) 
    "tripId", match x.tripId with  Some v -> JString (escapeString v)  | None -> JNull 
    ] |> Map.ofList |> JObject

let dumpWarning (x: Warning) =
    [
    "type",JString (x.``type``.ToString()) 
    "id", match x.id with  Some v -> JNumber  v  | None -> JNull 
    "icon", match x.icon with  Some v -> JString (escapeString v)  | None -> JNull 
    "summary", match x.summary with  Some v -> JString (escapeString v)  | None -> JNull 
    "text",JString (escapeString x.text) 
    "category", match x.category with  Some v -> JString (escapeString v)  | None -> JNull 
    "priority", match x.priority with  Some v -> JNumber  v  | None -> JNull 
    "products", match x.products with  Some v -> dumpProducts v  | None -> JNull 
    "edges", match x.edges with  Some v -> JArray [ for e in v do yield  match e with  Some v -> JString (v.ToString())  | None -> JNull ]  | None -> JNull 
    "events", match x.events with  Some v -> JArray [ for e in v do yield  match e with  Some v -> JString (v.ToString())  | None -> JNull ]  | None -> JNull 
    "validFrom", match x.validFrom with  Some v -> JString (escapeString v)  | None -> JNull 
    "validUntil", match x.validUntil with  Some v -> JString (escapeString v)  | None -> JNull 
    "modified", match x.modified with  Some v -> JString (escapeString v)  | None -> JNull 
    ] |> Map.ofList |> JObject

let dumpStopOver (x: StopOver) =
    [
    "stop",( match x.stop with | Station s -> dumpStation  s  | Stop s -> (match dumpStopFunc with | Some f -> f  s  | None -> JNull)| _ -> JNull )
    "departure", match x.departure with  Some v -> JString (escapeString v)  | None -> JNull 
    "departureDelay", match x.departureDelay with  Some v -> JNumber  v  | None -> JNull 
    "prognosedDeparture", match x.prognosedDeparture with  Some v -> JString (escapeString v)  | None -> JNull 
    "plannedDeparture", match x.plannedDeparture with  Some v -> JString (escapeString v)  | None -> JNull 
    "departurePlatform", match x.departurePlatform with  Some v -> JString (escapeString v)  | None -> JNull 
    "prognosedDeparturePlatform", match x.prognosedDeparturePlatform with  Some v -> JString (escapeString v)  | None -> JNull 
    "plannedDeparturePlatform", match x.plannedDeparturePlatform with  Some v -> JString (escapeString v)  | None -> JNull 
    "arrival", match x.arrival with  Some v -> JString (escapeString v)  | None -> JNull 
    "arrivalDelay", match x.arrivalDelay with  Some v -> JNumber  v  | None -> JNull 
    "prognosedArrival", match x.prognosedArrival with  Some v -> JString (escapeString v)  | None -> JNull 
    "plannedArrival", match x.plannedArrival with  Some v -> JString (escapeString v)  | None -> JNull 
    "arrivalPlatform", match x.arrivalPlatform with  Some v -> JString (escapeString v)  | None -> JNull 
    "prognosedArrivalPlatform", match x.prognosedArrivalPlatform with  Some v -> JString (escapeString v)  | None -> JNull 
    "plannedArrivalPlatform", match x.plannedArrivalPlatform with  Some v -> JString (escapeString v)  | None -> JNull 
    "remarks", match x.remarks with  Some v -> JArray [ for e in v do yield ( match e with | Hint s -> dumpHint  s  | Warning s -> dumpWarning  s  | _ -> JNull )]  | None -> JNull 
    "passBy", match x.passBy with  Some v -> JBool  v  | None -> JNull 
    "cancelled", match x.cancelled with  Some v -> JBool  v  | None -> JNull 
    ] |> Map.ofList |> JObject

let dumpAlternative (x: Alternative) =
    [
    "tripId",JString (escapeString x.tripId) 
    "direction", match x.direction with  Some v -> JString (escapeString v)  | None -> JNull 
    "line", match x.line with  Some v -> dumpLine v  | None -> JNull 
    "stop", match x.stop with  Some v -> ( match v with | Station s -> dumpStation  s  | Stop s -> (match dumpStopFunc with | Some f -> f  s  | None -> JNull)| _ -> JNull ) | None -> JNull 
    "when", match x.``when`` with  Some v -> JString (escapeString v)  | None -> JNull 
    "plannedWhen", match x.plannedWhen with  Some v -> JString (escapeString v)  | None -> JNull 
    "prognosedWhen", match x.prognosedWhen with  Some v -> JString (escapeString v)  | None -> JNull 
    "delay", match x.delay with  Some v -> JNumber  v  | None -> JNull 
    "platform", match x.platform with  Some v -> JString (escapeString v)  | None -> JNull 
    "plannedPlatform", match x.plannedPlatform with  Some v -> JString (escapeString v)  | None -> JNull 
    "prognosedPlatform", match x.prognosedPlatform with  Some v -> JString (escapeString v)  | None -> JNull 
    "remarks", match x.remarks with  Some v -> JArray [ for e in v do yield ( match e with | Hint s -> dumpHint  s  | Warning s -> dumpWarning  s  | _ -> JNull )]  | None -> JNull 
    "cancelled", match x.cancelled with  Some v -> JBool  v  | None -> JNull 
    "loadFactor", match x.loadFactor with  Some v -> JString (escapeString v)  | None -> JNull 
    "provenance", match x.provenance with  Some v -> JString (escapeString v)  | None -> JNull 
    "previousStopovers", match x.previousStopovers with  Some v -> JArray [ for e in v do yield dumpStopOver e ]  | None -> JNull 
    "nextStopovers", match x.nextStopovers with  Some v -> JArray [ for e in v do yield dumpStopOver e ]  | None -> JNull 
    ] |> Map.ofList |> JObject

let dumpTrip (x: Trip) =
    [
    "id",JString (escapeString x.id) 
    "origin",( match x.origin with | Station s -> dumpStation  s  | Stop s -> (match dumpStopFunc with | Some f -> f  s  | None -> JNull)| _ -> JNull )
    "destination",( match x.destination with | Station s -> dumpStation  s  | Stop s -> (match dumpStopFunc with | Some f -> f  s  | None -> JNull)| _ -> JNull )
    "departure", match x.departure with  Some v -> JString (escapeString v)  | None -> JNull 
    "plannedDeparture", match x.plannedDeparture with  Some v -> JString (escapeString v)  | None -> JNull 
    "prognosedArrival", match x.prognosedArrival with  Some v -> JString (escapeString v)  | None -> JNull 
    "departureDelay", match x.departureDelay with  Some v -> JNumber  v  | None -> JNull 
    "departurePlatform", match x.departurePlatform with  Some v -> JString (escapeString v)  | None -> JNull 
    "prognosedDeparturePlatform", match x.prognosedDeparturePlatform with  Some v -> JString (escapeString v)  | None -> JNull 
    "plannedDeparturePlatform", match x.plannedDeparturePlatform with  Some v -> JString (escapeString v)  | None -> JNull 
    "arrival", match x.arrival with  Some v -> JString (escapeString v)  | None -> JNull 
    "plannedArrival", match x.plannedArrival with  Some v -> JString (escapeString v)  | None -> JNull 
    "prognosedDeparture", match x.prognosedDeparture with  Some v -> JString (escapeString v)  | None -> JNull 
    "arrivalDelay", match x.arrivalDelay with  Some v -> JNumber  v  | None -> JNull 
    "arrivalPlatform", match x.arrivalPlatform with  Some v -> JString (escapeString v)  | None -> JNull 
    "prognosedArrivalPlatform", match x.prognosedArrivalPlatform with  Some v -> JString (escapeString v)  | None -> JNull 
    "plannedArrivalPlatform", match x.plannedArrivalPlatform with  Some v -> JString (escapeString v)  | None -> JNull 
    "stopovers", match x.stopovers with  Some v -> JArray [ for e in v do yield dumpStopOver e ]  | None -> JNull 
    "schedule", match x.schedule with  Some v -> JNumber  v  | None -> JNull 
    "price", match x.price with  Some v -> dumpPrice v  | None -> JNull 
    "operator", match x.operator with  Some v -> JNumber  v  | None -> JNull 
    "direction", match x.direction with  Some v -> JString (escapeString v)  | None -> JNull 
    "line", match x.line with  Some v -> dumpLine v  | None -> JNull 
    "reachable", match x.reachable with  Some v -> JBool  v  | None -> JNull 
    "cancelled", match x.cancelled with  Some v -> JBool  v  | None -> JNull 
    "walking", match x.walking with  Some v -> JBool  v  | None -> JNull 
    "loadFactor", match x.loadFactor with  Some v -> JString (escapeString v)  | None -> JNull 
    "distance", match x.distance with  Some v -> JNumber  v  | None -> JNull 
    "public", match x.``public`` with  Some v -> JBool  v  | None -> JNull 
    "transfer", match x.transfer with  Some v -> JBool  v  | None -> JNull 
    "cycle", match x.cycle with  Some v -> dumpCycle v  | None -> JNull 
    "alternatives", match x.alternatives with  Some v -> JArray [ for e in v do yield dumpAlternative e ]  | None -> JNull 
    "polyline", match x.polyline with  Some v -> dumpFeatureCollection v  | None -> JNull 
    "remarks", match x.remarks with  Some v -> JArray [ for e in v do yield ( match e with | Hint s -> dumpHint  s  | Warning s -> dumpWarning  s  | _ -> JNull )]  | None -> JNull 
    ] |> Map.ofList |> JObject

let dumpLeg (x: Leg) =
    [
    "tripId", match x.tripId with  Some v -> JString (escapeString v)  | None -> JNull 
    "origin",( match x.origin with | Station s -> dumpStation  s  | Stop s -> (match dumpStopFunc with | Some f -> f  s  | None -> JNull)| _ -> JNull )
    "destination",( match x.destination with | Station s -> dumpStation  s  | Stop s -> (match dumpStopFunc with | Some f -> f  s  | None -> JNull)| _ -> JNull )
    "departure", match x.departure with  Some v -> JString (escapeString v)  | None -> JNull 
    "plannedDeparture", match x.plannedDeparture with  Some v -> JString (escapeString v)  | None -> JNull 
    "prognosedArrival", match x.prognosedArrival with  Some v -> JString (escapeString v)  | None -> JNull 
    "departureDelay", match x.departureDelay with  Some v -> JNumber  v  | None -> JNull 
    "departurePlatform", match x.departurePlatform with  Some v -> JString (escapeString v)  | None -> JNull 
    "prognosedDeparturePlatform", match x.prognosedDeparturePlatform with  Some v -> JString (escapeString v)  | None -> JNull 
    "plannedDeparturePlatform", match x.plannedDeparturePlatform with  Some v -> JString (escapeString v)  | None -> JNull 
    "arrival", match x.arrival with  Some v -> JString (escapeString v)  | None -> JNull 
    "plannedArrival", match x.plannedArrival with  Some v -> JString (escapeString v)  | None -> JNull 
    "prognosedDeparture", match x.prognosedDeparture with  Some v -> JString (escapeString v)  | None -> JNull 
    "arrivalDelay", match x.arrivalDelay with  Some v -> JNumber  v  | None -> JNull 
    "arrivalPlatform", match x.arrivalPlatform with  Some v -> JString (escapeString v)  | None -> JNull 
    "prognosedArrivalPlatform", match x.prognosedArrivalPlatform with  Some v -> JString (escapeString v)  | None -> JNull 
    "plannedArrivalPlatform", match x.plannedArrivalPlatform with  Some v -> JString (escapeString v)  | None -> JNull 
    "stopovers", match x.stopovers with  Some v -> JArray [ for e in v do yield dumpStopOver e ]  | None -> JNull 
    "schedule", match x.schedule with  Some v -> JNumber  v  | None -> JNull 
    "price", match x.price with  Some v -> dumpPrice v  | None -> JNull 
    "operator", match x.operator with  Some v -> JNumber  v  | None -> JNull 
    "direction", match x.direction with  Some v -> JString (escapeString v)  | None -> JNull 
    "line", match x.line with  Some v -> dumpLine v  | None -> JNull 
    "reachable", match x.reachable with  Some v -> JBool  v  | None -> JNull 
    "cancelled", match x.cancelled with  Some v -> JBool  v  | None -> JNull 
    "walking", match x.walking with  Some v -> JBool  v  | None -> JNull 
    "loadFactor", match x.loadFactor with  Some v -> JString (escapeString v)  | None -> JNull 
    "distance", match x.distance with  Some v -> JNumber  v  | None -> JNull 
    "public", match x.``public`` with  Some v -> JBool  v  | None -> JNull 
    "transfer", match x.transfer with  Some v -> JBool  v  | None -> JNull 
    "cycle", match x.cycle with  Some v -> dumpCycle v  | None -> JNull 
    "alternatives", match x.alternatives with  Some v -> JArray [ for e in v do yield dumpAlternative e ]  | None -> JNull 
    "polyline", match x.polyline with  Some v -> dumpFeatureCollection v  | None -> JNull 
    "remarks", match x.remarks with  Some v -> JArray [ for e in v do yield ( match e with | Hint s -> dumpHint  s  | Warning s -> dumpWarning  s  | _ -> JNull )]  | None -> JNull 
    ] |> Map.ofList |> JObject

let dumpJourney (x: Journey) =
    [
    "type",JString (escapeString x.``type``) 
    "legs",JArray [ for e in x.legs do yield dumpLeg e ] 
    "refreshToken", match x.refreshToken with  Some v -> JString (escapeString v)  | None -> JNull 
    "remarks", match x.remarks with  Some v -> JArray [ for e in v do yield ( match e with | Hint s -> dumpHint  s  | Warning s -> dumpWarning  s  | _ -> JNull )]  | None -> JNull 
    "price", match x.price with  Some v -> dumpPrice v  | None -> JNull 
    "cycle", match x.cycle with  Some v -> dumpCycle v  | None -> JNull 
    "scheduledDays", match x.scheduledDays with  Some v -> dumpScheduledDays v  | None -> JNull 
    ] |> Map.ofList |> JObject

let dumpDuration (x: Duration) =
    [
    "duration",JNumber  x.duration 
    "stations",JArray [ for e in x.stations do yield ( match e with | Station s -> dumpStation  s  | Stop s -> (match dumpStopFunc with | Some f -> f  s  | None -> JNull)| Location s -> dumpLocation  s  | _ -> JNull )] 
    ] |> Map.ofList |> JObject

let dumpFrame (x: Frame) =
    [
    "origin",( match x.origin with | Stop s -> (match dumpStopFunc with | Some f -> f  s  | None -> JNull)| Location s -> dumpLocation  s  | _ -> JNull )
    "destination",( match x.destination with | Stop s -> (match dumpStopFunc with | Some f -> f  s  | None -> JNull)| Location s -> dumpLocation  s  | _ -> JNull )
    "t", match x.t with  Some v -> JNumber  v  | None -> JNull 
    ] |> Map.ofList |> JObject

let dumpMovement (x: Movement) =
    [
    "direction", match x.direction with  Some v -> JString (escapeString v)  | None -> JNull 
    "tripId", match x.tripId with  Some v -> JString (escapeString v)  | None -> JNull 
    "line", match x.line with  Some v -> dumpLine v  | None -> JNull 
    "location", match x.location with  Some v -> dumpLocation v  | None -> JNull 
    "nextStopovers", match x.nextStopovers with  Some v -> JArray [ for e in v do yield dumpStopOver e ]  | None -> JNull 
    "frames", match x.frames with  Some v -> JArray [ for e in v do yield dumpFrame e ]  | None -> JNull 
    "polyline", match x.polyline with  Some v -> dumpFeatureCollection v  | None -> JNull 
    ] |> Map.ofList |> JObject


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

