// ts2fable 0.7.1
module rec HafasClientTypes
open System
open Fable.Core
open Fable.Core.JS

type ReadonlyArray<'T> = System.Collections.Generic.IReadOnlyList<'T>

type [<AllowNullLiteral>] IExports =
    abstract createClient: profile: CreateClient.Profile * userAgent: string -> CreateClient.HafasClient

module CreateClient =

    type [<AllowNullLiteral>] ProductType =
        abstract id: string with get, set
        abstract mode: string with get, set
        abstract name: string with get, set
        abstract short: string with get, set

    type [<AllowNullLiteral>] Profile =
        abstract locale: string with get, set
        abstract products: ReadonlyArray<ProductType> with get, set
        abstract trip: bool option with get, set
        abstract radar: bool option with get, set
        abstract refreshJourney: bool option with get, set
        abstract reachableFrom: bool option with get, set

    type [<AllowNullLiteral>] Location =
        abstract ``type``: string with get, set
        abstract id: string option with get, set
        abstract name: string option with get, set
        abstract poi: bool option with get, set
        abstract address: string option with get, set
        abstract longitude: float option with get, set
        abstract latitude: float option with get, set
        abstract altitude: float option with get, set

    /// Each public transportation network exposes its products as boolean properties. See {@link ProductType}
    type [<AllowNullLiteral>] Products =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: product: string -> bool with get, set

    type [<AllowNullLiteral>] Facilities =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: product: string -> U2<string, bool> with get, set

    type [<AllowNullLiteral>] ReisezentrumOpeningHours =
        abstract Mo: string option with get, set
        abstract Di: string option with get, set
        abstract Mi: string option with get, set
        abstract Do: string option with get, set
        abstract Fr: string option with get, set
        abstract Sa: string option with get, set
        abstract So: string option with get, set

    type [<AllowNullLiteral>] Station =
        abstract ``type``: string with get, set
        abstract id: string with get, set
        abstract name: string with get, set
        abstract station: Station option with get, set
        abstract location: Location option with get, set
        abstract products: Products option with get, set
        abstract isMeta: bool option with get, set
        /// region ids
        abstract regions: ReadonlyArray<string> option with get, set
        abstract facilities: Facilities option with get, set
        abstract reisezentrumOpeningHours: ReisezentrumOpeningHours option with get, set

    type [<AllowNullLiteral>] IDs =
        /// DELFI Haltestellen ID
        abstract dhid: string option with get, set

    type [<AllowNullLiteral>] Stop =
        abstract ``type``: string with get, set
        abstract id: string with get, set
        abstract name: string with get, set
        abstract station: Station option with get, set
        abstract location: Location option with get, set
        abstract products: Products with get, set
        abstract lines: ReadonlyArray<Line> option with get, set
        abstract isMeta: bool option with get, set
        abstract reisezentrumOpeningHours: ReisezentrumOpeningHours option with get, set
        abstract ids: IDs option with get, set
        abstract loadFactor: string option with get, set

    type [<AllowNullLiteral>] Region =
        abstract ``type``: string with get, set
        abstract id: string with get, set
        abstract name: string with get, set
        /// station ids
        abstract stations: ReadonlyArray<string> with get, set

    type [<AllowNullLiteral>] Line =
        abstract ``type``: string with get, set
        abstract id: string option with get, set
        abstract name: string option with get, set
        abstract adminCode: string option with get, set
        abstract fahrtNr: string option with get, set
        abstract additionalName: string option with get, set
        abstract product: string option with get, set
        abstract ``public``: bool option with get, set
        abstract mode: LineMode with get, set
        /// routes ids
        abstract routes: ReadonlyArray<string> option with get, set
        abstract operator: Operator option with get, set
        abstract express: bool option with get, set
        abstract metro: bool option with get, set
        abstract night: bool option with get, set
        abstract nr: float option with get, set
        abstract symbol: string option with get, set

    type [<AllowNullLiteral>] Route =
        abstract ``type``: string with get, set
        abstract id: string with get, set
        abstract line: string with get, set
        abstract mode: LineMode with get, set
        /// stop ids
        abstract stops: ReadonlyArray<string> with get, set

    type [<AllowNullLiteral>] Cycle =
        abstract min: float option with get, set
        abstract max: float option with get, set
        abstract nr: float option with get, set

    type [<AllowNullLiteral>] ArrivalDeparture =
        abstract arrival: float option with get, set
        abstract departure: float option with get, set

    type [<AllowNullLiteral>] Schedule =
        abstract ``type``: string with get, set
        abstract id: string with get, set
        abstract route: string with get, set
        abstract mode: LineMode with get, set
        abstract sequence: ReadonlyArray<ArrivalDeparture> with get, set
        /// array of Unix timestamps
        abstract starts: ReadonlyArray<string> with get, set

    type [<AllowNullLiteral>] Operator =
        abstract ``type``: string with get, set
        abstract id: string with get, set
        abstract name: string with get, set

    type [<AllowNullLiteral>] Hint =
        abstract ``type``: string with get, set
        abstract code: string option with get, set
        abstract summary: string option with get, set
        abstract text: string with get, set
        abstract tripId: string option with get, set

    type [<AllowNullLiteral>] Geometry =
        abstract ``type``: string with get, set
        abstract coordinates: ResizeArray<float> with get, set

    type [<AllowNullLiteral>] Feature =
        abstract ``type``: string with get, set
        abstract properties: U2<Station, Stop> option with get, set
        abstract geometry: Geometry with get, set

    type [<AllowNullLiteral>] FeatureCollection =
        abstract ``type``: string with get, set
        abstract features: ReadonlyArray<Feature> with get, set

    type [<AllowNullLiteral>] StopOver =
        abstract stop: U2<Station, Stop> with get, set
        /// null, if last stopOver of trip
        abstract departure: string option with get, set
        abstract departureDelay: float option with get, set
        abstract plannedDeparture: string option with get, set
        abstract departurePlatform: string option with get, set
        abstract plannedDeparturePlatform: string option with get, set
        /// null, if first stopOver of trip
        abstract arrival: string option with get, set
        abstract arrivalDelay: float option with get, set
        abstract plannedArrival: string option with get, set
        abstract arrivalPlatform: string option with get, set
        abstract plannedArrivalPlatform: string option with get, set
        abstract remarks: ReadonlyArray<Hint> option with get, set

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
        abstract stopovers: ReadonlyArray<StopOver> with get, set
        abstract remarks: ReadonlyArray<Hint> option with get, set
        abstract line: Line option with get, set
        abstract direction: string option with get, set
        abstract reachable: bool option with get, set
        abstract polyline: FeatureCollection option with get, set

    type [<AllowNullLiteral>] Price =
        abstract amount: float with get, set
        abstract currency: string with get, set
        abstract hint: string option with get, set

    type [<AllowNullLiteral>] Alternative =
        abstract tripId: string with get, set
        abstract direction: string option with get, set
        abstract line: Line option with get, set
        abstract stop: U2<Station, Stop> option with get, set
        abstract plannedWhen: string option with get, set
        abstract ``when``: string option with get, set
        abstract delay: float option with get, set
        abstract platform: string option with get, set
        abstract plannedPlatform: string option with get, set
        abstract remarks: ReadonlyArray<Hint> option with get, set
        abstract cancelled: bool option with get, set
        abstract loadFactor: string option with get, set

    type [<AllowNullLiteral>] Leg =
        abstract tripId: string option with get, set
        abstract origin: U2<Station, Stop> with get, set
        abstract destination: U2<Station, Stop> with get, set
        abstract departure: string option with get, set
        abstract plannedDeparture: string with get, set
        abstract departureDelay: float option with get, set
        abstract departurePlatform: string option with get, set
        abstract plannedDeparturePlatform: string option with get, set
        abstract arrival: string option with get, set
        abstract plannedArrival: string with get, set
        abstract arrivalDelay: float option with get, set
        abstract arrivalPlatform: string option with get, set
        abstract plannedArrivalPlatform: string option with get, set
        abstract stopovers: ReadonlyArray<StopOver> option with get, set
        abstract schedule: float option with get, set
        abstract price: Price option with get, set
        abstract operator: float option with get, set
        abstract direction: string option with get, set
        abstract line: Line option with get, set
        abstract reachable: bool option with get, set
        abstract cancelled: bool option with get, set
        abstract walking: bool option with get, set
        abstract loadFactor: string option with get, set
        abstract distance: float option with get, set
        abstract ``public``: bool option with get, set
        abstract transfer: bool option with get, set
        abstract cycle: Cycle option with get, set
        abstract alternatives: ReadonlyArray<Alternative> option with get, set
        abstract polyline: FeatureCollection option with get, set
        abstract remarks: ReadonlyArray<Hint> option with get, set

    type [<AllowNullLiteral>] ScheduledDays =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: day: string -> bool with get, set

    type [<AllowNullLiteral>] Journey =
        abstract ``type``: string with get, set
        abstract legs: ReadonlyArray<Leg> with get, set
        abstract refreshToken: string option with get, set
        abstract remarks: ReadonlyArray<Hint> option with get, set
        abstract price: Price option with get, set
        abstract cycle: Cycle option with get, set
        abstract scheduledDays: ScheduledDays option with get, set

    type [<AllowNullLiteral>] Journeys =
        abstract earlierRef: string option with get, set
        abstract laterRef: string option with get, set
        abstract journeys: ReadonlyArray<Journey> with get, set

    type [<AllowNullLiteral>] Duration =
        abstract duration: float with get, set
        abstract stations: ReadonlyArray<U2<Station, Stop>> with get, set

    type [<AllowNullLiteral>] JourneysOptions =
        /// departure date, undefined corresponds to Date.Now
        abstract departure: DateTime option with get, set
        /// arrival date, departure and arrival are mutually exclusive.
        abstract arrival: DateTime option with get, set
        /// earlierThan, use {@link Journeys#earlierRef}, earlierThan and departure/arrival are mutually exclusive.
        abstract earlierThan: string option with get, set
        /// laterThan, use {@link Journeys#laterRef}, laterThan and departure/arrival are mutually exclusive.
        abstract laterThan: string option with get, set
        /// how many search results?
        abstract results: float option with get, set
        /// let journeys pass this station
        abstract via: string option with get, set
        /// return stations on the way?
        abstract stopovers: bool option with get, set
        /// Maximum nr of transfers. Default: Let HAFAS decide.
        abstract transfers: float option with get, set
        /// minimum time for a single transfer in minutes
        abstract transferTime: float option with get, set
        /// 'none', 'partial' or 'complete'
        abstract accessibility: string option with get, set
        /// only bike-friendly journeys
        abstract bike: bool option with get, set
        abstract products: Products option with get, set
        /// return tickets? only available with some profiles
        abstract tickets: bool option with get, set
        /// return a shape for each leg?
        abstract polylines: bool option with get, set
        /// parse & expose sub-stops of stations?
        abstract subStops: bool option with get, set
        /// parse & expose entrances of stops/stations?
        abstract entrances: bool option with get, set
        /// parse & expose hints & warnings?
        abstract remarks: bool option with get, set
        /// 'slow', 'normal', 'fast'
        abstract walkingSpeed: string option with get, set
        abstract startWithWalking: bool option with get, set
        /// language to get results in
        abstract language: string option with get, set
        /// parse which days each journey is valid on
        abstract scheduledDays: bool option with get, set

    type [<AllowNullLiteral>] LocationsOptions =
        /// find only exact matches?
        abstract fuzzy: bool option with get, set
        /// how many search results?
        abstract results: float option with get, set
        /// return stops/stations?
        abstract stops: bool option with get, set
        /// return addresses
        abstract addresses: bool option with get, set
        /// points of interest
        abstract poi: bool option with get, set
        /// parse & expose lines at each stop/station?
        abstract linesOfStops: bool option with get, set
        /// Language of the results
        abstract language: string option with get, set

    type [<AllowNullLiteral>] TripOptions =
        /// return stations on the way?
        abstract stopovers: bool option with get, set
        /// return a shape for the trip?
        abstract polyline: bool option with get, set
        /// parse & expose sub-stops of stations?
        abstract subStops: bool option with get, set
        /// parse & expose entrances of stops/stations?
        abstract entrances: bool option with get, set
        /// parse & expose hints & warnings?
        abstract remarks: bool option with get, set
        abstract language: string option with get, set

    type [<AllowNullLiteral>] StopOptions =
        /// parse & expose lines at the stop/station?
        abstract linesOfStops: bool option with get, set
        /// parse & expose sub-stops of stations?
        abstract subStops: bool option with get, set
        /// parse & expose entrances of stops/stations?
        abstract entrances: bool option with get, set
        abstract language: string option with get, set

    type [<AllowNullLiteral>] DeparturesArrivalsOptions =
        /// departure date, undefined corresponds to Date.Now
        abstract ``when``: DateTime option with get, set
        /// only show departures heading to this station
        abstract direction: string option with get, set
        /// show departures for the next n minutes
        abstract duration: float option with get, set
        /// max. number of results; `null` means "whatever HAFAS wants"
        abstract results: float option with get, set
        /// parse & expose sub-stops of stations?
        abstract subStops: bool option with get, set
        /// parse & expose entrances of stops/stations?
        abstract entrances: bool option with get, set
        /// parse & expose lines at the stop/station?
        abstract linesOfStops: bool option with get, set
        /// parse & expose hints & warnings?
        abstract remarks: bool option with get, set
        /// fetch & parse previous/next stopovers?
        abstract stopovers: bool option with get, set
        /// departures at related stations
        abstract includeRelatedStations: bool option with get, set
        /// language
        abstract language: string option with get, set

    type [<AllowNullLiteral>] RefreshJourneyOptions =
        /// return stations on the way?
        abstract stopovers: bool option with get, set
        /// return a shape for each leg?
        abstract polylines: bool option with get, set
        /// return tickets? only available with some profiles
        abstract tickets: bool option with get, set
        /// parse & expose sub-stops of stations?
        abstract subStops: bool option with get, set
        /// parse & expose entrances of stops/stations?
        abstract entrances: bool option with get, set
        /// parse & expose hints & warnings?
        abstract remarks: bool option with get, set
        abstract language: string option with get, set

    type [<AllowNullLiteral>] NearByOptions =
        /// maximum number of results
        abstract results: float option with get, set
        /// maximum walking distance in meters
        abstract distance: float option with get, set
        /// return points of interest?
        abstract poi: bool option with get, set
        /// return stops/stations?
        abstract stops: bool option with get, set
        /// parse & expose sub-stops of stations?
        abstract subStops: bool option with get, set
        /// parse & expose entrances of stops/stations?
        abstract entrances: bool option with get, set
        /// parse & expose lines at each stop/station?
        abstract linesOfStops: bool option with get, set
        abstract language: string option with get, set

    type [<AllowNullLiteral>] ReachableFromOptions =
        abstract ``when``: DateTime option with get, set
        /// maximum of transfers
        abstract maxTransfers: float option with get, set
        /// maximum travel duration in minutes, pass `null` for infinite
        abstract maxDuration: float option with get, set
        abstract products: Products option with get, set
        /// parse & expose sub-stops of stations?
        abstract subStops: bool option with get, set
        /// parse & expose entrances of stops/stations?
        abstract entrances: bool option with get, set

    type [<AllowNullLiteral>] HafasClient =
        /// Retrieves journeys
        abstract journeys: (U3<string, Station, Location> -> U3<string, Station, Location> -> JourneysOptions option -> Promise<Journeys>) with get, set
        abstract refreshJourney: (string -> RefreshJourneyOptions option -> Promise<Journey>) with get, set
        abstract trip: (string -> string -> TripOptions option -> Promise<Trip>) with get, set
        /// Retrieves departures
        abstract departures: (U2<string, Station> -> DeparturesArrivalsOptions option -> Promise<ResizeArray<Alternative>>) with get, set
        abstract arrivals: (U2<string, Station> -> DeparturesArrivalsOptions option -> Promise<ResizeArray<Alternative>>) with get, set
        /// Retrieves locations or stops
        abstract locations: (string -> LocationsOptions option -> Promise<ResizeArray<U3<Station, Stop, Location>>>) with get, set
        abstract stop: (string -> StopOptions option -> Promise<Stop>) with get, set
        /// Retrieves nearby stops
        abstract nearby: (Location -> NearByOptions option -> Promise<ResizeArray<Stop>>) with get, set
        abstract reachableFrom: (Location -> ReachableFromOptions option -> Promise<ResizeArray<Duration>>) with get, set

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
