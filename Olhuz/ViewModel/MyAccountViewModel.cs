using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Olhuz.Models;

namespace Olhuz.ViewModels
{
    public class MyAccountViewModel : INotifyPropertyChanged 
    {
        private User _user;

        public User User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(WelcomeMessage));
            }
        }

        public string WelcomeMessage => $"Olá, {User?.FullName}";

        public ICommand ChangePasswordCommand { get; }
        public ICommand LogoutCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MyAccountViewModel()
        {
            ChangePasswordCommand = new Command(OnChangePassword);
            LogoutCommand = new Command(OnLogout);

            LoadUserData();
        }

        private void LoadUserData()
        {
            User = new User
            {
                FullName = "Maria Oliveira",
                BirthDate = new DateOnly (1995, 05, 20),
                Email = "maria.oliveira@exemplo.com"
            };
        }

        private async void OnChangePassword()
        {
            await Application.Current.MainPage.DisplayAlert("Alterar senha", "Indo para a tela...", "OK");
        }

        private async void OnLogout()
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert(
                            "Sair da Conta",
                            "Tem certeza que deseja sair?",
                            "Sim", "Não");

            if (confirm)
            {
                await Application.Current.MainPage.DisplayAlert("Logout", "Você saiu da conta.", "OK");
                await Shell.Current.GoToAsync("///MainPage");
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
