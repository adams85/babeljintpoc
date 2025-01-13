using Acornima;
using BenchmarkDotNet.Attributes;
using Jint;
using Jint.Native;
using System.IO;
using System.Reflection;

namespace jint_parse_benchmark;

[MemoryDiagnoser]
public class ParseJsBenchmark
{
    private string _babelParserCode;
    private JsValue _babelParser = null!;
    private JsValue _babelParserOptions = null!;
    private Parser _acornimaParser = null!;

    [GlobalSetup]
    public void Setup()
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"jint_parse_benchmark.babel-parser.js")!;
        using var streamReader = new StreamReader(stream);
        {
            _babelParserCode = streamReader.ReadToEnd();
        }

        var babelParserModule = Engine.PrepareModule(_babelParserCode);

        var engine = new Engine(options => { });
        engine.Modules.Add("@babel/parser", b => b.AddModule(babelParserModule));

        var ns = engine.Modules.Import("@babel/parser");
        _babelParser = ns.Get("parse");

        _babelParserOptions = JsValue.FromObject(engine, new { sourceType = "module" });

        _acornimaParser = new Parser();
    }

    [Benchmark]
    public int AcornimaParse()
    {
        var ast = _acornimaParser.ParseModule(_babelParserCode);
        return ast.Body.Count;
    }

    [Benchmark]
    public int BabelParse()
    {
        var ast = _babelParser.Call(_babelParserCode, _babelParserOptions);
        return (int)ast.Get("program").Get("body").AsArray().Length;
    }
}
