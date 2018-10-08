Imports LogicExpression.Parser

Public Class TreeItem
	Inherits TreeViewItem

	Public Sub New(node As Node)
		Me.Header = node.ToString()
		Me.IsExpanded = True

		For Each child In node.Children
			Me.AddChild(New TreeItem(child))
		Next
	End Sub

End Class
