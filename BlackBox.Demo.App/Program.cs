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
            Console.WriteLine("  3. Parameter serves as both input and output.");
            Console.WriteLine("  4. One recording, multiple calls on external dependency.");
            Console.WriteLine("  5. Recording of static method with static dependency method.");
            Console.WriteLine("  6. Recording on types marked using assembly attribute.");
            Console.WriteLine("  7. Recording of dependencies in external APIs.");
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
                case 3:
                    InputOutputArgument.Demo.Run();
                    break;
                case 4:
                    MultipleCallsOnDependency.Demo.Run();
                    break;
                case 5:
                    StaticMethods.Demo.Run();
                    break;
                case 6: AssemblyAttribute.Demo.Run();
                    break;
                case 7 :
                    ExternalApiDependencyRecording.Demo.Run();
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
