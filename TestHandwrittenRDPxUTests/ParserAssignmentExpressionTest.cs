using System;
using System.Data;
using TestHandwrittenRDP;

namespace TestHandwrittenRDPxUTests
{
	public class ParserAssignmentExpressionTest
	{
        [Fact]
        public void Simplest()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"myNumber = 2;");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new ExpressionStatementRule(
                            new AssignmentExpressionRule(
                                new BaseToken(ETokenType.SIMPLE_ASSIGNMENT, "="),
                                new IdentifierRule("myNumber"),
                                new NumericLiteralRule(2)
                            ))
                    })
                );
        }

        [Fact]
        public void SimplestChained()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"myNumber = myobject = 42;");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new ExpressionStatementRule(
                            new AssignmentExpressionRule(
                                new BaseToken(ETokenType.SIMPLE_ASSIGNMENT, "="),
                                new IdentifierRule("myNumber"),
                                new AssignmentExpressionRule(
                                    new BaseToken(ETokenType.SIMPLE_ASSIGNMENT, "="),
                                    new IdentifierRule("myobject"),
                                    new NumericLiteralRule(42)
                                    )
                            ))
                    })
                );
        }

        [Fact]
        public void SimplestWithExpression()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"x = y + 2;");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new ExpressionStatementRule(
                            new AssignmentExpressionRule(
                                new BaseToken(ETokenType.SIMPLE_ASSIGNMENT, "="),
                                new IdentifierRule("x"),
                                new BinaryExpressionRule(
                                    new BaseToken(ETokenType.ADDITIVE_OPERATOR, "+"),
                                    new IdentifierRule("y"),
                                    new NumericLiteralRule(2)
                                )
                            ))
                    })
                );
        }

        [Fact]
        public void MultiplicativeAssignmentWithExpression()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"x /= 2 - y;");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new ExpressionStatementRule(
                            new AssignmentExpressionRule(
                                new BaseToken(ETokenType.COMPLEX_ASSIGNMENT, "/="),
                                new IdentifierRule("x"),
                                new BinaryExpressionRule(
                                    new BaseToken(ETokenType.ADDITIVE_OPERATOR, "-"),
                                    new NumericLiteralRule(2),
                                    new IdentifierRule("y")
                                )
                            ))
                    })
                );
        }

        [Fact]
        public void InvalidLeftHandSide()
        {
            var parsedResult = () => ParserAssignHelper.AssignParser(@"45 = 45;");

            var exception = Assert.Throws<SyntaxErrorException>(parsedResult);
            Assert.Equal("Invalid left-hand side in assignment expression", exception.Message);
        }
    }
}