module HafasClient

open FSharp.Core
open Fable.Core
open Fable.Core.JsInterop
open HafasClientTypes
open HafasClientTypesProfile

[<Import("default", "hafas-client")>]
let createClient (profile: obj) (name: string): HafasClient = jsNative

let createHafasClient profile name =
    let p =
        match profile with
        | Profile.Bvg -> importDefault "hafas-client/p/bvg"
        | Profile.Cmta -> importDefault "hafas-client/p/cmta"
        | Profile.Cfl -> importDefault "hafas-client/p/cfl"
        | Profile.Db -> importDefault "hafas-client/p/db"
        | Profile.DbBusradarNrw -> importDefault "hafas-client/p/db-busradar-nrw"
        | Profile.Hvv -> importDefault "hafas-client/p/hvv"
        | Profile.Insa -> importDefault "hafas-client/p/insa"
        | Profile.Invg -> importDefault "hafas-client/p/invg"
        | Profile.Nahsh -> importDefault "hafas-client/p/nahsh"
        | Profile.Nvv -> importDefault "hafas-client/p/nvv"
        | Profile.Oebb -> importDefault "hafas-client/p/oebb"
        | Profile.Pkp -> importDefault "hafas-client/p/pkp"
        | Profile.Rmv -> importDefault "hafas-client/p/rmv"
        | Profile.Rsag -> importDefault "hafas-client/p/rsag"
        | Profile.Saarfahrplan -> importDefault "hafas-client/p/saarfahrplan"
        | Profile.SBahnMunich -> importDefault "hafas-client/p/sbahn-muenchen"
        | Profile.Svv -> importDefault "hafas-client/p/svv"
        | Profile.Sncb -> importDefault "hafas-client/p/sncb"
        | Profile.Vbb -> importDefault "hafas-client/p/vbb"
        | Profile.Vbn -> importDefault "hafas-client/p/vbn"
        | Profile.Vmt -> importDefault "hafas-client/p/vmt"
        | Profile.Vsn -> importDefault "hafas-client/p/vsn"

    createClient p name
