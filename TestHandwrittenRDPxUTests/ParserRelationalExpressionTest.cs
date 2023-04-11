using System;
using TestHandwrittenRDP;

namespace TestHandwrittenRDPxUTests
{
	public class ParserRelationalExpressionTest : ParserUnitTestModule
    {
        [Fact]
        public void int_greater_int()
        {
            var parsedResult = Parser(@"2 > 3;");

            AssertAST(parsedResult,
                Program(
                    ExprStmt( Binary(GREATER, Int(2), Int(3) ) )
                    )
                );
        }

        [Fact]
        public void int_greatereq_var()
        {
            var parsedResult = Parser(@"2 >= x;");

            AssertAST(parsedResult,
                Program(
                    ExprStmt(Binary(GREATER_EQ, Int(2), Id("x")))
                    )
                );
        }

        [Fact]
        public void int_lesseq_var()
        {
            var parsedResult = Parser(@"2 <= x;");

            AssertAST(parsedResult,
                Program(
                    ExprStmt(Binary(LESS_EQ, Int(2), Id("x")))
                    )
                );
        }

        [Fact]
        public void var_greater_var_plus_int()
        {
            var parsedResult = Parser(@"x > y + 3;");

            AssertAST(parsedResult,
                Program(
                    ExprStmt(
                        Binary(
                            GREATER,
                            Id("x"),
                            Binary(PLUS, Id("y"), Int(3))
                            )
                        )
                    )
                );
        }
    }
}

