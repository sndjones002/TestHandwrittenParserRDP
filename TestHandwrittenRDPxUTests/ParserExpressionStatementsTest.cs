using System;
using System.Data;
using TestHandwrittenRDP;

namespace TestHandwrittenRDPxUTests
{
	public class ParserExpressionStatementsTest
	{
        [Fact]
        public void SimpleStatementsWithSemicolon()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"456;");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new ExpressionStatementRule(new NumericLiteralRule(456))
                    })
                );
        }

        [Fact]
        public void CompoundStatementsWithSemicolon()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"456;""hello"";");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new ExpressionStatementRule(new NumericLiteralRule(456)),
                        new ExpressionStatementRule(new StringLiteralRule("hello"))
                    })
                );
        }

        [Fact]
        public void CompoundStatementsWithComments()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"
      /* This is a number
       * with Multiline comment
      */
      456;
      // A String Expression
      ""hello"";
      ");

            ParserAssertHelper.AssertAST(parsedResult,
                new ProgramRule(
                    new List<BaseRule> {
                        new ExpressionStatementRule(new NumericLiteralRule(456)),
                        new ExpressionStatementRule(new StringLiteralRule("hello"))
                    })
                );
        }

        [Fact]
        public void SimpleStatementsWithoutSemicolonException()
        {
            var parsedResult = () => ParserAssignHelper.AssignParser(@"456");

            var exception = Assert.Throws<SyntaxErrorException>(parsedResult);
            Assert.Equal($"Unexpected end of input, expected: '{ETokenType.SEMICOLON}'", exception.Message);
        }
    }
}

