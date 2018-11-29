using Microsoft.Win32;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace WpfApplication
{
    static class JSONData
    {
        /// <summary>
        /// Список наборов данных из файла
        /// </summary>
        private static List<string> jsonDataSet = new List<string>();

        /// <summary>
        /// Счетчик считанных наборов данных
        /// </summary>
        private static int dataSetNum = 0;

        /// <summary>
        /// Начало json данных
        /// </summary>
        private static string startData = "-----";

        /// <summary>
        /// конец json данных
        /// </summary>
        private static string endData = "/-----";

        /// <summary>
        /// Набор данных о домах
        /// </summary>
        public static HousesDataSet HousesDataSet { get; set; }

        /// <summary>
        /// Набор данных о пользователях
        /// </summary>
        public static UsersDataSet UsersDataSet { get; set; }
        
        /// <summary>
        /// Десериализация файла
        /// </summary> 
        /// <param name="inputFilePath">Путь к JSON файлу</param>
        public static void DeserializeJsonFile(string inputFilePath)
        {
            // Если путь по умолчанию не найден...
            if (!File.Exists(inputFilePath))
            {
                // Вывести сообщение
                MessageBox.Show("Входной файл не найден, укажите путь","Файл не найден",MessageBoxButton.OK,MessageBoxImage.Warning);
                // Показать диалог открытия файла
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    // Присвоить переменной новый путь
                    inputFilePath = openFileDialog.FileName;
                }
            }

            // Открыть файл
            using (StreamReader sr = File.OpenText(inputFilePath))
            {
                string line;
                // Присвоить к переменной line каждую строку, пока не достигнет конца файла
                while ((line = sr.ReadLine()) != null)
                {
                    // К списку json данных добавляется элемент, если найдено начало данных
                    if (line.Equals(startData))
                    {
                        jsonDataSet.Add(null);
                    }
                    // Увеличить счетчик json данных если найден конец данных
                    else if (line.Equals(endData)) 
                    {
                        dataSetNum++;
                    }
                    // В остальных случаях добавить строку к к текущему элементу списка json данных, который определяется счетчиком
                    else if (dataSetNum > 0)
                    {
                        jsonDataSet[dataSetNum] += line;
                    } 
                }
            }

            // Для каждых json данных в списке данных.. 
            foreach (string jsonData in jsonDataSet)
            {
                // Десериализовать тип данных
                DataType dt = JsonConvert.DeserializeObject<DataType>(jsonData);

                // Если тип "HOUSES", десериализовать json данные как набор данных домов
                if (dt.Q.Equals("HOUSES"))
                {
                    HousesDataSet = JsonConvert.DeserializeObject<HousesDataSet>(jsonData);
                }
                // Если тип "USERS", десериализовать json данные как набор данных пользователей
                else if (dt.Q.Equals("USERS"))
                {
                    UsersDataSet = JsonConvert.DeserializeObject<UsersDataSet>(jsonData);
                }
            }
        }

        /// <summary>
        /// Сериализация данных и сохранение в файл
        /// </summary>
        /// <param name="outputFilePath">Путь к файлу для сохранения</param>
        public static void SaveDataToFile(string outputFilePath)
        {
            // Если путь по умолчанию не найден...
            if (!File.Exists(outputFilePath))
            {
                // Вывести сообщение
                MessageBox.Show("Выходной файл не найден, укажите путь", "Файл не найден", MessageBoxButton.OK, MessageBoxImage.Warning);
                // Показать диалог открытия файла
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    // Присвоить переменной новый путь
                    outputFilePath = openFileDialog.FileName;
                }
            }

            // Записать данные в выходной файл
            using (StreamWriter file = File.CreateText(outputFilePath))
            {
                // Поставить начало json данных
                file.WriteLine(startData);
                // Десериализовать и записать набор данных о домах
                file.WriteLine(JsonConvert.SerializeObject(HousesDataSet, Formatting.Indented));
                // Поставить конец json данных
                file.WriteLine(endData);
                // Поставить начало json данных
                file.WriteLine(startData);
                // Десериализовать и записать набор данных о пользователях
                file.WriteLine(JsonConvert.SerializeObject(UsersDataSet, Formatting.Indented));
                // Поставить конец json данных
                file.WriteLine(endData);
            }
        }
    }
}
