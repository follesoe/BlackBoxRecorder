' *----------------------------------------------------------------------------*
' *   Released to Public Domain by Gael Fraiteur                           
' *----------------------------------------------------------------------------*
' *   This file is part of samples of PostSharp.                                *
' *                                                                             *
' *   This sample is free software: you have an unlimited right to              *
' *   redistribute it and/or modify it.                                         *
' *                                                                             *
' *   This sample is distributed in the hope that it will be useful,            *
' *   but WITHOUT ANY WARRANTY; without even the implied warranty of            *
' *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.                      *
' *                                                                             *
' *----------------------------------------------------------------------------*/

Imports System.Transactions 


''' <summary>
''' Encapsulates a simple account (with an identifier and a balance) that
''' can be enlisted in a transaction.
''' </summary>
''' <remarks></remarks>
Public Class Account
    Implements ISinglePhaseNotification

    Dim _accountId As Integer
    Dim _committedBalance As Decimal
    Dim _tempBalance As Decimal
    Dim _transaction As System.Transactions.Transaction

    ''' <summary>
    ''' Initializes a new <see cref="Account"/>.
    ''' </summary>
    ''' <param name="accountId">Account number.</param>
    ''' <param name="initialBalance">Initial balance.</param>
    Public Sub New(ByVal accountId As Integer, ByVal initialBalance As Decimal)

        Me._accountId = accountId
        Me._committedBalance = initialBalance
        Me._tempBalance = initialBalance

    End Sub

    ''' <summary>
    ''' Gets the account number.
    ''' </summary>
    Public ReadOnly Property AccountId() As Integer
        Get
            Return Me._accountId
        End Get
    End Property

    ''' <summary>
    ''' Gets or sets the account balance.
    ''' </summary>
    ''' <value>A positive decimal.</value>
    ''' <remarks>
    ''' Changing the balance transparently enlists the current object in a transaction, if
    ''' any. If there is no current transaction, the current object does not behave
    ''' transactionally. The object can be enlisted only in one transaction.
    ''' </remarks>
    Public Property Balance() As Decimal
        Get
            Return Me._tempBalance
        End Get
        Set(ByVal value As Decimal)

            ' Throw an exception if the balance goes negative
            If value < 0 Then
                Throw New ArgumentOutOfRangeException("value", "The balance cannot be negative.")
            End If

            ' Get the transaction and enlist the current object if necessary.
            Dim transaction As System.Transactions.Transaction = System.Transactions.Transaction.Current
            If transaction <> Nothing Then
                If Me._transaction <> Nothing And Me._transaction <> transaction Then
                    Throw New InvalidOperationException("This account is already enlisted in another transaction.")
                End If
                transaction.EnlistVolatile(Me, EnlistmentOptions.None)
            Else
                Me._committedBalance = value
            End If

            ' Finally set the value
            Me._tempBalance = value
        End Set
    End Property

#Region "Implementation of ISinglePhaseNotification"
    Public Sub Commit(ByVal enlistment As System.Transactions.Enlistment) Implements System.Transactions.IEnlistmentNotification.Commit
        Me._committedBalance = Me._tempBalance
        enlistment.Done()
    End Sub

    Public Sub InDoubt(ByVal enlistment As System.Transactions.Enlistment) Implements System.Transactions.IEnlistmentNotification.InDoubt
        Console.WriteLine("In doubt??")
        enlistment.Done()
    End Sub

    Public Sub Prepare(ByVal preparingEnlistment As System.Transactions.PreparingEnlistment) Implements System.Transactions.IEnlistmentNotification.Prepare
        preparingEnlistment.Prepared()
    End Sub

    Public Sub Rollback(ByVal enlistment As System.Transactions.Enlistment) Implements System.Transactions.IEnlistmentNotification.Rollback
        Me._tempBalance = Me._committedBalance
        enlistment.Done()
    End Sub

    Public Sub SinglePhaseCommit(ByVal singlePhaseEnlistment As System.Transactions.SinglePhaseEnlistment) Implements System.Transactions.ISinglePhaseNotification.SinglePhaseCommit
        Me.Commit(singlePhaseEnlistment)
    End Sub
#End Region

End Class
