using System;
using TestHandwrittenRDP;

namespace TestHandwrittenRDPxUTests
{
	public class ParserLogicalExpressionTest
	{
		[Fact]
		public void Simple()
		{
            var parsedResult = ParserAssignHelper.AssignParser(@"x && 10;");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new ExpressionStatementRule(
                            new LogicalExpressionRule(
                                new BaseToken(ETokenType.LOGICAL_AND, "&&"),
                                new IdentifierRule("x"),
                                new NumericLiteralRule(10)
                            ))
                    })
                );
        }

        [Fact]
        public void LogicalANDWithBinary()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"x > 8 && y + 10;");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new ExpressionStatementRule(
                            new LogicalExpressionRule(
                                new BaseToken(ETokenType.LOGICAL_AND, "&&"),
                                new BinaryExpressionRule(
                                    new BaseToken(ETokenType.RELATIONAL_OPERATOR, ">"),
                                    new IdentifierRule("x"),
                                    new NumericLiteralRule(8)
                                    ),
                                new BinaryExpressionRule(
                                    new BaseToken(ETokenType.ADDITIVE_OPERATOR, "+"),
                                    new IdentifierRule("y"),
                                    new NumericLiteralRule(10)
                                    )
                            ))
                    })
                );
        }

        [Fact]
        public void LogicalORWithANDWithBinary()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"x > 8 && y + 10 || z * 2;");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new ExpressionStatementRule(
                            new LogicalExpressionRule(
                                new BaseToken(ETokenType.LOGICAL_OR, "||"),
                                new LogicalExpressionRule(
                                    new BaseToken(ETokenType.LOGICAL_AND, "&&"),
                                    new BinaryExpressionRule(
                                        new BaseToken(ETokenType.RELATIONAL_OPERATOR, ">"),
                                        new IdentifierRule("x"),
                                        new NumericLiteralRule(8)
                                        ),
                                    new BinaryExpressionRule(
                                        new BaseToken(ETokenType.ADDITIVE_OPERATOR, "+"),
                                        new IdentifierRule("y"),
                                        new NumericLiteralRule(10)
                                        )
                                ),
                                new BinaryExpressionRule(
                                    new BaseToken(ETokenType.MULTIPLICATIVE_OPERATOR, "*"),
                                    new IdentifierRule("z"),
                                    new NumericLiteralRule(2)
                                    )
                                )
                            )
                    })
                );
        }
    }
}

