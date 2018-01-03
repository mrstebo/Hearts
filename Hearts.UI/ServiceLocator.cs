using Caliburn.Micro;

namespace Hearts.UI
{
    public interface IServiceLocator
    {
        T Resolve<T>();
    }

    internal class ServiceLocator : IServiceLocator
    {
        public T Resolve<T>()
        {
            return IoC.Get<T>();
        }
    }
}
