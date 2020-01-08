using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aragas.QServer.GUI
{
    public class TaskCommand : ICommand
    {
        private readonly Predicate<object> _canExecute;
        private readonly Func<object, Task> _task;

        public event EventHandler CanExecuteChanged;

        public TaskCommand(Func<object, Task> task) : this(task, null) { }
        public TaskCommand(Func<object, Task> task, Predicate<object> canExecute)
        {
            _task = task;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);
        public async void Execute(object parameter) => await _task.Invoke(parameter);

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}