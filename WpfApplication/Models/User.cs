using System.Collections.ObjectModel;

namespace WpfApplication
{
    /// <summary>
    /// Элемент из <see cref="UsersItem.Items"/>
    /// </summary>
    public class User
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Группа пользователя
        /// </summary>
        public byte Group { get; set; }

        /// <summary>
        /// Номера телефонов пользователя
        /// </summary>
        public ObservableCollection<Phone> Phones { get; set; }
    }
}
