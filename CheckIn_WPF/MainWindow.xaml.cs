using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace CheckIn_WPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadStu();
        }
        /// <summary>
        ///从student.xml中加载学生数据 
        /// </summary>
        private void LoadStu()
        {
            //StorageFolder folder = ApplicationData.Current.LocalFolder;
            //XElement xElement;
            //try
            //{
            //    StorageFile file = await folder.GetFileAsync("Student.xml");
            //    using (var stream = await file.OpenStreamForReadAsync())
            //    {
            //        xElement = XElement.Load(stream);
            //    }
            //}
            //catch (Exception)
            //{
            //    xElement = XElement.Load(@"Student.xml");
            //    //await Logger.WriteAsync("缺失Student.xml,读取默认XML");
            //}
            XElement xElement = XElement.Load("Student.xml");
            SortedSet<Student> tempSet = new SortedSet<Student>();
            foreach (var item in xElement.Elements())
            {
                string name = item.Attribute("name").Value;
                int id = int.Parse(item.Attribute("id").Value);
                int row = int.Parse(item.Attribute("row").Value);
                int column = int.Parse(item.Attribute("column").Value);
                Student student = new Student(name, id, row, column);
                tempSet.Add(student);
                student.BtnClick += (s, e) => { SaveTemp(); };
                //student.Btnstu.AddHandler(PointerReleasedEvent, new PointerEventHandler(Btnstu_PointerReleased), true);
                student.ShowButtonOfStudent(GridTable);
            }
            App.Stus = tempSet;
            App.SaveStudentsAsync();
        }
        //private void Btnstu_PointerReleased(object sender, PointerRoutedEventArgs e)
        //{
        //    SaveTemp();
        //}
        private void SaveLog()
        {



            if (App.CurrentCheckKind == CheckKind.None)
            {

#if !DEBUG
                //await UMessageDialogAsync("不是正常的时间,请自行设置签到种类", "确定");
                MessageBox.Show("不是正常的时间,请自行设置签到种类", "确定");
                return;
#endif
            }
            try
            {
                XDocument xDoc;
                try
                {
                    xDoc = XDocument.Load(App.XmlFileName);
                }
                catch (Exception exp)//创建新的document
                {
                    if (exp.Message != "Root element is missing." && exp.Message != "Xml_MissingRoot")
                    {
                        //await Logger.WriteAsync(exp, true);
                        if (MessageBox.Show("致命的读取错误，按确定继续" + "exp.Message", "错误", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                        {
                            return;
                        }
                    }
                    else
                    {
                        //await Logger.WriteAsync("创建新的XML");
                    }

                    xDoc = new XDocument();
                    xDoc.Add(new XElement("Logs"));
                }
                string missId = "";//记录缺失的学号
                int missNum = 0;
                foreach (var item in App.Stus)
                {
                    if (item.CType == CheckType.Absent)
                    {
                        missId += item.Id.ToString() + ",";
                        missNum++;
                    }
                }
                if (missId.Length != 0)
                {
                    missId = missId.Substring(0, missId.Length - 1);
                }
                Console.WriteLine("MissID=" + missId);
                DateTime t = DateTime.Now;
                var i = (from x in xDoc.Root.Elements() where x.Attribute("checkKind").Value == App.CurrentCheckKind.ToString() && int.Parse(x.Attribute("dayOfWeek").Value) == App.CheckDayOfWeek select x).ToList();
                if (i.Count() == 0)
                {
                    xDoc.Element("Logs").Add(new XElement("Log",
    new XAttribute("checkKind", App.CurrentCheckKind),
    new XAttribute("dayOfWeek", (int)App.CheckDayOfWeek),
    new XAttribute("missId", missId),
    new XAttribute("time", App.TimeStamp())
    ));
                }
                else
                {
                    if (MessageBox.Show("似乎已经签到过了，确定继续", "疑惑", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)//UMessageDialogAsync("似乎已经签到过了", "吼啊", "取消") == 
                    {
                        return;
                    }
                    i.Last().SetAttributeValue("missId", missId);
                    i.Last().SetAttributeValue("time", App.TimeStamp());
                }
                string message = string.Format("+{0}s", missNum);
#if DEBUG
                message += "    程序运行在调试模式.如果你在工作,不用惊慌,正常签到后通知QHT即可";
#endif

                if (MessageBox.Show(message, "OK", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    xDoc.Save(App.XmlFileName);
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "/backup/");
                    xDoc.Save(AppDomain.CurrentDomain.BaseDirectory + "/backup/" + App.TimeStamp() + "_" + App.XmlFileName);
#if !DEBUG
                    App.Current.Shutdown();
#endif
                }
            }
            catch (Exception ex)
            {
                //await Logger.WriteAsync(ex);
            }
        }
        public static int UMessageDialogAsync(params string[] a)
        {
            return 1;
        }
        public void SaveTemp()
        {
            XDocument xDoc = new XDocument(
                new XElement(
                 "root",
                     new XElement("CheckHour", DateTime.Now.Hour),
                     new XElement("dayOfWeek", App.CheckDayOfWeek),
                     new XElement("students")
                            )
                                          );
            foreach (var item in App.Stus)
            {
                xDoc.Element("root").Element("students").Add(new XElement("student", new XAttribute("ID", item.Id), new XAttribute("CheckType", item.CType)));
            }
            //StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            //StorageFile file = await storageFolder.CreateFileAsync("temp.xml", CreationCollisionOption.ReplaceExisting);
            //using (var stream = await file.OpenStreamForWriteAsync())
            //{
            //    xDoc.Save(stream);
            //}
            xDoc.Save("temp.xml");
            Console.WriteLine("Save");
        }
        private void LoadTemp()
        {
            Console.WriteLine("LoadTemp");
            XDocument xDoc = LoadTempXml();
            var t = xDoc.Element("root").Element("students").Elements();
            int i = 0;
            foreach (var item in t)
            {
                var str = item.Attribute("CheckType").Value;
                App.Stus.ElementAt(i).CType = (CheckType)Enum.Parse(typeof(CheckType), str);
                i++;
            }

        }
        private bool CheckIfLoadTempAsync(XDocument xDoc)
        {
            if (xDoc == null)
            {
                return false;
            }
            else
            {
                if (xDoc.Element("root").Element("dayOfWeek").Value == App.CheckDayOfWeek.ToString() && xDoc.Element("root").Element("CheckHour").Value == DateTime.Now.Hour.ToString())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        private XDocument LoadTempXml()
        {
            //StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            //try
            //{
            //    StorageFile file = await storageFolder.CreateFileAsync("temp.xml", CreationCollisionOption.OpenIfExists);
            //    using (var stream = await file.OpenStreamForReadAsync())
            //    {
            //        return (XDocument.Load(stream));
            //    }
            //}
            //catch { return null; }
            return XDocument.Load("temp.xml");
        }
        /// <summary>
        /// 对MessageDialog进行封装
        /// </summary>
        /// <param name="message">消息框</param>
        /// <param name="info">确认,取消...</param>
        /// <returns></returns>
        //private static async Task<int> UMessageDialogAsync(string message, params string[] info)
        //{
        //    var dialog = new MessageDialog(message)
        //    {
        //        DefaultCommandIndex = 0,
        //        CancelCommandIndex = 1
        //    };
        //    for (int i = 0; i < info.Length; i++)
        //    {
        //        dialog.Commands.Add(new UICommand(info[i], cmd => { }, commandId: i));
        //    }
        //    var result = await dialog.ShowAsync();
        //    return (int)result.Id;
        //}
        private void BtnMain_Click(object sender, RoutedEventArgs e)
        {
            SaveLog();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Load");
            XDocument xDoc = LoadTempXml();
            if (CheckIfLoadTempAsync(xDoc))
            {
                LoadTemp();
            }
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }

        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            else
            {
                WindowState = WindowState.Maximized;
            }
        }

        private void BtnMain_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            App.wdAdmin.Show();
        }
    }
}
