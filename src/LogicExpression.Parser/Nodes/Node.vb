Public MustInherit Class Node

	Public Property Children As New List(Of Node)

	Public Overrides Function ToString() As String
		Return Me.GetType().Name
	End Function

End Class
