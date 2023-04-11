using System;
namespace TestHandwrittenRDPxUTests
{
	public class ParserUnaryExpressionTest : ParserUnitTestModule
    {
		[Fact]
		public void minus_var()
		{
            var parsedResult = Parser("-x;");

            AssertAST(parsedResult, Program(ExprStmt( Unary(MINUS, Id("x")) ) ) );
        }

        [Fact]
        public void not_var()
        {
            var parsedResult = Parser("!x;");

            AssertAST(parsedResult, Program(ExprStmt(Unary(NOT, Id("x")))));
        }

        [Fact]
        public void minus_not_var()
        {
            var parsedResult = Parser("-!x;");

            AssertAST(parsedResult, Program(ExprStmt(Unary(MINUS, Unary(NOT, Id("x")) ) ) ) );
        }

        [Fact]
        public void plus_var_into_minus_int()
        {
            var parsedResult = Parser("+x * -10;");

            AssertAST(parsedResult,
                Program(
                    ExprStmt(
                        Binary(
                            INTO,
                            Unary(PLUS, Id("x")),
                            Unary(MINUS, Int(10))
                            )
                        )
                    )
                );
        }
    }
}

