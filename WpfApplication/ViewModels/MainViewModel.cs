using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace WpfApplication
{
    [ImplementPropertyChanged]
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
        
        private string inputFilePath = @"D:\Projects\CS\WpfApplication\input";
        private string outputFilePath = @"D:\Projects\CS\WpfApplication\output";
        
        int currentHouseIndex;
        int currentUserIndex;

        public bool HousesNotEmpty { get; set; }
        public bool UsersNotEmpty { get; set; }
        public bool PhonesNotEmpty { get; set; }

        HousesDataSet housesDataSet;
        UsersDataSet usersDataSet;
        
        public Command NextHouseCommand { get; set; }
        public Command PrevHouseCommand { get; set; }
        public Command NewHouseCommand { get; set; }
        public Command DeleteHouseCommand { get; set; }

        public Command NextUserCommand { get; set; }
        public Command PrevUserCommand { get; set; }
        public Command NewUserCommand { get; set; }
        public Command DeleteUserCommand { get; set; }

        public Command<Phone> DeletePhoneCommand { get; set; }
        public Command AddPhoneCommand { get; set; }

        public Command SaveDataCommand { get; set; }

        public House CurrentHouse { get; set; }
        public User CurrentUser { get; set; }

        #region Constructor

        public MainViewModel()
        {
            JSONData.DeserializeJsonFile(inputFilePath);
            housesDataSet = JSONData.HousesDataSet;
            usersDataSet = JSONData.UsersDataSet;

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

            currentHouseIndex = 0;
            currentUserIndex = 0;

            ShowHouse(currentHouseIndex);
            ShowUser(currentUserIndex);

            HousesNotEmpty = housesDataSet.D.Items.Count != 0 ? true : false;
            UsersNotEmpty = usersDataSet.D.Items.Count != 0 ? true : false;
        }
        
        #endregion

        #region house object behaviour

        void ShowHouse(int index)
        {
            CurrentHouse = housesDataSet.D.Items[index];
            NextHouseCommand.IsEnabled = currentHouseIndex < (housesDataSet.D.Items.Count - 1);
            PrevHouseCommand.IsEnabled = currentHouseIndex > 0;
        }

        private void NextHouse()
        {
            ShowHouse(++currentHouseIndex);
        }

        private void PrevHouse()
        {
            ShowHouse(--currentHouseIndex);
        }

        private void NewHouse()
        {
            housesDataSet.D.Items.Add(new House());
            currentHouseIndex = housesDataSet.D.Items.Count - 1;
            ShowHouse(currentHouseIndex);
            if (!HousesNotEmpty)
            {
                DeleteHouseCommand.IsEnabled = true;
                HousesNotEmpty = true;
            }
        }

        /*private void SaveHouse()
        {
            housesDataSet.D.Items[currentHouseIndex] = CurrentHouse;
        }
        */
        private void DeleteHouse()
        {
            housesDataSet.D.Items.RemoveAt(currentHouseIndex);
            if (housesDataSet.D.Items.Count != 0)
            {
                if (currentHouseIndex < housesDataSet.D.Items.Count)
                {
                    ShowHouse(currentHouseIndex);
                }
                else
                {
                    ShowHouse(--currentHouseIndex);
                }
            }
            else
            {
                CurrentHouse = null;
                HousesNotEmpty = false;
            }
            DeleteHouseCommand.IsEnabled = housesDataSet.D.Items.Count != 0;
        }

        #endregion
        

        #region user object behaviour

        void ShowUser(int index)
        {
            CurrentUser = usersDataSet.D.Items[index];
            NextUserCommand.IsEnabled = currentUserIndex < (usersDataSet.D.Items.Count - 1);
            PrevUserCommand.IsEnabled = currentUserIndex > 0;
            PhonesNotEmpty = CurrentUser.Phones.Count == 0 ? false : true;
        }

        private void NextUser()
        {
            ShowUser(++currentUserIndex);
        }

        private void PrevUser()
        {
            ShowUser(--currentUserIndex);
        }

        private void NewUser()
        {
            usersDataSet.D.Items.Add(new User());
            currentUserIndex = usersDataSet.D.Items.Count - 1;
            ShowUser(currentUserIndex);
            if (!UsersNotEmpty)
            {
                DeleteUserCommand.IsEnabled = true;
                UsersNotEmpty = true;
            }
        }
        /*
        private void SaveUser()
        {
            usersDataSet.D.Items[currentUserIndex] = CurrentUser;
        }
        */
        private void DeleteUser()
        {
            usersDataSet.D.Items.RemoveAt(currentUserIndex);
            if(usersDataSet.D.Items.Count != 0)
            {
                if(currentUserIndex < usersDataSet.D.Items.Count)
                {
                    ShowUser(currentUserIndex);
                }
                else
                {
                    ShowUser(--currentUserIndex);
                }
            }
            else
            {
                CurrentUser = null;
                UsersNotEmpty = false;
            }
            DeleteUserCommand.IsEnabled = usersDataSet.D.Items.Count != 0;
        }

        /// <summary>
        /// добавление еще одного телефона для текущего пользователя
        /// </summary>
        private void AddPhone()
        {
            Phone phone = new Phone
            {
                Id = 0,
                Value = ""
            };
            if (CurrentUser.Phones == null)
            {
                CurrentUser.Phones = new ObservableCollection<Phone>();
            }

            CurrentUser.Phones.Add(phone);

            PhonesNotEmpty = true;
        }
        
        private void ExecuteDeletePhoneCommand(Phone phone)
        {
            CurrentUser.Phones.Remove(phone);
            if (CurrentUser.Phones.Count == 0)
            {
                PhonesNotEmpty = false;
            }
        }

        #endregion

        #region Save Data

        private void SaveAllData()
        {
            JSONData.HousesDataSet = housesDataSet;
            JSONData.UsersDataSet = usersDataSet;
            JSONData.SaveDataToFile(outputFilePath);
        }

        #endregion
    }
}
