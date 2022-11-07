﻿/* Empiria Extensions ****************************************************************************************
*                                                                                                            *
*  Module   : Empiria Expressions                        Component : Domain Layer                            *
*  Assembly : Empiria.Extensions.dll                     Pattern   : Information Holder                      *
*  Type     : Expression                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides a service to evaluate an expression.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Dynamic;

namespace Empiria.Expressions {

  /// <summary>Provides a service to evaluate an expression.</summary>
  public class Expression {

    private readonly string _expression;

    private readonly IExecutable _executable;

    public Expression(string expression) {
      Assertion.Require(expression, nameof(expression));

      _expression = expression;

      _executable = Compile();
    }


    public T Evaluate<T>(DynamicObject data) {
      return _executable.Execute<T>(data);
    }


    private IExecutable Compile() {
      var tokenizer = new Tokenizer();

      FixedList<IToken> tokens = tokenizer.Tokenize(_expression);

      var parser = new SyntaxTreeParser(tokens);

      SyntaxTree syntaxTree = parser.SyntaxTree();

      SymbolTable symbolTable = new SymbolTable(syntaxTree);

      return new ExpressionEvaluator(syntaxTree, symbolTable);
    }

  }  // class Expression

}  // namespace Empiria.Expressions