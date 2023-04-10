using System;
using System.Data;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using TestHandwrittenRDP;

namespace TestHandwrittenRDPxUTests
{
	public class ParserAssignmentExpressionTest : ParserUnitTestModule
    {
        [Fact]
        public void var_eq_int()
        {
            var parsedResult = Parser(@"x = 2;");

            AssertAST(parsedResult,
                Program(
                    ExprStmt(
                        Assign(ASSIGN, Id("x"), Int(2))
                        )
                    )
                );
        }

        [Fact]
        public void var_minuseq_int()
        {
            var parsedResult = Parser(@"x -= 2;");

            AssertAST(parsedResult,
                Program(
                    ExprStmt(
                        Assign(MINUS_EQUAL, Id("x"), Int(2))
                        )
                    )
                );
        }

        [Fact]
        public void var_eq_var_eq_int()
        {
            var parsedResult = Parser(@"x = y = 42;");

            AssertAST(parsedResult,
                Program(
                    ExprStmt(
                        Assign(ASSIGN, Id("x"), Assign(ASSIGN, Id("y"), Int(42)))
                        )
                    )
                );
        }

        [Fact]
        public void var_eq_var_plus_int()
        {
            var parsedResult = Parser(@"x = y + 2;");

            AssertAST(parsedResult,
                Program(
                    ExprStmt(
                        Assign(ASSIGN, Id("x"), BinExpr(PLUS, Id("y"), Int(2)))
                        )
                    )
                );
        }

        [Fact]
        public void var_eq_var_lor_int()
        {
            var parsedResult = Parser(@"x = y || 2;");

            AssertAST(parsedResult,
                Program(
                    ExprStmt(
                        Assign(ASSIGN, Id("x"), Logical(LOR, Id("y"), Int(2)))
                        )
                    )
                );
        }

        [Fact]
        public void var_eq_var_land_var()
        {
            var parsedResult = Parser(@"x = y && z;");

            AssertAST(parsedResult,
                Program(
                    ExprStmt(
                        Assign(ASSIGN, Id("x"), Logical(LAND, Id("y"), Id("z")))
                        )
                    )
                );
        }

        [Fact]
        public void var_diveq_int_minus_var()
        {
            var parsedResult = Parser(@"x /= 2 - y;");

            AssertAST(parsedResult,
                Program(
                    ExprStmt(
                        Assign(DIV_EQUAL, Id("x"), BinExpr(MINUS, Int(2), Id("y")))
                        )
                    )
                );
        }

        [Fact]
        public void ERR_int_eq_int()
        {
            AssertErr<SyntaxErrorException>(
                () => Parser(@"45 = 45;")!,
                "Invalid left-hand side in assignment expression"
                );
        }

        [Fact]
        public void ERR_var_plus_int_eq_int()
        {
            AssertErr<SyntaxErrorException>(
                () => Parser(@"x + 5 = 45;")!,
                "Invalid left-hand side in assignment expression"
                );
        }
    }
}