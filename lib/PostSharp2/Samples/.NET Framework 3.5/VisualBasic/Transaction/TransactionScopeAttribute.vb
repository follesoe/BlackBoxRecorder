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

Imports PostSharp.Aspects
Imports System.Transactions

''' <summary>
''' Custom attribute that, when applied on a method, specifies that it should
''' be executed in a transaction.
''' </summary>
''' <remarks></remarks>
<Serializable()> _
Public NotInheritable Class TransactionScopeAttribute
    Inherits OnMethodBoundaryAspect

    Private _scopeOption As TransactionScopeOption = TransactionScopeOption.Required
    Private _isolationLevel As IsolationLevel = Transactions.IsolationLevel.ReadCommitted
    Private _interopOption As EnterpriseServicesInteropOption = EnterpriseServicesInteropOption.Automatic
    Private _timeout As Single = 60

    ''' <summary>
    ''' Initializes a new instance of the <see cref="TransactionScopeAttribute"/> custom attribute.
    ''' </summary>
    Public Sub New()
    End Sub

    ''' <summary>
    ''' Gets or sets the transaction scope options.
    ''' </summary>
    ''' <value><see cref="TransactionScopeOption.Required"/>,
    ''' <see cref="TransactionScopeOption.RequiresNew"/> or <see cref="TransactionScopeOption.Suppress"/>.
    ''' <see cref="TransactionScopeOption.Required"/> is the default value.
    ''' </value>
    Public Property ScopeOption() As TransactionScopeOption
        Get
            Return Me._scopeOption
        End Get
        Set(ByVal value As TransactionScopeOption)
            Me._scopeOption = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the transaction isolation level.
    ''' </summary>
    ''' <value>One of the values of the <see cref="IsolationLevel"/> enumeration. The default
    ''' value is <see cref="IsolationLevel.ReadCommitted"/>.</value>
    Public Property IsolationLevel() As IsolationLevel
        Get
            Return Me._isolationLevel
        End Get
        Set(ByVal value As IsolationLevel)
            Me._isolationLevel = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the transaction timeout, in seconds.
    ''' </summary>
    ''' <value>A strictly positive real number determining the number of seconds after which
    ''' an unconfirmed transaction should be rolled back. The default value is 60 seconds.</value>
    Public Property Timeout() As Single
        Get
            Return Me._timeout
        End Get
        Set(ByVal value As Single)
            Me._timeout = value
        End Set
    End Property

    ''' <summary>
    ''' Method executed <b>before</b> the body of methods to which this aspect is applied.
    ''' </summary>
    ''' <param name="eventArgs">Event arguments specifying which method
    ''' is being executed and which are its arguments.</param>
    ''' <remarks>We create a new <see cref="TransactionScope"/> and assign it to
    ''' the state of this join point.</remarks>
    Public Overrides Sub OnEntry(ByVal eventArgs As MethodExecutionArgs)

        Dim transactionOptions As TransactionOptions
        transactionOptions.Timeout = TimeSpan.FromSeconds(Me._timeout)
        transactionOptions.IsolationLevel = Me._isolationLevel

        eventArgs.MethodExecutionTag = New TransactionScope(Me._scopeOption, transactionOptions, Me._interopOption)

    End Sub
    

    ''' <summary>
    ''' Method executed <b>after</b> the body of methods to which this aspect is applied.
    ''' </summary>
    ''' <param name="eventArgs">Event arguments specifying which method
    ''' is being executed and which are its arguments.</param>
    ''' <remarks>
    ''' We commit or rollback the transaction that was opened in the <see cref="OnEntry"/> method.
    ''' </remarks>
    Public Overrides Sub OnExit(ByVal eventArgs As MethodExecutionArgs)

        Dim transactionScope As TransactionScope = eventArgs.MethodExecutionTag

        If eventArgs.Exception Is Nothing Then
            transactionScope.Complete()
        End If

        transactionScope.Dispose()

    End Sub

End Class
