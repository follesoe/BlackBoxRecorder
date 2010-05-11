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

Module Module1

    Dim account1 As Account = New Account(1, 100)
    Dim account2 As Account = New Account(2, 200)

    ''' <summary>
    ''' Displays the current balance of accounts.
    ''' </summary>
    Sub Display()
        Console.WriteLine("Balance of account 1 is {0}.", account1.Balance)
        Console.WriteLine("Balance of account 2 is {0}.", account2.Balance)
    End Sub


    ''' <summary>
    ''' Transfers some money from one account to another. This operation is transactional.
    ''' </summary>
    ''' <param name="fromAccount">Source account.</param>
    ''' <param name="toAccount">Destination account.</param>
    ''' <param name="amount">Amount to be transfered.</param>
    <TransactionScope()> _
    Sub Transfer(ByVal fromAccount As Account, ByVal toAccount As Account, ByVal amount As Decimal)
        fromAccount.Balance -= amount
        toAccount.Balance += amount
    End Sub

    ''' <summary>
    ''' Entry point.
    ''' </summary>
    Sub Main()
        Console.WriteLine("BALANCES BEFORE")
        Display()
        Transfer(account1, account2, 50)
        Console.WriteLine("BALANCES AFTER SUCCESSFUL TRANSACTION")
        Display()
        Try
            Transfer(account1, account2, -500)
        Catch ex As Exception
            Console.WriteLine("Exception: {0}", ex.Message)
        End Try
        Console.WriteLine("BALANCES AFTER FAILED TRANSACTION")
        Display()

    End Sub

End Module
