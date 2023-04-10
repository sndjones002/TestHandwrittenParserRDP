using System;
using TestHandwrittenRDP;

namespace TestHandwrittenRDPxUTests
{
	public class ParserBlockStatementTest : ParserUnitTestModule
	{
        [Fact]
        public void SimpleBlockStatement()
        {
            var parsedResult = Parser(@"{}");

            AssertAST(parsedResult, Program(Block()));
        }

        [Fact]
        public void EmptyStatementInBlock()
        {
            var parsedResult = Parser(@"{;}");

            AssertAST(parsedResult, Program(Block(Empty())) );
        }

        [Fact]
        public void SimpleBlockStatementWithLiterals()
        {
            var parsedResult = Parser(@"{456;
""hello"";}");

            AssertAST(parsedResult,
                Program(Block(ExprStmt(Int(456)), ExprStmt(Str("hello"))))
                );
        }

        [Fact]
        public void SimpleBlockStatementWithNestedBlocks()
        {
            var parsedResult = Parser(@"
{
    456;
    {
        ""hello"";
    }
}");

            AssertAST(parsedResult,
                Program(
                    Block(
                        ExprStmt(Int(456)),
                        Block(ExprStmt(Str("hello")))
                        )
                    )
                );
        }
    }
}

