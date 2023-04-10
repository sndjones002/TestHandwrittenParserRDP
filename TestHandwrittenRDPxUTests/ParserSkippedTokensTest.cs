using System;
using TestHandwrittenRDP;

namespace TestHandwrittenRDPxUTests
{
	public class ParserSkippedTokensTest : ParserUnitTestModule
    {
        [Fact]
        public void spaces_str_spaces_expr()
        {
            var parsedResult = Parser("    \"hello\"   ;");

            AssertAST(parsedResult, Program(ExprStmt(Str("hello"))) );
        }

        [Fact]
        public void nl_intexp_nl()
        {
            var parsedResult = Parser(@"
      456;
      ");

            AssertAST(parsedResult, Program(ExprStmt(Int(456))) );
        }

        [Fact]
        public void nl_scomment_int_nl()
        {
            var parsedResult = Parser(@"
      // This is a number
      456;
      ");

            AssertAST(parsedResult, Program(ExprStmt(Int(456))) );
        }

        [Fact]
        public void nl_mcomment_int_nl()
        {
            var parsedResult = Parser(@"
      /* This is a number
       * with Multiline comment
      */
      456;
      ");

            AssertAST(parsedResult, Program(ExprStmt(Int(456))) );
        }
    }
}

