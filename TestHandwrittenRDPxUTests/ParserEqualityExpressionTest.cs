using System;
using TestHandwrittenRDP;

namespace TestHandwrittenRDPxUTests
{
	public class ParserEqualityExpressionTest : ParserUnitTestModule
    {
        [Fact]
        public void EqualityWithBooleanCheck()
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
        public void EqualityWithBooleanCheckRight()
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
        public void EqualityWithAdditiveCheckRight()
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
        public void NonEqualityWithBooleanCheck()
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
        public void NonEqualityWithBooleanCheckRight()
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
        public void NonEqualityWithAdditiveCheckRight()
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
    }
}

