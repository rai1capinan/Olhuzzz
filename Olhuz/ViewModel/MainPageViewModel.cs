using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls; // Para Command

namespace Olhuz.ViewModel
{
    public class MainPageViewModel
    {
        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }

        public MainPageViewModel()
        {
            // Comandos assíncronos usando lambda
            LoginCommand = new Command(async () => await OnLogin());
            RegisterCommand = new Command(async () => await OnRegister());
        }

        private async Task OnLogin()
        {
            // Vai para a página LoginPage
            await Shell.Current.GoToAsync(nameof(Olhuz.ViewModel.MainPageViewModel));
        }

        private async Task OnRegister()
        {
            // Vai para a página RegisterPage
            //await Shell.Current.GoToAsync(nameof(Olhuz.ViewModel.));
        }
    }
}
