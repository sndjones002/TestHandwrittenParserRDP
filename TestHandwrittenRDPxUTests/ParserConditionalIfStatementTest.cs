using System;
using TestHandwrittenRDP;

namespace TestHandwrittenRDPxUTests
{
	public class ParserConditionalIfStatementTest : ParserUnitTestModule
    {
		[Fact]
		public void Simple()
		{
            var parsedResult = Parser(@"
		if(x) {
		  x = 1;
		}
		else {
		  x = 2;
		}
");
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

        [Fact]
        public void SimpleOnlyIf()
        {
            var parsedResult = Parser(@"
		if(x) {
		  x = 1;
		}
");
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
        public void SimpleOnlyIfNoBraces()
        {
            var parsedResult = Parser(@"
		if(x)  x = 1;
");

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
        public void ComplexNestedIf()
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
        public void ComplexNestedIfWithExpression()
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
        public void ComplexNestedIfWithExpressionBooleanEquality()
        {
            var parsedResult = Parser(@"
		if(x > 10 == y)  if(y) {} else { y + 1; }
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
                        null
                        )
                    )
                );
        }
    }
}

