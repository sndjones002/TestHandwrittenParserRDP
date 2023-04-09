using System;
using System.Data;

namespace TestHandwrittenRDP
{
	public class RecursiveDescentParserForOOP
    {
		private string _data;
		private TokenizerForParser _tokenizer;
		private BaseToken _lookahead;
		private ASTFactoryBase _astFactory;

        public RecursiveDescentParserForOOP(ASTFactoryBase astFactory = null)
		{
			this._data = string.Empty;
            this._tokenizer = new TokenizerForParser();
			this._astFactory = astFactory ?? new ASTFactoryDefault();
		}

		public BaseRule Parse(string data)
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
                throw new SyntaxErrorException($"Unexpected token: '{token.Value}', expected: '{tokenType}'");

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
        public BaseRule Program()
		{
			return _astFactory.Program(this.StatementList());
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
        private List<BaseRule> StatementList(ETokenType? stopLookaheadType = null)
		{
			var statementList = new List<BaseRule>();

			while(this._lookahead != null &&
				(stopLookaheadType != null? this._lookahead.TokenType != stopLookaheadType : true))
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
		///		| EmptyStatement
		///		;
        /// </summary>
        private BaseRule Statement()
		{
			switch (this._lookahead.TokenType)
			{
				// Checking the semicolon for empty statement.
				// This is a case of predictive parsing, we are looking ahead
				case ETokenType.SEMICOLON:
					return this.EmptyStatement();
				case ETokenType.OPEN_CURLY_BRACES:
					return this.BlockStatement();
				default:
                    return this.ExpressionStatement();
            }
		}

        /// <summary>
        /// EmptyStatement
		///		: ';'
		///		;
        /// </summary>
        /// <returns></returns>
        private BaseRule EmptyStatement()
		{
            this.Eat(ETokenType.SEMICOLON);
			return _astFactory.EmptyStatement();
        }

        /// <summary>
        /// BlockStatement
        ///		: '{' OptStatementList '}'
        ///		;
        /// </summary>
        /// <returns></returns>
        private BaseRule BlockStatement()
		{
			this.Eat(ETokenType.OPEN_CURLY_BRACES);

			// This is the optional statement list
			var blockBody = (this._lookahead.TokenType == ETokenType.CLOSE_CURLY_BRACES)? new List<BaseRule>() : this.StatementList(ETokenType.CLOSE_CURLY_BRACES);

			this.Eat(ETokenType.CLOSE_CURLY_BRACES);

			return this._astFactory.BlockStatement(blockBody);
		}

        /// <summary>
        /// ExpressionStatement
		///		: Expression ';'
        /// </summary>
        /// <returns></returns>
        private BaseRule ExpressionStatement()
		{
			var expression = this.Expression();

			if (expression == null)
				return null;

			this.Eat(ETokenType.SEMICOLON);
			return this._astFactory.ExpressionStatement(expression);
        }

        /// <summary>
        /// Expression
        ///		: AdditiveExpression
        ///		;
        /// </summary>
        /// <returns></returns>
        private BaseRule Expression()
		{
			return this.AdditiveExpression();
		}

        /// <summary>
        /// Literal (Operand) has the higher precedence than the operator.
        /// That means the lietral will be inside the binary expression.
        /// [The closer the production is to the starting symbol the lower the
        /// precedence it has.]
        /// 
        /// AdditiveExpression
        ///		: MultiplicativeExpression
        ///		| AdditiveExpression ADDITIVE_OPERATOR MultiplicativeExpression
		///		--> (remove left recursion)
        ///		;
        /// </summary>
        /// <returns></returns>
        private BaseRule AdditiveExpression()
		{
			return this.BinaryExpression(ETokenType.ADDITIVE_OPERATOR, this.MultiplicativeExpression);
		}

        /// <summary>
        /// MultiplicativeExpression
		///		: PrimaryExpression
		///		| MultiplicativeExpression MULTIPLICATIVE_OPERATOR PrimaryExpression
		///		--> (Remove left recursion)
		///		;
        /// </summary>
        /// <returns></returns>
        private BaseRule MultiplicativeExpression()
		{
            return this.BinaryExpression(ETokenType.MULTIPLICATIVE_OPERATOR, this.PrimaryExpression);
        }

        /// <summary>
        /// MultiplicativeExpression and AdditiveExpression has the same logic differs only by
		/// rule builder method and the token types.
        /// </summary>
        /// <param name="tokenType"></param>
        /// <param name="ruleBuilder"></param>
        /// <returns></returns>
        private BaseRule BinaryExpression(ETokenType tokenType, Func<BaseRule> ruleBuilder)
		{
            var left = ruleBuilder();

            while (this._lookahead != null && this._lookahead.TokenType == tokenType)
            {
                var op = this.Eat(tokenType);
                var right = ruleBuilder();

                left = new BinaryExpressionRule(op, left, right);
            }

            return left;
        }

        /// <summary>
        /// PrimaryExpression
		///		: Literal
		///		| ParenthesizedExpression
		///		;
        /// </summary>
        /// <returns></returns>
        private BaseRule PrimaryExpression()
		{
			switch(this._lookahead.TokenType)
			{
				case ETokenType.LEFT_PARENTHESIS:
					return this.ParenthesizedExpression();
				default:
                    return this.Literal();
            }
		}

        /// <summary>
        /// ParenthesizedExpression
		///		: '(' Expression ')'
		///		;
        /// </summary>
        /// <returns></returns>
        private BaseRule ParenthesizedExpression()
		{
			this.Eat(ETokenType.LEFT_PARENTHESIS);

			var expression = this.Expression();

			this.Eat(ETokenType.RIGHT_PARENTHESIS);

			return expression;
		}

        /// <summary>
        /// Literals
        ///		: Numeric Literals
        ///		| String Literals
        /// </summary>
        /// <returns></returns>
        private BaseRule Literal()
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
        private NumericLiteralRule NumericLiteral()
        {
			var token = this.Eat(ETokenType.NUMBER);
			return this._astFactory.NumericLiteral(token.Value);
        }

        /// <summary>
        /// StringLiteral expects
        ///		: String token
        /// </summary>
        /// <returns></returns>
        private StringLiteralRule StringLiteral()
        {
            var token = this.Eat(ETokenType.STRING);

			// Slice the string to save without double quotes
            return this._astFactory.StringLiteral(token.Value);
        }
    }
}

