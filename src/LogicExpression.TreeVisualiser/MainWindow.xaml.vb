Imports System.ComponentModel

Class MainWindow
	Implements INotifyPropertyChanged

	Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

	Private _inputText As String = ""
	Public Property InputText As String
		Get
			Return _inputText
		End Get
		Set(value As String)
			_inputText = value
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Tree"))
		End Set
	End Property

	Public ReadOnly Property Tree As IEnumerable(Of TreeViewItem)
		Get
			Try
				Dim parser As New Parser.Parser(InputText)
				Return New TreeViewItem() {New TreeItem(parser.Tree)}
			Catch ex As Exception
				Return New TreeViewItem() {New TreeViewItem() With {.Header = "Invalid input"}}
			End Try
		End Get
	End Property

	Public Sub New()
		InitializeComponent()
		DataContext = Me
	End Sub

End Class
