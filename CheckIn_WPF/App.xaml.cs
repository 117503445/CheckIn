using System.IO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace CheckIn_WPF
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static WdAdmin wdAdmin;
        private static SortedSet<Student> stus = new SortedSet<Student>();
        static int checkDayOfWeek = (int)DateTime.Now.DayOfWeek;
        public static string TimeStamp()
        {
            var t = DateTime.Now;
            return string.Format("{0},{1},{2},{3},{4}", t.Month, t.Day, t.Hour, t.Minute, t.Second);
        }
        public static void SaveStudentsAsync()
        {
            XDocument xDoc = new XDocument(
                   new XElement(
                    "students"
)
                                             );
            foreach (var item in App.Stus)
            {
                xDoc.Element("students").Add(new XElement("student", new XAttribute("id", item.Id), new XAttribute("name", item.Name), new XAttribute("column", item.Column), new XAttribute("row", item.Row)));
            }
            xDoc.Save(AppDomain.CurrentDomain.BaseDirectory + "/Student.xml");
        }
        public static string XmlFileName
        {
            get
            {
                System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("zh-CN");
                System.Globalization.Calendar calendar = cultureInfo.Calendar;
                int weekOfYear = calendar.GetWeekOfYear(DateTime.Now, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
#if DEBUG
                return weekOfYear.ToString() + "_DEBUG.xml";
#else
                return weekOfYear.ToString() + ".xml";
#endif

            }
        }

        private static CheckKind currentCheckKind = CheckKind.None;
        public static CheckKind CurrentCheckKind
        {
            set
            {
                currentCheckKind = value;
            }
            get
            {
                if (currentCheckKind == CheckKind.None)
                {
                    int hour = DateTime.Now.Hour;
                    foreach (int item in Enum.GetValues(typeof(CheckKind)))
                    {
                        if (hour == item)
                        {
                            return (CheckKind)item;
                        }
                    }
                    return CheckKind.None;
                }
                else
                {
                    return currentCheckKind;
                }

            }
        }

        public static int CheckDayOfWeek { get => checkDayOfWeek; set => checkDayOfWeek = value; }
        public static SortedSet<Student> Stus
        {
            get
            {
                if (stus.Count != NumStudents)
                {
                    //await Logger.WriteAsync("未满足48人的限制"); 
                }
                return stus;
            }
            set => stus = value;
        }
        private const int NumStudents = 48;

        public static string path_Dir_Root = AppDomain.CurrentDomain.BaseDirectory;
        public static string path_Dir_File = path_Dir_Root + @"\File\";
        public static string path_File_TempXML = path_Dir_File + @"\temp.xml";
        public static string path_Dir_Backup = path_Dir_File + @"\backup\";

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Directory.CreateDirectory(path_Dir_File);
            Directory.CreateDirectory(path_Dir_Backup);
        }
    }
}
