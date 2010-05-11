using System;
using BlackBox.CodeGeneration;

namespace BlackBox.Demo.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ConfigreBlackBox();

            Console.WriteLine("[BlackBox Recorder Examples]");
            Console.WriteLine();
            Console.WriteLine("  1. Simple Anemic Domain Model (BL, DAL, Entity)");
            Console.WriteLine("  2. Multiple calls to external web resource.");
            Console.WriteLine();
            Console.Write("Pick demo to run recording on: ");

            int demo = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();

            switch(demo)
            {
                case 1:
                    SimpleAnemic.Demo.Run();
                    break;
                case 2:
                    WebDependencies.Demo.Run();
                    break;
            }            

            Console.WriteLine("Click a key to exit...");
            Console.ReadLine();
        }

        private static void ConfigreBlackBox()
        {
            Configuration.OutputDirectory = @"..\..\..\BlackBox.Demo.Tests\";
            Configuration.TestFlavour = TestFlavour.CreateXunit();
        }
    }
}
