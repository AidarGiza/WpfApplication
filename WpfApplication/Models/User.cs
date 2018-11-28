using System.Collections.ObjectModel;

namespace WpfApplication
{
    public class User
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public byte Group { get; set; }
        public ObservableCollection<Phone> Phones { get; set; }
    }
}
