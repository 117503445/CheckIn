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

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace CheckIn
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        List<Student> stus = new List<Student>();
        private void Btn1_Click(object sender, RoutedEventArgs e)
        {
            //string s;
            //using (Stream file = await file_demonstration.OpenStreamForReadAsync())
            //{
            //    using (StreamReader read = new StreamReader(file))
            //    {
            //        s = read.ReadToEnd();
            //    }
            //}


        }

        private void Page_Loading(FrameworkElement sender, object args)
        {
            XElement xElement = XElement.Load(@"Assets\Student.xml");
            foreach (var item in xElement.Elements())
            {
                //Debug.WriteLine(item);
                string name = item.Attribute("name").Value;
                int id = int.Parse(item.Attribute("id").Value);
                int row = int.Parse(item.Attribute("row").Value);
                int column = int.Parse(item.Attribute("column").Value);
                Student student = new Student(name, id, row, column, grid);
            }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Exit();
        }

        private async void BtnSave_ClickAsync(object sender, RoutedEventArgs e)
        {
            //XElement xlog = new XElement("log");
            //DateTime t = DateTime.Now;
            //xlog.Add(new XAttribute("time", $"{0},{1},1,2"));
            //xElement.Add(xlog);
            //Debug.WriteLine(xElement);
            //XDocument xDocument = new XDocument(xElement);
            try
            {
                Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                Windows.Storage.StorageFile sampleFile = await storageFolder.CreateFileAsync("sample.xml",
                    Windows.Storage.CreationCollisionOption.OpenIfExists);

                string text = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
                Debug.WriteLine(text);
                XDocument xDoc;//Debug.WriteLine();
                try
                {
                    xDoc = XDocument.Load(text);
                }
                catch (Exception)
                {
                    xDoc = new XDocument();
                    xDoc.Add(new XElement("Logs"));
                    Debug.WriteLine(xDoc);
                }
                DateTime t = DateTime.Now;
                xDoc.Element("Logs").Add(new XElement("Log", new XAttribute("time", 
                    string.Format("{0},{1},{2},{3}",t.Month,t.Day,t.Hour,t.Minute,t.Second))));
                var stream = await sampleFile.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);
                using (var outputStream = stream.GetOutputStreamAt(0))
                {
                    using (var dataWriter = new Windows.Storage.Streams.DataWriter(outputStream))
                    {
                        dataWriter.WriteString(xDoc.ToString());
                        await dataWriter.StoreAsync();
                        await outputStream.FlushAsync();
                    }
                }
                stream.Dispose(); // Or use the stream variable (see previous code snippet) with a using statement as well.
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex);
            }
            

        }
    }
    public class Student
    {
        private Button button = new Button();
        private string name = "";
        private int id = 0;
        private int row = 0;
        private int column = 0;

        public Student(string name, int id, int row, int column, Grid grid)
        {
            this.Name = name;
            this.Id = id;
            this.Row = row;
            Column = column;

            button.Width = 120;
            button.Height = 60;
            button.Name = "Btn" + id.ToString();
            button.Content = name;

            grid.Children.Add(button);
            Grid.SetRow(button, 1);
            Grid.SetColumn(button, 1);
            button.Margin = new Thickness(150 * column, 90 * row, 0, 0);
            button.HorizontalAlignment = HorizontalAlignment.Left;
            button.VerticalAlignment = VerticalAlignment.Top;

            button.Click += BtnStu_Click;
        }
        public void BtnStu_Click(object sender, RoutedEventArgs e)
        {
            Debug.Write(Name);
        }

        public Button Button { get => button; set => button = value; }
        public string Name { get => name; set => name = value; }
        public int Id { get => id; set => id = value; }
        public int Row { get => row; set => row = value; }
        public int Column { get => column; set => column = value; }
    }
}
