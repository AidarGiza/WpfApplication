namespace WpfApplication
{
    /// <summary>
    /// Элемент из коллекции <see cref="HousesItem.Items"/>
    /// </summary>
    public class House
    {
        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Полный адрес
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Тип дома
        /// </summary>
        public byte Type { get; set; }

        /// <summary>
        /// Количество этажей в доме
        /// </summary>
        public byte Flors { get; set; }
    }
}
