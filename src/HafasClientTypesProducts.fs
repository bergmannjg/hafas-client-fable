// ts2fable 0.7.1
module rec HafasClientTypesProducts
open System
open Fable.Core
open Fable.Core.JS

open  HafasClientTypes

type [<AllowNullLiteral>] ProductsBvg =
    inherit Products
    abstract bus: bool with get, set
    abstract express: bool with get, set
    abstract ferry: bool with get, set
    abstract regional: bool with get, set
    abstract suburban: bool with get, set
    abstract subway: bool with get, set
    abstract tram: bool with get, set

type [<AllowNullLiteral>] ProductsDb =
    inherit Products
    abstract bus: bool with get, set
    abstract ferry: bool with get, set
    abstract national: bool with get, set
    abstract nationalExpress: bool with get, set
    abstract regional: bool with get, set
    abstract regionalExp: bool with get, set
    abstract suburban: bool with get, set
    abstract subway: bool with get, set
    abstract taxi: bool with get, set
    abstract tram: bool with get, set

type [<AllowNullLiteral>] ProductsHvv =
    inherit Products
    abstract akn: bool with get, set
    abstract ``anruf-sammel-taxi``: bool with get, set
    abstract bus: bool with get, set
    abstract ``express-bus``: bool with get, set
    abstract ferry: bool with get, set
    abstract ``long-distance-bus``: bool with get, set
    abstract ``long-distance-train``: bool with get, set
    abstract ``regional-express-train``: bool with get, set
    abstract ``regional-train``: bool with get, set
    abstract suburban: bool with get, set
    abstract subway: bool with get, set

type [<AllowNullLiteral>] ProductsInsa =
    inherit Products
    abstract bus: bool with get, set
    abstract national: bool with get, set
    abstract nationalExpress: bool with get, set
    abstract regional: bool with get, set
    abstract suburban: bool with get, set
    abstract tourismTrain: bool with get, set
    abstract tram: bool with get, set

type [<AllowNullLiteral>] ProductsNahsh =
    inherit Products
    abstract bus: bool with get, set
    abstract ferry: bool with get, set
    abstract interregional: bool with get, set
    abstract national: bool with get, set
    abstract nationalExpress: bool with get, set
    abstract onCall: bool with get, set
    abstract regional: bool with get, set
    abstract suburban: bool with get, set
    abstract subway: bool with get, set
    abstract tram: bool with get, set

type [<AllowNullLiteral>] ProductsOebb =
    inherit Products
    abstract bus: bool with get, set
    abstract ferry: bool with get, set
    abstract interregional: bool with get, set
    abstract national: bool with get, set
    abstract nationalExpress: bool with get, set
    abstract onCall: bool with get, set
    abstract regional: bool with get, set
    abstract suburban: bool with get, set
    abstract subway: bool with get, set
    abstract tram: bool with get, set

type [<AllowNullLiteral>] ProductsPkp =
    inherit Products
    abstract bus: bool with get, set
    abstract ``high-speed-train``: bool with get, set
    abstract ``long-distance-train``: bool with get, set
    abstract ``regional-train``: bool with get, set

type [<AllowNullLiteral>] ProductsRsag =
    inherit Products
    abstract bus: bool with get, set
    abstract ferry: bool with get, set
    abstract ``ic-ec``: bool with get, set
    abstract ice: bool with get, set
    abstract ``long-distance-train``: bool with get, set
    abstract ``on-call``: bool with get, set
    abstract ``regional-train``: bool with get, set
    abstract ``s-bahn``: bool with get, set
    abstract tram: bool with get, set
    abstract ``u-bahn``: bool with get, set

type [<AllowNullLiteral>] ProductsSBahnMunich =
    inherit Products
    abstract bus: bool with get, set
    abstract ``ic-ec``: bool with get, set
    abstract ice: bool with get, set
    abstract ``ir-d``: bool with get, set
    abstract ``on-call``: bool with get, set
    abstract region: bool with get, set
    abstract sbahn: bool with get, set
    abstract tram: bool with get, set
    abstract ubahn: bool with get, set

type [<AllowNullLiteral>] ProductsSncb =
    inherit Products
