using System;
using TestHandwrittenRDP;

namespace TestHandwrittenRDPxUTests
{
	public class ParserLogicalExpressionTest : ParserUnitTestModule
    {
		[Fact]
		public void var_land_int()
		{
            var parsedResult = Parser(@"x && 10;");

            AssertAST(parsedResult,
                Program(
                    ExprStmt(Logical(LAND, Id("x"), Int(10)))
                    )
                );
        }

        [Fact]
        public void var_greater_int_land_var_plus_int()
        {
            var parsedResult = Parser(@"x > 8 && y + 10;");

            AssertAST(parsedResult,
                Program(
                    ExprStmt(
                        Logical(
                            LAND,
                            BinExpr(GREATER, Id("x"), Int(8)),
                            BinExpr(PLUS, Id("y"), Int(10))
                            )
                        )
                    )
                );
        }

        [Fact]
        public void int_greater_int_land_var_plus_int_lor_var_into_int()
        {
            var parsedResult = Parser(@"x > 8 && y + 10 || z * 2;");

            AssertAST(parsedResult,
                Program(
                    ExprStmt(
                        Logical(
                            LOR,
                            Logical(
                                LAND,
                                BinExpr(GREATER, Id("x"), Int(8)),
                                BinExpr(PLUS, Id("y"), Int(10))
                                ),
                            BinExpr(INTO, Id("z"), Int(2))
                            )
                        )
                    )
                );
        }
    }
}

