var path = require("path");
var fs = require("fs");

function resolve(filePath) {
    return path.join(__dirname, filePath)
}

var nodeModulesDir = resolve("../node_modules");

var nodeExternals = {};
fs.readdirSync(nodeModulesDir)
    .filter(function (x) {
        return ['.bin'].indexOf(x) === -1;
    })
    .forEach(function (mod) {
        nodeExternals[mod] = 'commonjs ' + mod;
    });

module.exports = {
    mode: "development",
    entry: "./src/JourneyInfoApp.fsproj",
    devtool: "inline-source-map",
    target: "node",
    externals: nodeExternals,
    output: {
        path: path.join(__dirname, "../build"),
        filename: "bundle.js",
        devtoolModuleFilenameTemplate: info =>
            path.resolve(info.absoluteResourcePath).replace(/\\/g, '/'),
    },
    module: {
        rules: [{
            test: /\.fs(x|proj)?$/,
            use: "fable-loader"
        }]
    },
    resolve: {
        modules: [nodeModulesDir]
    },
}