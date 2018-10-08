Imports System.Text.RegularExpressions

Public Class Lexer

	Private Shared IdentifierRegex As Regex = New Regex("^[a-zA-Z0-9_-]+")

	Public ReadOnly Property input As String

	Public Sub New(input As String)
		Me.input = input
	End Sub

	Public Iterator Function GetSymbols() As IEnumerable(Of Symbol)
		Dim pointer = 0
		While pointer < input.Length
			Dim sinceInput = input.Substring(pointer)

			If sinceInput.StartsWith("=>") Then
				Yield New ImplicationSymbol()
				pointer += 2
			ElseIf sinceInput.StartsWith("<=>") Then
				Yield New EquivalenceSymbol()
				pointer += 3
			ElseIf sinceInput.StartsWith("/\") Then
				Yield New ConjunctionSymbol()
				pointer += 2
			ElseIf sinceInput.StartsWith("\/") Then
				Yield New DisjunctionSymbol()
				pointer += 2
			ElseIf sinceInput.StartsWith("!") Then
				Yield New NegationSymbol()
				pointer += 1
			ElseIf sinceInput.StartsWith("(") Then
				Yield New RoundBracketBeginSymbol()
				pointer += 1
			ElseIf sinceInput.StartsWith(")") Then
				Yield New RoundBracketEndSymbol()
				pointer += 1
			ElseIf IdentifierRegex.IsMatch(sinceInput) Then
				Dim match = IdentifierRegex.Match(sinceInput)
				Yield New IdentifierSymbol(match.Value)
				pointer += match.Value.Length
			Else
				Debug.WriteLine($"Lexer: Unexpected character was found in input at ${pointer}: {sinceInput(0)}")
				pointer += 1
			End If
		End While
	End Function
End Class
