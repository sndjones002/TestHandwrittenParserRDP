using System;
using System.Linq.Expressions;

namespace TestHandwrittenRDP
{
	public abstract class ASTFactoryBase
	{
        public virtual BaseRuleList Program(List<BaseRule> body)
        {
            return new BaseRuleList(body.ToArray(), ELiteralType.Program);
        }

        public virtual BaseRule EmptyStatement()
        {
            return new BaseRule(null, ELiteralType.EmptyStatement);
        }

        public virtual BaseRuleList BlockStatement(List<BaseRule> blockBody)
        {
            return new BaseRuleList(blockBody.ToArray(), ELiteralType.BlockStatement);
        }

        public virtual BaseRule ExpressionStatement(BaseRule expression)
        {
            return new BaseRule(expression, ELiteralType.ExpressionStatement);
        }

        public virtual NumericLiteralRule NumericLiteral(string value)
        {
            return new NumericLiteralRule(int.Parse(value));
        }

        public virtual StringLiteralRule StringLiteral(string value)
        {
            return new StringLiteralRule(value[1..^1]);
        }
    }
}

