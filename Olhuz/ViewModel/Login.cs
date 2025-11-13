using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Olhuz.Services;
using Olhuz.Models;
using Microsoft.Maui.Controls;
using Olhuz.ViewModel;

namespace Olhuz.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly AuthService _authService;

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        private bool _isPasswordHidden = true;
        public bool IsPasswordHidden
        {
            get => _isPasswordHidden;
            set
            {
                _isPasswordHidden = value;
                OnPropertyChanged();
                PasswordIcon = _isPasswordHidden ? "eye_closed.png" : "eye_open.png";
            }
        }

        private string _passwordIcon = "eye_closed.png";
        public string PasswordIcon
        {
            get => _passwordIcon;
            set { _passwordIcon = value; OnPropertyChanged(); }
        }

        // Comandos
        public ICommand LoginCommand { get; }
        public ICommand TogglePasswordCommand { get; }

        public LoginViewModel()
        {
            _authService = new AuthService();

            LoginCommand = new Command(async () => await LoginAsync());
            TogglePasswordCommand = new Command(TogglePasswordVisibility);
        }

        private void TogglePasswordVisibility()
        {
            IsPasswordHidden = !IsPasswordHidden;
        }

        private async Task LoginAsync()
        {
            IsLoading = true;

            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Erro", "Por favor, preencha todos os campos.", "OK");

                IsLoading = false;

                return;

            }


            var loginRequest = new LoginRequest { Email = Email, PassWordHash = Password };
            var response = await _authService.LoginAsync(loginRequest);

            if (response.Erro)
            {
                await Application.Current.MainPage.DisplayAlert("erro", response.Message, "OK");
                IsLoading = false;
                return;
            }

            await Application.Current.MainPage.DisplayAlert("Sucesso", "Login realizado com sucesso!", "OK");
            IsLoading = false;
            await Shell.Current.GoToAsync($"{nameof(HomeViewModel)}");
        }

        public event PropertyChangedEventHandler PropertyChanged; 

        protected void OnPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
