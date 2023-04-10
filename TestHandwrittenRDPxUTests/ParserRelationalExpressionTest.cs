using System;
using TestHandwrittenRDP;

namespace TestHandwrittenRDPxUTests
{
	public class ParserRelationalExpressionTest
	{
        [Fact]
        public void GreaterThan()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"2 > 3;");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new ExpressionStatementRule(
                            new BinaryExpressionRule(
                                new BaseToken(ETokenType.RELATIONAL_OPERATOR, ">"),
                                new NumericLiteralRule(2),
                                new NumericLiteralRule(3)
                            ))
                    })
                );
        }

        [Fact]
        public void GreaterThanEqual()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"2 >= x;");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new ExpressionStatementRule(
                            new BinaryExpressionRule(
                                new BaseToken(ETokenType.RELATIONAL_OPERATOR, ">="),
                                new NumericLiteralRule(2),
                                new IdentifierRule("x")
                            ))
                    })
                );
        }

        [Fact]
        public void LessThanEqual()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"2 <= x;");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new ExpressionStatementRule(
                            new BinaryExpressionRule(
                                new BaseToken(ETokenType.RELATIONAL_OPERATOR, "<="),
                                new NumericLiteralRule(2),
                                new IdentifierRule("x")
                            ))
                    })
                );
        }

        [Fact]
        public void RelationalLowerPrecedenceThanAdditive()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"x > y + 3;");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new ExpressionStatementRule(
                            new BinaryExpressionRule(
                                new BaseToken(ETokenType.RELATIONAL_OPERATOR, ">"),
                                new IdentifierRule("x"),
                                new BinaryExpressionRule(
                                    new BaseToken(ETokenType.ADDITIVE_OPERATOR, "+"),
                                    new IdentifierRule("y"),
                                    new NumericLiteralRule(3)
                                    )
                            ))
                    })
                );
        }
    }
}

