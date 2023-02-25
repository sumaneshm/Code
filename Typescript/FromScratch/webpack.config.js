const path = require('path');

module.exports = {
    entry : './src/index.ts',
    mode:'development',
    module:{
        rules: [
            {
                test:/\.tsx?$/,
                use: 'ts-loader',
                exclude: /node_modules/,
            },
        ],
    },
    resolve: {
        extensions: ['.ts', '.js'],
    },
    output: {
        filename: 'bundle.js',
        path: path.resolve(__dirname, 'bin'),
    },
    devServer: {
        static: 
            [
                {directory: path.resolve(__dirname, 'public')},
                {directory: path.resolve(__dirname, 'bin')},                
            ]

            // directory: [path.resolve(__dirname, "public"), path.resolve(__dirname, 'bin')],
            ,
          port: 7777,
    }

} 