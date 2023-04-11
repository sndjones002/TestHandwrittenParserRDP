using System;
namespace TestHandwrittenRDPxUTests
{
	public class ParserWhileTest : ParserUnitTestModule
	{
		[Theory]
        [TextFileData("simple_while.txt")]
        public void simple_while(string code)
		{
            var parsedResult = Parser(code);

            AssertAST(parsedResult,
                Program(
                    While(
                        Binary(GREATER_EQ, Id("x"), Int(10)),
                        Block(
                            ExprStmt(Assign(MINUS_EQUAL, Id("x"), Int(1)))
                            )
                        )
                    )
                );
        }

        [Theory]
        [TextFileData("simple_do_while.txt")]
        public void simple_do_while(string code)
        {
            var parsedResult = Parser(code);

            AssertAST(parsedResult,
                Program(
                    DoWhile(
                        Block(
                            ExprStmt(Assign(MINUS_EQUAL, Id("x"), Int(1)))
                            ),
                        Binary(GREATER_EQ, Id("x"), Int(10))
                        )
                    )
                );
        }

        [Theory]
        [TextFileData("simple_for.txt")]
        public void simple_for(string code)
        {
            var parsedResult = Parser(code);

            AssertAST(parsedResult,
                Program(
                    For(
                        VarStmt( VarDecl(Id("i"), Int(0)) ),
                        Binary(LESS, Id("i"), Int(10)),
                        Assign(ADD_EQUAL, Id("i"), Int(1)),
                        Block(
                            ExprStmt( Assign(ADD_EQUAL, Id("x"), Int(1)) )
                            )
                        )
                    )
                );
        }

        [Theory]
        [TextFileData("infinite_for.txt")]
        public void inifinite_for(string code)
        {
            var parsedResult = Parser(code);

            AssertAST(parsedResult,
                Program(
                    For(
                        null,
                        null,
                        null,
                        Block(
                            ExprStmt(Assign(ADD_EQUAL, Id("x"), Int(1)))
                            )
                        )
                    )
                );
        }

        /// <summary>
        /// TODO: Sequence Expressions or Comma seperated expressions
        /// </summary>
        /// <param name="code"></param>
        [Fact]
        public void sequence_expr_for()
        {
            var parsedResult = Parser(@"for(i = 0, z = 1; ;) {}");

        }
    }
}

