# F# App with hafas-client

This F# App uses the [hafas-client](https://github.com/public-transport/hafas-client) JavaScript library.

It benefits from the [fable](https://fable.io/) infrastructure to

* develop the program with F# and use JavaScript libraries in a type-safe way  
* run the F# program as a JavaScript application.

The connection from hafas-client JavaScript to F# is established in two steps:

* there is a TypeScipt declaration file [hafas-client-types.ts](./ts/hafas-client-types.ts) for the hafas-client interface,
* the parser [ts2fable](https://www.npmjs.com/package/ts2fable) generates the corresponding F# types.

The F# App is compiled to Javasrcipt with [fable-splitter](https://www.npmjs.com/package/fable-splitter) and can be run with node.js.

## Installation

* install [.NET Core SDK](https://dotnet.microsoft.com/download)
* install [node.js](https://nodejs.org/en/)
* npm init
* generate F# types: **ts2fable ./ts/hafas-client-types.ts ./src/HafasClientTypes.fs**
* compile F# to JavaScript: **fable-splitter src -o build --commonjs**
* run JavaScript program: **node build/JourneyInfoApp.js Db journeys Berlin Paris**

## Evaluation of TypeScipt declaration file

The App prints two json objects:

* JSon dump of JavaScript object with [Fable.SimpleJson](https://github.com/Zaid-Ajaj/Fable.SimpleJson) (runtime view)
* JSon dump of JavaScript object with generated functions by [JsonGenerator](./JsonGenerator) (compile time view)

Example. usage: **node build/JourneyInfoApp.js Db journeys Berlin Hannover | csplit -b "%02d.json" - 2**

You can compare the two json files to evaluate the coverage of the TypeScipt declaration file with the hafas-client interface.