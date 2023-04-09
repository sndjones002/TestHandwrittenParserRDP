using System;
using System.Data;
using System.Text.RegularExpressions;

namespace TestHandwrittenRDP
{
	public class TokenizerForParser
    {
        private string _data;
        private int _cursor;

		private List<(string Regexterm, ETokenType Token)> _tokens =
			new()
			{
				("\\G\\d+", ETokenType.NUMBER),
                ("\\G\"[^\"]*\"", ETokenType.STRING),
				("\\G\\s+", ETokenType.WHITESPACES), // returns null BaseToken, includes newlines
				("\\G//.*", ETokenType.COMMENT),
                (@"\G/\*[\s\S]*?\*/", ETokenType.MULTILINE_COMMENT),
                (@"\G;", ETokenType.SEMICOLON),
                (@"\G{", ETokenType.OPEN_CURLY_BRACES),
                (@"\G}", ETokenType.CLOSE_CURLY_BRACES),
                (@"\G[+-]", ETokenType.ADDITIVE_OPERATOR),
                (@"\G[*/]", ETokenType.MULTIPLICATIVE_OPERATOR),
                (@"\G\(", ETokenType.LEFT_PARENTHESIS),
                (@"\G\)", ETokenType.RIGHT_PARENTHESIS),
            };

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

				switch(tokenType)
				{
					case ETokenType.NUMBER:
                        return new NumberToken(matched.Value);
					case ETokenType.STRING:
                        return new StringToken(matched.Value);
                    case ETokenType.WHITESPACES:
                        return new WhitespacesToken(matched.Value);
                    case ETokenType.COMMENT:
                        return new CommentToken(matched.Value);
                    case ETokenType.MULTILINE_COMMENT:
                        return new MultilineCommentToken(matched.Value);
                    case ETokenType.SEMICOLON:
                        return new BaseToken(ETokenType.SEMICOLON, matched.Value);
                    case ETokenType.OPEN_CURLY_BRACES:
                        return new BaseToken(ETokenType.OPEN_CURLY_BRACES, matched.Value);
                    case ETokenType.CLOSE_CURLY_BRACES:
                        return new BaseToken(ETokenType.CLOSE_CURLY_BRACES, matched.Value);
                    case ETokenType.ADDITIVE_OPERATOR:
                        return new BaseToken(ETokenType.ADDITIVE_OPERATOR, matched.Value);
                    case ETokenType.MULTIPLICATIVE_OPERATOR:
                        return new BaseToken(ETokenType.MULTIPLICATIVE_OPERATOR, matched.Value);
                    case ETokenType.LEFT_PARENTHESIS:
                        return new BaseToken(ETokenType.LEFT_PARENTHESIS, matched.Value);
                    case ETokenType.RIGHT_PARENTHESIS:
                        return new BaseToken(ETokenType.RIGHT_PARENTHESIS, matched.Value);
                    default:
						return null;
                }
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

