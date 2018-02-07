using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Windows.Storage;

namespace CheckIn
{
    public static class Logger
    {
        static StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

#if DEBUG
        static string nameLog = "DebugLog.txt";
#else
  static string nameLog = "Log.txt";
#endif
        public static async Task WriteAsync(Exception ex, bool isSerious=false)
        {
            StorageFile file = await storageFolder.CreateFileAsync(nameLog, CreationCollisionOption.OpenIfExists);
            string str = DateTime.Now.ToString() + ";" + (isSerious ? "SERIOUS" : "") + "ERROR" + ";" + ex.Message + "\r\n";
            await FileIO.AppendTextAsync(file, str);
            Debug.WriteLine(ex.ToString());
        }
        public static async Task WriteAsync(string info)
        {
            string str = DateTime.Now.ToString() + ";" + "INFO" + ";" + info + "\r\n";
            StorageFile file = await storageFolder.CreateFileAsync(nameLog, CreationCollisionOption.OpenIfExists);
            await FileIO.AppendTextAsync(file, str);
            Debug.WriteLine(info);
        }
    }
}
