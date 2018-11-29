namespace WpfApplication
{

    /// <summary>
    /// Элемент из коллекции <see cref="User.Phones"/>
    /// </summary>
    public class Phone
    {
        /// <summary>
        /// Идентификатор номера телефона
        /// </summary>
        public byte Id { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        public string Value { get; set; }
    }
}
