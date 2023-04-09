using System;
using TestHandwrittenRDP;

namespace TestHandwrittenRDPxUTests
{
	public class ParserBlockStatementTest
	{
        [Fact]
        public void SimpleBlockStatement()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"{}");

            ParserAssertHelper.AssertAST(parsedResult,
                new BaseRuleList(
                    new BaseRule[] {
                        new BaseRuleList(
                            new BaseRule[] {
                            }, ELiteralType.BlockStatement)
                    }, ELiteralType.Program)
                );
        }

        [Fact]
        public void EmptyStatement()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@";");

            ParserAssertHelper.AssertAST(parsedResult,
                new BaseRuleList(
                    new BaseRule[] {
                        new BaseRule(null, ELiteralType.EmptyStatement)
                    }, ELiteralType.Program)
                );
        }

        [Fact]
        public void EmptyStatementInBlock()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"{;}");

            ParserAssertHelper.AssertAST(parsedResult,
                new BaseRuleList(
                    new BaseRule[] {
                        new BaseRuleList(
                            new BaseRule[] {
                                new BaseRule(null, ELiteralType.EmptyStatement)
                            }, ELiteralType.BlockStatement)
                
                    }, ELiteralType.Program)
                   );
        }

        [Fact]
        public void SimpleBlockStatementWithLiterals()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"{456;
""hello"";}");

            ParserAssertHelper.AssertAST(parsedResult,
                new BaseRuleList(
                    new BaseRule[] {
                        new BaseRuleList(new BaseRule[]
                        {
                            new BaseRule(new NumericLiteralRule(456), ELiteralType.ExpressionStatement),
                            new BaseRule(new StringLiteralRule("hello"), ELiteralType.ExpressionStatement)
                        }, ELiteralType.BlockStatement)
                    }, ELiteralType.Program)
                );
        }

        [Fact]
        public void SimpleBlockStatementWithNestedBlocks()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"
{
    456;
    {
        ""hello"";
    }
}");

            ParserAssertHelper.AssertAST(parsedResult,
                new BaseRuleList(
                    new BaseRule[] {
                        new BaseRuleList(new BaseRule[]
                        {
                            new BaseRule(new NumericLiteralRule(456), ELiteralType.ExpressionStatement),
                            new BaseRuleList(new BaseRule[]
                            {
                                new BaseRule(new StringLiteralRule("hello"), ELiteralType.ExpressionStatement)
                            }, ELiteralType.BlockStatement)
                        }, ELiteralType.BlockStatement)
                    }, ELiteralType.Program)
                );
        }
    }
}

