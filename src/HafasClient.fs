module HafasClient

open FSharp.Core
open Fable.Core
open HafasClientTypes

[<Import("default", "hafas-client")>]
let createClient (profile: obj) (name: string): HafasClient = jsNative

[<Import("default", "hafas-client/p/db")>]
let dbProfile: obj = jsNative

[<Import("default", "hafas-client/p/cfl")>]
let cflProfile: obj = jsNative

[<Import("default", "hafas-client/p/vbb")>]
let vbbProfile: obj = jsNative

[<Import("default", "hafas-client/p/sncb")>]
let sncbProfile: obj = jsNative

let createHafasClient profile name =
    let p =
        match profile with
        | Profile.Db -> dbProfile
        | Profile.Cfl -> cflProfile
        | Profile.Vbb -> vbbProfile
        | Profile.Sncb -> sncbProfile
        | unkown -> failwithf "Unkown Profile value: `%O`" unkown
    createClient p name
