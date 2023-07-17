const path = require('path');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const webpack = require('webpack');

module.exports = {
    entry: {
        bundle: [
            './wwwroot/js/site.js',
            './wwwroot/scss/site.scss'
        ]
    },
    output: {
        filename: '[name].js',
        path: path.resolve(__dirname, 'wwwroot/dist')
    },
    module: {
        rules: [
            {
                test: /\.s[ac]ss$/i,
                use: [
                    MiniCssExtractPlugin.loader,
                    'css-loader',
                    'postcss-loader',
                    'sass-loader'
                ]
            },
            {
                test: /bootstrap[/\\]dist[/\\]js[/\\]bootstrap\.bundle\.js$/,
                loader: 'imports-loader',
                options: {
                    imports: {
                        moduleName: 'bootstrap',
                        syntax: 'default',
                        name: 'bootstrap',
                    },
                },
            },
            {
                test: /\.(png|svg|jpg|jpeg|gif)$/i,
                type: 'asset/resource'
            },
            {
                test: /\.(woff|woff2|eot|ttf|otf)$/i,
                type: 'asset/resource'
            }
        ]
    },
    plugins: [
        new webpack.ProvidePlugin({
            $: 'jquery',
            jQuery: 'jquery',
            'window.jQuery': 'jquery',
            bootstrap: 'bootstrap/dist/js/bootstrap.bundle'
        }),
        new MiniCssExtractPlugin({
            filename: '[name].css'
        })
    ],
    resolve: {
        modules: [path.resolve(__dirname, 'node_modules')],
    }
};
