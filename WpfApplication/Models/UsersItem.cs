using System.Collections.Generic;

namespace WpfApplication
{
    /// <summary>
    /// Элемент из коллекции <see cref="UsersDataSet.D"/>
    /// </summary>
    class UsersItem
    {
        /// <summary>
        /// Коллекция данных о пользователях
        /// </summary>
        public List<User> Items { get; set; }
    }
}
