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
        test: /bootstrap[/\\]dist[/\\]js[/\\]bootstrap\.js$/,
        use: 'imports-loader?jQuery=jquery'
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
      }),
    new MiniCssExtractPlugin({
      filename: '[name].css'
    })
  ]
};
