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

        public static BaseRuleList AssignAST_SingleExpression(BaseRule? baseLiteral)
        {
            return new BaseRuleList(
                       new BaseRule[]
                       {
                        new BaseRule(baseLiteral, ELiteralType.ExpressionStatement)
                       },
                       ELiteralType.Program
                   );
        }
    }
}

