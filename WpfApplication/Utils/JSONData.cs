using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

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
            using (StreamReader sr = File.OpenText(inputFilePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Equals(startData))
                    {
                        jsonDataSet.Add(null);
                    }
                    else if (line.Equals(endData))
                    {
                        dataSetNum++;
                    }
                    else jsonDataSet[dataSetNum] += line;
                }
            }

            foreach (string jsonData in jsonDataSet)
            {
                DataType dt = JsonConvert.DeserializeObject<DataType>(jsonData);

                if (dt.Q.Equals("HOUSES"))
                {
                    HousesDataSet = JsonConvert.DeserializeObject<HousesDataSet>(jsonData);
                }
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
            using (StreamWriter file = File.CreateText(outputFilePath))
            {
                file.WriteLine(startData);
                file.WriteLine(JsonConvert.SerializeObject(HousesDataSet, Formatting.Indented));
                file.WriteLine(endData);
                file.WriteLine(startData);
                file.WriteLine(JsonConvert.SerializeObject(UsersDataSet, Formatting.Indented));
                file.WriteLine(endData);
            }
        }
    }
}
