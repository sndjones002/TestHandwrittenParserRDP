using System;
namespace TestHandwrittenRDP
{
	public enum ELiteralType
	{
		Program,
		NumericLiteral,
		StringLiteral,
		BooleanLiteral,
		NullLiteral,
		Identifier,

		EmptyStatement,
		ExpressionStatement,
		BlockStatement,

		BinaryExpression,
        UnaryExpression,
        LogicalExpression,

		AssignmentExpression,

        VariableStatement,
		VariableDeclaration,

		WhileStatement,
        DoWhileStatement,
        ForStatement,

        IfStatement,
        FunctionDeclaration,
        ReturnStatement,
    }

    public enum ETokenType
	{
		KEYWORD_LET,
		KEYWORD_IF,
        KEYWORD_ELSE,
        KEYWORD_TRUE,
        KEYWORD_FALSE,
        KEYWORD_NULL,
        KEYWORD_WHILE,
        KEYWORD_DO,
        KEYWORD_FOR,
        KEYWORD_DEF,
		KEYWORD_RETURN,

        NUMBER,
        STRING,

        #region Skipped Tokens

		/// <summary>
		/// These section describes which are skipped in the language
		/// </summary>

        WHITESPACES, // It includes newlines as well
		COMMENT,
		MULTILINE_COMMENT,

        #endregion Skipped Tokens

        #region Markers

		/// <summary>
		/// This section identifies the tokens which marks the beginning
		/// or end of a specific Parsing Rule
		/// </summary>

        SEMICOLON,   // Marks the end of ExpressionStatement
		OPEN_CURLY_BRACES, // Marks the start of BlockStatement
        CLOSE_CURLY_BRACES, // Marks the end of BlockStatement

		LEFT_PARENTHESIS,
        RIGHT_PARENTHESIS,
		COMMA,

        #endregion Markers

		RELATIONAL_OPERATOR,
        ADDITIVE_OPERATOR,
		MULTIPLICATIVE_OPERATOR,
		EQUALITY_OPERATOR,
		LOGICAL_AND,
		LOGICAL_OR,
		LOGICAL_NOT,

		IDENTIFIER,
		SIMPLE_ASSIGNMENT, // Only '=' sign, e.g., x = 2;
		COMPLEX_ASSIGNMENT, // +=, -=, *=, /=, etc.,
    }
}

