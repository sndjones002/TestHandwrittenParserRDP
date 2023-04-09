using System;
using System.Text.Json;
using TestHandwrittenRDP;

namespace TestHandwrittenRDPxUTests
{
	public static class ParserAssertHelper
	{
        public static void AssertAST(BaseLiteral? resultAst, ProgramLiteral expectedAst)
        {
            Assert.NotNull(resultAst);
            Assert.Equal(
                JsonSerializer.Serialize(resultAst),
                JsonSerializer.Serialize(expectedAst)
                );
        }
    }
}

