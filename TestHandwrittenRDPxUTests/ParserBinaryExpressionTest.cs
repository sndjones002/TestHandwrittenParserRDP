using System;
using System.Data;
using Newtonsoft.Json.Linq;
using TestHandwrittenRDP;

namespace TestHandwrittenRDPxUTests
{
    public class ParserBinaryExpressionTest
    {
        [Fact]
        public void Additive()
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
        public void AdditiveIdentifier()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"x + y;");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new ExpressionStatementRule(
                            new BinaryExpressionRule(
                                new BaseToken(ETokenType.ADDITIVE_OPERATOR, "+"),
                                new IdentifierRule("x"),
                                new IdentifierRule("y")
                            ))
                    })
                );
        }

        [Fact]
        public void AdditiveComplex()
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
        public void AdditiveWithMultiplicativeComplex()
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
        public void MultiplicativeComplex()
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
        public void WithParenthesis()
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
        public void WithParenthesisLeft()
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
        public void WithParenthesisComplexDivision()
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
        public void WithParenthesisComplexDivisionRight()
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
        public void WithNoRightParenthesis()
        {
            var parsedResult = () => ParserAssignHelper.AssignParser(@"(5 + 3;");

            var exception = Assert.Throws<SyntaxErrorException>(parsedResult);
            Assert.Equal($"Unexpected token: ';', expected: '{ETokenType.RIGHT_PARENTHESIS}'", exception.Message);
        }
    }
}

