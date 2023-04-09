using System;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace TestHandwrittenRDP
{
    [JsonDerivedType(typeof(ProgramLiteral))]
    [JsonDerivedType(typeof(NumericLiteral))]
    [JsonDerivedType(typeof(StringLiteral))]

    [JsonDerivedType(typeof(StatementRule))]
    [JsonDerivedType(typeof(ExpressionStatementRule))]
    [JsonDerivedType(typeof(BlockStatementRule))]

    public record BaseLiteral(ELiteralType LiteralType);

    public record NumericLiteral(int Value) : BaseLiteral(ELiteralType.NumericLiteral);
    public record StringLiteral(string Value) : BaseLiteral(ELiteralType.StringLiteral);

    public record StatementRule(ELiteralType LiteralType) : BaseLiteral(LiteralType);
    public record ExpressionStatementRule(BaseLiteral Statement) : StatementRule(ELiteralType.ExpressionStatement);
    public record BlockStatementRule(BaseLiteral[] Body) : StatementRule(ELiteralType.BlockStatement);

    public record ProgramLiteral(BaseLiteral[] Body) : BaseLiteral(ELiteralType.Program);
}

