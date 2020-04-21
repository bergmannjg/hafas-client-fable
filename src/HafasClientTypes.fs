// ts2fable 0.7.1
module rec HafasClientTypes
open System
open Fable.Core
open Fable.Core.JS


type [<AllowNullLiteral>] IExports =
    abstract createClient: profile: obj * userAgent: string -> HafasClient

type [<AllowNullLiteral>] Location =
    abstract ``type``: string with get, set
    abstract id: string option with get, set
    abstract name: string option with get, set
    abstract poi: bool option with get, set
    abstract address: string option with get, set
    abstract longitude: float option with get, set
    abstract latitude: float option with get, set
    abstract altitude: float option with get, set

type [<AllowNullLiteral>] Products =
    abstract nationalExpress: bool option with get, set
    abstract national: bool option with get, set
    abstract regionalExp: bool option with get, set
    abstract regional: bool option with get, set
    abstract suburban: bool option with get, set
    abstract bus: bool option with get, set
    abstract express: bool option with get, set
    abstract ferry: bool option with get, set
    abstract subway: bool option with get, set
    abstract tram: bool option with get, set
    abstract taxi: bool option with get, set

type [<AllowNullLiteral>] Station =
    abstract ``type``: string with get, set
    abstract id: string with get, set
    abstract name: string with get, set
    abstract station: Station option with get, set
    abstract location: Location option with get, set
    abstract products: Products option with get, set
    abstract regions: ResizeArray<string> option with get, set

type [<AllowNullLiteral>] Stop =
    abstract ``type``: string with get, set
    abstract id: string with get, set
    abstract station: Station option with get, set
    abstract name: string with get, set
    abstract location: Location option with get, set
    abstract products: Products with get, set

type [<AllowNullLiteral>] Region =
    abstract ``type``: string with get, set
    abstract id: string with get, set
    abstract name: string with get, set
    abstract stations: ResizeArray<string> with get, set

type [<AllowNullLiteral>] Line =
    abstract ``type``: string with get, set
    abstract id: string with get, set
    abstract name: string with get, set
    abstract adminCode: string option with get, set
    abstract fahrtNr: string option with get, set
    abstract product: string option with get, set
    abstract ``public``: bool option with get, set
    abstract mode: LineMode with get, set
    abstract routes: ResizeArray<string> option with get, set
    abstract operator: Operator option with get, set

type [<AllowNullLiteral>] Route =
    abstract ``type``: string with get, set
    abstract id: string with get, set
    abstract line: string with get, set
    abstract mode: LineMode with get, set
    abstract stops: ResizeArray<string> with get, set

type [<AllowNullLiteral>] ArrivalDeparture =
    abstract arrival: float option with get, set
    abstract departure: float option with get, set

type [<AllowNullLiteral>] Schedule =
    abstract ``type``: string with get, set
    abstract id: string with get, set
    abstract route: string with get, set
    abstract mode: LineMode with get, set
    abstract sequence: ResizeArray<ArrivalDeparture> with get, set
    abstract starts: ResizeArray<string> with get, set

type [<AllowNullLiteral>] Operator =
    abstract ``type``: string with get, set
    abstract id: string with get, set
    abstract name: string with get, set

type [<AllowNullLiteral>] Hint =
    abstract ``type``: string with get, set
    abstract code: string with get, set
    abstract summary: string option with get, set
    abstract text: string with get, set

type [<AllowNullLiteral>] StopOver =
    abstract stop: U2<Station, Stop> with get, set
    abstract departure: string option with get, set
    abstract departureDelay: float option with get, set
    abstract plannedDeparture: string option with get, set
    abstract departurePlatform: string option with get, set
    abstract plannedDeparturePlatform: string option with get, set
    abstract arrival: string option with get, set
    abstract arrivalDelay: float option with get, set
    abstract plannedArrival: string option with get, set
    abstract arrivalPlatform: string option with get, set
    abstract plannedArrivalPlatform: string option with get, set
    abstract remarks: ResizeArray<Hint> option with get, set

type [<AllowNullLiteral>] Trip =
    abstract id: string with get, set
    abstract origin: Stop with get, set
    abstract departure: string with get, set
    abstract departurePlatform: string option with get, set
    abstract plannedDeparture: string with get, set
    abstract plannedDeparturePlatform: string option with get, set
    abstract departureDelay: float option with get, set
    abstract destination: Stop with get, set
    abstract arrival: string with get, set
    abstract arrivalPlatform: string option with get, set
    abstract plannedArrival: string with get, set
    abstract plannedArrivalPlatform: string option with get, set
    abstract arrivalDelay: float option with get, set
    abstract stopovers: ResizeArray<StopOver> with get, set
    abstract remarks: ResizeArray<Hint> option with get, set
    abstract line: Line option with get, set
    abstract direction: string option with get, set
    abstract reachable: bool option with get, set

type [<AllowNullLiteral>] Price =
    abstract amount: float with get, set
    abstract currency: string with get, set
    abstract hint: string option with get, set

