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

let isStringEnumAttribute (g: Type) =
    let attributes = g.GetCustomAttributesData()
    (attributes.Any(fun a -> a.AttributeType.FullName = "Fable.Core.StringEnumAttribute"))

let hasDumpFunction (g: Type) =
    if g.FullName.StartsWith("HafasClientTypes") then
        not (isStringEnumAttribute g)
    else
        false

let getU2Expr (name: string) (t1: Type) (t2:Type) (getValueStmt: string -> Type -> string) =
    let line = System.Collections.Generic.List()
    sprintf " match %s with " name |> line.Add
    "| " + t1.Name + " s -> " + (getValueStmt " s " t1 ) |> line.Add
    "| " + t2.Name + " s -> " + (getValueStmt " s " t2 ) |> line.Add
    "| _ -> JNull " |> line.Add
    line |> String.concat ""

let getU3Expr (name: string) (t1: Type) (t2:Type) (t3:Type) (getValueStmt: string -> Type -> string) =
    let line = System.Collections.Generic.List()
    sprintf " match %s with " name |> line.Add
    "| " + t1.Name + " s -> " + (getValueStmt " s " t1 ) |> line.Add
    "| " + t2.Name + " s -> " + (getValueStmt " s " t2 ) |> line.Add
    if  t3.Name <> "Object" then
        "| " + t3.Name + " s -> " + (getValueStmt " s " t3 ) |> line.Add
    else 
        "| U3.Case3 s -> JNull " |> line.Add
    "| _ -> JNull " |> line.Add
    line |> String.concat ""

let getArrayExpr (prop: string) (g: Type) valName (getValueStmt: string -> Type -> string) =
    let line = System.Collections.Generic.List()
    "JArray [ " |> line.Add
    "for e in " + valName + " do yield " |> line.Add
    getValueStmt "e" g |> line.Add
    "] " |> line.Add
    line |> String.concat ""

let getOptionExpr (prop: string) (g: Type) (getValueStmt: string -> Type -> string) =
    let q = getQuotes4Member prop

    let op =
        if prop.Contains '-' then "?" else "." // todo: why we need this
    " " + sprintf "match %s with " (q + prop + q) + " Some v -> " + getValueStmt "v" g 
    + " | None -> JNull "

let rec getValueExpr (varName: string) (g: Type) =
    if hasDumpFunction g then
        if g.Name = "Stop" then "(match dumpStopFunc with | Some f -> f " + varName + " | None -> JNull)"
        else dump + g.Name + " " + varName + " "
    else if g.Name = "Object" then
        "JString (" + varName + ".ToString()) "
    else if g.Name = "FSharpOption`1" then
        getOptionExpr varName g.GenericTypeArguments.[0] getValueExpr
    else if g.Name = "List`1" || g.Name = "IReadOnlyList`1" then
        getArrayExpr varName g.GenericTypeArguments.[0] varName getValueExpr
    else if g.Name = "U2`2" then
        "(" + (getU2Expr varName g.GenericTypeArguments.[0] g.GenericTypeArguments.[1] getValueExpr) + ")"
    else if g.Name = "U3`3" then
        "(" + (getU3Expr varName g.GenericTypeArguments.[0] g.GenericTypeArguments.[1] g.GenericTypeArguments.[2] getValueExpr) + ")"
    else if (isStringEnumAttribute g) then // todo
        "JString (" + varName + ".ToString()) "
    else
        let ft = (getFormat g.Name)
        if ft = "JString" then ft + " (escapeString " + varName + ") " else ft + " " + varName + " "

let getStmtsOfProp (prop: PropertyInfo) =
    let pt = prop.PropertyType
    let q = getQuotes4Member prop.Name
    tab + "\"" + prop.Name + "\"," + (getValueExpr ("x." + q + prop.Name + q) pt)

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
