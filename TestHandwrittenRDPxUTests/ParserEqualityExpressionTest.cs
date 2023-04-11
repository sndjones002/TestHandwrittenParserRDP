using System;
using TestHandwrittenRDP;

namespace TestHandwrittenRDPxUTests
{
	public class ParserEqualityExpressionTest : ParserUnitTestModule
    {
        [Fact]
        public void relational_equalto_true()
        {
            var parsedResult = Parser(@"x > 0 == true;");

            AssertAST(parsedResult,
                Program(
                    ExprStmt(
                        BinExpr(
                            EQUAL_TO,
                            BinExpr(GREATER, Id("x"), Int(0)),
                            Bool(true)
                            )
                        )
                    )
                );
        }

        [Fact]
        public void false_equalto_relational()
        {
            var parsedResult = Parser(@"false == x > 0;");

            AssertAST(parsedResult,
                Program(
                    ExprStmt(
                        BinExpr(
                            EQUAL_TO,
                            Bool(false),
                            BinExpr(GREATER, Id("x"), Int(0))
                            )
                        )
                    )
                );
        }

        [Fact]
        public void false_notequal_additive()
        {
            var parsedResult = Parser(@"false != x + y;");

            AssertAST(parsedResult,
                Program(
                    ExprStmt(
                        BinExpr(
                            NOT_EQUAL,
                            Bool(false),
                            BinExpr(PLUS, Id("x"), Id("y"))
                            )
                        )
                    )
                );
        }

        //////

        [Fact]
        public void relational_notequal_true()
        {
            var parsedResult = Parser(@"x >= 0 != true;");

            AssertAST(parsedResult,
                Program(
                    ExprStmt(
                        BinExpr(
                            NOT_EQUAL,
                            BinExpr(GREATER_EQ, Id("x"), Int(0)),
                            Bool(true)
                            )
                        )
                    )
                );
        }

        [Fact]
        public void null_noteual_relational()
        {
            var parsedResult = Parser(@"null != x > 0;");

            AssertAST(parsedResult,
                Program(
                    ExprStmt(
                        BinExpr(
                            NOT_EQUAL,
                            Null(),
                            BinExpr(GREATER, Id("x"), Int(0))
                            )
                        )
                    )
                );
        }

        [Fact]
        public void false_notequal_complex_additive()
        {
            var parsedResult = Parser(@"false != x + y * 3;");

            AssertAST(parsedResult,
                Program(
                    ExprStmt(
                        BinExpr(
                            NOT_EQUAL,
                            Bool(false),
                            BinExpr(
                                PLUS,
                                Id("x"),
                                BinExpr(INTO, Id("y"), Int(3))
                                )
                            )
                        )
                    )
                );
        }
    }
}

