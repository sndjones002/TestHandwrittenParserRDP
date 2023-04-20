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

        public virtual UnaryExpressionRule UnaryExpression(BaseToken token, BaseRule arg)
        {
            return new UnaryExpressionRule(token, arg);
        }

        public virtual BinaryExpressionRule BinaryExpression(BaseToken token, BaseRule left, BaseRule right)
        {
            return new BinaryExpressionRule(token, left, right);
        }

        public virtual LogicalExpressionRule LogicalExpression(BaseToken token, BaseRule left, BaseRule right)
        {
            return new LogicalExpressionRule(token, left, right);
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

        public BaseRule ReturnStatement(BaseRule argument)
        {
            return new ReturnStatementRule(argument);
        }

        public BaseRule VariableDeclaration(BaseRule id, BaseRule init)
        {
            return new VariableDeclarationRule(id, init);
        }

        public BaseRule FunctionDeclaration(BaseRule name, List<BaseRule> parameters, BaseRule body)
        {
            return new FunctionDeclarationRule(name, parameters, body);
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

        public BaseRule WhileStatement(BaseRule test, BaseRule body)
        {
            return new WhileStatementRule(test, body);
        }

        public BaseRule DoWhileStatement(BaseRule body, BaseRule test)
        {
            return new DoWhileStatementRule(body, test);
        }

        public BaseRule ForStatement(BaseRule init, BaseRule test, BaseRule update, BaseRule body)
        {
            return new ForStatementRule(init, test, update, body);
        }

        public BaseRule MemberExpression(bool computed, BaseRule obj, BaseRule property)
        {
            return new MemberExpressionRule(computed, obj, property);
        }

        public BaseRule CallExpression(BaseRule callee, List<BaseRule> args)
        {
            return new CallExpressionRule(callee, args);
        }

        public BaseRule ClassDeclaration(BaseRule id, BaseRule baseClass, BaseRule body)
        {
            return new ClassDeclarationRule(id, baseClass, body);
        }

        public BaseRule ThisExpression()
        {
            return new ThisExpressionRule();
        }

        public BaseRule BaseCall()
        {
            return new BaseCallRule();
        }

        public BaseRule NewExpression(BaseRule callee, List<BaseRule> args)
        {
            return new NewExpressionRule(callee, args);
        }
    }
}

