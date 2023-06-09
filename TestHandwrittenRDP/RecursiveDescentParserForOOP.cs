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
		///		| IfStatement
		///		| IterationStatement
		///		| FunctionDeclaration
		///		| ReturnStatement
		///		| ClassDeclaration
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
                case ETokenType.KEYWORD_IF:
                    return this.IfStatement();
				case ETokenType.KEYWORD_DEF:
					return this.FunctionDeclaration();
				case ETokenType.KEYWORD_RETURN:
					return this.ReturnStatement();
				case ETokenType.KEYWORD_WHILE:
				case ETokenType.KEYWORD_DO:
				case ETokenType.KEYWORD_FOR:
					return this.IterationStatement();
				case ETokenType.KEYWORD_CLASS:
					return this.ClassDeclaration();
                default:
                    return this.ExpressionStatement();
            }
		}

        /// <summary>
        /// ClassDeclaration
		///		: 'class' Identifier OptClassExtends BlockStatement
		///		;
        /// </summary>
        /// <returns></returns>
        private BaseRule ClassDeclaration()
		{
			this.Eat(ETokenType.KEYWORD_CLASS);

			var id = this.Identifier();

			var baseClass = (this._lookahead.TokenType == ETokenType.COLON) ?
				this.ClassExtends() : null;

			var body = this.BlockStatement();

			return this._astFactory.ClassDeclaration(id, baseClass, body);
		}

        /// <summary>
        /// ClassExtends
		///		: ':' Identifier
		///		;
        /// </summary>
        /// <returns></returns>
        private BaseRule ClassExtends()
		{
			this.Eat(ETokenType.COLON);
			return this.Identifier();
		}

        /// <summary>
        /// FunctionDeclaration
		///		: 'def' Identifier '(' OptFormalParameterList ')' BlockStatement
		///		;
        /// </summary>
        /// <returns></returns>
        private BaseRule FunctionDeclaration()
		{
			this.Eat(ETokenType.KEYWORD_DEF);

			var name = this.Identifier();

			this.Eat(ETokenType.LEFT_PARENTHESIS);
			var parameters = (this._lookahead.TokenType != ETokenType.RIGHT_PARENTHESIS) ?
					this.FormalParameterList() : null;

			this.Eat(ETokenType.RIGHT_PARENTHESIS);

			var body = this.BlockStatement();

			return this._astFactory.FunctionDeclaration(name, parameters, body);
		}

        /// <summary>
        /// FormalParameterList
		///		: Identifier
		///		| FormalParameterList ',' Identifier
		///		;
        /// </summary>
        /// <returns></returns>
        private List<BaseRule> FormalParameterList()
		{
			List<BaseRule> parameters = new List<BaseRule>();

			do
			{
				parameters.Add(this.Identifier());
            } while(this._lookahead.TokenType == ETokenType.COMMA &&
				this.Eat(ETokenType.COMMA) != null);

			return parameters.Count == 0? null : parameters;
        }

        /// <summary>
        /// ReturnStatement
		///		: 'return' OptExpression ';'
		///		;
        /// </summary>
        /// <returns></returns>
        private BaseRule ReturnStatement()
		{
			this.Eat(ETokenType.KEYWORD_RETURN);

			var argument = (this._lookahead.TokenType != ETokenType.SEMICOLON) ?
					this.Expression() : null;
			this.Eat(ETokenType.SEMICOLON);

			return this._astFactory.ReturnStatement(argument);
		}

        /// <summary>
        /// IterationStatement
		///		: WhileStatement
		///		| DoWhileStatement
		///		| ForStatement
        /// </summary>
        /// <returns></returns>
        private BaseRule IterationStatement()
		{
			switch(this._lookahead.TokenType)
			{
				case ETokenType.KEYWORD_WHILE:
					return this.WhileStatement();
                case ETokenType.KEYWORD_DO:
                    return this.DoWhileStatement();
                case ETokenType.KEYWORD_FOR:
                    return this.ForStatement();
            }

			// Code should never reach this part
			return null;
		}

        /// <summary>
        /// WhileStatement
		///		: 'while' '(' Expression ')' Statement
		///		;
        /// </summary>
        /// <returns></returns>
        private BaseRule WhileStatement()
		{
			this.Eat(ETokenType.KEYWORD_WHILE);

			this.Eat(ETokenType.LEFT_PARENTHESIS);
			var test = this.Expression();
			this.Eat(ETokenType.RIGHT_PARENTHESIS);

			var body = this.Statement();

			return this._astFactory.WhileStatement(test, body);
		}

        /// <summary>
        /// DoWhileStatement
		///		: 'do' Statement 'while' '(' Expression ')' ';'
        /// </summary>
        /// <returns></returns>
        private BaseRule DoWhileStatement()
        {
            this.Eat(ETokenType.KEYWORD_DO);

            var body = this.Statement();

            this.Eat(ETokenType.KEYWORD_WHILE);

            this.Eat(ETokenType.LEFT_PARENTHESIS);
            var test = this.Expression();
            this.Eat(ETokenType.RIGHT_PARENTHESIS);

            this.Eat(ETokenType.SEMICOLON);

            return this._astFactory.DoWhileStatement(body, test);
        }

        /// <summary>
        /// ForStatement
		///		: 'for' '(' ForStatementInit ';' OptBinaryExpression ';' OptAssignmentExpression ')' Statement
		///		;
        /// </summary>
        /// <returns></returns>
        private BaseRule ForStatement()
        {
			this.Eat(ETokenType.KEYWORD_FOR);
			this.Eat(ETokenType.LEFT_PARENTHESIS);

			BaseRule init = (this._lookahead.TokenType != ETokenType.SEMICOLON) ?
				this.ForStatementInit() : null;
			this.Eat(ETokenType.SEMICOLON);

			BaseRule test = (this._lookahead.TokenType != ETokenType.SEMICOLON) ?
				this.Expression() : null;
            this.Eat(ETokenType.SEMICOLON);

            BaseRule update = (this._lookahead.TokenType != ETokenType.RIGHT_PARENTHESIS) ?
				this.Expression() : null;

            this.Eat(ETokenType.RIGHT_PARENTHESIS);

            var body = this.Statement();

            return this._astFactory.ForStatement(init, test, update, body);
        }

        /// <summary>
        /// ForStatementInit
		///		: VariableStatementInit
		///		| Expression
		///		;
        /// </summary>
        /// <returns></returns>
        private BaseRule ForStatementInit()
		{
			if(this._lookahead.TokenType == ETokenType.KEYWORD_LET)
			{
				return this.VariableStatementInit();
            }

			return this.Expression();
		}

        /// <summary>
		/// The problem with else clause arises when there are nested if statements and
		/// only one else at the end. The question is to which if it bounds to?
		/// In Java, C, C#, you bind the else to the closest if.
		/// This means you will shift the else token.
		///
		/// However in manual parsing there is no issue, it is an issue with automatically
		/// generated parsers.
		/// 
        /// IfStatement
		///		: 'if' '(' Expression ')' Statement
		///		| 'if' '(' Expression ')' Statement 'else' Statement
		///		;
        /// </summary>
        /// <returns></returns>
        private BaseRule IfStatement()
		{
			this.Eat(ETokenType.KEYWORD_IF);
			this.Eat(ETokenType.LEFT_PARENTHESIS);

			var test = this.Expression();

			this.Eat(ETokenType.RIGHT_PARENTHESIS);

			var consequent = this.Statement();
            BaseRule alternate = null;

			if(this._lookahead != null &&
				this._lookahead.TokenType == ETokenType.KEYWORD_ELSE)
			{
				this.Eat(ETokenType.KEYWORD_ELSE);
                alternate = this.Statement();
            }

			return this._astFactory.IfStatement(test, consequent, alternate);
		}

        /// <summary>
        /// VariableStatementInit
		///		: 'let' VariableDeclarationList
		///		;
        /// </summary>
        /// <returns></returns>
        private BaseRule VariableStatementInit()
		{
            this.Eat(ETokenType.KEYWORD_LET);

            var declarations = this.VariableDeclarationList();

            return this._astFactory.VariableStatement(declarations);
        }

        /// <summary>
        /// VariableStatement
		///		: VariableStatementInit ';'
		///		;
        /// </summary>
        /// <returns></returns>
        private BaseRule VariableStatement()
		{
			var variableStatement = this.VariableStatementInit();
			this.Eat(ETokenType.SEMICOLON);

			return variableStatement;
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
            } while (this._lookahead != null && this._lookahead.TokenType == ETokenType.COMMA &&
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
			var init = (this._lookahead != null && this._lookahead.TokenType != ETokenType.SEMICOLON &&
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
			var blockBody = (this._lookahead != null && this._lookahead.TokenType == ETokenType.CLOSE_CURLY_BRACES)? new List<BaseRule>() : this.StatementList(ETokenType.CLOSE_CURLY_BRACES);

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
		///		: LogicalORExpression
		///		| LeftHandSideExpression AssignmentOperator AssignmentExpression
		///		;
        /// </summary>
        /// <returns></returns>
        private BaseRule AssignmentExpression()
		{
			var left = this.LogicalORExpression();

			if (this._lookahead == null || !TokenizerForParser.IsAssignmentToken(this._lookahead))
				return left;

			return new AssignmentExpressionRule(
				this.AssignmentOperator(),
				this.CheckValidAssignmentTarget(left),
				this.AssignmentExpression()
				);
		}

        /// <summary>
        /// Logical OR has even lower precedence than Logical AND
        ///
        /// LogicalORExpression
        ///		: LogicalANDExpression LOGICAL_OR LogicalORExpression
        ///		| LogicalORExpression
        ///		;
        /// </summary>
        /// <returns></returns>
        private BaseRule LogicalORExpression()
        {
            return this.LogicalExpression(ETokenType.LOGICAL_OR, this.LogicalANDExpression);

        }

        /// <summary>
        /// Logical AND has lower precedence than equality but has higher
		/// precedence than Assignment
        ///
        /// LogicalANDExpression
		///		: EqualityExpression LOGICAL_AND LogicalANDExpression
		///		| EqualityExpression
		///		;
        /// </summary>
        /// <returns></returns>
        private BaseRule LogicalANDExpression()
		{
			return this.LogicalExpression(ETokenType.LOGICAL_AND, this.EqualityExpression);

        }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private BaseRule LogicalExpression(ETokenType tokenType, Func<BaseRule> ruleBuilder)
		{
			var left = ruleBuilder();

			while(this._lookahead != null &&
				this._lookahead.TokenType == tokenType)
			{
                var op = this.Eat(tokenType);
                var right = ruleBuilder();

                left = this._astFactory.LogicalExpression(op, left, right);
            }

            return left;
        }

        /// <summary>
        /// EQUALITY_OPERATOR == !=
        ///
        /// EqualityExpression
		///		: RelationalExpression EQUALITY_OPERATOR EqualityExpression
		///		| RelationalExpression
        /// </summary>
        /// <returns></returns>
        private BaseRule EqualityExpression()
		{
            return this.BinaryExpression(ETokenType.EQUALITY_OPERATOR, this.RelationalExpression);
        }

        /// <summary>
		/// RELATIONAL_OPERATORS <, >=, <, <=
		/// 
        /// RelationalExpression
		///		: AdditiveExpression
		///		| AdditiveExpression RELATIONAL_OPERATORS RelationalExpression
		///		;
        /// </summary>
        /// <returns></returns>
        private BaseRule RelationalExpression()
		{
            return this.BinaryExpression(ETokenType.RELATIONAL_OPERATOR, this.AdditiveExpression);
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
			if (rule.LiteralType == ELiteralType.Identifier || rule.LiteralType == ELiteralType.MemberExpression)
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
		///		: UnaryExpression
		///		| MultiplicativeExpression MULTIPLICATIVE_OPERATOR UnaryExpression
		///		--> (Remove left recursion)
		///		;
        /// </summary>
        /// <returns></returns>
        private BaseRule MultiplicativeExpression()
		{
            return this.BinaryExpression(ETokenType.MULTIPLICATIVE_OPERATOR, this.UnaryExpression);
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

                left = this._astFactory.BinaryExpression(op, left, right);
            }

            return left;
        }

        /// <summary>
        /// UnaryExpression
		///		: LeftHandSideExpression
		///		| ADDITIVE_OPERATOR UnaryExpression
		///		| LOGICAL_NOT UnaryExpression
		///		;
        /// </summary>
        /// <returns></returns>
        private BaseRule UnaryExpression()
		{
			BaseToken op = null;

			switch(this._lookahead.TokenType)
			{
				case ETokenType.ADDITIVE_OPERATOR:
				case ETokenType.LOGICAL_NOT:
					op = this.Eat(this._lookahead.TokenType);
					break;
			}

			if (op != null)
			{
				return this._astFactory.UnaryExpression(op, this.UnaryExpression());
			}
			else
				return this.LeftHandSideExpression();
		}

        /// <summary>
        /// LeftHandSideExpression
        ///		: CallMemberExpression
        ///		;
        /// </summary>
        /// <returns></returns>
        private BaseRule LeftHandSideExpression()
        {
            return this.CallMemberExpression();
        }

        /// <summary>
        /// CallMemberExpression
		///		: CallExpression // with 'base' 
		///		| MemberExpression
		///		| CallExpression
		///		;
        /// </summary>
        /// <returns></returns>
        private BaseRule CallMemberExpression()
		{
			if(this._lookahead.TokenType == ETokenType.KEYWORD_BASE)
				return this.CallExpression(this.BaseCall());

            var member = this.MemberExpression();

			if (this._lookahead != null && this._lookahead.TokenType == ETokenType.LEFT_PARENTHESIS)
				return this.CallExpression(member);
			return member;
        }

        /// <summary>
        /// Generic Call expression helper
        ///
		/// CallExpression
		///		: Callee Arguments
		///		;
		///		
        /// Callee
		///		: MemberExpression
		///		| CallExpression
		///		;
		///		
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        private BaseRule CallExpression(BaseRule callee)
		{
			var callExpression = this._astFactory.CallExpression(callee, this.Arguments());

			if (this._lookahead != null && this._lookahead.TokenType == ETokenType.LEFT_PARENTHESIS)
				callExpression = this.CallExpression(callExpression);

			return callExpression;
        }

        /// <summary>
        /// Arguments
		///		: '(' OptArgumentList ')'
		///		;
        /// </summary>
        /// <returns></returns>
        private List<BaseRule> Arguments()
		{
			this.Eat(ETokenType.LEFT_PARENTHESIS);

			var arguments = (this._lookahead.TokenType != ETokenType.RIGHT_PARENTHESIS) ?
				this.ArgumentList() : null;

			this.Eat(ETokenType.RIGHT_PARENTHESIS);

			return arguments;
        }

        /// <summary>
        /// ArgumentList
		///		: AssignmentExpression
		///		| ArgumentList ',' AssignmentExpression
		///		;
        /// </summary>
        /// <returns></returns>
        private List<BaseRule> ArgumentList()
		{
			var argumentList = new List<BaseRule>();

			do
			{
				argumentList.Add(this.AssignmentExpression());
			} while (this._lookahead.TokenType == ETokenType.COMMA &&
				this.Eat(ETokenType.COMMA) != null);

			return argumentList;
        }

        /// <summary>
        /// MemberExpression
		///		: PrimaryExpression
		///		| MemberExpression '.' Identifier
		///		| MemberExpression '[' Expression ']'
		///		;
        /// </summary>
        /// <returns></returns>
        private BaseRule MemberExpression()
		{
			var obj = this.PrimaryExpression();

			while(this._lookahead != null && (this._lookahead.TokenType == ETokenType.CHAR_DOT ||
				this._lookahead.TokenType == ETokenType.CHAR_LEFTBRACKET))
			{
				if(this._lookahead.TokenType == ETokenType.CHAR_DOT)
				{
					this.Eat(ETokenType.CHAR_DOT);
					var property = this.Identifier();

					obj = this._astFactory.MemberExpression(false, obj, property);
				}

                if (this._lookahead.TokenType == ETokenType.CHAR_LEFTBRACKET)
                {
					this.Eat(ETokenType.CHAR_LEFTBRACKET);
                    var property = this.Expression();
                    this.Eat(ETokenType.CHAR_RIGHTBRACKET);
                    
                    obj = this._astFactory.MemberExpression(true, obj, property);
                }
            }

			return obj;
		}

        /// <summary>
        /// PrimaryExpression
		///		: Literal
		///		| ParenthesizedExpression
		///		| Identifier
		///		| ThisExpression
		///		| NewExpression
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
				case ETokenType.IDENTIFIER:
					return this.Identifier();
				case ETokenType.KEYWORD_THIS:
					return this.ThisExpression();
				case ETokenType.KEYWORD_NEW:
					return this.NewExpression();
				default:
                    throw new SyntaxErrorException("Unexpected primary expression");
            }
		}

        /// <summary>
        /// NewExpression
		///		: 'new' MemberExpression Arguments
		///		;
		///
		/// e.g., new MyNamespace.MyClass(1, 2)
		/// 
        /// </summary>
        /// <returns></returns>
        private BaseRule NewExpression()
		{
			this.Eat(ETokenType.KEYWORD_NEW);

			return this._astFactory.NewExpression(this.MemberExpression(), this.ArgumentList());
		}

        /// <summary>
        /// ThisExpression
		///		: 'this'
        /// </summary>
        /// <returns></returns>
        private BaseRule ThisExpression()
		{
			this.Eat(ETokenType.KEYWORD_THIS);

			return this._astFactory.ThisExpression();
		}

        /// <summary>
		/// To support that the callee is base type we have to introduce a AST Node.
		/// 
        /// BaseCall
        ///		: 'this'
        /// </summary>
        /// <returns></returns>
        private BaseRule BaseCall()
        {
            this.Eat(ETokenType.KEYWORD_BASE);

            return this._astFactory.BaseCall();
        }

        /// <summary>
        /// Literals
        /// </summary>
        /// <param name="tokenType"></param>
        /// <returns></returns>
        private bool IsLiteral(ETokenType tokenType)
		{
			return tokenType == ETokenType.NUMBER ||
				tokenType == ETokenType.STRING ||
				tokenType == ETokenType.KEYWORD_FALSE ||
				tokenType == ETokenType.KEYWORD_TRUE ||
                tokenType == ETokenType.KEYWORD_NULL
                ;
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
        /// Literal
        ///		: NumericLiteral
        ///		| StringLiteral
		///		| BooleanLiteral
		///		| NullLiteral
		///		;
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
                case ETokenType.KEYWORD_TRUE:
                    return this.BooleanLiteral(ETokenType.KEYWORD_TRUE);
                case ETokenType.KEYWORD_FALSE:
                    return this.BooleanLiteral(ETokenType.KEYWORD_FALSE);
                case ETokenType.KEYWORD_NULL:
                    return this.NullLiteral();
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

        /// <summary>
        /// BooleanLiteral expects
        ///		: boolean token
        /// </summary>
        /// <returns></returns>
        private BooleanLiteralRule BooleanLiteral(ETokenType tokenType)
        {
            var token = this.Eat(tokenType);

            // Slice the string to save without double quotes
            return this._astFactory.BooleanLiteral(token.Value);
        }

        /// <summary>
        /// NullLiteral expects
        ///		: null token
        /// </summary>
        /// <returns></returns>
        private NullLiteralRule NullLiteral()
        {
            var token = this.Eat(ETokenType.KEYWORD_NULL);

            // Slice the string to save without double quotes
            return this._astFactory.NullLiteral(token.Value);
        }
    }
}

