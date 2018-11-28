using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace WpfApplication
{
    static class JSONData
    {
        private static List<string> jsonDataSet = new List<string>();
        private static int fileNum = 0;
        private static string startData = "-----";
        private static string endData = "/-----";

        public static HousesDataSet HousesDataSet { get; set; }
        public static UsersDataSet UsersDataSet { get; set; }
        
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
                        fileNum++;
                    }
                    else jsonDataSet[fileNum] += line;
                }
            }

            foreach (string jsonData in jsonDataSet)
            {
                DeserializeJSON(jsonData);
            }
        }

        private static void DeserializeJSON(string jText)
        {

            DataType dt = JsonConvert.DeserializeObject<DataType>(jText);

            if (dt.Q.Equals("HOUSES"))
            {
                HousesDataSet = JsonConvert.DeserializeObject<HousesDataSet>(jText);

            }
            else if (dt.Q.Equals("USERS"))
            {
                UsersDataSet = JsonConvert.DeserializeObject<UsersDataSet>(jText);
            }
        }
        
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
