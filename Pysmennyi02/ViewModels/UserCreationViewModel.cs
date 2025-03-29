using CommunityToolkit.Mvvm.Input;
using Pysmennyi02.Models;
using Pysmennyi02.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pysmennyi02.ViewModels
{
    public class UserCreationViewModel : INotifyPropertyChanged
    {
        private Action _toInputViewAction;
        private Action _toResultsAction;

        private Action _activateLoader;
        private Action _deactivateLoader;

        private Person? _person;

        public Person? Person
        {
            get => _person;
        }

        private string? _name;
        private string? _surname;
        private string? _email;
        private DateTime? _birthDate;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string? Name
        {
            get { return _name; }
            set
            {
                _name = value; OnPropertyChanged(nameof(Name));
                CreatePersonCommand.NotifyCanExecuteChanged();
            }
        }
        public string? Surname
        {
            get { return _surname; }
            set
            {
                _surname = value; OnPropertyChanged(nameof(Surname));
                CreatePersonCommand.NotifyCanExecuteChanged();
            }
        }
        public string? Email
        {
            get { return _email; }
            set
            {
                _email = value; OnPropertyChanged(nameof(Email));
                CreatePersonCommand.NotifyCanExecuteChanged();
            }
        }
        public DateTime? BirthDate
        {
            get { return _birthDate; }
            set
            {
                _birthDate = value;
                OnPropertyChanged(nameof(BirthDate));
                CreatePersonCommand.NotifyCanExecuteChanged();
            }
        }


        public RelayCommand CreatePersonCommand { get; }

        public RelayCommand ReturnToCreateMenuCommand { get; }



        public UserCreationViewModel(Action toInputViewAction, Action toResultsAction, Action activateLoader, Action deactivateLoader)
        {
            _toInputViewAction = toInputViewAction;
            _toResultsAction = toResultsAction;
            _activateLoader = activateLoader;
            _deactivateLoader = deactivateLoader;
            CreatePersonCommand = new RelayCommand(async () => await CreatePerson(_name, _surname, _email, _birthDate), CanExecuteCreatePerson);
            ReturnToCreateMenuCommand = new RelayCommand(ReturnToCreateMenu);
        }

        public async Task CreatePerson(string? name, string? surname, string? email, DateTime? birthDate)
        {
            try
            {
                if (!CanExecuteCreatePerson())
                {
                    ExceptionHandlingService.ShowMessage("Please fill all fields", "Error");
                    return;
                }
                _activateLoader();
                _person = await Task.Run(() => new Person(name, surname, email, birthDate.Value));
                OnPropertyChanged(nameof(Person));
                _deactivateLoader();
                _toResultsAction();
            }
            catch (Exception e)
            {
                _deactivateLoader();
                ExceptionHandlingService.ShowMessage(e.Message, "Error");
            }
        }

        public void ReturnToCreateMenu()
        {
            _person = null;
            Name = null;
            Surname = null;
            Email = null;
            BirthDate = null;

            _toInputViewAction();
        }

        public bool CanExecuteCreatePerson()
        {
            return !string.IsNullOrEmpty(_name) && !string.IsNullOrEmpty(_surname) && !string.IsNullOrEmpty(_email) && _birthDate != null;
        }

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


    }
}
