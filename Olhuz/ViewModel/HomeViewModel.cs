using System.Windows.Input;

namespace Olhuz.ViewModel
{
    internal class HomeViewModel
    {
        public ICommand OnReadImageTapped { get; }
        public ICommand OnReadingHistoryTapped { get; }
        public ICommand OnSettingsTapped { get; }
        public ICommand OnMyAccountTapped { get; }

        public HomeViewModel()
        {
            OnReadImageTapped = new Command(async () => await NavigateTo(nameof(OnReadImageTapped)));
            OnReadingHistoryTapped = new Command(async () => await NavigateTo(nameof(OnReadingHistoryTapped)));
            OnSettingsTapped = new Command(async () => await NavigateTo(nameof(OnSettingsTapped)));
            OnMyAccountTapped = new Command(async () => await NavigateTo(nameof(OnMyAccountTapped)));
        }

        private async Task NavigateTo(string route)
        {
            await Shell.Current.GoToAsync(route);
        }
    }
}
