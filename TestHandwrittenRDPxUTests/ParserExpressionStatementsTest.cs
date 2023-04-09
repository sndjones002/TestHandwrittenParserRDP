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
                new BaseRuleList(
                    new BaseRule[] {
                        new BaseRule(new NumericLiteralRule(456), ELiteralType.ExpressionStatement)
                    }, ELiteralType.Program)
                );
        }

        [Fact]
        public void CompoundStatementsWithSemicolon()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"456;""hello"";");

            ParserAssertHelper.AssertAST(parsedResult,
                new BaseRuleList(
                    new BaseRule[] {
                        new BaseRule(new NumericLiteralRule(456), ELiteralType.ExpressionStatement),
                        new BaseRule(new StringLiteralRule("hello"), ELiteralType.ExpressionStatement)
                    }, ELiteralType.Program)
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
                new BaseRuleList(
                    new BaseRule[] {
                        new BaseRule(new NumericLiteralRule(456), ELiteralType.ExpressionStatement),
                        new BaseRule(new StringLiteralRule("hello"), ELiteralType.ExpressionStatement)
                    }, ELiteralType.Program)
                );
        }

        [Fact]
        public void SimpleStatementsWithoutSemicolonException()
        {
            var parsedResult = () => ParserAssignHelper.AssignParser(@"
      /* This is a number
       * with Multiline comment
      */
      456;
      // A String Expression
      ""hello""
      "
            );

            var exception = Assert.Throws<SyntaxErrorException>(parsedResult);
            Assert.Equal($"Unexpected end of input, expected: '{ETokenType.SEMICOLON}'", exception.Message);
        }
    }
}

