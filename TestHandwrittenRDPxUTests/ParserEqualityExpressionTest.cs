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
                        Binary(
                            EQUAL_TO,
                            Binary(GREATER, Id("x"), Int(0)),
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
                        Binary(
                            EQUAL_TO,
                            Bool(false),
                            Binary(GREATER, Id("x"), Int(0))
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
                        Binary(
                            NOT_EQUAL,
                            Bool(false),
                            Binary(PLUS, Id("x"), Id("y"))
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
                        Binary(
                            NOT_EQUAL,
                            Binary(GREATER_EQ, Id("x"), Int(0)),
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
                        Binary(
                            NOT_EQUAL,
                            Null(),
                            Binary(GREATER, Id("x"), Int(0))
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
                        Binary(
                            NOT_EQUAL,
                            Bool(false),
                            Binary(
                                PLUS,
                                Id("x"),
                                Binary(INTO, Id("y"), Int(3))
                                )
                            )
                        )
                    )
                );
        }
    }
}

