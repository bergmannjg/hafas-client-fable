module HafasClient

open FSharp.Core
open Fable.Core
open Fable.Core.JsInterop
open HafasClientTypes.CreateClient
open HafasClientTypesProfile

type HafasProfile = HafasClientTypes.CreateClient.Profile

[<Import("default", "hafas-client")>]
let createClient (profile: HafasProfile) (name: string): HafasClient = jsNative

let createHafasClient profile name =
    let p =
        match profile with
        | Profile.Bvg -> importDefault<HafasProfile> "hafas-client/p/bvg"
        | Profile.Cmta -> importDefault<HafasProfile> "hafas-client/p/cmta"
        | Profile.Cfl -> importDefault<HafasProfile> "hafas-client/p/cfl"
        | Profile.Db -> importDefault<HafasProfile> "hafas-client/p/db"
        | Profile.DbBusradarNrw -> importDefault<HafasProfile> "hafas-client/p/db-busradar-nrw"
        | Profile.Hvv -> importDefault<HafasProfile> "hafas-client/p/hvv"
        | Profile.Insa -> importDefault<HafasProfile> "hafas-client/p/insa"
        | Profile.Invg -> importDefault<HafasProfile> "hafas-client/p/invg"
        | Profile.Nahsh -> importDefault<HafasProfile> "hafas-client/p/nahsh"
        | Profile.Nvv -> importDefault<HafasProfile> "hafas-client/p/nvv"
        | Profile.Oebb -> importDefault<HafasProfile> "hafas-client/p/oebb"
        | Profile.Pkp -> importDefault<HafasProfile> "hafas-client/p/pkp"
        | Profile.Rmv -> importDefault<HafasProfile> "hafas-client/p/rmv"
        | Profile.Rsag -> importDefault<HafasProfile> "hafas-client/p/rsag"
        | Profile.Saarfahrplan -> importDefault<HafasProfile> "hafas-client/p/saarfahrplan"
        | Profile.SBahnMunich -> importDefault<HafasProfile> "hafas-client/p/sbahn-muenchen"
        | Profile.Svv -> importDefault<HafasProfile> "hafas-client/p/svv"
        | Profile.Sncb -> importDefault<HafasProfile> "hafas-client/p/sncb"
        | Profile.Vbb -> importDefault<HafasProfile> "hafas-client/p/vbb"
        | Profile.Vbn -> importDefault<HafasProfile> "hafas-client/p/vbn"
        | Profile.Vmt -> importDefault<HafasProfile> "hafas-client/p/vmt"
        | Profile.Vsn -> importDefault<HafasProfile> "hafas-client/p/vsn"

    createClient p name