type [<AllowNullLiteral>] Leg =
    abstract tripId: string option with get, set
    abstract origin: U2<Station, Stop> with get, set
    abstract destination: U2<Station, Stop> with get, set
    abstract departure: string with get, set
    abstract plannedDeparture: string with get, set
    abstract departureDelay: float option with get, set
    abstract departurePlatform: string option with get, set
    abstract plannedDeparturePlatform: string option with get, set
    abstract arrival: string with get, set
    abstract plannedArrival: string with get, set
    abstract arrivalDelay: float option with get, set
    abstract arrivalPlatform: string option with get, set
    abstract plannedArrivalPlatform: string option with get, set
    abstract stopovers: ResizeArray<StopOver> option with get, set
    abstract schedule: float option with get, set
    abstract price: Price option with get, set
    abstract operator: float option with get, set
    abstract direction: string option with get, set
    abstract line: Line option with get, set
    abstract reachable: bool option with get, set
    abstract cancelled: bool option with get, set
    abstract walking: bool option with get, set
    abstract loadFactor: string option with get, set

type [<AllowNullLiteral>] Journey =
    abstract ``type``: string with get, set
    abstract legs: ResizeArray<Leg> with get, set
    abstract refreshToken: string option with get, set
    abstract price: Price option with get, set

type [<AllowNullLiteral>] Journeys =
    abstract journeys: ResizeArray<Journey> with get, set

type [<AllowNullLiteral>] Duration =
    abstract duration: float with get, set
    abstract stations: ResizeArray<U2<Station, Stop>> with get, set

type [<AllowNullLiteral>] JourneysOptions =
    abstract departure: DateTime option with get, set
    abstract arrival: DateTime option with get, set
    abstract results: float option with get, set
    abstract via: string option with get, set
    abstract stopovers: bool option with get, set
    abstract transfers: float option with get, set
    abstract transferTime: float option with get, set
    abstract accessibility: string option with get, set
    abstract bike: bool option with get, set
    abstract products: Products option with get, set
    abstract tickets: bool option with get, set
    abstract polylines: bool option with get, set
    abstract remarks: bool option with get, set
    abstract walkingSpeed: string option with get, set
    abstract startWithWalking: bool option with get, set
    abstract language: string option with get, set
    abstract scheduledDays: bool option with get, set

type [<AllowNullLiteral>] LocationsOptions =
    abstract fuzzy: bool option with get, set
    abstract results: float option with get, set
    abstract stops: bool option with get, set
    abstract addresses: bool option with get, set
    abstract poi: bool option with get, set
    abstract linesOfStops: bool option with get, set
    abstract language: string option with get, set

type [<AllowNullLiteral>] TripOptions =
    abstract stopovers: bool option with get, set
    abstract polyline: bool option with get, set
    abstract remarks: bool option with get, set
    abstract language: string option with get, set

type [<AllowNullLiteral>] StopOptions =
    abstract linesOfStops: bool option with get, set
    abstract language: string option with get, set

type [<AllowNullLiteral>] DeparturesArrivalsOptions =
    abstract ``when``: DateTime option with get, set
    abstract direction: string option with get, set
    abstract duration: float option with get, set
    abstract results: float option with get, set
    abstract linesOfStops: bool option with get, set
    abstract remarks: bool option with get, set
    abstract stopovers: bool option with get, set
    abstract includeRelatedStations: bool option with get, set
    abstract language: string option with get, set

type [<AllowNullLiteral>] RefreshJourneyOptions =
    abstract stopovers: bool option with get, set
    abstract polylines: bool option with get, set
    abstract tickets: bool option with get, set
    abstract remarks: bool option with get, set
    abstract language: string option with get, set

type [<AllowNullLiteral>] NearByOptions =
    abstract results: float option with get, set
    abstract distance: float option with get, set
    abstract poi: bool option with get, set
    abstract stops: bool option with get, set
    abstract linesOfStops: bool option with get, set
    abstract language: string option with get, set

type [<AllowNullLiteral>] ReachableFromOptions =
    abstract ``when``: DateTime option with get, set
    abstract maxTransfers: float option with get, set
    abstract maxDuration: float option with get, set
    abstract products: Products option with get, set

type [<AllowNullLiteral>] HafasClient =
    abstract journeys: (U3<string, Station, Location> -> U3<string, Station, Location> -> JourneysOptions option -> Promise<Journeys>) with get, set
    abstract refreshJourney: (string -> RefreshJourneyOptions option -> Promise<Journey>) with get, set
    abstract trip: (string -> string -> TripOptions option -> Promise<Trip>) with get, set
    abstract departures: (U2<string, Station> -> DeparturesArrivalsOptions option -> Promise<Journeys>) with get, set
    abstract arrivals: (U2<string, Station> -> DeparturesArrivalsOptions option -> Promise<Journeys>) with get, set
    abstract locations: (string -> LocationsOptions option -> Promise<ResizeArray<U2<Stop, Location>>>) with get, set
    abstract stop: (string -> StopOptions option -> Promise<Stop>) with get, set
    abstract nearBy: (Location -> NearByOptions option -> Promise<Stop>) with get, set
    abstract reachableFrom: (Location -> ReachableFromOptions option -> Promise<ResizeArray<Duration>>) with get, set

type [<StringEnum>] [<RequireQualifiedAccess>] Profile =
    | Bvg
    | Cmta
    | DbBusradarNrw
    | Insa
    | Nahsh
    | Oebb
    | Rsag
    | SBahnMunich
    | Vbb
    | Vmt
    | Cfl
    | Db
    | Hvv
    | Invg
    | Nvv
    | Pkp
    | Rmv
    | Saarfahrplan
    | Sncb
    | Vbn
    | Vsn

type [<StringEnum>] [<RequireQualifiedAccess>] LineMode =
    | Train
    | Bus
    | Watercraft
    | Taxi
    | Gondola
    | Aircraft
    | Car
    | Bicycle
    | Walking
