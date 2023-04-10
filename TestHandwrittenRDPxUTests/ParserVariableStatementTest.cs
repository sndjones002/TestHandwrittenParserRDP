using System;
using System.Data;
using TestHandwrittenRDP;

namespace TestHandwrittenRDPxUTests
{
	public class ParserVariableStatementTest : ParserUnitTestModule
    {
        [Fact]
        public void one_vardecl()
        {
            var parsedResult = Parser(@"let y;");

            AssertAST(parsedResult,
                Program(
                    VarStmt( VarDecl(Id("y"), null)) )
                );
        }

        [Fact]
        public void multiple_vardecl()
        {
            var parsedResult = Parser(@"let y, b;");

            AssertAST(parsedResult,
                Program(
                    VarStmt(
                        VarDecl(Id("y"), null),
                        VarDecl(Id("b"), null)
                        )
                    )
                );
        }

        [Fact]
        public void one_vardecl_init()
        {
            var parsedResult = Parser(@"let y = 23;");

            AssertAST(parsedResult,
                Program(
                    VarStmt(VarDecl(Id("y"), Int(23))) )
                );
        }

        [Fact]
        public void vardecl_vardecl_init()
        {
            var parsedResult = Parser(@"let x, y = 9;");

            AssertAST(parsedResult,
                Program(
                    VarStmt(
                        VarDecl(Id("x"), null),
                        VarDecl(Id("y"), Int(9))
                        )
                    )
                );
        }

        [Fact]
        public void vardecl_var_assign_int()
        {
            var parsedResult = Parser(@"let x = y = 9;");

            AssertAST(parsedResult,
                Program(
                    VarStmt(
                        VarDecl(Id("x"), Assign(ASSIGN, Id("y"), Int(9)))
                        )
                    )
                );
        }

        [Fact]
        public void vardecl_str_vardecl_int()
        {
            var parsedResult = Parser(@"let x = ""hello"", y = 9;");

            AssertAST(parsedResult,
                Program(
                    VarStmt(
                        VarDecl(Id("x"), Str("hello")),
                        VarDecl(Id("y"), Int(9))
                        )
                    )
                );
        }

        [Fact]
        public void ERR_vardecl_str_vardecl_int()
        {
            AssertErr<SyntaxErrorException>(
                () => Parser(@"let x = ""hello"" y = 9;")!,
                $"Unexpected token: 'y', expected: '{ETokenType.SEMICOLON}'"
                );
        }

        [Fact]
        public void ERR_vardecl_vardecl()
        {
            AssertErr<SyntaxErrorException>(
                () => Parser(@"let x y;")!,
                $"Unexpected token: 'y', expected: '{ETokenType.SIMPLE_ASSIGNMENT}'"
                );
        }
    }
}

