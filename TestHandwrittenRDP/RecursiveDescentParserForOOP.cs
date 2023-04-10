﻿using System;
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

		/// <summary>
		/// Main function to parse the code
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
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
		///		| VariableStatement
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
                case ETokenType.KEYWORD_LET:
                    return this.VariableStatement();
                default:
                    return this.ExpressionStatement();
            }
		}


        /// <summary>
        /// VariableStatement
		///		: 'let' VariableDeclarationList ';'
		///		;
        /// </summary>
        /// <returns></returns>
        private BaseRule VariableStatement()
		{
			this.Eat(ETokenType.KEYWORD_LET);

			var declarations = this.VariableDeclarationList();

			this.Eat(ETokenType.SEMICOLON);

			return this._astFactory.VariableStatement(declarations);
        }

        /// <summary>
        /// VariableDeclarationList
		///		: VariableDeclaration
		///		| VariableDeclarationList ',' VariableDeclaration
		///		--> (Left recursion not allowed in LL Grammer, so to remove it in code
		///		     use a while loop)
		///		;
        /// </summary>
        /// <returns></returns>
        private List<BaseRule> VariableDeclarationList()
		{
			var declarations = new List<BaseRule>();

			do
			{
				declarations.Add(this.VariableDeclaration());
            } while (this._lookahead.TokenType == ETokenType.COMMA &&
				this.Eat(ETokenType.COMMA) != null);

			return declarations;

        }

        /// <summary>
        /// VariableDeclaration
		///		: IDENTIFIER OptVariableInitialization
		///		;
        /// </summary>
        /// <returns></returns>
        private BaseRule VariableDeclaration()
		{
			var id = this.Identifier();
			var init = (this._lookahead.TokenType != ETokenType.SEMICOLON &&
						this._lookahead.TokenType != ETokenType.COMMA) ?
						this.VariableInitializer() : null;

			return this._astFactory.VariableDeclaration(id, init);
		}

        /// <summary>
        /// VariableInitializer
		///		: SIMPLE_ASSIGNMENT AssignmentExpression
		///		;
        /// </summary>
        /// <returns></returns>
        private BaseRule VariableInitializer()
		{
			this.Eat(ETokenType.SIMPLE_ASSIGNMENT);
			return this.AssignmentExpression();
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
        ///		: AssignmentExpression
        ///		;
        /// </summary>
        /// <returns></returns>
        private BaseRule Expression()
		{
			return this.AssignmentExpression();
		}

        /// <summary>
		/// Assignment can be chained like x = y = 42;
		/// In this case recursion is Right recursion, we dont need to convert to
		/// equivalent multiple rules as LL(1) can handle Right recursion.
		/// 
        /// AssignmentExpression
		///		: AdditiveExpression
		///		| LeftHandSideExpression AssignmentOperator AssignmentExpression
		///		;
        /// </summary>
        /// <returns></returns>
        private BaseRule AssignmentExpression()
		{
			var left = this.AdditiveExpression();

			if (this._lookahead == null || !TokenizerForParser.IsAssignmentToken(this._lookahead))
				return left;

			return new AssignmentExpressionRule(
				this.AssignmentOperator(),
				this.CheckValidAssignmentTarget(left),
				this.AssignmentExpression()
				);
		}

        /// <summary>
        /// Consume assigment operator token
        ///
        /// AssignmentOperator
		///		: SIMPLE_ASSIGNMENT
		///		| COMPLEX_ASSIGNMENT
		///		;
        /// </summary>
        /// <returns></returns>
        private BaseToken AssignmentOperator()
		{
			if (this._lookahead.TokenType == ETokenType.SIMPLE_ASSIGNMENT)
				return this.Eat(ETokenType.SIMPLE_ASSIGNMENT);
			return this.Eat(ETokenType.COMPLEX_ASSIGNMENT);
		}

        /// <summary>
        /// LeftHandSideExpression
		///		: Idenitifer
		///		;
        /// </summary>
        /// <returns></returns>
        private BaseRule LeftHandSideExpression()
		{
			return this.Identifier();
		}

        /// <summary>
        /// Identifier
		///		: IDENTIFIER
		///		;
        /// </summary>
        /// <returns></returns>
        private BaseRule Identifier()
		{
			var token = this.Eat(ETokenType.IDENTIFIER);
            return this._astFactory.Identifier(token.Value);
        }

		/// <summary>
		/// Extra check if the left hand side is a valid assignment target
		/// </summary>
		/// <param name="rule"></param>
		/// <returns></returns>
		/// <exception cref="SyntaxErrorException"></exception>
		private BaseRule CheckValidAssignmentTarget(BaseRule rule)
		{
			if (rule.LiteralType == ELiteralType.Identifier)
				return rule;
			throw new SyntaxErrorException("Invalid left-hand side in assignment expression");
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
		///		| LeftHandSideExpression
		///		;
        /// </summary>
        /// <returns></returns>
        private BaseRule PrimaryExpression()
		{
			if(this.IsLiteral(this._lookahead.TokenType))
				return this.Literal();

            switch (this._lookahead.TokenType)
			{
				case ETokenType.LEFT_PARENTHESIS:
					return this.ParenthesizedExpression();
				default:
                    return this.LeftHandSideExpression();
            }
		}

		private bool IsLiteral(ETokenType tokenType)
		{
			return tokenType == ETokenType.NUMBER || tokenType == ETokenType.STRING;
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

