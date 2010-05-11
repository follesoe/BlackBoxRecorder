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

using Librarian.Data;
using Librarian.Entities;
using Librarian.Framework;

namespace Librarian.BusinessRules
{
    [BusinessRuleApplies( "DeleteCustomer" )]
    internal class CannotCloseCustomerWithOpenRental : BusinessRule
    {
        public override bool Evaluate( object item )
        {
            EntityRef<Customer> customer = (EntityRef<Customer>) item;

            return StorageContext.Current.Exists<Rental>( rental => rental.Customer.Equals( customer ) && !rental.Closed
                );
        }
    }
}
