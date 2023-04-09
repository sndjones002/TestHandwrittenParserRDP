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

        [Fact]
        public void EmptyStatement()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@";");

            ParserAssertHelper.AssertAST(parsedResult, new ProgramLiteral(new BaseLiteral[] { new StatementRule(ELiteralType.EmptyStatement) }));
        }

        [Fact]
        public void EmptyStatementInBlock()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"{;}");

            ParserAssertHelper.AssertAST(parsedResult, new ProgramLiteral(new BaseLiteral[] {
                new BlockStatementRule(new StatementRule[] { new StatementRule(ELiteralType.EmptyStatement) })
                
            }));
        }

        [Fact]
        public void SimpleBlockStatementWithLiterals()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"{456;
""hello"";}");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramLiteral(
                    new BaseLiteral[] {
                        new BlockStatementRule(new StatementRule[]
                        {
                            new ExpressionStatementRule(
                                new NumericLiteral(456)
                                ),
                            new ExpressionStatementRule(
                                new StringLiteral("hello")
                                )
                        }
                        )
                    }
                    )
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
                new ProgramLiteral(
                    new BaseLiteral[] {
                        new BlockStatementRule(new StatementRule[]
                        {
                            new ExpressionStatementRule(
                                new NumericLiteral(456)
                                ),
                            new BlockStatementRule(new StatementRule[]
                            {
                                new ExpressionStatementRule(
                                    new StringLiteral("hello")
                                    )
                            })
                        }
                        )
                    }
                    )
                );
        }
    }
}

