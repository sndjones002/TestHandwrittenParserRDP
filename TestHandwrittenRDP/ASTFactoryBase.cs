using System;
using System.Linq.Expressions;

namespace TestHandwrittenRDP
{
	public abstract class ASTFactoryBase
	{
        public virtual ProgramRule Program(List<BaseRule> body)
        {
            return new ProgramRule(body);
        }

        public virtual EmptyStatementRule EmptyStatement()
        {
            return new EmptyStatementRule();
        }

        public virtual BlockStatementRule BlockStatement(List<BaseRule> blockBody)
        {
            return new BlockStatementRule(blockBody);
        }

        public virtual ExpressionStatementRule ExpressionStatement(BaseRule expression)
        {
            return new ExpressionStatementRule(expression);
        }

        public virtual NumericLiteralRule NumericLiteral(string value)
        {
            return new NumericLiteralRule(int.Parse(value));
        }

        public virtual StringLiteralRule StringLiteral(string value)
        {
            return new StringLiteralRule(value[1..^1]);
        }

        public BaseRule Identifier(string value)
        {
            return new IdentifierRule(value);
        }

        public BaseRule VariableStatement(List<BaseRule> declarations)
        {
            return new VariableStatementRule(declarations);
        }

        public BaseRule VariableDeclaration(BaseRule id, BaseRule init)
        {
            return new VariableDeclarationRule(id, init);
        }

        public BaseRule IfStatement(BaseRule test, BaseRule consequent, BaseRule alternate)
        {
            return new IfStatementRule(test, consequent, alternate);
        }

        public BooleanLiteralRule BooleanLiteral(string value)
        {
            return new BooleanLiteralRule(bool.Parse(value));
        }

        public NullLiteralRule NullLiteral(string value)
        {
            return new NullLiteralRule();
        }
    }
}

