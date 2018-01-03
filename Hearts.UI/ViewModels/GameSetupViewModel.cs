using System.Collections.ObjectModel;
using PropertyChanged;

namespace Hearts.UI.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class GameSetupViewModel
    {
        public ObservableCollection<string> AvailableBots { get; } = new ObservableCollection<string>();

        public string SelectedBot { get; set; }

        public ObservableCollection<string> SelectedBots { get; } = new ObservableCollection<string>();

        public void AddBot()
        {
            SelectedBots.Add(SelectedBot);
            AvailableBots.Remove(SelectedBot);
            SelectedBot = null;
        }
    }
}
