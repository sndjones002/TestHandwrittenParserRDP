using System;
using System.Text.Json;
using TestHandwrittenRDP;

namespace TestHandwrittenRDPxUTests
{
	public static class ParserAssignHelper
	{
        public static BaseLiteral? AssignParser(string toParse)
        {
            return new MyParser().Parse(toParse);
        }

        public static ProgramLiteral AssignAST_SingleExpression(BaseLiteral? baseLiteral)
        {
            return new ProgramLiteral(
                       new BaseLiteral[]
                       {
                       new ExpressionStatementRule(baseLiteral)
                       }
                   );
        }
    }
}

