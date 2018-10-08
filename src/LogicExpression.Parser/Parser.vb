
Public Class Parser

	''' <summary>
	''' Zásobník symbolů v opačném pořadí. Díky toho je odhazováním nečteme zezadu, ale zepředu.
	''' </summary>
	''' <returns></returns>
	Public ReadOnly Property Symbols As Stack(Of Symbol)

	''' <summary>
	''' Čtením je vyvoláno parsování stromu
	''' </summary>
	''' <returns></returns>
	Private ReadOnly Property LazyTree As New Lazy(Of Expression)(AddressOf ParseLogicExpression)

	''' <summary>
	''' Vrátí naparsovaný strom
	''' </summary>
	Public ReadOnly Property Tree As Expression
		Get
			Try
				Return LazyTree.Value
			Catch ex As InvalidOperationException
				Throw New Exception("Logic expression is not valid")
			End Try
		End Get
	End Property

	''' <summary>
	''' Vytvoří parser jehož vstupem je předaný řetězec
	''' </summary>
	Public Sub New(input As String)
		Dim l As New Lexer(input)
		Symbols = New Stack(Of Symbol)(l.GetSymbols().Reverse())
	End Sub

	''' <summary>
	''' Vytvoří parser jehož vstupem je předaná kolekce logických symbolů.
	''' </summary>
	''' <param name="symbols"></param>
	Public Sub New(symbols As IEnumerable(Of Symbol))
		Me.Symbols = New Stack(Of Symbol)(symbols.Reverse())
	End Sub

	Private Function ParseLogicExpression() As Expression
		Dim ex As New Expression()

		While Symbols.Any()
			Dim current = Symbols.First()

			If TypeOf current Is RoundBracketBeginSymbol Then
				ex.Children.Add(ParseSubExpression())
			ElseIf TypeOf current Is NegationSymbol Then
				ex.Children.Add(ParseNegation())
			ElseIf TypeOf current Is IdentifierSymbol Then
				ex.Children.Add(ParseSimpleIdentifier())
			ElseIf TypeOf current Is ImplicationSymbol Then
				Dim leftOp = ex.Children.FirstOrDefault()
				ex.Children.Remove(leftOp)
				If leftOp Is Nothing Then
					Throw New InvalidOperationException()
				End If
				ex.Children.Add(ParseBinaryOperator(Of Implication)(leftOp))
			ElseIf TypeOf current Is EquivalenceSymbol Then
				Dim leftOp = ex.Children.FirstOrDefault()
				ex.Children.Remove(leftOp)
				If leftOp Is Nothing Then
					Throw New InvalidOperationException()
				End If
				ex.Children.Add(ParseBinaryOperator(Of Equivalence)(leftOp))
			ElseIf TypeOf current Is ConjunctionSymbol Then
				Dim leftOp = ex.Children.FirstOrDefault()
				ex.Children.Remove(leftOp)
				If leftOp Is Nothing Then
					Throw New InvalidOperationException()
				End If
				ex.Children.Add(ParseBinaryOperator(Of Conjunction)(leftOp))
			ElseIf TypeOf current Is DisjunctionSymbol Then
				Dim leftOp = ex.Children.FirstOrDefault()
				ex.Children.Remove(leftOp)
				If leftOp Is Nothing Then
					Throw New InvalidOperationException()
				End If
				ex.Children.Add(ParseBinaryOperator(Of Disjunction)(leftOp))
			Else
				Throw New InvalidOperationException()
			End If
		End While

		Return ex
	End Function

	Private Function ParseNegation() As Negation
		Symbols.Pop()
		Dim neg As New Negation()

		Dim current = Symbols.First()
		If TypeOf current Is IdentifierSymbol Then
			neg.Children.Add(ParseSimpleIdentifier())
			Return neg
		ElseIf TypeOf current Is RoundBracketBeginSymbol Then
			neg.Children.Add(ParseSubExpression())
			Return neg
		Else
			Throw New InvalidOperationException()
		End If

		Return neg
	End Function

	Private Function ParseSubExpression() As SubExpression
		Symbols.Pop()
		Dim subex As New SubExpression()

		Dim current = Symbols.First()
		Do
			If TypeOf current Is RoundBracketBeginSymbol Then
				subex.Children.Add(ParseSubExpression())
			ElseIf TypeOf current Is NegationSymbol Then
				subex.Children.Add(ParseNegation())
			ElseIf TypeOf current Is IdentifierSymbol Then
				subex.Children.Add(ParseIdentifierOperand())
			ElseIf TypeOf current Is ImplicationSymbol Then
				Dim leftOp = subex.Children.FirstOrDefault()
				If leftOp Is Nothing Then
					Throw New InvalidOperationException()
				End If
				subex.Children.Remove(leftOp)
				subex.Children.Add(ParseBinaryOperator(Of Implication)(leftOp))
			ElseIf TypeOf current Is equivalenceSymbol Then
				Dim leftOp = subex.Children.FirstOrDefault()
				If leftOp Is Nothing Then
					Throw New InvalidOperationException()
				End If
				subex.Children.Remove(leftOp)
				subex.Children.Add(ParseBinaryOperator(Of Equivalence)(leftOp))
			ElseIf TypeOf current Is ConjunctionSymbol Then
				Dim leftOp = subex.Children.FirstOrDefault()
				If leftOp Is Nothing Then
					Throw New InvalidOperationException()
				End If
				subex.Children.Remove(leftOp)
				subex.Children.Add(ParseBinaryOperator(Of Conjunction)(leftOp))
			ElseIf TypeOf current Is DisjunctionSymbol Then
				Dim leftOp = subex.Children.FirstOrDefault()
				If leftOp Is Nothing Then
					Throw New InvalidOperationException()
				End If
				subex.Children.Remove(leftOp)
				subex.Children.Add(ParseBinaryOperator(Of Disjunction)(leftOp))
			End If
			current = Symbols.FirstOrDefault()
			If current Is Nothing Then
				Return subex
			End If
		Loop While TypeOf current IsNot RoundBracketEndSymbol
		Symbols.Pop()
		Return subex
	End Function

	Private Function ParseSimpleIdentifier() As Identifier
		Return New Identifier(Symbols.Pop())
	End Function

	Private Function ParseIdentifierOperand() As Node
		Dim l = Symbols.First()
		Symbols.Pop()
		Dim id As New Identifier(l)

		Dim current = Symbols.FirstOrDefault()
		If current Is Nothing Then
			Return id
		Else
			If TypeOf current Is ImplicationSymbol Then
				Return ParseBinaryOperator(Of Implication)(id)
			ElseIf TypeOf current Is ConjunctionSymbol Then
				Return ParseBinaryOperator(Of Conjunction)(id)
			ElseIf TypeOf current Is DisjunctionSymbol Then
				Return ParseBinaryOperator(Of Disjunction)(id)
			ElseIf TypeOf current Is EquivalenceSymbol Then
				Return ParseBinaryOperator(Of Equivalence)(id)
			ElseIf TypeOf current Is RoundBracketEndSymbol Then
				Return id
			Else
				Throw New InvalidOperationException()
			End If
		End If
	End Function

	Private Function ParseBinaryOperator(Of TargetType As Node)(leftOperand As Node) As TargetType
		Symbols.Pop()
		Dim impl As TargetType = GetType(TargetType).GetConstructor(New Type() {}).Invoke(New Object() {})

		impl.Children.Add(leftOperand)

		Dim current = Symbols.FirstOrDefault()
		If current Is Nothing Then
			Throw New InvalidOperationException()
		End If
		If TypeOf current Is NegationSymbol Then
			impl.Children.Add(ParseNegation())
		ElseIf TypeOf current Is IdentifierSymbol Then
			impl.Children.Add(ParseSimpleIdentifier())
		ElseIf TypeOf current Is RoundBracketBeginSymbol Then
			impl.Children.Add(ParseSubExpression())
		Else
			Throw New InvalidOperationException()
		End If
		Return impl
	End Function

End Class
