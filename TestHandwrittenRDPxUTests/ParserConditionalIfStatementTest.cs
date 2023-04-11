using System;
using TestHandwrittenRDP;

namespace TestHandwrittenRDPxUTests
{
	public class ParserConditionalIfStatementTest : ParserUnitTestModule
    {
		[Theory]
        [TextFileData("if_else_simple.txt")]
		public void if_else_simple(string code)
		{
            var parsedResult = Parser(code);

			AssertAST(parsedResult,
				Program(
                    If(
                        Id("x"),
                        Block(ExprStmt(Assign(ASSIGN, Id("x"), Int(1)))),
                        Block(ExprStmt(Assign(ASSIGN, Id("x"), Int(2))))
                        )
                    )
                );
        }

        [Theory]
        [TextFileData("only_if_simple.txt")]
        public void only_if_simple(string code)
        {
            var parsedResult = Parser(code);

            AssertAST(parsedResult,
                Program(
                    If(
                        Id("x"),
                        Block( ExprStmt( Assign(ASSIGN, Id("x"), Int(1)) ) ),
                        null
                        )
                    )
                );
        }

        [Fact]
        public void only_if_no_bra()
        {
            var parsedResult = Parser(@" if(x)  x = 1; ");

            AssertAST(parsedResult,
                Program(
                    If(
                        Id("x"),
                        ExprStmt(Assign(ASSIGN, Id("x"), Int(1))),
                        null
                        )
                    )
                );
        }

        [Fact]
        public void if_nested_if_else()
        {
            var parsedResult = Parser(@"
		if(x)  if(y) {} else { y + 1; }
");

            AssertAST(parsedResult,
                Program(
                    If(
                        Id("x"),
                        If(
                            Id("y"),
                            Block(),
                            Block( ExprStmt( BinExpr(PLUS, Id("y"), Int(1)) ) )
                            ),
                        null
                        )
                    )
                );
        }

        [Fact]
        public void if_logic_nested_if_else()
        {
            var parsedResult = Parser(@"
		if(x > 10)  if(y) {} else { y + 1; }
");

            AssertAST(parsedResult,
                Program(
                    If(
                        BinExpr(GREATER, Id("x"), Int(10)),
                        If(
                            Id("y"),
                            Block(),
                            Block(ExprStmt(BinExpr(PLUS, Id("y"), Int(1))))
                            ),
                        null
                        )
                    )
                );
        }

        [Fact]
        public void if_logic_nested_if_else_else()
        {
            var parsedResult = Parser(@"
		if(x > 10 == y)  if(y) {} else { y + 1; } else {}
");

            AssertAST(parsedResult,
                Program(
                    If(
                        BinExpr(
                            EQUAL_TO,
                            BinExpr(GREATER, Id("x"), Int(10)),
                            Id("y")
                            ),
                        If(
                            Id("y"),
                            Block(),
                            Block(ExprStmt(BinExpr(PLUS, Id("y"), Int(1))))
                            ),
                        Block()
                        )
                    )
                );
        }
    }
}

