using System;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace TestHandwrittenRDP
{
    [JsonDerivedType(typeof(ReturnStatementRule))]
    [JsonDerivedType(typeof(FunctionDeclarationRule))]
    [JsonDerivedType(typeof(IfStatementRule))]
    [JsonDerivedType(typeof(VariableStatementRule))]
    [JsonDerivedType(typeof(VariableDeclarationRule))]
    [JsonDerivedType(typeof(AssignmentExpressionRule))]
    [JsonDerivedType(typeof(IdentifierRule))]
    [JsonDerivedType(typeof(BinaryExpressionRule))]
    [JsonDerivedType(typeof(UnaryExpressionRule))]
    [JsonDerivedType(typeof(LogicalExpressionRule))]
    [JsonDerivedType(typeof(ProgramRule))]
    [JsonDerivedType(typeof(BlockStatementRule))]
    [JsonDerivedType(typeof(EmptyStatementRule))]
    [JsonDerivedType(typeof(WhileStatementRule))]
    [JsonDerivedType(typeof(DoWhileStatementRule))]
    [JsonDerivedType(typeof(ForStatementRule))]
    [JsonDerivedType(typeof(ExpressionStatementRule))]
    [JsonDerivedType(typeof(NumericLiteralRule))]
    [JsonDerivedType(typeof(StringLiteralRule))]
    [JsonDerivedType(typeof(BooleanLiteralRule))]
    [JsonDerivedType(typeof(NullLiteralRule))]
    [JsonDerivedType(typeof(BaseRuleList))]
    public record BaseRule(ELiteralType LiteralType);

    public record ProgramRule(List<BaseRule> Body) : BaseRule(ELiteralType.Program);
    public record BlockStatementRule(List<BaseRule> Body) : BaseRule(ELiteralType.BlockStatement);
    public record EmptyStatementRule() : BaseRule(ELiteralType.EmptyStatement);
    public record ExpressionStatementRule(BaseRule Expression) : BaseRule(ELiteralType.ExpressionStatement);
    public record BaseRuleList(List<BaseRule> Body, ELiteralType LiteralType) : BaseRule(LiteralType);

    public record NumericLiteralRule(int Value) : BaseRule(ELiteralType.NumericLiteral);
    public record StringLiteralRule(string Value) : BaseRule(ELiteralType.StringLiteral);
    public record BooleanLiteralRule(bool Value) : BaseRule(ELiteralType.BooleanLiteral);
    public record NullLiteralRule() : BaseRule(ELiteralType.NullLiteral);
    public record IdentifierRule(string Value) : BaseRule(ELiteralType.Identifier);

    public record BinaryExpressionRule(BaseToken Operator, BaseRule Left, BaseRule Right) : BaseRule(ELiteralType.BinaryExpression);
    public record UnaryExpressionRule(BaseToken Operator, BaseRule Argument) : BaseRule(ELiteralType.UnaryExpression);
    public record LogicalExpressionRule(BaseToken Operator, BaseRule Left, BaseRule Right) : BaseRule(ELiteralType.LogicalExpression);

    public record AssignmentExpressionRule(BaseToken Operator, BaseRule Left, BaseRule Right) : BaseRule(ELiteralType.AssignmentExpression);

    public record VariableStatementRule(List<BaseRule> Declarations) : BaseRule(ELiteralType.VariableStatement);
    public record VariableDeclarationRule(BaseRule Id, BaseRule Init) : BaseRule(ELiteralType.VariableDeclaration);
    public record WhileStatementRule(BaseRule Test, BaseRule Body) : BaseRule(ELiteralType.WhileStatement);
    public record DoWhileStatementRule(BaseRule Body, BaseRule Test) : BaseRule(ELiteralType.DoWhileStatement);
    public record ForStatementRule(BaseRule Init, BaseRule Test, BaseRule Update, BaseRule Body) : BaseRule(ELiteralType.ForStatement);

    public record IfStatementRule(BaseRule Test, BaseRule Consequent, BaseRule Alternate) : BaseRule(ELiteralType.IfStatement);

    public record FunctionDeclarationRule(BaseRule Name, List<BaseRule> Parameters, BaseRule Body) : BaseRule(ELiteralType.FunctionDeclaration);

    public record ReturnStatementRule(BaseRule Argument) : BaseRule(ELiteralType.ReturnStatement);
}

