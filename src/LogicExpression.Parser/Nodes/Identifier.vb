Imports LogicExpression.Parser

Friend Class Identifier
	Inherits Node

	Public ReadOnly Property Symbol As IdentifierSymbol

	Public Sub New(symbol As IdentifierSymbol)
		Me.symbol = symbol
	End Sub

	Public Overrides Function ToString() As String
		Return $"Identifier {Symbol.Name}"
	End Function
End Class
