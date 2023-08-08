const path = require('path');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');

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
                    'sass-loader'
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
                type: 'asset/resource',
                generator: {
                    filename: 'webfonts/[name][ext]'
                },
                include: [
                    path.resolve(__dirname, 'node_modules/@fortawesome/fontawesome-free'),
                ]
            }
        ]
    },
    plugins: [
        new MiniCssExtractPlugin({
            filename: '[name].css',
        })
    ],
    resolve: {
        modules: [path.resolve(__dirname, 'node_modules')]
    },
    devtool: 'source-map'
};
