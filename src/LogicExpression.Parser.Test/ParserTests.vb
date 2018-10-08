Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()> Public Class ParserTests

	<TestMethod()> Public Sub TestMethod1()
		Dim par As New Parser("A => B")
		Dim tree = par.Tree

		Assert.IsTrue(tree.Children.Count = 1)
		Assert.IsTrue(TypeOf tree.Children(0) Is Implication)
		Assert.IsTrue(tree.Children(0).Children.Count = 2)
		Assert.IsTrue(TypeOf tree.Children(0).Children(0) Is Identifier)
		Assert.IsTrue(TypeOf tree.Children(0).Children(1) Is Identifier)
		Assert.IsTrue(CType(tree.Children(0).Children(0), Identifier).Symbol.Name = "A")
		Assert.IsTrue(CType(tree.Children(0).Children(1), Identifier).Symbol.Name = "B")
	End Sub

	<TestMethod()> Public Sub TestMethod2()
		Dim par As New Parser("A /\ B")
		Dim tree = par.Tree

		Assert.IsTrue(tree.Children.Count = 1)
		Assert.IsTrue(TypeOf tree.Children(0) Is Conjunction)
		Assert.IsTrue(tree.Children(0).Children.Count = 2)
		Assert.IsTrue(TypeOf tree.Children(0).Children(0) Is Identifier)
		Assert.IsTrue(TypeOf tree.Children(0).Children(1) Is Identifier)
		Assert.IsTrue(CType(tree.Children(0).Children(0), Identifier).Symbol.Name = "A")
		Assert.IsTrue(CType(tree.Children(0).Children(1), Identifier).Symbol.Name = "B")
	End Sub

	<TestMethod()> Public Sub TestMethod3()
		Dim par As New Parser("A \/ B")
		Dim tree = par.Tree

		Assert.IsTrue(tree.Children.Count = 1)
		Assert.IsTrue(TypeOf tree.Children(0) Is Disjunction)
		Assert.IsTrue(tree.Children(0).Children.Count = 2)
		Assert.IsTrue(TypeOf tree.Children(0).Children(0) Is Identifier)
		Assert.IsTrue(TypeOf tree.Children(0).Children(1) Is Identifier)
		Assert.IsTrue(CType(tree.Children(0).Children(0), Identifier).Symbol.Name = "A")
		Assert.IsTrue(CType(tree.Children(0).Children(1), Identifier).Symbol.Name = "B")
	End Sub

	<TestMethod()> Public Sub TestMethod4()
		Dim par As New Parser("A \/ !B")
		Dim tree = par.Tree

		Assert.IsTrue(tree.Children.Count = 1)
		Assert.IsTrue(TypeOf tree.Children(0) Is Disjunction)
		Assert.IsTrue(tree.Children(0).Children.Count = 2)
		Assert.IsTrue(TypeOf tree.Children(0).Children(0) Is Identifier)
		Assert.IsTrue(TypeOf tree.Children(0).Children(1) Is Negation)
		Assert.IsTrue(CType(tree.Children(0).Children(0), Identifier).Symbol.Name = "A")
		Assert.IsTrue(CType(tree.Children(0).Children(1).Children(0), Identifier).Symbol.Name = "B")
	End Sub


	<TestMethod()> Public Sub TestMethod5()
		Dim par As New Parser("!A \/ B")
		Dim tree = par.Tree

		Assert.IsTrue(tree.Children.Count = 1)
		Assert.IsTrue(TypeOf tree.Children(0) Is Disjunction)
		Assert.IsTrue(tree.Children(0).Children.Count = 2)
		Assert.IsTrue(TypeOf tree.Children(0).Children(0) Is Negation)
		Assert.IsTrue(TypeOf tree.Children(0).Children(1) Is Identifier)
		Assert.IsTrue(CType(tree.Children(0).Children(0).Children(0), Identifier).Symbol.Name = "A")
		Assert.IsTrue(CType(tree.Children(0).Children(1), Identifier).Symbol.Name = "B")
	End Sub

	<TestMethod()> Public Sub TestMethod6()
		Dim par As New Parser("!A \/ (B => C)")
		Dim tree = par.Tree

		Assert.IsTrue(tree.Children.Count = 1)
		Assert.IsTrue(TypeOf tree.Children(0) Is Disjunction)
		Assert.IsTrue(tree.Children(0).Children.Count = 2)
		Assert.IsTrue(TypeOf tree.Children(0).Children(0) Is Negation)
		Assert.IsTrue(TypeOf tree.Children(0).Children(1) Is SubExpression)
		Assert.IsTrue(CType(tree.Children(0).Children(0).Children(0), Identifier).Symbol.Name = "A")
		Assert.IsTrue(tree.Children(0).Children(1).Children.Count = 1)
		Assert.IsTrue(TypeOf tree.Children(0).Children(1).Children(0) Is Implication)
		Assert.IsTrue(tree.Children(0).Children(1).Children(0).Children.Count = 2)
		Assert.IsTrue(TypeOf tree.Children(0).Children(1).Children(0).Children(0) Is Identifier)
		Assert.IsTrue(TypeOf tree.Children(0).Children(1).Children(0).Children(1) Is Identifier)
		Assert.IsTrue(CType(tree.Children(0).Children(1).Children(0).Children(0), Identifier).Symbol.Name = "B")
		Assert.IsTrue(CType(tree.Children(0).Children(1).Children(0).Children(1), Identifier).Symbol.Name = "C")
	End Sub

	<TestMethod()> Public Sub TestMethod7()
		Dim par As New Parser("!(A <=> E) /\ (B => C)")
		Dim tree = par.Tree

		Assert.IsTrue(tree.Children.Count = 1)
		Assert.IsTrue(TypeOf tree.Children(0) Is Conjunction)
		Assert.IsTrue(tree.Children(0).Children.Count = 2)
		Assert.IsTrue(TypeOf tree.Children(0).Children(0) Is Negation)
		Assert.IsTrue(TypeOf tree.Children(0).Children(1) Is SubExpression)
		Assert.IsTrue(tree.Children(0).Children(0).Children.Count = 1)
		Assert.IsTrue(TypeOf tree.Children(0).Children(0).Children(0) Is SubExpression)
		Assert.IsTrue(tree.Children(0).Children(0).Children(0).Children.Count = 1)
		Assert.IsTrue(TypeOf tree.Children(0).Children(0).Children(0).Children(0) Is Equivalence)
		Assert.IsTrue(tree.Children(0).Children(0).Children(0).Children(0).Children.Count = 2)
		Assert.IsTrue(TypeOf tree.Children(0).Children(0).Children(0).Children(0).Children(0) Is Identifier)
		Assert.IsTrue(CType(tree.Children(0).Children(0).Children(0).Children(0).Children(0), Identifier).Symbol.Name = "A")
		Assert.IsTrue(TypeOf tree.Children(0).Children(0).Children(0).Children(0).Children(1) Is Identifier)
		Assert.IsTrue(CType(tree.Children(0).Children(0).Children(0).Children(0).Children(1), Identifier).Symbol.Name = "E")
		Assert.IsTrue(tree.Children(0).Children(1).Children.Count = 1)
		Assert.IsTrue(TypeOf tree.Children(0).Children(1).Children(0) Is Implication)
		Assert.IsTrue(tree.Children(0).Children(1).Children(0).Children.Count = 2)
		Assert.IsTrue(TypeOf tree.Children(0).Children(1).Children(0).Children(0) Is Identifier)
		Assert.IsTrue(CType(tree.Children(0).Children(1).Children(0).Children(0), Identifier).Symbol.Name = "B")
		Assert.IsTrue(CType(tree.Children(0).Children(1).Children(0).Children(1), Identifier).Symbol.Name = "C")
	End Sub


End Class