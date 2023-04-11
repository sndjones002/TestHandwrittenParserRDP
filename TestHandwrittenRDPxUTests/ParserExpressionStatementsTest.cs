using System;
using System.Data;
using TestHandwrittenRDP;

namespace TestHandwrittenRDPxUTests
{
	public partial class ParserExpressionStatementsTest : ParserUnitTestModule
    {
        [Fact]
        public void only_int()
        {
            var parsedResult = Parser(@"456;");

            AssertAST(parsedResult, Program( ExprStmt(Int(456)) ) );
        }

        [Fact]
        public void int_str()
        {
            var parsedResult = Parser(@"456;""hello"";");

            AssertAST(parsedResult, Program(ExprStmt(Int(456)), ExprStmt(Str("hello"))));
        }

        [Fact]
        public void mcomment_int_scomment_str()
        {
            var parsedResult = Parser(@"
      /* This is a number
       * with Multiline comment
      */
      456;
      // A String Expression
      ""hello"";
      ");

            AssertAST(parsedResult, Program(ExprStmt(Int(456)), ExprStmt(Str("hello"))));
        }

        [Fact]
        public void empty()
        {
            var parsedResult = Parser(@";");

            AssertAST(parsedResult, Program(Empty()) );
        }

        [Fact]
        public void int_no_semicolon()
        {
            AssertErr<SyntaxErrorException>(
                () => Parser(@"456")!,
                $"Unexpected end of input, expected: '{ETokenType.SEMICOLON}'"
                );
        }
    }
}

