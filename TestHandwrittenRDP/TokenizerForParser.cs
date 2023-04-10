using System;
using System.Data;
using System.Text.RegularExpressions;

namespace TestHandwrittenRDP
{
	public class TokenizerForParser
    {
        private string _data;
        private int _cursor;

        /// <summary>
        /// Order of the Tokens are very important
        /// </summary>
		private List<(string Regexterm, ETokenType Token)> _tokens =
			new()
			{
                #region Comments, Whitespaces, Newlines, to be skipped

                (@"\G\s+", ETokenType.WHITESPACES), // returns null BaseToken, includes newlines
				(@"\G//.*", ETokenType.COMMENT),
                (@"\G/\*[\s\S]*?\*/", ETokenType.MULTILINE_COMMENT),

                #endregion Comments, Whitespaces, Newlines, to be skipped

                #region Symbols, Delimiters

                (@"\G;", ETokenType.SEMICOLON),
                (@"\G{", ETokenType.OPEN_CURLY_BRACES),
                (@"\G}", ETokenType.CLOSE_CURLY_BRACES),
                (@"\G\(", ETokenType.LEFT_PARENTHESIS),
                (@"\G\)", ETokenType.RIGHT_PARENTHESIS),
                (@"\G,", ETokenType.COMMA),

                #endregion Symbols

                #region Literals

                (@"\G\blet\b", ETokenType.KEYWORD_LET),
                (@"\G\bif\b", ETokenType.KEYWORD_IF),
                (@"\G\btrue\b", ETokenType.KEYWORD_TRUE),
                (@"\G\bfalse\b", ETokenType.KEYWORD_FALSE),
                (@"\G\bnull\b", ETokenType.KEYWORD_NULL),

                (@"\G\belse\b", ETokenType.KEYWORD_ELSE),
                (@"\G\d+", ETokenType.NUMBER),
                (@"\G""[^""]*""", ETokenType.STRING),
                (@"\G\w+", ETokenType.IDENTIFIER),

                #endregion Literals

                #region Operations, Mathematical

                (@"\G[=!]=", ETokenType.EQUALITY_OPERATOR), // Needs to be recognised before SIMPLE_ASSIGNMENT

                #endregion Operations, Mathematical

                #region Assignments

                (@"\G[*/+-]=", ETokenType.COMPLEX_ASSIGNMENT), // Order of Parsing tokens is very important
                (@"\G=", ETokenType.SIMPLE_ASSIGNMENT),

                #endregion Assignments

                #region Operations, Mathematical

                (@"\G[+-]", ETokenType.ADDITIVE_OPERATOR),
                (@"\G[*/]", ETokenType.MULTIPLICATIVE_OPERATOR),
                (@"\G[><]=?", ETokenType.RELATIONAL_OPERATOR),
                (@"\G&&", ETokenType.LOGICAL_AND),
                (@"\G\|\|", ETokenType.LOGICAL_OR),

                #endregion Operations, Mathematical
            };

        public static bool IsAssignmentToken(BaseToken token)
        {
            return token.TokenType == ETokenType.SIMPLE_ASSIGNMENT ||
                token.TokenType == ETokenType.COMPLEX_ASSIGNMENT;
        }

        public void Init(string data)
		{
			if (string.IsNullOrEmpty(data))
				throw new ArgumentNullException("Null data cannot be parsed");

			this._data = data;
			this._cursor = 0;
		}

		/// <summary>
		/// To get the next token
		/// These basically implements a state machine.
		/// </summary>
		/// <returns></returns>
		public BaseToken GetNextToken()
		{
			if (!this.HasMoreTokens())
				return null;

			foreach ((string Regexterm, ETokenType Token) in _tokens)
			{
				var token = Matched(Regexterm, Token);

				if(token != null)
				{
                    // Should skip token, e.g., whitespaces
                    if (ShouldTokenBeSkipped(token.TokenType))
                        return this.GetNextToken();
					else
                        return token;
                }
			}

			throw new SyntaxErrorException($"Unexpected token found: \"{PeekChar()}\"");
		}

		private BaseToken Matched(string regexTerm, ETokenType tokenType)
        {
			var regex = new Regex(regexTerm);
            var matched = regex.Match(this._data, this._cursor);
			if (matched != null && matched.Success)
			{
				MoveAhead(matched.Length);

                if (!Enum.IsDefined<ETokenType>(tokenType))
                    return null;
                else
                    return new BaseToken(tokenType, matched.Value);
			}
			else
				return null;
        }

        private char PeekChar()
        {
            if (!this.HasMoreTokens())
                return '\0';
            return this._data[this._cursor];
        }

		private void MoveAhead(int step = 1)
		{
			this._cursor += step;
        }

        private bool HasMoreTokens()
        {
			return this._cursor < this._data.Length;
        }

		private bool ShouldTokenBeSkipped(ETokenType tokenType)
		{
			return tokenType == ETokenType.WHITESPACES ||
				tokenType == ETokenType.COMMENT ||
				tokenType == ETokenType.MULTILINE_COMMENT;
		}
    }
}

