// ts2fable 0.7.1
module rec HafasClientTypes
open System
open Fable.Core
open Fable.Core.JS

type ReadonlyArray<'T> = System.Collections.Generic.IReadOnlyList<'T>


type [<AllowNullLiteral>] IExports =
    abstract createClient: commonProfile: CreateClient.Profile * userAgent: string * ?opt: obj -> CreateClient.HafasClient

module CreateClient =

    /// A ProductType relates to how a means of transport "works" in local context.
    /// Example: Even though S-Bahn and U-Bahn in Berlin are both trains, they have different operators, service patterns,
    /// stations and look different. Therefore, they are two distinct products subway and suburban.
    type [<AllowNullLiteral>] ProductType =
        abstract id: string with get, set
        abstract mode: ProductTypeMode with get, set
        abstract name: string with get, set
        abstract short: string with get, set
        abstract bitmasks: ResizeArray<float> with get, set
        abstract ``default``: bool with get, set

    /// A profile is a specific customisation for each endpoint.
    /// It parses data from the API differently, add additional information, or enable non-default methods.
    type [<AllowNullLiteral>] Profile =
        abstract locale: string with get, set
        abstract timezone: string with get, set
        abstract endpoint: string with get, set
        abstract products: ReadonlyArray<ProductType> with get, set
        abstract trip: bool option with get, set
        abstract radar: bool option with get, set
        abstract refreshJourney: bool option with get, set
        abstract reachableFrom: bool option with get, set
        abstract journeysWalkingSpeed: bool option with get, set

    /// A location object is used by other items to indicate their locations.
    type [<AllowNullLiteral>] Location =
        abstract ``type``: string with get, set
        abstract id: string option with get, set
        abstract name: string option with get, set
        abstract poi: bool option with get, set
        abstract address: string option with get, set
        abstract longitude: float option with get, set
        abstract latitude: float option with get, set
        abstract altitude: float option with get, set
        abstract distance: float option with get, set

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

    /// A station is a larger building or area that can be identified by a name.
    /// It is usually represented by a single node on a public transport map.
    /// Whereas a stop usually specifies a location, a station often is a broader area
    /// that may span across multiple levels or buildings.
    type [<AllowNullLiteral>] Station =
        abstract ``type``: string with get, set
        abstract id: string option with get, set
        abstract name: string option with get, set
        abstract station: Station option with get, set
        abstract location: Location option with get, set
        abstract products: Products option with get, set
        abstract isMeta: bool option with get, set
        /// region ids
        abstract regions: ReadonlyArray<string> option with get, set
        abstract facilities: Facilities option with get, set
        abstract reisezentrumOpeningHours: ReisezentrumOpeningHours option with get, set
        abstract stops: ReadonlyArray<U3<Station, Stop, Location>> option with get, set
        abstract entrances: ReadonlyArray<Location> option with get, set
        abstract transitAuthority: string option with get, set
        abstract distance: float option with get, set

    /// Ids of a Stop, i.e. dhid as 'DELFI Haltestellen ID'
    type [<AllowNullLiteral>] Ids =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: id: string -> string with get, set

    /// A stop is a single small point or structure at which vehicles stop.
    /// A stop always belongs to a station. It may for example be a sign, a basic shelter or a railway platform.
    type [<AllowNullLiteral>] Stop =
        abstract ``type``: string with get, set
        abstract id: string with get, set
        abstract name: string option with get, set
        abstract station: Station option with get, set
        abstract location: Location option with get, set
        abstract products: Products option with get, set
        abstract lines: ReadonlyArray<Line> option with get, set
        abstract isMeta: bool option with get, set
        abstract reisezentrumOpeningHours: ReisezentrumOpeningHours option with get, set
        abstract ids: Ids option with get, set
        abstract loadFactor: string option with get, set
        abstract entrances: ReadonlyArray<Location> option with get, set
        abstract transitAuthority: string option with get, set
        abstract distance: float option with get, set

    /// A region is a group of stations, for example a metropolitan area or a geographical or cultural region.
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
        abstract mode: ProductTypeMode option with get, set
        /// routes ids
        abstract routes: ReadonlyArray<string> option with get, set
        abstract operator: Operator option with get, set
        abstract express: bool option with get, set
        abstract metro: bool option with get, set
        abstract night: bool option with get, set
        abstract nr: float option with get, set
        abstract symbol: string option with get, set

    /// A route represents a single set of stations, of a single line.
    type [<AllowNullLiteral>] Route =
        abstract ``type``: string with get, set
        abstract id: string with get, set
        abstract line: string with get, set
        abstract mode: ProductTypeMode with get, set
        /// stop ids
        abstract stops: ReadonlyArray<string> with get, set

    type [<AllowNullLiteral>] Cycle =
        abstract min: float option with get, set
        abstract max: float option with get, set
        abstract nr: float option with get, set

    type [<AllowNullLiteral>] ArrivalDeparture =
        abstract arrival: float option with get, set
        abstract departure: float option with get, set

    /// There are many ways to format schedules of public transport routes.
    /// This one tries to balance the amount of data and consumability.
    /// It is specifically geared towards urban public transport, with frequent trains and homogenous travels.
    type [<AllowNullLiteral>] Schedule =
        abstract ``type``: string with get, set
        abstract id: string with get, set
        abstract route: string with get, set
        abstract mode: ProductTypeMode with get, set
        abstract sequence: ReadonlyArray<ArrivalDeparture> with get, set
        /// array of Unix timestamps
        abstract starts: ReadonlyArray<string> with get, set

    type [<AllowNullLiteral>] Operator =
        abstract ``type``: string with get, set
        abstract id: string with get, set
        abstract name: string with get, set

    type [<AllowNullLiteral>] Hint =
        abstract ``type``: HintType with get, set
        abstract code: string option with get, set
        abstract summary: string option with get, set
        abstract text: string with get, set
        abstract tripId: string option with get, set

    type [<AllowNullLiteral>] Warning =
        abstract ``type``: WarningType with get, set
        abstract id: float option with get, set
        abstract icon: string option with get, set
        abstract summary: string option with get, set
        abstract text: string with get, set
        abstract category: string option with get, set
        abstract priority: float option with get, set
        abstract products: Products option with get, set
        abstract edges: ResizeArray<obj option> option with get, set
        abstract events: ResizeArray<obj option> option with get, set
        abstract validFrom: string option with get, set
        abstract validUntil: string option with get, set
        abstract modified: string option with get, set

    type [<AllowNullLiteral>] Geometry =
        abstract ``type``: string with get, set
        abstract coordinates: ResizeArray<float> with get, set

    type [<AllowNullLiteral>] Feature =
        abstract ``type``: string with get, set
        abstract properties: U3<Station, Stop, obj> option with get, set
        abstract geometry: Geometry with get, set

    type [<AllowNullLiteral>] FeatureCollection =
        abstract ``type``: string with get, set
        abstract features: ReadonlyArray<Feature> with get, set

    /// A stopover represents a vehicle stopping at a stop/station at a specific time.
    type [<AllowNullLiteral>] StopOver =
        abstract stop: U2<Station, Stop> with get, set
        /// null, if last stopOver of trip
        abstract departure: string option with get, set
        abstract departureDelay: float option with get, set
        abstract prognosedDeparture: string option with get, set
        abstract plannedDeparture: string option with get, set
        abstract departurePlatform: string option with get, set
        abstract prognosedDeparturePlatform: string option with get, set
        abstract plannedDeparturePlatform: string option with get, set
        /// null, if first stopOver of trip
        abstract arrival: string option with get, set
        abstract arrivalDelay: float option with get, set
        abstract prognosedArrival: string option with get, set
        abstract plannedArrival: string option with get, set
        abstract arrivalPlatform: string option with get, set
        abstract prognosedArrivalPlatform: string option with get, set
        abstract plannedArrivalPlatform: string option with get, set
        abstract remarks: ReadonlyArray<U2<Hint, Warning>> option with get, set
        abstract passBy: bool option with get, set
        abstract cancelled: bool option with get, set

    /// Trip – a vehicle stopping at a set of stops at specific times
    type [<AllowNullLiteral>] Trip =
        abstract id: string with get, set
        abstract origin: U2<Station, Stop> with get, set
        abstract destination: U2<Station, Stop> with get, set
        abstract departure: string option with get, set
        abstract plannedDeparture: string option with get, set
        abstract prognosedArrival: string option with get, set
        abstract departureDelay: float option with get, set
        abstract departurePlatform: string option with get, set
        abstract prognosedDeparturePlatform: string option with get, set
        abstract plannedDeparturePlatform: string option with get, set
        abstract arrival: string option with get, set
        abstract plannedArrival: string option with get, set
        abstract prognosedDeparture: string option with get, set
        abstract arrivalDelay: float option with get, set
        abstract arrivalPlatform: string option with get, set
        abstract prognosedArrivalPlatform: string option with get, set
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
        abstract remarks: ReadonlyArray<U2<Hint, Warning>> option with get, set

    type [<AllowNullLiteral>] Price =
        abstract amount: float with get, set
        abstract currency: string with get, set
        abstract hint: string option with get, set

    type [<AllowNullLiteral>] Alternative =
        abstract tripId: string with get, set
        abstract direction: string option with get, set
        abstract line: Line option with get, set
        abstract stop: U2<Station, Stop> option with get, set
        abstract ``when``: string option with get, set
        abstract plannedWhen: string option with get, set
        abstract prognosedWhen: string option with get, set
        abstract delay: float option with get, set
        abstract platform: string option with get, set
        abstract plannedPlatform: string option with get, set
        abstract prognosedPlatform: string option with get, set
        abstract remarks: ReadonlyArray<U2<Hint, Warning>> option with get, set
        abstract cancelled: bool option with get, set
        abstract loadFactor: string option with get, set
        abstract provenance: string option with get, set
        abstract previousStopovers: ReadonlyArray<StopOver> option with get, set
        abstract nextStopovers: ReadonlyArray<StopOver> option with get, set

    /// Leg of journey
    type [<AllowNullLiteral>] Leg =
        abstract tripId: string option with get, set
        abstract origin: U2<Station, Stop> with get, set
        abstract destination: U2<Station, Stop> with get, set
        abstract departure: string option with get, set
        abstract plannedDeparture: string option with get, set
        abstract prognosedArrival: string option with get, set
        abstract departureDelay: float option with get, set
        abstract departurePlatform: string option with get, set
        abstract prognosedDeparturePlatform: string option with get, set
        abstract plannedDeparturePlatform: string option with get, set
        abstract arrival: string option with get, set
        abstract plannedArrival: string option with get, set
        abstract prognosedDeparture: string option with get, set
        abstract arrivalDelay: float option with get, set
        abstract arrivalPlatform: string option with get, set
        abstract prognosedArrivalPlatform: string option with get, set
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
        abstract remarks: ReadonlyArray<U2<Hint, Warning>> option with get, set

    type [<AllowNullLiteral>] ScheduledDays =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: day: string -> bool with get, set

    /// A journey is a computed set of directions to get from A to B at a specific time.
    /// It would typically be the result of a route planning algorithm.
    type [<AllowNullLiteral>] Journey =
        abstract ``type``: string with get, set
        abstract legs: ReadonlyArray<Leg> with get, set
        abstract refreshToken: string option with get, set
        abstract remarks: ReadonlyArray<U2<Hint, Warning>> option with get, set
        abstract price: Price option with get, set
        abstract cycle: Cycle option with get, set
        abstract scheduledDays: ScheduledDays option with get, set

    type [<AllowNullLiteral>] Journeys =
        abstract earlierRef: string option with get, set
        abstract laterRef: string option with get, set
        abstract journeys: ReadonlyArray<Journey> option with get, set
        abstract realtimeDataFrom: float option with get, set

    type [<AllowNullLiteral>] Duration =
        abstract duration: float with get, set
        abstract stations: ReadonlyArray<U3<Station, Stop, Location>> with get, set

    type [<AllowNullLiteral>] Frame =
        abstract origin: U2<Stop, Location> with get, set
        abstract destination: U2<Stop, Location> with get, set
        abstract t: float option with get, set

    type [<AllowNullLiteral>] Movement =
        abstract direction: string option with get, set
        abstract tripId: string option with get, set
        abstract line: Line option with get, set
        abstract location: Location option with get, set
        abstract nextStopovers: ReadonlyArray<StopOver> option with get, set
        abstract frames: ReadonlyArray<Frame> option with get, set
        abstract polyline: FeatureCollection option with get, set

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
        /// start with walking
        abstract startWithWalking: bool option with get, set
        /// language to get results in
        abstract language: string option with get, set
        /// parse which days each journey is valid on
        abstract scheduledDays: bool option with get, set
        abstract ``when``: DateTime option with get, set

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
        /// parse & expose sub-stops of stations?
        abstract subStops: bool option with get, set
        /// parse & expose entrances of stops/stations?
        abstract entrances: bool option with get, set
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
        /// Language of the results
        abstract language: string option with get, set

    type [<AllowNullLiteral>] StopOptions =
        /// parse & expose lines at the stop/station?
        abstract linesOfStops: bool option with get, set
        /// parse & expose sub-stops of stations?
        abstract subStops: bool option with get, set
        /// parse & expose entrances of stops/stations?
        abstract entrances: bool option with get, set
        /// parse & expose hints & warnings?
        abstract remarks: bool option with get, set
        /// Language of the results
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
        /// language
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
        /// language
        abstract language: string option with get, set

    type [<AllowNullLiteral>] ReachableFromOptions =
        /// when
        abstract ``when``: DateTime option with get, set
        /// maximum of transfers
        abstract maxTransfers: float option with get, set
        /// maximum travel duration in minutes, pass `null` for infinite
        abstract maxDuration: float option with get, set
        /// products
        abstract products: Products option with get, set
        /// parse & expose sub-stops of stations?
        abstract subStops: bool option with get, set
        /// parse & expose entrances of stops/stations?
        abstract entrances: bool option with get, set

    type [<AllowNullLiteral>] BoundingBox =
        abstract north: float with get, set
        abstract west: float with get, set
        abstract south: float with get, set
        abstract east: float with get, set

    type [<AllowNullLiteral>] RadarOptions =
        /// maximum number of vehicles
        abstract results: float option with get, set
        /// nr of frames to compute
        abstract frames: float option with get, set
        /// optionally an object of booleans
        abstract products: U2<bool, obj> option with get, set
        /// compute frames for the next n seconds
        abstract duration: float option with get, set
        /// parse & expose sub-stops of stations?
        abstract subStops: bool option with get, set
        /// parse & expose entrances of stops/stations?
        abstract entrances: bool option with get, set
        /// return a shape for the trip?
        abstract polylines: bool option with get, set
        /// when
        abstract ``when``: DateTime option with get, set

    type [<AllowNullLiteral>] HafasClient =
        /// Retrieves journeys
        abstract journeys: (U3<string, Station, Location> -> U3<string, Station, Location> -> JourneysOptions option -> Promise<Journeys>) with get, set
        /// refreshes a Journey
        abstract refreshJourney: (string -> RefreshJourneyOptions option -> Promise<Journey>) option with get, set
        /// Refetch information about a trip
        abstract trip: (string -> string -> TripOptions option -> Promise<Trip>) option with get, set
        /// Retrieves departures
        abstract departures: (U2<string, Station> -> DeparturesArrivalsOptions option -> Promise<ResizeArray<Alternative>>) with get, set
        /// Retrieves arrivals
        abstract arrivals: (U2<string, Station> -> DeparturesArrivalsOptions option -> Promise<ResizeArray<Alternative>>) with get, set
        /// Retrieves locations or stops
        abstract locations: (string -> LocationsOptions option -> Promise<ResizeArray<U3<Station, Stop, Location>>>) with get, set
        /// Retrieves information about a stop
        abstract stop: (string -> StopOptions option -> Promise<U3<Station, Stop, Location>>) with get, set
        /// Retrieves nearby stops from location
        abstract nearby: (Location -> NearByOptions option -> Promise<ResizeArray<U3<Station, Stop, Location>>>) with get, set
        /// Retrieves stations reachable within a certain time from a location
        abstract reachableFrom: (Location -> ReachableFromOptions option -> Promise<ResizeArray<Duration>>) option with get, set
        /// Retrieves all vehicles currently in an area.
        abstract radar: (BoundingBox -> RadarOptions option -> Promise<ResizeArray<Movement>>) option with get, set

    type [<StringEnum>] [<RequireQualifiedAccess>] ProductTypeMode =
        | Train
        | Bus
        | Watercraft
        | Taxi
        | Gondola
        | Aircraft
        | Car
        | Bicycle
        | Walking

    type [<StringEnum>] [<RequireQualifiedAccess>] HintType =
        | Hint
        | Status
        | [<CompiledName "foreign-id">] ForeignId
        | [<CompiledName "local-fare-zone">] LocalFareZone
        | [<CompiledName "stop-website">] StopWebsite
        | [<CompiledName "stop-dhid">] StopDhid
        | [<CompiledName "transit-authority">] TransitAuthority

    type [<StringEnum>] [<RequireQualifiedAccess>] WarningType =
        | Status
        | Warning
