#region Released to Public Domain by Gael Fraiteur
/*----------------------------------------------------------------------------*
 *   This file is part of samples of PostSharp.                                *
 *                                                                             *
 *   This sample is free software: you have an unlimited right to              *
 *   redistribute it and/or modify it.                                         *
 *                                                                             *
 *   This sample is distributed in the hope that it will be useful,            *
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of            *
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.                      *
 *                                                                             *
 *----------------------------------------------------------------------------*/
#endregion

using System;
using System.Collections.Generic;
using Librarian.Data;
using Librarian.Entities;
using Librarian.Framework;

namespace Librarian.BusinessProcesses
{
    //[Trace]
    internal class CustomerProcesses : SessionBoundService, ICustomerProcesses
    {
        private readonly CashboxProcesses cashboxProcesses;

        public CustomerProcesses( ServerSession session )
            : base( session )
        {
            this.cashboxProcesses = new CashboxProcesses( session );
        }

        [Transaction]
        public Customer CreateCustomer( Customer customer )
        {
            if ( customer == null )
                throw new ArgumentNullException( "customer" );

            // Check business rules.
            BusinessRulesManager.Assert( "InsertCustomer", customer );

            StorageContext.Current.Insert( customer );
            return customer;
        }

      //  [Transaction]
        public void DeleteCustomer( EntityRef<Customer> customerRef )
        {
            if ( customerRef.IsNull )
                throw new ArgumentNullException( "customerRef" );

            // Get a safe version of the customer.
            Customer customer = customerRef.GetVanillaEntity();

            // Check business rules.
            BusinessRulesManager.Assert( "DeleteCustomer", customer );

            // Update the 'Deleted' flag.
            customer.Deleted = true;
            StorageContext.Current.Update( customer );
        }

        [Transaction]
        public void UpdateCustomer( Customer customer )
        {
            if ( customer == null )
                throw new ArgumentNullException( "customer" );

            // Check business rules.
            BusinessRulesManager.Assert( "UpdateCustomer",
                                         new OldNewPair<Customer>( (Customer) customer.GetVanilla(), customer ) );

            StorageContext.Current.Update( customer );
        }

        [Transaction]
        public void AddCustomerAccountLine( CustomerAccountLine accountLine )
        {
            Customer customer = accountLine.Customer.GetVanillaEntity();
            customer.CurrentBalance += accountLine.Amount;
            StorageContext.Current.Update( customer );
            StorageContext.Current.Insert( accountLine );
        }

        public IEnumerable<Customer> FindCustomers( string customerId, string firstName, string lastName, int max )
        {
            return StorageContext.Current.Find<Customer>( customer => (string.IsNullOrEmpty( customerId ) ||
                                                                       string.Compare( customer.CustomerId, customerId, true ) == 0) &&
                                                                      (string.IsNullOrEmpty( firstName ) ||
                                                                       customer.FirstName.ToLower().Contains( firstName.ToLower() )) &&
                                                                      (string.IsNullOrEmpty( lastName ) ||
                                                                       customer.LastName.ToLower().Contains( lastName.ToLower() )), max );
        }

        public CustomerInfo GetCustomerInfo( EntityRef<Customer> customer, bool getAllRentals )
        {
            if ( customer.IsNull )
                throw new ArgumentNullException( "customer" );

            CustomerInfo customerInfo = new CustomerInfo {Customer = customer.GetVanillaEntity(), Rentals = new List<RentalInfo>()};

            foreach ( Rental rental in StorageContext.Current.Find<Rental>( candidate => candidate.Customer == customer && (getAllRentals || !candidate.Closed) ) )
            {
                RentalInfo rentalInfo = new RentalInfo();
                rentalInfo.Book = rental.Book.Entity;
                rentalInfo.Rental = rental;
                rentalInfo.Notes = new List<Note>( StorageContext.Current.Find<Note>( candidate => candidate.Owner == rental ) );
                customerInfo.Rentals.Add( rentalInfo );
            }

            customerInfo.AccountLines = new List<CustomerAccountLine>( StorageContext.Current.Find<CustomerAccountLine>(
                                                                           candidate => candidate.Customer == customer ) );

            return customerInfo;
        }

        public void AcceptCustomerPayment( EntityRef<Customer> customer, decimal amount )
        {
            CustomerAccountLine accountLine = new CustomerAccountLine
                                                  {
                                                      Amount = amount,
                                                      Customer = customer,
                                                      Date = DateTime.Now,
                                                      Description = "Customer Payment",
                                                      Employee = this.Session.Employee
                                                  };

            this.AddCustomerAccountLine( accountLine );

            CashboxOperation cashboxOperation = new CashboxOperation
                                                    {
                                                        Amount = amount,
                                                        Cashbox = this.Session.Cashbox,
                                                        Date = DateTime.Now,
                                                        Description = string.Format( "Customer Payment: {0} ({1} {2})",
                                                                                     customer.Entity.CustomerId, customer.Entity.FirstName,
                                                                                     customer.Entity.LastName ),
                                                        Employee = this.Session.Employee
                                                    };

            this.cashboxProcesses.RegisterCashboxOperation( cashboxOperation );
        }
    }
}
