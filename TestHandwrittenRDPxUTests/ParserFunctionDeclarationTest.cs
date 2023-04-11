using System;
using TestHandwrittenRDP;

namespace TestHandwrittenRDPxUTests
{
	public class ParserFunctionDeclarationTest : ParserUnitTestModule
    {
        [Theory]
        [TextFileData("simple_function.txt")]
        public void simple_function(string code)
        {
            var parsedResult = Parser(code);

            AssertAST(parsedResult,
                Program(
                    Func(
                        Id("square"),
                        Parameters(Id("x"))!,
                        Block(Return (Binary(INTO, Id("x"), Id("x") ) ) )
                        )
                    )
                );
        }

        [Fact]
        public void empty_function()
        {
            var parsedResult = Parser(@"def empty(){return;}");

            AssertAST(parsedResult,
                Program(
                    Func(
                        Id("empty"),
                        null,
                        Block(Return(null!))
                        )
                    )
                );
        }
    }
}

