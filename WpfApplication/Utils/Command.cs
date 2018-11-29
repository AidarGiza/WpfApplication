using System;
using System.Windows.Input;

namespace WpfApplication
{
    /// <summary>
    /// Команда для выполнения действия <see cref="Action"/>
    /// </summary>
    public class Command : ICommand
    {
        private bool _isEnabled = true;

        /// <summary>
        /// Возможность выполнения комманды
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Действие выполняемое командой
        /// </summary>
        public Action Action { get; set; }
        public event EventHandler CanExecuteChanged;

        public Command(Action action)
        {
            Action = action;
        }

        public bool CanExecute(object parameter)
        {
            return IsEnabled;
        }

        /// <summary>
        /// Выполнить действие <see cref="Action"/>
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            Action?.Invoke();
        }
    }

    public class Command<T> : ICommand
    {
        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public Action<T> Action { get; set; }
        public event EventHandler CanExecuteChanged;

        public Command(Action<T> action)
        {
            Action = action;
        }

        public bool CanExecute(object parameter)
        {
            return IsEnabled;
        }

        public void Execute(object parameter)
        {
            if (Action != null && parameter is T)
                Action((T)parameter);
        }
    }
}
