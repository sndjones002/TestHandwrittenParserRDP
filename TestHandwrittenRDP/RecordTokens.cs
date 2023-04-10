using System;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace TestHandwrittenRDP
{
    [JsonDerivedType(typeof(KeywordToken))]
    [JsonDerivedType(typeof(NumberToken))]
    [JsonDerivedType(typeof(StringToken))]
    [JsonDerivedType(typeof(IdentifierToken))]
    public record BaseToken(ETokenType TokenType, string Value);

    public record NumberToken(string Value) : BaseToken(ETokenType.NUMBER, Value);
    public record StringToken(string Value) : BaseToken(ETokenType.STRING, Value);
    public record WhitespacesToken(string Value) : BaseToken(ETokenType.WHITESPACES, Value);
    public record CommentToken(string Value) : BaseToken(ETokenType.COMMENT, Value);
    public record MultilineCommentToken(string Value) : BaseToken(ETokenType.MULTILINE_COMMENT, Value);
    public record IdentifierToken(string Value) : BaseToken(ETokenType.IDENTIFIER, Value);
    public record KeywordToken(ETokenType TokenType, string Value) : BaseToken(TokenType, Value);
}

