using System;
namespace TestHandwrittenRDPxUTests
{
	public class ParserCallExpressionTest : ParserUnitTestModule
	{
        [Fact]
        public void func()
        {
            var parsedResult = Parser(@"foo(x);");

            AssertAST(parsedResult,
                Program(ExprStmt(
                    Call(Id("foo"), new () { Id("x") })
                    )
                ));
        }

        [Fact]
        public void func_func()
        {
            var parsedResult = Parser(@"foo(x)();");

            AssertAST(parsedResult,
                Program(ExprStmt(
                    Call( Call(Id("foo"), new() { Id("x") }), null)
                    )
                ));
        }

        [Fact]
        public void memberfunc_func()
        {
            var parsedResult = Parser(@"console.log(x, y);");

            AssertAST(parsedResult,
                Program(ExprStmt(
                    Call(
                        Member(false, Id("console"), Id("log")),
                        new() { Id("x"), Id("y") })
                    )
                ));
        }
    }
}

