using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Test.ObjectComparison;

namespace BlackBox.Testing
{
    public class ObjectMismatchException : Exception
    {
        public ObjectMismatchException(IEnumerable<ObjectComparisonMismatch> mismatches) 
            : base(mismatches.ToMismatchDetailsString())
        {
        }
    }
}
