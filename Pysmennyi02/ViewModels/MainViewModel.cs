using System.ComponentModel;
using System.Windows;


namespace Pysmennyi02.ViewModels
{
    public enum ScreenMode { InputScreen, ResultScreen }

    public class MainViewModel : INotifyPropertyChanged
    {

        private Visibility _loaderVisibility;

        public Visibility LoaderVisibility
        {
            get => _loaderVisibility;
        }

        private bool _contentIsEnabled;

        public bool ContentIsEnabled
        {
            get => _contentIsEnabled;
        }

        private UserCreationViewModel _userCreationViewModel;

        public UserCreationViewModel UserCreationViewModel
        {
            get => _userCreationViewModel;
        }
        private ScreenMode _currentScreen;
        public ScreenMode CurrentScreen
        {
            get => _currentScreen;
            set
            {
                _currentScreen = value;
                OnPropertyChanged(nameof(CurrentScreen));
            }
        }

        public MainViewModel()
        {
            CurrentScreen = ScreenMode.InputScreen;
            _contentIsEnabled = true;
            _loaderVisibility = Visibility.Collapsed;
            _userCreationViewModel = new UserCreationViewModel(GoToInput, GoToResults, ActivateLoader, DeactivateLoader);
        }

        public void GoToResults()
        {
            CurrentScreen = ScreenMode.ResultScreen;
        }

        public void GoToInput()
        {
            CurrentScreen = ScreenMode.InputScreen;
        }

        public void ActivateLoader()
        {
            _loaderVisibility = Visibility.Visible;
            _contentIsEnabled = false;
            OnPropertyChanged(nameof(LoaderVisibility));
            OnPropertyChanged(nameof(ContentIsEnabled));
        }

        public void DeactivateLoader()
        {
            _loaderVisibility = Visibility.Collapsed;
            _contentIsEnabled = true;
            OnPropertyChanged(nameof(LoaderVisibility));
            OnPropertyChanged(nameof(ContentIsEnabled));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
