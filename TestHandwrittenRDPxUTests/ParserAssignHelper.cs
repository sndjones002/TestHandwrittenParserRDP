using System;
using System.Text.Json;
using TestHandwrittenRDP;

namespace TestHandwrittenRDPxUTests
{
	public static class ParserAssignHelper
	{
        public static BaseRule? AssignParser(string toParse)
        {
            return new RecursiveDescentParserForOOP().Parse(toParse);
        }

        public static BaseRule AssignAST_SingleExpression(BaseRule? baseLiteral)
        {
            return new ProgramRule(
                       new List<BaseRule>
                       {
                        new ExpressionStatementRule(baseLiteral)
                       });
        }
    }
}

