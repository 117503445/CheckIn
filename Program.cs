using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace CheckIn
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            bool bCreatedNew;
            Mutex m = new Mutex(false, "bxSys.exe", out bCreatedNew);
            if (bCreatedNew) //未运行
                Application.Run(new FrmMain());
            else
                return;
        }
    }
}
