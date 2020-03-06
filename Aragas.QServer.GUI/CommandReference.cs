using System;
using System.Windows;
using System.Windows.Input;

namespace Aragas.QServer.GUI
{
    public class CommandReference : Freezable, ICommand
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(CommandReference),
            new PropertyMetadata(OnCommandChanged));

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CommandReference commandReference)
            {
                if (e.OldValue is ICommand oldCommand)
                    oldCommand.CanExecuteChanged -= commandReference.CanExecuteChanged;

                if (e.NewValue is ICommand newCommand)
                    newCommand.CanExecuteChanged += commandReference.CanExecuteChanged;
            }
        }

        public event EventHandler? CanExecuteChanged;

        public ICommand Command { get => (ICommand) GetValue(CommandProperty); set => SetValue(CommandProperty, value); }

        public bool CanExecute(object parameter) => Command.CanExecute(parameter);
        public void Execute(object parameter) => Command.Execute(parameter);

        protected override Freezable CreateInstanceCore() => new CommandReference();
    }
}