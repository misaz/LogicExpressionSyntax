Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()> Public Class LexerTests

	<TestMethod()> Public Sub TestLexer1()
		Dim l As New Lexer("A /\ B")
		Dim output = New Stack(l.GetSymbols().Reverse().ToArray())

		Assert.IsTrue(TypeOf output.Pop() Is IdentifierSymbol)
		Assert.IsTrue(TypeOf output.Pop() Is ConjunctionSymbol)
		Assert.IsTrue(TypeOf output.Pop() Is IdentifierSymbol)
		Assert.IsTrue(output.Count = 0)
	End Sub

	<TestMethod()> Public Sub TestLexer2()
		Dim l As New Lexer("(A /\ B)")
		Dim output = New Stack(l.GetSymbols().Reverse().ToArray())

		Assert.IsTrue(TypeOf output.Pop() Is RoundBracketBeginSymbol)
		Assert.IsTrue(TypeOf output.Pop() Is IdentifierSymbol)
		Assert.IsTrue(TypeOf output.Pop() Is ConjunctionSymbol)
		Assert.IsTrue(TypeOf output.Pop() Is IdentifierSymbol)
		Assert.IsTrue(TypeOf output.Pop() Is RoundBracketEndSymbol)
		Assert.IsTrue(output.Count = 0)
	End Sub

	<TestMethod()> Public Sub TestLexer3()
		Dim l As New Lexer("A => B")
		Dim output = New Stack(l.GetSymbols().Reverse().ToArray())

		Assert.IsTrue(TypeOf output.Pop() Is IdentifierSymbol)
		Assert.IsTrue(TypeOf output.Pop() Is ImplicationSymbol)
		Assert.IsTrue(TypeOf output.Pop() Is IdentifierSymbol)
		Assert.IsTrue(output.Count = 0)
	End Sub

	<TestMethod()> Public Sub TestLexer4()
		Dim l As New Lexer("A \/ B")
		Dim output = New Stack(l.GetSymbols().Reverse().ToArray())

		Assert.IsTrue(TypeOf output.Pop() Is IdentifierSymbol)
		Assert.IsTrue(TypeOf output.Pop() Is DisjunctionSymbol)
		Assert.IsTrue(TypeOf output.Pop() Is IdentifierSymbol)
		Assert.IsTrue(output.Count = 0)
	End Sub

	<TestMethod()> Public Sub TestLexer5()
		Dim l As New Lexer("A <=> B")
		Dim output = New Stack(l.GetSymbols().Reverse().ToArray())

		Assert.IsTrue(TypeOf output.Pop() Is IdentifierSymbol)
		Assert.IsTrue(TypeOf output.Pop() Is EquivalenceSymbol)
		Assert.IsTrue(TypeOf output.Pop() Is IdentifierSymbol)
		Assert.IsTrue(output.Count = 0)
	End Sub

	<TestMethod()> Public Sub TestLexer6()
		Dim l As New Lexer("A /\ !B")
		Dim output = New Stack(l.GetSymbols().Reverse().ToArray())

		Assert.IsTrue(TypeOf output.Pop() Is IdentifierSymbol)
		Assert.IsTrue(TypeOf output.Pop() Is ConjunctionSymbol)
		Assert.IsTrue(TypeOf output.Pop() Is NegationSymbol)
		Assert.IsTrue(TypeOf output.Pop() Is IdentifierSymbol)
		Assert.IsTrue(output.Count = 0)
	End Sub

	<TestMethod()> Public Sub TestLexer7()
		Dim l As New Lexer("(A /\ !B) => !CDEF")
		Dim output = New Stack(l.GetSymbols().Reverse().ToArray())

		Assert.IsTrue(TypeOf output.Pop() Is RoundBracketBeginSymbol)
		Assert.IsTrue(TypeOf output.Pop() Is IdentifierSymbol)
		Assert.IsTrue(TypeOf output.Pop() Is ConjunctionSymbol)
		Assert.IsTrue(TypeOf output.Pop() Is NegationSymbol)
		Assert.IsTrue(TypeOf output.Pop() Is IdentifierSymbol)
		Assert.IsTrue(TypeOf output.Pop() Is RoundBracketEndSymbol)
		Assert.IsTrue(TypeOf output.Pop() Is ImplicationSymbol)
		Assert.IsTrue(TypeOf output.Pop() Is NegationSymbol)
		Assert.IsTrue(TypeOf output.Pop() Is IdentifierSymbol)
		Assert.IsTrue(output.Count = 0)
	End Sub


End Class