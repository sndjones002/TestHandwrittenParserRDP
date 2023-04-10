using System;
using TestHandwrittenRDP;

namespace TestHandwrittenRDPxUTests
{
	public class ParserConditionalIfStatementTest
	{
		[Fact]
		public void Simple()
		{
            var parsedResult = ParserAssignHelper.AssignParser(@"
		if(x) {
		  x = 1;
		}
		else {
		  x = 2;
		}
");
			ParserAssertHelper.AssertAST(parsedResult,
				new ProgramRule(
					new List<BaseRule> {
						new IfStatementRule(
							new IdentifierRule("x"),
							new BlockStatementRule(
								new List<BaseRule>
								{
									new ExpressionStatementRule(
										new AssignmentExpressionRule(
											new BaseToken(ETokenType.SIMPLE_ASSIGNMENT, "="),
											new IdentifierRule("x"),
											new NumericLiteralRule(1)
											)
										)
								}),
                            new BlockStatementRule(
                                new List<BaseRule>
                                {
                                    new ExpressionStatementRule(
                                        new AssignmentExpressionRule(
                                            new BaseToken(ETokenType.SIMPLE_ASSIGNMENT, "="),
                                            new IdentifierRule("x"),
                                            new NumericLiteralRule(2)
                                            )
                                        )
                                })
                            )
					})
				); ;
        }

        [Fact]
        public void SimpleOnlyIf()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"
		if(x) {
		  x = 1;
		}
");
            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new IfStatementRule(
                            new IdentifierRule("x"),
                            new BlockStatementRule(
                                new List<BaseRule>
                                {
                                    new ExpressionStatementRule(
                                        new AssignmentExpressionRule(
                                            new BaseToken(ETokenType.SIMPLE_ASSIGNMENT, "="),
                                            new IdentifierRule("x"),
                                            new NumericLiteralRule(1)
                                            )
                                        )
                                }),
                            null
                            )
                    })
                ); ;
        }

        [Fact]
        public void SimpleOnlyIfNoBraces()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"
		if(x)  x = 1;
");
            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new IfStatementRule(
                            new IdentifierRule("x"),
                            new ExpressionStatementRule(
                                new AssignmentExpressionRule(
                                    new BaseToken(ETokenType.SIMPLE_ASSIGNMENT, "="),
                                    new IdentifierRule("x"),
                                    new NumericLiteralRule(1)
                                    )
                                ),
                            null
                            )
                    })
                ); ;
        }

        [Fact]
        public void ComplexNestedIf()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"
		if(x)  if(y) {} else { y + 1; }
");
            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new IfStatementRule(
                            new IdentifierRule("x"),
                            new IfStatementRule(
                                new IdentifierRule("y"),
                                new BlockStatementRule(new List<BaseRule>()),
                                new BlockStatementRule(
                                    new List<BaseRule>
                                    {
                                        new ExpressionStatementRule(
                                            new BinaryExpressionRule(
                                                new BaseToken(ETokenType.ADDITIVE_OPERATOR, "+"),
                                                new IdentifierRule("y"),
                                                new NumericLiteralRule(1)
                                            ))
                                    })),
                            null
                            )
                    })
                ); ;
        }

        [Fact]
        public void ComplexNestedIfWithExpression()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"
		if(x > 10)  if(y) {} else { y + 1; }
");
            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new IfStatementRule(
                                new BinaryExpressionRule(
                                    new BaseToken(ETokenType.RELATIONAL_OPERATOR, ">"),
                                    new IdentifierRule("x"),
                                    new NumericLiteralRule(10)
                                ),
                            new IfStatementRule(
                                new IdentifierRule("y"),
                                new BlockStatementRule(new List<BaseRule>()),
                                new BlockStatementRule(
                                    new List<BaseRule>
                                    {
                                        new ExpressionStatementRule(
                                            new BinaryExpressionRule(
                                                new BaseToken(ETokenType.ADDITIVE_OPERATOR, "+"),
                                                new IdentifierRule("y"),
                                                new NumericLiteralRule(1)
                                            ))
                                    })),
                            null
                            )
                    })
                ); ;
        }
    }
}

