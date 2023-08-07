const path = require('path');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const webpack = require('webpack');

module.exports = {
    mode: 'development',
    entry: {
        bundle: [
            './wwwroot/js/site.js',
            './wwwroot/scss/site.scss'
        ]
    },
    output: {
        filename: 'bundle.js',
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
                    {
                        loader: 'sass-loader',
                        options: {
                            sassOptions: {
                                includePaths: [path.resolve(__dirname, 'node_modules')]
                            }
                        }
                    }
                ]
            },
            {
                test: /\.css$/,
                use: [MiniCssExtractPlugin.loader, 'css-loader']
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
                test: /\.(woff(2)?|ttf|eot|svg)(\?v=\d+\.\d+\.\d+)?$/,
                use: [
                    {
                        loader: 'file-loader',
                        options: {
                            name: '[name].[ext]',
                            outputPath: 'webfonts/'
                        }
                    }
                ],
                include: [
                    path.resolve(__dirname, 'node_modules/@fortawesome/fontawesome-free'),
                ]
            }
        ]
    },
    plugins: [
        new webpack.ProvidePlugin({
            $: 'jquery',
            jQuery: 'jquery',
            'window.jQuery': 'jquery',
            bootstrap: 'bootstrap/dist/js/bootstrap.bundle',
            select2: 'select2',
        }),
        new MiniCssExtractPlugin({
            filename: '[name].css',
        })
    ],
    resolve: {
        modules: [path.resolve(__dirname, 'node_modules')]
    }
};
