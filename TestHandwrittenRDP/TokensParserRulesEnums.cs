using System;
namespace TestHandwrittenRDP
{
	public enum ELiteralType
	{
		Program,
		NumericLiteral,
		StringLiteral,

		EmptyStatement,
		ExpressionStatement,
		BlockStatement
	}

    public enum ESERuleType
	{
		begin
	}

    public enum ETokenType
	{
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

        #endregion Markers
    }
}

