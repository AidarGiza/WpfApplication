using PropertyChanged;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace WpfApplication
{

    /// <summary>
    /// Модель представления приложения
    /// </summary>
    [ImplementPropertyChanged]
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        #region Local fields

        /// <summary>
        /// Полный путь ко входному JSON файлу
        /// </summary>
        private string inputFilePath = @"D:\Projects\CS\WpfApplication\input";

        /// <summary>
        /// Полный путь к выходному JSON файлу
        /// </summary>
        private string outputFilePath = @"D:\Projects\CS\WpfApplication\output";

        /// <summary>
        /// Индекс текущего элемента из набора данных о домах
        /// </summary>
        private int currentHouseIndex;

        /// <summary>
        /// Индекс текущего элемента из набора данных о пользователях
        /// </summary>
        private int currentUserIndex;

        /// <summary>
        /// Набор данных о домах
        /// </summary>
        private HousesDataSet housesDataSet;

        /// <summary>
        /// Набор данных о пользователях
        /// </summary>
        private UsersDataSet usersDataSet;

        #endregion
        
        #region Properties

        /// <summary>
        /// В наборе данных о домах есть элементы 
        /// </summary>
        public bool HousesNotEmpty { get; set; }

        /// <summary>
        /// В наборе данных о пользователях есть элементы
        /// </summary>
        public bool UsersNotEmpty { get; set; }

        /// <summary>
        /// У текущего пользователя список с данными о телефонах не пустой
        /// </summary>
        public bool PhonesNotEmpty { get; set; }

        /// <summary>
        /// Команда для показа данных о следующем доме из набора данных
        /// </summary>
        public Command NextHouseCommand { get; set; }

        /// <summary>
        /// Команда для показа данных о предыдущем доме из набора данных
        /// </summary>
        public Command PrevHouseCommand { get; set; }

        /// <summary>
        /// Команда для создания нового элемента в набор данных о домах
        /// </summary>
        public Command NewHouseCommand { get; set; }

        /// <summary>
        /// Команда для удаления данных о текущем элементе из набора данных 
        /// </summary>
        public Command DeleteHouseCommand { get; set; }

        /// <summary>
        /// Команда для показа данных о следующем пользователе из набора данных
        /// </summary>
        public Command NextUserCommand { get; set; }

        /// <summary>
        /// Команда для показа данных о предыдущем пользователе из набора данных
        /// </summary>
        public Command PrevUserCommand { get; set; }

        /// <summary>
        /// Команда для создания нового элемента в набор данных о пользователях
        /// </summary>
        public Command NewUserCommand { get; set; }

        /// <summary>
        /// Команда для удаления данных о текущем пользователе из набора данных
        /// </summary>
        public Command DeleteUserCommand { get; set; }

        /// <summary>
        /// Команда для удаления данных о выбранном телефоне из списка телефонов текущего пользователя из набора данных
        /// </summary>
        public Command<Phone> DeletePhoneCommand { get; set; }

        /// <summary>
        /// Команда для создания нового элемента в список телефонов для текущего пользователя в набор данных
        /// </summary>
        public Command AddPhoneCommand { get; set; }
        
        /// <summary>
        /// Команда для сохранения всех данных в файл
        /// </summary>
        public Command SaveDataCommand { get; set; }

        /// <summary>
        /// Текущий элемент из набора данных о домах
        /// </summary>
        public House CurrentHouse { get; set; }

        /// <summary>
        /// Текущий элемент из набора данных о пользователях
        /// </summary>
        public User CurrentUser { get; set; }

        #endregion
        
        #region Constructor

        /// <summary>
        /// Конструктор модели представления
        /// </summary>
        public MainViewModel()
        {
            //Десериализация файла
            JSONData.DeserializeJsonFile(inputFilePath);
            housesDataSet = JSONData.HousesDataSet;
            usersDataSet = JSONData.UsersDataSet;

            //Создание команд
            NextHouseCommand = new Command(NextHouse);
            PrevHouseCommand = new Command(PrevHouse);
            NewHouseCommand = new Command(NewHouse);
            DeleteHouseCommand = new Command(DeleteHouse);

            NextUserCommand = new Command(NextUser);
            PrevUserCommand = new Command(PrevUser);
            NewUserCommand = new Command(NewUser);
            DeleteUserCommand = new Command(DeleteUser);

            DeletePhoneCommand = new Command<Phone>(ExecuteDeletePhoneCommand);
            AddPhoneCommand = new Command(AddPhone);

            SaveDataCommand = new Command(SaveAllData);

            //Присваивание нулевых значений идекса
            currentHouseIndex = 0;
            currentUserIndex = 0;

            //Показ первых элементов наборов данных
            ShowHouse(currentHouseIndex);
            ShowUser(currentUserIndex);

            //Определение не пусты ли наборы данных 
            HousesNotEmpty = housesDataSet.D.Items.Count != 0 ? true : false;
            UsersNotEmpty = usersDataSet.D.Items.Count != 0 ? true : false;
        }
        
        #endregion

        #region house object behaviour

        /// <summary>
        /// Показать данные о доме из набора даных по <paramref name="index"/> 
        /// </summary>
        /// <param name="index"></param>
        void ShowHouse(int index)
        {
            // Запись в переменную текущего элемента из набора данных о домах
            CurrentHouse = housesDataSet.D.Items[index];

            // Определить возможность прехода к следующему и предыдущему элементу набора данных
            NextHouseCommand.IsEnabled = currentHouseIndex < (housesDataSet.D.Items.Count - 1);
            PrevHouseCommand.IsEnabled = currentHouseIndex > 0;
        }

        /// <summary>
        /// Показать следующий элемент из набора данных о домах
        /// </summary>
        private void NextHouse()
        {
            ShowHouse(++currentHouseIndex);
        }

        /// <summary>
        /// Показать предыдущий элемент из набора данных о домах
        /// </summary>
        private void PrevHouse()
        {
            ShowHouse(--currentHouseIndex);
        }

        /// <summary>
        /// Добавить новый элемент в набор данных <see cref="housesDataSet"/>
        /// </summary>
        private void NewHouse()
        {
            // Добавлние нового пустого элемета в набор данных
            housesDataSet.D.Items.Add(new House
            {
                Name = "",
                Address = "",
                Type = 0,
                Flors = 0
            });
            // Присваивание индекса нового элемента в переменную индекса текущего элемента
            currentHouseIndex = housesDataSet.D.Items.Count - 1;
            // Показать данные нового элемента
            ShowHouse(currentHouseIndex);

            // Если набор данных был пуст
            if (!HousesNotEmpty)
            {
                // Сделать доступным команду удаления
                DeleteHouseCommand.IsEnabled = true;
                // Установить флаг, что набор данных больше не пуст
                HousesNotEmpty = true;
            }
        }

        /// <summary>
        /// Удаление текущего элемента из набора данных <see cref="housesDataSet"/>
        /// </summary>
        private void DeleteHouse()
        {
            // Удаление элемента по индексу текущего элемента набора данных
            housesDataSet.D.Items.RemoveAt(currentHouseIndex);
            // Если в наборе данных еще остались элеметы...
            if (housesDataSet.D.Items.Count != 0)
            {
                // ...Показать следующий элемент набора данных, если он существует, иначе показать предыдущий
                if (currentHouseIndex < housesDataSet.D.Items.Count)
                {
                    ShowHouse(currentHouseIndex);
                }
                else
                {
                    ShowHouse(--currentHouseIndex);
                }
            }
            else // Иначе
            {
                // Присвоить текущему элементу пустое значение
                CurrentHouse = null;
                // Установить флаг, что элементы в наборе данных отсутствуют
                HousesNotEmpty = false;
                // Сделать недоступным команду удаления
                DeleteHouseCommand.IsEnabled = false;
            }
        }

        #endregion
        
        #region user object behaviour
        
        /// <summary>
        /// Показать данные о пользователе из набора данных по <paramref name="index"/>
        /// </summary>
        /// <param name="index"></param>
        void ShowUser(int index)
        {
            // Запись в переменную текущего элемента из набора данных о пользователях
            CurrentUser = usersDataSet.D.Items[index];

            // Определить возможность прехода к следующему и предыдущему элементу набора данных
            NextUserCommand.IsEnabled = currentUserIndex < (usersDataSet.D.Items.Count - 1);
            PrevUserCommand.IsEnabled = currentUserIndex > 0;

            // Установить флаг наличия списка телефонов у текущего элемента набора данных
            PhonesNotEmpty = CurrentUser.Phones.Count != 0 ? true : false;
        }

        /// <summary>
        /// Показать следующий элемент из набора данных о пользователях
        /// </summary>
        private void NextUser()
        {
            ShowUser(++currentUserIndex);
        }

        /// <summary>
        /// Показать предыдущий элемент из набора данных о пользователях
        /// </summary>
        private void PrevUser()
        {
            ShowUser(--currentUserIndex);
        }

        /// <summary>
        /// Добавить новый элемент в набор данных <see cref="usersDataSet"/>
        /// </summary>
        private void NewUser()
        {
            // Добавлние нового пустого элемета в набор данных
            usersDataSet.D.Items.Add(new User
            {
                Name = "",
                Login = "",
                Group = 0,
                Phones = new ObservableCollection<Phone>()
            });
            // Присваивание индекса нового элемента в переменную индекса текущего элемента
            currentUserIndex = usersDataSet.D.Items.Count - 1;
            // Показать данные нового элемента
            ShowUser(currentUserIndex);

            // Если набор данных был пуст
            if (!UsersNotEmpty)
            {
                // Сделать доступным команду удаления
                DeleteUserCommand.IsEnabled = true;
                // Установить флаг, что набор данных больше не пуст
                UsersNotEmpty = true;
            }
            // Установить флаг, что список телефонов текущего элемента набора данных пуст
            PhonesNotEmpty = false;
        }

        /// <summary>
        /// Удаление текущего элемента из набора данных <see cref="usersDataSet"/>
        /// </summary>
        private void DeleteUser()
        {
            // Удаление элемента по индексу текущего элемента набора данных
            usersDataSet.D.Items.RemoveAt(currentUserIndex);
            // Если в наборе данных еще остались элеметы...
            if (usersDataSet.D.Items.Count != 0)
            {
                // ...Показать следующий элемент набора данных, если он существует, иначе показать предыдущий
                if (currentUserIndex < usersDataSet.D.Items.Count)
                {
                    ShowUser(currentUserIndex);
                }
                else
                {
                    ShowUser(--currentUserIndex);
                }
            }
            else // Иначе
            {
                // Присвоить текущему элементу пустое значение
                CurrentUser = null;
                // Установить флаг, что элементы в наборе данных отсутствуют
                UsersNotEmpty = false;
                // Сделать недоступным команду удаления
                DeleteUserCommand.IsEnabled = usersDataSet.D.Items.Count != 0;
            }
        }

        /// <summary>
        /// Добавление элемента в список телефонов для текушего пользователя
        /// </summary>
        private void AddPhone()
        {
            // Создание пустого элемента Phone 
            Phone phone = new Phone
            {
                Id = 0,
                Value = ""
            };
            // Добавить элемент в список телефонов для текущего пользователя
            CurrentUser.Phones.Add(phone);
            // Установть флаг наличия элемента в списке телефонов для текущего пользователя
            PhonesNotEmpty = true;
        }
        
        /// <summary>
        /// Удаление выбранного элемента из списка телефонов для текущего пользователя
        /// </summary>
        /// <param name="phone">Удаляемый элемент</param>
        private void ExecuteDeletePhoneCommand(Phone phone)
        {
            // Удаление элемента из списка телефонов для текущего пользователя
            CurrentUser.Phones.Remove(phone);
            // Установить флаг отсутствия элементов в списке телефонов текущего пользователя
            if (CurrentUser.Phones.Count == 0)
            {
                PhonesNotEmpty = false;
            }
        }

        #endregion

        #region Save Data

        /// <summary>
        /// Сохранение всех данных в файл
        /// </summary>
        private void SaveAllData()
        {
            // Передать наборы данных в статический класс для сериализации
            JSONData.HousesDataSet = housesDataSet;
            JSONData.UsersDataSet = usersDataSet;

            // Сериалезировать наборы данных и сохранить в файл
            JSONData.SaveDataToFile(outputFilePath);
        }

        #endregion
    }
}