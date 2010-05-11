using System;
using System.Threading;

namespace Cache
{
    internal class Program
    {
        public static void Main( string[] args )
        {
            Console.WriteLine( "1 ->" + GetDifficultResult( 1 ) );
            Console.WriteLine( "2 ->" + GetDifficultResult( 2 ) );
            Console.WriteLine( "1 ->" + GetDifficultResult( 1 ) );
            Console.WriteLine( "2 ->" + GetDifficultResult( 2 ) );
        }

        [Cache]
        private static int GetDifficultResult( int arg )
        {
            // If the following text is printed, the method was not cached.
            Console.WriteLine( "Some difficult work!" );
            Thread.Sleep( 1000 );
            return arg;
        }
    }
}