using System;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace TestHandwrittenRDP
{
    [JsonDerivedType(typeof(BinaryExpressionRule))]
    [JsonDerivedType(typeof(ProgramRule))]
    [JsonDerivedType(typeof(BlockStatementRule))]
    [JsonDerivedType(typeof(EmptyStatementRule))]
    [JsonDerivedType(typeof(ExpressionStatementRule))]
    [JsonDerivedType(typeof(NumericLiteralRule))]
    [JsonDerivedType(typeof(StringLiteralRule))]
    [JsonDerivedType(typeof(BaseRuleList))]
    public record BaseRule(ELiteralType LiteralType);

    public record ProgramRule(List<BaseRule> Body) : BaseRule(ELiteralType.Program);
    public record BlockStatementRule(List<BaseRule> Body) : BaseRule(ELiteralType.BlockStatement);
    public record EmptyStatementRule() : BaseRule(ELiteralType.EmptyStatement);
    public record ExpressionStatementRule(BaseRule Expression) : BaseRule(ELiteralType.ExpressionStatement);
    public record BaseRuleList(BaseRule[] Body, ELiteralType LiteralType) : BaseRule(LiteralType);

    public record NumericLiteralRule(int Value) : BaseRule(ELiteralType.NumericLiteral);
    public record StringLiteralRule(string Value) : BaseRule(ELiteralType.StringLiteral);

    public record BinaryExpressionRule(BaseToken Operator, BaseRule Left, BaseRule Right) : BaseRule(ELiteralType.BinaryExpression);
}

