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
                // Вызвать событие, если он не null
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Действие выполняемое командой
        /// </summary>
        public Action Action { get; set; }

        /// <summary>
        /// Вызывается при изменении <see cref="IsEnabled"/>
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="action">Дейстие выполняемое командой</param>
        public Command(Action action)
        {
            Action = action;
        }
        
        /// <summary>
        /// Определяет, может ли выполнятся
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
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
            // Выполнить Action, если он не null
            Action?.Invoke();
        }
    }

    /// <summary>
    /// Команда для выполнения действия <see cref="Action"/> с универсальны параметром
    /// </summary>
    public class Command<T> : ICommand
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
                // Вызвать событие, если он не null
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Действие выполняемое командой с универсальным параметром
        /// </summary>
        public Action<T> Action { get; set; }

        /// <summary>
        /// Вызывается при изменении <see cref="IsEnabled"/>
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="action">Дейстие выполняемое командой с универсальным параметром</param>
        public Command(Action<T> action)
        {
            Action = action;
        }

        /// <summary>
        /// Определяет, может ли выполнятся
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
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
            // Выполнить Action, если он не null
            if (Action != null && parameter is T)
                Action((T)parameter);
        }
    }
}
