using System;
using TestHandwrittenRDP;

namespace TestHandwrittenRDPxUTests
{
	public class ParserBlockStatementTest
	{
        [Fact]
        public void SimpleBlockStatement()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"{}");

            ParserAssertHelper.AssertAST(parsedResult, new ProgramLiteral(new BaseLiteral[] { new BlockStatementRule(new StatementRule[] { }) }));
        }
    }
}

