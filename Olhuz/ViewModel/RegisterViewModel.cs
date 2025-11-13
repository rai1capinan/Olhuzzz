using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Olhuz.ViewModels
{
    // Implementa INotifyPropertyChanged para permitir que a View reaja a mudanças de propriedade no ViewModel
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // Método auxiliar para notificar que uma propriedade mudou
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Método auxiliar para definir o valor da propriedade e notificar a mudança
        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}