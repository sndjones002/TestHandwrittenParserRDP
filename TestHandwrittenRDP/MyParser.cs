using System;
using System.Data;

namespace TestHandwrittenRDP
{
	public class MyParser
	{
		private string _data;
		private MyTokenizer _tokenizer;
		private BaseToken _lookahead;

        public MyParser()
		{
			this._data = string.Empty;
            this._tokenizer = new MyTokenizer();
		}

		public BaseLiteral Parse(string data)
		{
			this._data = data;
			this._tokenizer.Init(data);

			// Prime the tokenizer to obtain the first token which is the lookahead
			// This lookahead is used for predictive parsing.
			// Since we obtain one token lookahead such parser is called LL(1) Prsing.
			this._lookahead = this._tokenizer.GetNextToken();

            return Program();
        }

		/// <summary>
		/// Expects a token of a given type.
		/// </summary>
		/// <param name="tokenType"></param>
		/// <returns></returns>
		private BaseToken Eat(ETokenType tokenType)
		{
			var token = this._lookahead;

			if (token == null)
				throw new SyntaxErrorException($"Unexpected end of input, expected: '{tokenType}'");

            if (token.TokenType != tokenType)
                throw new SyntaxErrorException($"Unexpected token: '${token.Value}', expected: '{tokenType}'");

			// Advance tokenizer to next token
			this._lookahead = this._tokenizer.GetNextToken();

			return token;
        }

        /// <summary>
        /// Main entry point
		/// Program
        ///		: StatementList
		///		;
        /// </summary>
        /// <returns>The full Program AST</returns>
        public BaseLiteral Program()
		{
			return new ProgramLiteral(this.StatementList().ToArray());
		}

        /// <summary>
		/// LL parsers cannot handle left recursion.
		/// So, when we deal with LL Parsers
		///		1. We have to make it right recursive, or
		///		2. Manually split out the recursion to multiple statements
		/// 
        /// StatementList
        ///		: Statement
        ///		| StatementList Statement --> Statement Statement Statement Statement
		///		;
        /// </summary>
        /// <returns></returns>
        private List<StatementRule> StatementList()
		{
			var statementList = new List<StatementRule>();

			while(this._lookahead != null)
			{
				var statement = this.Statement();

				if (statement == null)
					break;

                statementList.Add(statement);
            }

			return statementList;
        }

        /// <summary>
        /// Statement
		///		: ExpressionStatement
		///		| BlockStatement
		///		;
        /// </summary>
        private StatementRule Statement()
		{
			switch (this._lookahead.TokenType)
			{
				case ETokenType.OPEN_CURLY_BRACES:
					return this.BlockStatement();
				default:
                    return this.ExpressionStatement();
            }
		}

		/// <summary>
		/// BlockStatement
		///		: '{' OptStatementList '}'
		///		;
		/// </summary>
		/// <returns></returns>
		private BlockStatementRule BlockStatement()
		{
			this.Eat(ETokenType.OPEN_CURLY_BRACES);

			var blockBody = (this._lookahead.TokenType == ETokenType.CLOSE_CURLY_BRACES)? new List<StatementRule>() : this.StatementList();

			this.Eat(ETokenType.CLOSE_CURLY_BRACES);

			return new BlockStatementRule(blockBody.ToArray());
		}

        /// <summary>
        /// ExpressionStatement
		///		: Expression ';'
        /// </summary>
        /// <returns></returns>
        private ExpressionStatementRule ExpressionStatement()
		{
			var expression = this.Expression();

			if (expression == null)
				return null;

			this.Eat(ETokenType.SEMICOLON);
            return new ExpressionStatementRule(expression);
        }

		/// <summary>
		/// Expression
		///		: Literal
		///		;
		/// </summary>
		/// <returns></returns>
		private BaseLiteral Expression()
		{
			return this.Literal();
		}

        /// <summary>
        /// Literals
        ///		: Numeric Literals
        ///		| String Literals
        /// </summary>
        /// <returns></returns>
        private BaseLiteral Literal()
		{
			switch (this._lookahead.TokenType)
			{
				case ETokenType.NUMBER:
					return this.NumericLiteral();
				case ETokenType.STRING:
					return this.StringLiteral();
				default:
					return null;
			}
		}

		/// <summary>
		/// NumericLiteral expects
		///		: Number token
		/// </summary>
		/// <returns></returns>
        private NumericLiteral NumericLiteral()
        {
			var token = this.Eat(ETokenType.NUMBER);
			return new NumericLiteral(int.Parse(token.Value));
        }

        /// <summary>
        /// StringLiteral expects
        ///		: String token
        /// </summary>
        /// <returns></returns>
        private StringLiteral StringLiteral()
        {
            var token = this.Eat(ETokenType.STRING);

			// Slice the string to save without double quotes
            return new StringLiteral(token.Value[1..^1]);
        }
    }
}

