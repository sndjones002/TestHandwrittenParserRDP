﻿using System;
using TestHandwrittenRDP;

namespace TestHandwrittenRDPxUTests
{
	public class EmptyClass
	{
        [Fact]
        public void SkipWhitespaces()
        {
            var parsedResult = ParserAssignHelper.AssignParser("    \"hello\"   ;");

            ParserAssertHelper.AssertAST(parsedResult, ParserAssignHelper.AssignAST_SingleExpression(new StringLiteral("hello")));
        }

        [Fact]
        public void SkipWhitespacesWithNewline()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"
      456;
      ");

            ParserAssertHelper.AssertAST(parsedResult, ParserAssignHelper.AssignAST_SingleExpression(new NumericLiteral(456)));
        }

        [Fact]
        public void SkipComment()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"
      // This is a number
      456;
      ");

            ParserAssertHelper.AssertAST(parsedResult, ParserAssignHelper.AssignAST_SingleExpression(new NumericLiteral(456)));
        }

        [Fact]
        public void SkipMultilineComment()
        {
            var parsedResult = ParserAssignHelper.AssignParser(@"
      /* This is a number
       * with Multiline comment
      */
      456;
      ");

            ParserAssertHelper.AssertAST(parsedResult, ParserAssignHelper.AssignAST_SingleExpression(new NumericLiteral(456)));
        }
    }
}

