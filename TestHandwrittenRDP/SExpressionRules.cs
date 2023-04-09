using System;
using System.Text.Json.Serialization;

namespace TestHandwrittenRDP
{
    [JsonDerivedType(typeof(StringSELiteralRule))]
    [JsonDerivedType(typeof(NumericSELiteralRule))]
    [JsonDerivedType(typeof(ExpressionSERule))]
    [JsonDerivedType(typeof(BaseSERuleList))]
    public record BaseSERule();

    public record StringSELiteralRule(string Value) : BaseSERule();
    public record NumericSELiteralRule(int Value) : BaseSERule();
    public record ExpressionSERule(BaseSERule Value) : BaseSERule();

    public record BaseSERuleList(BaseSERule[] Body) : BaseSERule();
}

