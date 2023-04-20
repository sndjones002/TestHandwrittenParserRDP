using System;
namespace TestHandwrittenRDPxUTests
{
	public class ParserMemberExpressionTest : ParserUnitTestModule
    {
        [Fact]
        public void obj_property()
        {
            var parsedResult = Parser(@"x.y;");

            AssertAST(parsedResult,
                Program(
                    ExprStmt(Member(false, Id("x"), Id("y")))
                    )
                );
        }

        [Fact]
        public void obj_property_assign()
        {
            var parsedResult = Parser(@"x.y = 1;");

            AssertAST(parsedResult,
                Program(
                    ExprStmt(
                        Assign(ASSIGN, Member(false, Id("x"), Id("y")), Int(1))
                        )
                    )
                );
        }

        [Fact]
        public void obj_property_computed_assign()
        {
            var parsedResult = Parser(@"x[0] = 1;");

            AssertAST(parsedResult,
                Program(
                    ExprStmt(
                        Assign(ASSIGN, Member(true, Id("x"), Int(0)), Int(1))
                        )
                    )
                );
        }

        [Fact]
        public void nested_obj_property_computed_assign()
        {
            var parsedResult = Parser(@"x.y.z[""d""] = 1;");

            AssertAST(parsedResult,
                Program(
                    ExprStmt(
                        Assign(
                            ASSIGN,
                            Member(
                                true,
                                Member(false, Member(false, Id("x"), Id("y")), Id("z")),
                                Str("d")
                                ),
                            Int(1)
                            )
                        )
                    )
                );
        }
    }
}

