using Caliburn.Micro;

namespace Hearts.UI.ViewModels
{
    public class ShellViewModel : Conductor<object>.Collection.OneActive
    {
        private readonly IServiceLocator _serviceLocator;

        public ShellViewModel(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        protected override void OnActivate()
        {
            var vm = _serviceLocator.Resolve<GameSetupViewModel>();

            ChangeActiveItem(vm, true);

            base.OnActivate();
        }
    }
}
