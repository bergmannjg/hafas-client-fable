module HafasClient

open FSharp.Core
open Fable.Core
open HafasClientTypes

[<Import("default", "hafas-client")>]
let createClient (profile: obj) (name: string): HafasClient = jsNative

[<Import("default", "hafas-client/p/bvg")>]
let bvgProfile: obj = jsNative

[<Import("default", "hafas-client/p/db")>]
let dbProfile: obj = jsNative

[<Import("default", "hafas-client/p/cfl")>]
let cflProfile: obj = jsNative

[<Import("default", "hafas-client/p/insa")>]
let insaProfile: obj = jsNative

[<Import("default", "hafas-client/p/nahsh")>]
let nahshProfile: obj = jsNative

[<Import("default", "hafas-client/p/oebb")>]
let oebbProfile: obj = jsNative

[<Import("default", "hafas-client/p/pkp")>]
let pkpProfile: obj = jsNative

[<Import("default", "hafas-client/p/sncb")>]
let sncbProfile: obj = jsNative


[<Import("default", "hafas-client/p/vbb")>]
let vbbProfile: obj = jsNative

let createHafasClient profile name =
    let p =
        match profile with
        | Profile.Bvg -> bvgProfile
        | Profile.Db -> dbProfile
        | Profile.Cfl -> cflProfile
        | Profile.Insa -> insaProfile
        | Profile.Nahsh -> nahshProfile
        | Profile.Oebb -> oebbProfile
        | Profile.Pkp -> pkpProfile
        | Profile.Sncb -> sncbProfile
        | Profile.Vbb -> vbbProfile
        | unkown -> failwithf "Unkown Profile value: `%O`" unkown
    createClient p name
