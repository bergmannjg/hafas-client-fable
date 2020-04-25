module HafasClient

open FSharp.Core
open Fable.Core
open HafasClientTypes

[<Import("default", "hafas-client")>]
let createClient (profile: obj) (name: string): HafasClient = jsNative

[<Import("default", "hafas-client/p/bvg")>]
let bvgProfile: obj = jsNative

[<Import("default", "hafas-client/p/cmta")>]
let cmtaProfile: obj = jsNative

[<Import("default", "hafas-client/p/db")>]
let dbProfile: obj = jsNative

[<Import("default", "hafas-client/p/db-busradar-nrw")>]
let dbbusradarnrwProfile: obj = jsNative

[<Import("default", "hafas-client/p/cfl")>]
let cflProfile: obj = jsNative

[<Import("default", "hafas-client/p/hvv")>]
let hvvProfile: obj = jsNative

[<Import("default", "hafas-client/p/insa")>]
let insaProfile: obj = jsNative

[<Import("default", "hafas-client/p/invg")>]
let invgProfile: obj = jsNative

[<Import("default", "hafas-client/p/nahsh")>]
let nahshProfile: obj = jsNative

[<Import("default", "hafas-client/p/nvv")>]
let nvvProfile: obj = jsNative

[<Import("default", "hafas-client/p/oebb")>]
let oebbProfile: obj = jsNative

[<Import("default", "hafas-client/p/pkp")>]
let pkpProfile: obj = jsNative

[<Import("default", "hafas-client/p/rmv")>]
let rmvProfile: obj = jsNative

[<Import("default", "hafas-client/p/rsag")>]
let rsagProfile: obj = jsNative

[<Import("default", "hafas-client/p/saarfahrplan")>]
let saarfahrplanProfile: obj = jsNative

[<Import("default", "hafas-client/p/sncb")>]
let sncbProfile: obj = jsNative

[<Import("default", "hafas-client/p/sbahn-muenchen")>]
let sMuenchenProfile: obj = jsNative

[<Import("default", "hafas-client/p/svv")>]
let svvProfile: obj = jsNative

[<Import("default", "hafas-client/p/vbb")>]
let vbbProfile: obj = jsNative

[<Import("default", "hafas-client/p/vbn")>]
let vbnProfile: obj = jsNative

[<Import("default", "hafas-client/p/vmt")>]
let vmtProfile: obj = jsNative

[<Import("default", "hafas-client/p/vsn")>]
let vsnProfile: obj = jsNative

let createHafasClient profile name =
    let p =
        match profile with
        | Profile.Bvg -> bvgProfile
        | Profile.Cmta -> cmtaProfile
        | Profile.Cfl -> cflProfile
        | Profile.Db -> dbProfile
        | Profile.DbBusradarNrw -> dbbusradarnrwProfile
        | Profile.Hvv -> hvvProfile
        | Profile.Insa -> insaProfile
        | Profile.Invg -> invgProfile
        | Profile.Nahsh -> nahshProfile
        | Profile.Nvv -> nvvProfile
        | Profile.Oebb -> oebbProfile
        | Profile.Pkp -> pkpProfile
        | Profile.Rmv -> rmvProfile
        | Profile.Rsag -> rsagProfile
        | Profile.Saarfahrplan -> saarfahrplanProfile
        | Profile.SBahnMunich -> sMuenchenProfile
        | Profile.Svv -> svvProfile
        | Profile.Sncb -> sncbProfile
        | Profile.Vbb -> vbbProfile
        | Profile.Vbn -> vbnProfile
        | Profile.Vmt -> vmtProfile
        | Profile.Vsn -> vsnProfile
    createClient p name
