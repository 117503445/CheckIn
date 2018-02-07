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

        public static async Task WriteAsync(Exception ex, bool isSerious)
        {
            StorageFile file = await storageFolder.CreateFileAsync("Log.txt", CreationCollisionOption.OpenIfExists);
            string str = DateTime.Now.ToString() + ";" + (isSerious ? "Serious" : "") + "ERROR" + ";" + ex.Message + "\r\n";
            await FileIO.AppendTextAsync(file, str);
            Debug.Fail(ex.ToString());
        }
        public static async Task WriteAsync(string info)
        {
            string str = DateTime.Now.ToString() + ";" + "INFO" + ";" + info + "\r\n";
            StorageFile file = await storageFolder.CreateFileAsync("Log.txt", CreationCollisionOption.OpenIfExists);
            await FileIO.AppendTextAsync(file, str);
            Debug.Write(info);
        }
    }
}
