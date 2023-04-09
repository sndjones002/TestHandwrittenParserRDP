using System;
using System.Data;

namespace TestHandwrittenRDP
{
	public class SExpressionSerializer
	{
		public static BaseSERule Serialize(BaseRule rule)
		{
            return Visit(rule);
		}

        private static BaseSERule Visit(BaseRule rule)
        {
            if (rule == null)
                return null;

            switch (rule.LiteralType)
            {
                case ELiteralType.Program:
                case ELiteralType.BlockStatement:
                    var rules = new List<BaseSERule>();
                    var blockRules = (rule as BaseRuleList).Body;

                    foreach (var item in blockRules)
                    {
                        var ruleItem = Visit(item);

                        if(ruleItem != null)
                            rules.Add(ruleItem);
                    }

                    return new BaseSERuleList(
                    new BaseSERule[]
                    {
                        new StringSELiteralRule("begin"),
                        new BaseSERuleList(rules.ToArray())
                    });
                case ELiteralType.EmptyStatement:
                    return null;
                case ELiteralType.ExpressionStatement:
                    return new ExpressionSERule(Visit(rule.Rule));
                case ELiteralType.StringLiteral:
                    return new StringSELiteralRule($"{(rule as StringLiteralRule).Value}");
                case ELiteralType.NumericLiteral:
                    return new NumericSELiteralRule((rule as NumericLiteralRule).Value);
                default:
                    throw new SyntaxErrorException($"Unknown rule : {rule.LiteralType}");
            }
        }
    }
}

