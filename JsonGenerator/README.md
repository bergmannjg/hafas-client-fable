# JSonGenerator

This tool generates functions for all types in [HafasClientTypes.fs](../src/HafasClientTypes.fs).

Each function has a serialization for all members of the type with [Fable.SimpleJson](https://github.com/Zaid-Ajaj/Fable.SimpleJson).

## Installation

* install [.NET Core SDK](https://dotnet.microsoft.com/download)
* dotnet restore
* run f# program: **dotnet run -p JsonGenerator/JsonGenerator.fsproj > src/HafasClientTypesDump.fs**
