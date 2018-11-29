using System.Windows;

namespace WpfApplication
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Стандартный конструктор
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            // создание модели представления
            DataContext = new MainViewModel();
        }
    }
}
