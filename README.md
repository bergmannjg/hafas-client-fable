# F# App with hafas-client

This F# App uses the [hafas-client](https://github.com/public-transport/hafas-client) JavaScript library.

It benefits from the [fable](https://fable.io/) infrastructure to

* develop the program with F# and use JavaScript libraries in a type-safe way  
* run the F# program as a JavaScript application.

The connection from hafas-client JavaScript to F# is established in two steps:

* there is a TypeScipt declaration file [hafas-client-types](./types/hafas-client/index.d.ts) for the hafas-client interface,
* the parser [ts2fable](https://www.npmjs.com/package/ts2fable) generates the corresponding F# types.

The F# App is compiled to Javasrcipt with [fable-splitter](https://www.npmjs.com/package/fable-splitter) and can be run with node.js.

## Installation

* install [.NET Core SDK](https://dotnet.microsoft.com/download)
* install [node.js](https://nodejs.org/en/)
* npm install
* generate F# types: **npx ts2fable ./types/hafas-client/index.d.ts ./src/HafasClientTypes.fs**
* generate dump functions: **dotnet run -p JsonGenerator/JsonGenerator.fsproj > src/HafasClientTypesDump.fs**
* compile F# to JavaScript: **npx webpack --config src/webpack.config.js**
* run JavaScript program: **node build/JourneyInfoApp.js Db journeys Berlin Paris**

## Evaluation of TypeScipt declaration file

If the check option is enabled, the App prints two json objects:

* JSon dump of JavaScript object with [JSON.stringify](https://developer.mozilla.org/de/docs/Web/JavaScript/Reference/Global_Objects/JSON/stringify) (runtime view)
* JSon dump of JavaScript object with generated functions by [JsonGenerator](./JsonGenerator) (compile time view)

Example. usage: **node build/JourneyInfoApp.js Db check-journeys Berlin Hannover | csplit -b "%02d.json" - 2**

You can compare the two json files to evaluate the coverage of the TypeScipt declaration file with the hafas-client interface,
see [runtests](./scripts/runtests)