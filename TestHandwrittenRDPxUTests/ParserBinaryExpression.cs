using System;
using System.Data;
using Newtonsoft.Json.Linq;
using TestHandwrittenRDP;

namespace TestHandwrittenRDPxUTests
{
    public class ParserBinaryExpression
    {
        [Fact]
        public void BinaryStatementPlus()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"2 + 3;");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new ExpressionStatementRule(
                            new BinaryExpressionRule(
                                new BaseToken(ETokenType.ADDITIVE_OPERATOR, "+"),
                                new NumericLiteralRule(2),
                                new NumericLiteralRule(3)
                            ))
                    })
                );
        }

        [Fact]
        public void BinaryStatementAdditiveComplex()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"5 + 3 - 4;");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new ExpressionStatementRule(
                            new BinaryExpressionRule(
                                new BaseToken(ETokenType.ADDITIVE_OPERATOR, "-"),
                                new BinaryExpressionRule(
                                    new BaseToken(ETokenType.ADDITIVE_OPERATOR, "+"),
                                    new NumericLiteralRule(5),
                                    new NumericLiteralRule(3)
                                ),
                                new NumericLiteralRule(4)
                            ))
                    })
                );
        }

        [Fact]
        public void BinaryStatementAdditiveWithMultiplicativeComplex()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"5 + 3 * 4;");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new ExpressionStatementRule(
                            new BinaryExpressionRule(
                                new BaseToken(ETokenType.ADDITIVE_OPERATOR, "+"),
                                new NumericLiteralRule(5),
                                new BinaryExpressionRule(
                                    new BaseToken(ETokenType.MULTIPLICATIVE_OPERATOR, "*"),
                                    new NumericLiteralRule(3),
                                    new NumericLiteralRule(4)
                                )
                            ))
                    })
                );
        }

        [Fact]
        public void BinaryStatementMultiplicativeComplex()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"5 * 3 * 4;");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new ExpressionStatementRule(
                            new BinaryExpressionRule(
                                new BaseToken(ETokenType.MULTIPLICATIVE_OPERATOR, "*"),
                                new BinaryExpressionRule(
                                    new BaseToken(ETokenType.MULTIPLICATIVE_OPERATOR, "*"),
                                    new NumericLiteralRule(5),
                                    new NumericLiteralRule(3)
                                ),
                                new NumericLiteralRule(4)
                            ))
                    })
                );
        }

        [Fact]
        public void BinaryStatementWithParenthesis()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"(5 + 3);");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new ExpressionStatementRule(
                            new BinaryExpressionRule(
                                new BaseToken(ETokenType.ADDITIVE_OPERATOR, "+"),
                                new NumericLiteralRule(5),
                                new NumericLiteralRule(3)
                            ))
                    })
                );
        }

        [Fact]
        public void BinaryStatementWithParenthesisComplex()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"(5 + 3) * 4;");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new ExpressionStatementRule(
                            new BinaryExpressionRule(
                                new BaseToken(ETokenType.MULTIPLICATIVE_OPERATOR, "*"),
                                new BinaryExpressionRule(
                                    new BaseToken(ETokenType.ADDITIVE_OPERATOR, "+"),
                                    new NumericLiteralRule(5),
                                    new NumericLiteralRule(3)
                                ),
                                new NumericLiteralRule(4)
                            ))
                    })
                );
        }

        [Fact]
        public void BinaryStatementWithParenthesisComplexDivision()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"(5 + 3) / 4;");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new ExpressionStatementRule(
                            new BinaryExpressionRule(
                                new BaseToken(ETokenType.MULTIPLICATIVE_OPERATOR, "/"),
                                new BinaryExpressionRule(
                                    new BaseToken(ETokenType.ADDITIVE_OPERATOR, "+"),
                                    new NumericLiteralRule(5),
                                    new NumericLiteralRule(3)
                                ),
                                new NumericLiteralRule(4)
                            ))
                    })
                );
        }

        [Fact]
        public void BinaryStatementWithParenthesisComplexDivisionChangeOrder()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"4 / (5 + 3);");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new ExpressionStatementRule(
                            new BinaryExpressionRule(
                                new BaseToken(ETokenType.MULTIPLICATIVE_OPERATOR, "/"),
                                new NumericLiteralRule(4),
                                new BinaryExpressionRule(
                                    new BaseToken(ETokenType.ADDITIVE_OPERATOR, "+"),
                                    new NumericLiteralRule(5),
                                    new NumericLiteralRule(3)
                                )
                            ))
                    })
                );
        }

        [Fact]
        public void BinaryStatementWithParenthesisSimplest()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"(5);");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new ExpressionStatementRule(new NumericLiteralRule(5))
                    })
                );
        }

        [Fact]
        public void BinaryStatementWithNoRightParenthesis()
        {
            var parsedResult = () => ParserAssignHelper.AssignParser(@"(5 + 3;");

            var exception = Assert.Throws<SyntaxErrorException>(parsedResult);
            Assert.Equal($"Unexpected token: ';', expected: '{ETokenType.RIGHT_PARENTHESIS}'", exception.Message);
        }
    }
}

