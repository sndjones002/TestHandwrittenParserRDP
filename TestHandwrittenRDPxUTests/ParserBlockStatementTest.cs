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

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new BlockStatementRule(new List<BaseRule>())
                    })
                );
        }

        [Fact]
        public void EmptyStatementInBlock()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"{;}");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new BlockStatementRule(
                            new List<BaseRule> {
                                new EmptyStatementRule()
                            })
                    })
                   );
        }

        [Fact]
        public void SimpleBlockStatementWithLiterals()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"{456;
""hello"";}");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new BlockStatementRule(new List<BaseRule>
                        {
                            new ExpressionStatementRule(new NumericLiteralRule(456)),
                            new ExpressionStatementRule(new StringLiteralRule("hello"))
                        })
                    })
                );
        }

        [Fact]
        public void SimpleBlockStatementWithNestedBlocks()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"
{
    456;
    {
        ""hello"";
    }
}");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new BlockStatementRule(new List<BaseRule>
                        {
                            new ExpressionStatementRule(new NumericLiteralRule(456)),
                            new BlockStatementRule(new List<BaseRule>
                            {
                                new ExpressionStatementRule(new StringLiteralRule("hello"))
                            })
                        })
                    })
                );
        }
    }
}

