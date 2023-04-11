using System;
using System.Data;
using Newtonsoft.Json.Linq;
using TestHandwrittenRDP;

namespace TestHandwrittenRDPxUTests
{
    public class ParserBinaryExpressionTest : ParserUnitTestModule
    {
        [Fact]
        public void int_plus_int()
        {
            var parsedResult = Parser(@"2 + 3;");

            AssertAST(parsedResult,
                Program(
                    ExprStmt(
                        Binary(PLUS, Int(2), Int(3))
                        )
                    )
                );
        }

        [Fact]
        public void var_plus_var()
        {
            var parsedResult = Parser(@"x + y;");

            AssertAST(parsedResult,
                Program(
                    ExprStmt(
                        Binary(PLUS, Id("x"), Id("y"))
                        )
                    )
                );
        }

        [Fact]
        public void int_plus_int_minus_int()
        {
            var parsedResult = Parser(@"5 + 3 - 4;");

            AssertAST(parsedResult,
                Program(
                    ExprStmt(
                        Binary(
                            MINUS,
                            Binary(PLUS, Int(5), Int(3)),
                            Int(4)
                            )
                        )
                    )
                );
        }

        [Fact]
        public void int_plus_int_into_int()
        {
            var parsedResult = Parser(@"5 + 3 * 4;");

            AssertAST(parsedResult,
                Program(
                    ExprStmt(
                        Binary(
                            PLUS,
                            Int(5),
                            Binary(INTO, Int(3), Int(4))
                            )
                        )
                    )
                );
        }

        [Fact]
        public void int_into_int_into_int()
        {
            var parsedResult = Parser(@"5 * 3 * 4;");

            AssertAST(parsedResult,
                Program(
                    ExprStmt(
                        Binary(
                            INTO,
                            Binary(INTO, Int(5), Int(3)),
                            Int(4)
                            )
                        )
                    )
                );
        }

        [Fact]
        public void bra_int_plus_int_bra()
        {
            var parsedResult = Parser(@"(5 + 3);");

            AssertAST(parsedResult,
                Program(
                    ExprStmt(
                        Binary(PLUS, Int(5), Int(3))
                        )
                    )
                );
        }

        [Fact]
        public void bra_int_plus_int_bra_into_int()
        {
            var parsedResult = Parser(@"(5 + 3) * 4;");

            AssertAST(parsedResult,
                Program(
                    ExprStmt(
                        Binary(
                            INTO,
                            Binary(PLUS, Int(5), Int(3)),
                            Int(4)
                            )
                        )
                    )
                );
        }

        [Fact]
        public void bra_int_plus_int_bra_div_int()
        {
            var parsedResult = Parser(@"(5 + 3) / 4;");

            AssertAST(parsedResult,
                Program(
                    ExprStmt(
                        Binary(
                            DIVIDE,
                            Binary(PLUS, Int(5), Int(3)),
                            Int(4)
                            )
                        )
                    )
                );
        }

        [Fact]
        public void int_div_bra_int_plus_int_bra()
        {
            var parsedResult = Parser(@"4 / (5 + 3);");

            AssertAST(parsedResult,
                Program(
                    ExprStmt(
                        Binary(
                            DIVIDE,
                            Int(4),
                            Binary(PLUS, Int(5), Int(3))
                            )
                        )
                    )
                );
        }

        [Fact]
        public void ERR_bra_int_plus_int()
        {
            var ruleBuilder = () => Parser(@"(5 + 3;");

            AssertErr<SyntaxErrorException>(
                () => Parser(@"(5 + 3;")!,
                $"Unexpected token: ';', expected: '{ETokenType.RIGHT_PARENTHESIS}'"
                );
        }
    }
}

