using Jint;
using Jint.Native;
using Jint.Native.Json;
using System.Reflection;

const string codeToParse = "interface I { prop: string }";

string babelParserCode;
using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"jint_parse_ts.babel-parser.js")!;
using var streamReader = new StreamReader(stream);
{
    babelParserCode = streamReader.ReadToEnd();
}

var babelParserModule = Engine.PrepareModule(babelParserCode);

var engine = new Engine(options => { });
engine.Modules.Add("@babel/parser", b => b.AddModule(babelParserModule));

var ns = engine.Modules.Import("@babel/parser");
var parse = ns.Get("parse");
var ast = parse.Call(codeToParse, JsValue.FromObject(engine, new { plugins = new string[] { "typescript" } }));

var serializer = new JsonSerializer(engine);
Console.WriteLine(serializer.Serialize(ast, JsValue.Undefined, "  "));
