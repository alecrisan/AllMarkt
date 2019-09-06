const path = require("path");
const VueLoaderPlugin = require("vue-loader/lib/plugin");
const HtmlWebpackPlugin = require("html-webpack-plugin");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");

const isProduction = process.argv.indexOf("-p", 2) >= 0;
module.exports = {
    mode: isProduction ? "production" : "development",
    devtool: isProduction ? false : "inline-source-map",
    context: path.resolve(__dirname, 'ClientApp'),
    entry: {
        app: path.join("folder", "index.js").replace(/^folder/, ".")
    },
    output: {
        path: path.join(__dirname, "wwwroot"),
        publicPath: "wwwroot",
        filename: "[name].js",
    },
    resolve: {
        alias: {
            "vue$": "vue/dist/vue.esm.js"
        },
        extensions: [".js", ".vue"]
    },
    module: {
        rules: [
            {
                test: /\.js$/,
                loader: "babel-loader",
                exclude: /node_modules/
            },
            {
                test: /\.vue$/,
                loader: "vue-loader"
            },
            {
                test: /\.scss$/,
                use: [
                    MiniCssExtractPlugin.loader,
                    "css-loader",
                    {
                        loader: "postcss-loader",
                        options: {
                            plugins: function () {
                                return [
                                    require("autoprefixer")
                                ];
                            }
                        }
                    },
                    "sass-loader"
                ]
            }
        ]
    },
    plugins: [
        new VueLoaderPlugin(),
        new HtmlWebpackPlugin({
            title: "VueJs Intoduction",
            minify: isProduction,
            hash: true
        }),
        new MiniCssExtractPlugin()
    ]
};