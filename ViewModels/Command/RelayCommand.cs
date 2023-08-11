using System;
using System.Windows.Input;

namespace DED_MonitoringSensor.ViewModels.Command
{
    class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action _execute;

        public RelayCommand(Action execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _execute.Invoke();
        }
    }
}
