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
    [BusinessRuleApplies( "OpenRental" )]
    public class TooManyOpenRentalsWhenOpenRental : BusinessRule
    {
        public override bool Evaluate( object item )
        {
            Rental rental = (Rental) item;
            int openRentalNumber = 0;

            foreach ( Rental otherRental in StorageContext.Current.Find<Rental>( candidate => candidate.Customer == rental.Customer && !candidate.Closed ) )
            {
                openRentalNumber ++;
            }

            return openRentalNumber < rental.Customer.GetVanillaEntity().MaxOpenRentals;
        }
    }
}
