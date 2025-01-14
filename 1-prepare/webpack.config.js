export default {
  mode: "production",
  entry: {
    "babel-parser": "./node_modules/@babel/parser/lib/index.js",
  },
  optimization: {
    minimize: true,
  },
  output: {
    filename: "[name].js",
    library: { type: "module" },
  },
  experiments: {
    outputModule: true,
  },
  // module: {
  //   rules: [
  //     {
  //       test: /\.[c|m]?js$/,
  //       use: [{
  //         loader: "babel-loader",
  //         options: {
  //           "presets": ["@babel/preset-env"],
  //         },
  //       }],
  //     },
  //   ],
  // },
};
