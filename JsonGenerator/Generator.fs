module Generator

open System
open System.Reflection
open System.Linq

let tab = "    "
let dump = "dump"

let getFormat typename =
    match typename with
    | "String" -> "JString"
    | "Double" -> "JNumber "
    | "Boolean" -> "JBool "
    | _ -> "JObject"

let (|SpecialChars|_|) (name: string) =
    if name.Contains '-' then Some true else None

let getQuotes4Member name =
    match name with
    | "type"
    | "public"
    | "when" -> "``"
    | SpecialChars name -> "``"
    | _ -> ""

let getQuotes4Value (formatName: string) (valName: string) =
    formatName = "s" && valName <> "null" && valName <> "undefined"

let hasDumpFunction (g: Type) =
    if g.FullName.StartsWith("HafasClientTypes") then
        let attributes = g.GetCustomAttributesData()
        not (attributes.Any(fun a -> a.AttributeType.FullName = "Fable.Core.StringEnumAttribute"))
    else
        false

let getU2StationStopExpr (name: string) =
    let line = System.Collections.Generic.List()
    sprintf " match %s with " name |> line.Add
    "| Station station -> " + dump + "Station station " |> line.Add
    "| Stop stop -> " + dump + "Stop stop " |> line.Add
    "| _ -> JNull " |> line.Add
    line |> String.concat ""

let getArrayExpr (prop: PropertyInfo) (g: Type) valName (getValueStmt: PropertyInfo -> Type -> string -> string) =
    let line = System.Collections.Generic.List()
    "JArray [ " |> line.Add
    "for e in " + valName + " do yield " |> line.Add
    getValueStmt prop g "e" |> line.Add
    "] " |> line.Add
    line |> String.concat ""

let getOptionExpr (prop: PropertyInfo) (g: Type) (getValueStmt: PropertyInfo -> Type -> string -> string) =
    let q = getQuotes4Member prop.Name

    let op =
        if prop.Name.Contains '-' then "?" else "." // todo: why we need this
    " " + sprintf "match x%s%s with " op (q + prop.Name + q) + " Some v -> " + getValueStmt prop g "v"
    + " | None -> JNull "

let rec getValueExpr (prop: PropertyInfo) (g: Type) (varName: string) =
    if hasDumpFunction g then
        dump + g.Name + " " + varName + " "
    else if g.Name = "FSharpOption`1" then
        getOptionExpr prop g.GenericTypeArguments.[0] getValueExpr
    else if g.Name = "List`1" || g.Name = "IReadOnlyList`1" then
        getArrayExpr prop g.GenericTypeArguments.[0] varName getValueExpr
    else if g.Name = "U2`2" then
        "(" + (getU2StationStopExpr varName) + ")"
    else if g.Name = "LineMode" || g.Name = "ProductTypeMode" then // todo
        "JString (" + varName + ".ToString()) "
    else
        let ft = (getFormat g.Name)
        if ft = "JString" then ft + " (escapeString " + varName + ") " else ft + " " + varName + " "

let getStmtsOfProp (prop: PropertyInfo) =
    let pt = prop.PropertyType
    let q = getQuotes4Member prop.Name
    tab + "\"" + prop.Name + "\"," + (getValueExpr prop pt ("x." + q + prop.Name + q))

let getStmtsOfProps (ty: Type) =
    ty.GetProperties(BindingFlags.Public ||| BindingFlags.Instance)
    |> Array.fold (fun desc prop -> desc + getStmtsOfProp prop + "\n") ""

let generateDumpFunction (ty: Type) =
    let stmtsOfProps = getStmtsOfProps ty
    let functionName = dump + ty.Name

    let recPrefix =
        if stmtsOfProps.IndexOf(functionName) >= 0 then "rec " else ""

    let line = System.Collections.Generic.List()
    sprintf "let %s%s (x: %s) =\n" recPrefix functionName ty.Name
    |> line.Add
    tab + "[\n" + stmtsOfProps + tab + "] |> Map.ofList |> JObject\n" |> line.Add
    line
    |> String.concat ""
    |> printfn "%s"
