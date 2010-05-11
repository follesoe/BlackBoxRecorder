using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

[assembly: Trace.QuickTrace(AttributeTargetAssemblies = "mscorlib", AttributeTargetTypes = "System.Threading.*")]

namespace Trace
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Diagnostics.Trace.Listeners.Add( new System.Diagnostics.TextWriterTraceListener( Console.Out ) );

            Method1();
            Factorial( 5 );
            Factorial(-5);
        }

        [QuickTrace]
        static void Method1()
        {
            Method2();
        }

        [QuickTrace]
        static void Method2()
        {
            Method3();
        }

        [QuickTrace]
        static void Method3()
        {
            Thread.Sleep( 10 );
        }

        [FullTrace]
        static int Factorial(int i)
        {
            if (i <= 1 && i >= -1) return i;
            if (i<0)
            {
                return i*Factorial( i + 1 );
            }
            else
            {
                return i*Factorial( i - 1 );
            }
        }



    }
}
