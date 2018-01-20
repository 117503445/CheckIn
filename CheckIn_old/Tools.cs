using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CheckIn
{
   public static class  Tools
    {
        public static bool isFormAdminActive = false;
        public static bool tDEBUG = false;
        public static string databasePath = System.Windows.Forms.Application.StartupPath + @"\\file\\mdb\\log.mdb";
        public static string[] studentData;
    }
}
