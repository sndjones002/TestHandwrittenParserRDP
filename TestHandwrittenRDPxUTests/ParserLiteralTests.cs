using System;
using System.Data;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using TestHandwrittenRDP;

namespace TestHandwrittenRDPxUTests;

public class ParserLiteralTests
{
    [Fact]
    public void NumericLiteral()
    {
        var parsedResult = ParserAssignHelper.AssignParser("4567;");

        ParserAssertHelper.AssertAST(parsedResult, ParserAssignHelper.AssignAST_SingleExpression(new NumericLiteralRule(4567)));
    }

    [Fact]
    public void StringLiteral()
    {
        var parsedResult = ParserAssignHelper.AssignParser("\"hello\";");

        ParserAssertHelper.AssertAST(parsedResult, ParserAssignHelper.AssignAST_SingleExpression(new StringLiteralRule("hello")));
    }

    [Fact]
    public void WithParenthesisSimplest()
    {
        var parsedResult = ParserAssignHelper.AssignParser(@"((5));");

        ParserAssertHelper.AssertAST(parsedResult,
            new ProgramRule(
                new List<BaseRule> {
                        new ExpressionStatementRule(new NumericLiteralRule(5))
                })
            );
    }
}
