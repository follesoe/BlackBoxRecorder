using BlackBox.Recorder;

namespace BlackBox.Demo.App.InputOutputArgument
{
    public class SomeBL
    {
        [Recording]
        public void DoSomething(SomeEntity entity)
        {
            entity.Name = entity.Name + " edited...";
            entity.Income = entity.Income*1.1;
        }
    }
}
