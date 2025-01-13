import { parse } from "./dist/babel-parser.js";

const codeToParse = "interface I { prop: string }";

const ast = parse(codeToParse, { plugins: ["typescript"] });

console.log(JSON.stringify(ast, void 0, " "));