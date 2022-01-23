using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace DailyCodingLanguagesApp
{
    class FileBackupIO
    {
        public static void SaveTips(SortedDictionary<DateTime, LanguageOfTheDay> tips)
        {
            try
            {
                string fileNameToSave = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Tips.dat");
                Serialize(tips, File.Open(fileNameToSave, FileMode.Create));
            }
            catch(Exception ex)
            {

            }
        }
        public static SortedDictionary<DateTime, LanguageOfTheDay> LoadTips()
        {
            try
            {
                string fileNameToSave = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Tips.dat");
                if (File.Exists(fileNameToSave))
                {
                    return Deserialize<SortedDictionary<DateTime, LanguageOfTheDay>>(File.Open(fileNameToSave, FileMode.Open));
                }
            }
            catch(Exception ex)
            {

            }
            return new SortedDictionary<DateTime, LanguageOfTheDay>();
        }

        //https://stackoverflow.com/a/36335767
        public static void Serialize<Object>(Object dictionary, Stream stream)
        {
            // serialize the collection to a file      
            using (stream)
            {
                // create BinaryFormatter
                BinaryFormatter bin = new BinaryFormatter();
                // serialize the collection (EmployeeList1) to file (stream)
                bin.Serialize(stream, dictionary);
            }
        }

        public static Object Deserialize<Object>(Stream stream) where Object : new()
        {
            Object ret = CreateInstance<Object>();
            using (stream)
            {
                // create BinaryFormatter
                BinaryFormatter bin = new BinaryFormatter();
                // deserialize the collection (Employee) from file (stream)
                ret = (Object)bin.Deserialize(stream);
            }
            return ret;
        }

        // function to create instance of T
        public static Object CreateInstance<Object>() where Object : new()
        {
            return (Object)Activator.CreateInstance(typeof(Object));
        }
    }
}
