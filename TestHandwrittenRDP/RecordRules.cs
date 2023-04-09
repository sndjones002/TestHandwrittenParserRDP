using System;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace TestHandwrittenRDP
{
    [JsonDerivedType(typeof(NumericLiteralRule))]
    [JsonDerivedType(typeof(StringLiteralRule))]
    [JsonDerivedType(typeof(BaseRuleList))]
    public record BaseRule(BaseRule Rule, ELiteralType LiteralType);

    public record BaseRuleList(BaseRule[] Body, ELiteralType LiteralType) : BaseRule(null, LiteralType);

    public record NumericLiteralRule(int Value) : BaseRule(null, ELiteralType.NumericLiteral);
    public record StringLiteralRule(string Value) : BaseRule(null, ELiteralType.StringLiteral);
}

