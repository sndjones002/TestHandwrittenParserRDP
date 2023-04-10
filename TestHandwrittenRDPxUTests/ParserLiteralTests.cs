using System;
using System.Data;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using TestHandwrittenRDP;

namespace TestHandwrittenRDPxUTests;

public class ParserLiteralTests : ParserUnitTestModule
{
    [Fact]
    public void NumericLiteral()
    {
        var parsedResult = Parser("4567;");

        AssertAST(parsedResult, Program(ExprStmt(Int(4567))) );
    }

    [Fact]
    public void StringLiteral()
    {
        var parsedResult = Parser("\"hello\";");

        AssertAST(parsedResult, Program(ExprStmt(Str("hello"))) );
    }

    [Fact]
    public void WithParenthesisSimplest()
    {
        var parsedResult = Parser(@"((5));");

        AssertAST(parsedResult, Program(ExprStmt(Int(5))) );
    }
}
