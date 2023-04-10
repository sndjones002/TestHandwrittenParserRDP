using System;
using TestHandwrittenRDP;

namespace TestHandwrittenRDPxUTests
{
	public class ParserEqualityExpressionTest
	{
        [Fact]
        public void EqualityWithBooleanCheck()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"x > 0 == true;");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new ExpressionStatementRule(
                            new BinaryExpressionRule(
                                new BaseToken(ETokenType.EQUALITY_OPERATOR, "=="),
                                new BinaryExpressionRule(
                                    new BaseToken(ETokenType.RELATIONAL_OPERATOR, ">"),
                                    new IdentifierRule("x"),
                                    new NumericLiteralRule(0)
                                    ),
                                new BooleanLiteralRule(true)
                            ))
                    })
                );
        }

        [Fact]
        public void EqualityWithBooleanCheckRight()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"false == x > 0;");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new ExpressionStatementRule(
                            new BinaryExpressionRule(
                                new BaseToken(ETokenType.EQUALITY_OPERATOR, "=="),
                                new BooleanLiteralRule(false),
                                new BinaryExpressionRule(
                                    new BaseToken(ETokenType.RELATIONAL_OPERATOR, ">"),
                                    new IdentifierRule("x"),
                                    new NumericLiteralRule(0)
                                    )
                            ))
                    })
                );
        }

        [Fact]
        public void EqualityWithAdditiveCheckRight()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"false == x + y;");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new ExpressionStatementRule(
                            new BinaryExpressionRule(
                                new BaseToken(ETokenType.EQUALITY_OPERATOR, "=="),
                                new BooleanLiteralRule(false),
                                new BinaryExpressionRule(
                                    new BaseToken(ETokenType.ADDITIVE_OPERATOR, "+"),
                                    new IdentifierRule("x"),
                                    new IdentifierRule("y")
                                    )
                            ))
                    })
                );
        }

        //////

        [Fact]
        public void NonEqualityWithBooleanCheck()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"x >= 0 != true;");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new ExpressionStatementRule(
                            new BinaryExpressionRule(
                                new BaseToken(ETokenType.EQUALITY_OPERATOR, "!="),
                                new BinaryExpressionRule(
                                    new BaseToken(ETokenType.RELATIONAL_OPERATOR, ">="),
                                    new IdentifierRule("x"),
                                    new NumericLiteralRule(0)
                                    ),
                                new BooleanLiteralRule(true)
                            ))
                    })
                );
        }

        [Fact]
        public void NonEqualityWithBooleanCheckRight()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"null != x > 0;");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new ExpressionStatementRule(
                            new BinaryExpressionRule(
                                new BaseToken(ETokenType.EQUALITY_OPERATOR, "!="),
                                new NullLiteralRule(),
                                new BinaryExpressionRule(
                                    new BaseToken(ETokenType.RELATIONAL_OPERATOR, ">"),
                                    new IdentifierRule("x"),
                                    new NumericLiteralRule(0)
                                    )
                            ))
                    })
                );
        }

        [Fact]
        public void NonEqualityWithAdditiveCheckRight()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"false != x + y;");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new ExpressionStatementRule(
                            new BinaryExpressionRule(
                                new BaseToken(ETokenType.EQUALITY_OPERATOR, "!="),
                                new BooleanLiteralRule(false),
                                new BinaryExpressionRule(
                                    new BaseToken(ETokenType.ADDITIVE_OPERATOR, "+"),
                                    new IdentifierRule("x"),
                                    new IdentifierRule("y")
                                    )
                            ))
                    })
                );
        }
    }
}

