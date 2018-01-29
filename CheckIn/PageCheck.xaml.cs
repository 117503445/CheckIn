using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Xml.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using System.Text;
using Windows.UI.Popups;
using Windows.Storage.Streams;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Shapes;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace CheckIn
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class PageCheck : Page
    {
        List<Student> stus = new List<Student>();
        private CheckKind CurrentCheckKind { get { return GetCheckKind(); } }
        public PageCheck()
        {
            this.InitializeComponent();
            LoadStu();
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            Debug.WriteLine("path={0};", folder.Path);

        }
        /// <summary>
        ///从student.xml中加载学生数据 
        /// </summary>
        private void LoadStu()
        {
            XElement xElement = XElement.Load(@"Assets\Student.xml");
            foreach (var item in xElement.Elements())
            {
                //Debug.WriteLine(item);
                string name = item.Attribute("name").Value;
                int id = int.Parse(item.Attribute("id").Value);
                int row = int.Parse(item.Attribute("row").Value);
                int column = int.Parse(item.Attribute("column").Value);
                Student student = new Student(name, id, row, column, GridTable);
                stus.Add(student);
                student.Button.Click += BtnStu_Click;
            }
        }

        private void BtnStu_Click(object sender, RoutedEventArgs e)
        {
            SaveTemp();
        }
        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // SaveLog();
            //LoadTemp();
            bool isBlank = true;
            foreach (var item in stus)
            {
                if (item.CType!=CheckType.Present)
                {
                    isBlank = false;
                    break;
                }
            }
            if (isBlank)
            {
                if (await CheckIfLoadTempAsync())
                {
                    LoadTemp();
                }
            }
            else
            {
                SaveLog();
            }
        }
        private async void SaveLog()
        {
            if (CurrentCheckKind == CheckKind.None)
            {
                Debug.WriteLine("不是正常的时间");
#if !DEBUG
return;
#endif
            }
            try
            {
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                XDocument xDoc;
                StorageFile file = await storageFolder.CreateFileAsync(App.XmlFileName, CreationCollisionOption.OpenIfExists);
                try
                {
                    using (var stream = await file.OpenStreamForReadAsync())
                    {
                        xDoc = XDocument.Load(stream);
                    }
                }
                catch (Exception)
                {
                    xDoc = new XDocument();
                    XElement root = new XElement("Logs");
                    xDoc.Add(root);
                }
                string missId = "";
                int missNum = 0;
                foreach (var item in stus)
                {
                    //Debug.WriteLine(item.Id);
                    //Debug.WriteLine(item.Button.IsChecked);
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
                DateTime t = DateTime.Now;
                xDoc.Element("Logs").Add(new XElement("Log",
                    new XAttribute("checkKind", CurrentCheckKind),
                    new XAttribute("dayOfWeek", (int)DateTime.Now.DayOfWeek),
                    new XAttribute("missId", missId),
                    new XAttribute("time", string.Format("{0},{1},{2},{3}", t.Month, t.Day, t.Hour, t.Minute, t.Second))
                    ));

                var dialog = new MessageDialog(string.Format("+{0}s", missNum));

                dialog.Commands.Add(new UICommand("吼啊", cmd => { }, commandId: 0));
                dialog.Commands.Add(new UICommand("取消", cmd => { }, commandId: 1));

                //设置默认按钮，不设置的话默认的确认按钮是第一个按钮
                dialog.DefaultCommandIndex = 0;
                dialog.CancelCommandIndex = 1;

                //获取返回值
                var result = await dialog.ShowAsync();
                if ((int)result.Id == 0)
                {
                    using (var stream = await file.OpenStreamForWriteAsync())
                    {
                        xDoc.Save(stream);
                    }
                }

            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex);
            }
        }
        private async void SaveTemp()
        {
            XDocument xDoc = new XDocument(
                new XElement(
                 "root",
                     new XElement("CheckType", CurrentCheckKind),
                     new XElement("dayOfWeek", (int)DateTime.Now.DayOfWeek),
                     new XElement("students")
                            )
                                          );
            foreach (var item in stus)
            {
                xDoc.Element("root").Element("students").Add(new XElement("student", new XAttribute("ID", item.Id), new XAttribute("CheckType", item.CType)));
            }
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await storageFolder.CreateFileAsync("temp.xml", CreationCollisionOption.ReplaceExisting);
            using (var stream = await file.OpenStreamForWriteAsync())
            {
                xDoc.Save(stream);
            }
        }
        private async void LoadTemp()
        {
            XDocument xDoc = await LoadTempXml();
            var t = xDoc.Element("root").Element("students").Elements();
            int i = 0;
            foreach (var item in t)
            {
                var str = item.Attribute("CheckType").Value;
                stus[i].CType = (CheckType)Enum.Parse(typeof(CheckType), str);
                i++;
            }

        }
        private async Task<bool> CheckIfLoadTempAsync()
        {
            XDocument xDoc = await LoadTempXml();
            if (xDoc == new XDocument())
            {
                return false;
            }
            else
            {
                if (xDoc.Element("root").Element("dayOfWeek").Value == DateTime.Now.DayOfWeek.ToString() || xDoc.Element("root").Element("CheckType").Value == CurrentCheckKind.ToString())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        private async Task<XDocument> LoadTempXml()
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            try
            {
                StorageFile file = await storageFolder.CreateFileAsync("temp.xml", CreationCollisionOption.OpenIfExists);
                using (var stream = await file.OpenStreamForReadAsync())
                {
                    return (XDocument.Load(stream));
                }
            }
            catch { return new XDocument(); }
        }
        private CheckKind GetCheckKind()
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

    }
    public class Student
    {
        private Button button = new Button();

        private string name = "";
        private int id = 0;
        private int row = 0;
        private int column = 0;
        private Ellipse ellipse;
        private CheckType cType = CheckType.Present;
        public CheckType CType
        {
            get { return cType; }
            set
            {
                switch (value)
                {
                    case CheckType.Present:
                        ellipse.Opacity = 0;
                        Debug.WriteLine("在场");
                        break;
                    case CheckType.Absent:
                        ellipse.Opacity = 1;
                        ellipse.Fill = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 0, 0));
                        Debug.WriteLine("不在场");
                        break;
                    case CheckType.Leave:
                        ellipse.Opacity = 1;
                        ellipse.Fill = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 255));
                        Debug.WriteLine("Leave");
                        break;
                    default:
                        break;
                }
                cType = value;
            }
        }

        public Student(string name, int id, int row, int column, Grid grid)
        {

            this.Name = name;
            this.Id = id;
            this.Row = row;
            Column = column;
            //Button.Name = "Btn" + id.ToString();
            //Button.Content = name;

            StackPanel stackPanel = new StackPanel
            {
                Padding = new Thickness(0),
                Orientation = Orientation.Horizontal
            };
            TextBlock textBlock = new TextBlock
            {
                Text = Name,
                FontSize = 18
            };
            ellipse = new Ellipse
            {
                Margin = new Thickness(0, 0, 0, 0),
                Width = 18,
                Height = 18,
                //Fill = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 0, 0))
            }; stackPanel.Children.Add(ellipse);
            stackPanel.Children.Add(textBlock);
            button.HorizontalContentAlignment = HorizontalAlignment.Left;
            button.Content = stackPanel;
            grid.Children.Add(Button);
            Grid.SetRow(Button, 2 * row - 2);
            Grid.SetColumn(Button, 2 * column - 2);
            //button.Margin = new Thickness(150 * column, 90 * row, 0, 0);
            button.HorizontalAlignment = HorizontalAlignment.Stretch;
            button.VerticalAlignment = VerticalAlignment.Stretch;
            button.Click += Button_Click;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (CType == CheckType.Present)
            {
                CType = CheckType.Absent;
            }
            else if (CType == CheckType.Absent)
            {
                CType = CheckType.Leave;
            }
            else
            {
                CType = CheckType.Present;
            }
        }



        public string Name { get => name; set => name = value; }
        public int Id { get => id; set => id = value; }
        public int Row { get => row; set => row = value; }
        public int Column { get => column; set => column = value; }
        public Button Button { get => button; set => button = value; }
    }
    public enum CheckKind
    {
        MorningRead = 6,
        MorningExercise = 9,
        MorningEye = 10,
        NoonSleep = 12,
        AfternoonEye = 15,
        NightStudy = 17,
        NightEye = 20,
        None = 37628,
    }
    public enum CheckType
    {
        /// <summary>
        /// 在场
        /// </summary>
        Present,
        /// <summary>
        /// 缺席
        /// </summary>
        Absent,
        /// <summary>
        /// 请假
        /// </summary>
        Leave
    }
}
