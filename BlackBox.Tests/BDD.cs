namespace BlackBox.Tests
{
    public abstract class BDD<T> where T : BDD<T>
    {
        protected T Given { get { return (T) this; } }
        protected T And { get { return (T)this; } }
        protected T When { get { return (T)this; } }
        protected T Then { get { return (T)this; } }
    }
}
