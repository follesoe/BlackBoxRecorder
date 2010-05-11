using System;

namespace BlackBox.Demo.App.InputOutputArgument
{
    public class Demo
    {
        public static void Run()
        {
            Console.WriteLine("Demo of recording on method manipulating the input argument.");

            var bussinessLogic = new SomeBL();
            var entity = new SomeEntity {Name = "My name", Income = 1000};

            Console.WriteLine("Before method call: {0} {1}", entity.Name, entity.Income);

            bussinessLogic.DoSomething(entity);

            Console.WriteLine("After method call: {0} {1}", entity.Name, entity.Income);
        }
    }
}
