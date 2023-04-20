using System;
using System.Data;
using System.Text.Json;
using TestHandwrittenRDP;

namespace TestHandwrittenRDPxUTests
{
	public partial class ParserUnitTestModule
	{
        #region Token Generators

        protected BaseToken ASSIGN = new BaseToken(ETokenType.SIMPLE_ASSIGNMENT, "=");
        protected BaseToken PLUS = new BaseToken(ETokenType.ADDITIVE_OPERATOR, "+");
        protected BaseToken MINUS = new BaseToken(ETokenType.ADDITIVE_OPERATOR, "-");
        protected BaseToken INTO = new BaseToken(ETokenType.MULTIPLICATIVE_OPERATOR, "*");
        protected BaseToken DIVIDE = new BaseToken(ETokenType.MULTIPLICATIVE_OPERATOR, "/");
        protected BaseToken ADD_EQUAL = new BaseToken(ETokenType.COMPLEX_ASSIGNMENT, "+=");
        protected BaseToken MINUS_EQUAL = new BaseToken(ETokenType.COMPLEX_ASSIGNMENT, "-=");
        protected BaseToken MUL_EQUAL = new BaseToken(ETokenType.COMPLEX_ASSIGNMENT, "*=");
        protected BaseToken DIV_EQUAL = new BaseToken(ETokenType.COMPLEX_ASSIGNMENT, "/=");
        protected BaseToken LOR = new BaseToken(ETokenType.LOGICAL_OR, "||");
        protected BaseToken LAND = new BaseToken(ETokenType.LOGICAL_AND, "&&");
        protected BaseToken GREATER = new BaseToken(ETokenType.RELATIONAL_OPERATOR, ">");
        protected BaseToken LESS = new BaseToken(ETokenType.RELATIONAL_OPERATOR, "<");
        protected BaseToken GREATER_EQ = new BaseToken(ETokenType.RELATIONAL_OPERATOR, ">=");
        protected BaseToken LESS_EQ = new BaseToken(ETokenType.RELATIONAL_OPERATOR, "<=");
        protected BaseToken EQUAL_TO = new BaseToken(ETokenType.EQUALITY_OPERATOR, "==");
        protected BaseToken NOT_EQUAL = new BaseToken(ETokenType.EQUALITY_OPERATOR, "!=");
        protected BaseToken NOT = new BaseToken(ETokenType.LOGICAL_NOT, "!");

        #endregion Token Generators

        #region Rule Generators

        protected List<BaseRule>? Parameters(params BaseRule[] rules)
        {
            return (rules != null) ? rules.ToList() : null;
        }

        protected BaseRule Program(params BaseRule[] rules)
		{
			return new ProgramRule(rules != null ? rules.ToList() : null);
		}

        protected BaseRule ExprStmt(BaseRule rule)
		{
			return new ExpressionStatementRule(rule);
        }

        protected BaseRule Assign(BaseToken token, BaseRule left, BaseRule right)
        {
            return new AssignmentExpressionRule(token, left, right);
        }

        protected BaseRule Binary(BaseToken token, BaseRule left, BaseRule right)
        {
            return new BinaryExpressionRule(token, left, right);
        }

        protected BaseRule Unary(BaseToken token, BaseRule argument)
        {
            return new UnaryExpressionRule(token, argument);
        }

        protected BaseRule Logical(BaseToken token, BaseRule left, BaseRule right)
        {
            return new LogicalExpressionRule(token, left, right);
        }

        protected BaseRule If(BaseRule test, BaseRule consequent, BaseRule? alternate)
        {
            return new IfStatementRule(test, consequent, alternate);
        }

        protected BaseRule Block(params BaseRule[] rules)
        {
            return new BlockStatementRule(rules != null ? rules.ToList() : null);
        }

        protected BaseRule VarStmt(params BaseRule[] rules)
        {
            return new VariableStatementRule(rules != null ? rules.ToList() : null);
        }

        protected BaseRule VarDecl(BaseRule id, BaseRule? init)
        {
            return new VariableDeclarationRule(id, init);
        }

        protected BaseRule While(BaseRule test, BaseRule body)
        {
            return new WhileStatementRule(test, body);
        }

        protected BaseRule Member(bool computed, BaseRule obj, BaseRule property)
        {
            return new MemberExpressionRule(computed, obj, property);
        }

        protected BaseRule Call(BaseRule callee, List<BaseRule>? args)
        {
            return new CallExpressionRule(callee, args);
        }

        protected BaseRule DoWhile(BaseRule body, BaseRule test)
        {
            return new DoWhileStatementRule(body, test);
        }

        protected BaseRule For(BaseRule? init, BaseRule? test, BaseRule? update, BaseRule body)
        {
            return new ForStatementRule(init, test, update, body);
        }

        protected BaseRule Func(BaseRule name, List<BaseRule> parameters, BaseRule body)
        {
            return new FunctionDeclarationRule(name, parameters, body);
        }

        protected BaseRule Return(BaseRule argument)
        {
            return new ReturnStatementRule(argument);
        }

        protected BaseRule Empty()
        {
            return new EmptyStatementRule();
        }

        protected BaseRule Id(string id)
        {
            return new IdentifierRule(id);
        }

        protected BaseRule Int(int value)
        {
            return new NumericLiteralRule(value);
        }

        protected BaseRule Str(string value)
        {
            return new StringLiteralRule(value);
        }

        protected BaseRule Bool(bool value)
        {
            return new BooleanLiteralRule(value);
        }

        protected BaseRule Null()
        {
            return new NullLiteralRule();
        }

        protected BaseRule Class(BaseRule id, BaseRule baseClass, BaseRule body)
        {
            return new ClassDeclarationRule(id, baseClass, body);
        }

        #endregion Rule Generators

        #region Helpers

        protected BaseRule? Parser(string toParse)
        {
            return new RecursiveDescentParserForOOP().Parse(toParse);
        }

        protected void AssertAST(BaseRule? resultAst, BaseRule expectedAst)
        {
            Assert.NotNull(resultAst);

            Assert.Equal(
                JsonSerializer.Serialize(resultAst),
                JsonSerializer.Serialize(expectedAst)
                );
        }

        protected void AssertErr<TException>(Func<BaseRule> ruleBuilder, string message)
            where TException : Exception
        {
            var exception = Assert.Throws<SyntaxErrorException>(ruleBuilder);

            Assert.NotNull(exception);
            Assert.Equal(message, exception.Message);
        }

        #endregion Helpers
    }
}

