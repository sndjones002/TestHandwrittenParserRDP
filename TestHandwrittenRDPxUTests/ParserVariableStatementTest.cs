using System;
using TestHandwrittenRDP;

namespace TestHandwrittenRDPxUTests
{
	public class ParserVariableStatementTest
	{
        [Fact]
        public void Simple()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"let y;");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new VariableStatementRule(
                            new List<BaseRule>
                            {
                                new VariableDeclarationRule(
                                    new IdentifierRule("y"),
                                    null)
                            })
                    })
                );
        }

        [Fact]
        public void Multiple()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"let y, b;");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new VariableStatementRule(
                            new List<BaseRule>
                            {
                                new VariableDeclarationRule(
                                    new IdentifierRule("y"),
                                    null),
                                new VariableDeclarationRule(
                                    new IdentifierRule("b"),
                                    null)
                            })
                    })
                );
        }

        [Fact]
        public void SimpleWithAssignment()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"let y = 23;");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new VariableStatementRule(
                            new List<BaseRule>
                            {
                                new VariableDeclarationRule(
                                    new IdentifierRule("y"),
                                    new NumericLiteralRule(23))
                            })
                    })
                );
        }

        [Fact]
        public void MultipleWithSingleAssignment()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"let x, y = 9;");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new VariableStatementRule(
                            new List<BaseRule>
                            {
                                new VariableDeclarationRule(
                                    new IdentifierRule("x"),
                                    null),
                                new VariableDeclarationRule(
                                    new IdentifierRule("y"),
                                    new NumericLiteralRule(9))
                            })
                    })
                );
        }

        [Fact]
        public void MultipleWithAssignmentRecursive()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"let x = y = 9;");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new VariableStatementRule(
                            new List<BaseRule>
                            {
                                new VariableDeclarationRule(
                                    new IdentifierRule("x"),
                                    new AssignmentExpressionRule(
                                        new BaseToken(ETokenType.SIMPLE_ASSIGNMENT, "="),
                                        new IdentifierRule("y"),
                                        new NumericLiteralRule(9)
                                    )
                                ),
                            })
                    })
                );
        }

        [Fact]
        public void MultipleAssignment()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"let x = 2, y = 9;");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new VariableStatementRule(
                            new List<BaseRule>
                            {
                                new VariableDeclarationRule(
                                    new IdentifierRule("x"),
                                    new NumericLiteralRule(2)),
                                new VariableDeclarationRule(
                                    new IdentifierRule("y"),
                                    new NumericLiteralRule(9))
                            })
                    })
                );
        }
    }
}

