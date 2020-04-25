module Generator

open System
open System.Reflection
open System.Linq
open Fable.SimpleJson

let generateDumpFunction (ty: Type) =
    let indent = "--"
    let tab = "    "

    let getFormat typename =
        match typename with
        | "String" -> "JString"
        | "Double" -> "JNumber "
        | "Boolean" -> "JBool "
        | _ -> "JObject"

    let (|SpecialChars|_|) (name:string) = 
        if name.Contains '-' then Some true else None

    let getQuotes4Member name =
        match name with
        | "type" -> "``"
        | "public" -> "``"
        | "when" -> "``"
        | SpecialChars name -> "``"
        | _ -> ""

    let getQuotes4Value (formatName: string) (valName: string) =
        formatName = "s" && valName <> "null" && valName <> "undefined"

    let getPrintNullStmt txtName = "JNull\n"

    let findAttributeWithName (attributes: System.Collections.Generic.IList<CustomAttributeData>) (typeName: string) =
        attributes.Any(fun a -> a.AttributeType.FullName = typeName)

    let hasDumpFunction (g: Type) =
        if g.FullName.StartsWith("HafasClientTypes") then
            let attrs = g.GetCustomAttributesData()
            not (findAttributeWithName attrs "Fable.Core.StringEnumAttribute")
        else
            false

    let getU2StationStopStmt (name: string) =
        let line = System.Collections.Generic.List()
        sprintf " match %s with " name |> line.Add
        sprintf "| Station station -> dumpStation station " |> line.Add
        sprintf "| Stop stop -> dumpStop stop " |> line.Add
        sprintf "| _ -> JNull " |> line.Add
        line |> String.concat ""

    let getArrayValueStmt (prop: PropertyInfo) (g: Type) valName =
        let line = System.Collections.Generic.List()
        "JArray [ " |> line.Add
        "for e in " + valName + " do yield " |> line.Add
        if hasDumpFunction g then "dump" + g.Name + " e"
        else if g.Name = "U2`2" then getU2StationStopStmt "e"
        else (getFormat g.Name) + " e"
        |> line.Add
        "]\n" |> line.Add
        line |> String.concat ""

    let getValueStmt (prop: PropertyInfo) (g: Type) (varName: string) =
        if hasDumpFunction g
        then "dump" + g.Name + " " + varName + "\n"
        else if g.Name = "List`1"
        then getArrayValueStmt prop g.GenericTypeArguments.[0] varName
        else if g.Name = "LineMode" // todo
        then "JString (" + varName + ".ToString())\n"
        else 
            let ft = (getFormat g.Name)
            if ft = "JString" then ft + " (escapeString " + varName + ")\n"
            else ft + " " + varName + "\n"


    let getOptionTypeSomeStmt (prop: PropertyInfo) (g: Type) =
        tab + tab + sprintf "| Some v -> " + getValueStmt prop g "v"

    let getOptionTypeNoneStmt (prop: PropertyInfo) = tab + tab + sprintf "| None -> " + getPrintNullStmt prop.Name

    let getOptionTypeStmt (prop: PropertyInfo) (g: Type) =
        let q = getQuotes4Member prop.Name
        let op = if prop.Name.Contains '-' then "?" else "." // todo: why we need this
        "\n" + tab + sprintf "match x%s%s with\r\n" op (q + prop.Name + q) + getOptionTypeSomeStmt prop g
        + getOptionTypeNoneStmt prop

    let getGenericTypeStmt (prop: PropertyInfo) (pt: Type) (genericTypes: Type []) =
        match pt.Name with
        | "FSharpOption`1" -> getOptionTypeStmt prop genericTypes.[0]
        | "U2`2" when genericTypes.[0].Name = "Station" && genericTypes.[1].Name = "Stop" ->
            (getU2StationStopStmt ("x." + prop.Name) + "\n")
        | "List`1" | "IReadOnlyList`1" -> getArrayValueStmt prop genericTypes.[0] ("x." + prop.Name)
        | _ -> tab + "\"" + prop.Name + "\": JString \"undefined\"\n"

    let getStmtsOfProp (prop: PropertyInfo) =
        let pt = prop.PropertyType
        tab + "\"" + prop.Name + "\"," + match pt.IsGenericType with
                                         | true -> getGenericTypeStmt prop pt pt.GenericTypeArguments
                                         | false ->
                                             let q = getQuotes4Member prop.Name
                                             getValueStmt prop pt ("x." + q + prop.Name + q)

    let getStmtsOfProps =
        ty.GetProperties(BindingFlags.Public ||| BindingFlags.Instance)
        |> Array.fold (fun desc prop -> desc + getStmtsOfProp prop) ""

    let stmtsOfProps = getStmtsOfProps
    let functionName = "dump" + ty.Name
    let isRec = stmtsOfProps.IndexOf(functionName) >= 0
    printfn "let %s%s (x: %s) =" (if isRec then "rec " else "") functionName ty.Name
    printfn "%s[" tab
    printf "%s" getStmtsOfProps
    printfn "%s]" tab
    printfn "%s|> Map.ofList" tab
    printfn "%s|> JObject\n" tab
