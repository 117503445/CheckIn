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
namespace CheckIn
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class PageCheck : Page
    {


        public PageCheck()
        {
            this.InitializeComponent(); LoadStu();
            Debug.WriteLine(ApplicationData.Current.LocalFolder.Path);
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
                Student student = new Student(name, id, row, column);
                App.Stus.Add(student);
                student.Button.Click += BtnStu_Click;
                student.ShowButtonOfStudent(GridTable);
            }
        }


        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (await CheckIfLoadTempAsync())
            {
                LoadTemp();
            }
        }
        private void BtnStu_Click(object sender, RoutedEventArgs e)
        {
            SaveTemp();
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // SaveLog();
            //LoadTemp();
            //bool isBlank = true;
            //foreach (var item in stus)
            //{
            //    if (item.CType != CheckType.Present)
            //    {
            //        isBlank = false;
            //        break;
            //    }
            //}
            //if (isBlank)
            //{

            //}
            //else
            //{
            SaveLog();
            // }
        }
        private async void SaveLog()
        {
            //Debug.WriteLine("");
            //Debug.WriteLine("-------------开始读取--------------");
            //Debug.WriteLine("");
            if (App.CurrentCheckKind == CheckKind.None)
            {
                Debug.WriteLine("不是正常的时间");
#if !DEBUG
                //var dialog = new MessageDialog("不是正常的时间,请自行设置签到种类")
                //{
                //    DefaultCommandIndex = 0,
                //};
                //dialog.Commands.Add(new UICommand("确定", cmd => { }, commandId: 0));
                await UMessageDialogAsync("不是正常的时间,请自行设置签到种类", "确定");
                return;
#endif
            }
            try
            {
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile file = await storageFolder.CreateFileAsync(App.XmlFileName, CreationCollisionOption.OpenIfExists);
                XDocument xDoc;
                try
                {
                    using (var stream = await file.OpenStreamForReadAsync())
                    {
                        xDoc = XDocument.Load(stream);
                    }
                    //Debug.WriteLine("读取XML成功");
                    //Debug.WriteLine("---!---");
                    //Debug.WriteLine(xDoc);
                    //Debug.WriteLine("---?---");
                }
                catch (Exception ex)//创建新的document
                {
                    //Debug.WriteLine("读取失败");
                    //Debug.WriteLine("---!---");
                    if (ex.Message != "Root element is missing.")
                    {
                        Debug.WriteLine("致命的读取错误:" + ex.Message);
                    }
                    //Debug.WriteLine("---?---");
                    xDoc = new XDocument();
                    XElement root = new XElement("Logs");
                    xDoc.Add(root);
                }
                string missId = "";//记录缺失的学号
                int missNum = 0;
                foreach (var item in App.Stus)
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
                //Debug.WriteLine(string.Format("missId={0}", missId));
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
                    //Debug.WriteLine("---!---");
                    //Debug.WriteLine("添加记录");
                    //Debug.WriteLine(xDoc);
                    //Debug.WriteLine("---?---");

                }
                else
                {
                    if (await UMessageDialogAsync("似乎已经签到过了", "吼啊", "取消") == 1)
                    {
                        return;
                    }
                    //Debug.WriteLine("修改了" + i.Last().ToString());
                    i.Last().SetAttributeValue("missId", missId);
                    i.Last().SetAttributeValue("time", App.TimeStamp());
                    //Debug.WriteLine("---!---");
                    //Debug.WriteLine("修改纪录");
                    //Debug.WriteLine(xDoc);
                    //Debug.WriteLine("---?---");
                }

                //var dialog = new MessageDialog(string.Format("+{0}s", missNum))
                //{
                //    DefaultCommandIndex = 0,
                //    CancelCommandIndex = 1
                //};
                //dialog.Commands.Add(new UICommand("吼啊", cmd => { }, commandId: 0));
                //dialog.Commands.Add(new UICommand("取消", cmd => { }, commandId: 1));
                ////获取返回值
                //var result = await dialog.ShowAsync();
                string message = string.Format("+{0}s", missNum);
#if DEBUG
                message += "    程序运行在调试模式.如果你在工作,不用惊慌,正常签到后通知QHT即可";
#endif
                if (await UMessageDialogAsync(message, "吼啊", "取消") == 0)
                {
                    await FileIO.WriteTextAsync(file, xDoc.ToString());

                    //Debug.WriteLine("---!---");
                    //Debug.WriteLine("保存");
                    //Debug.WriteLine(xDoc);
                    //Debug.WriteLine("---?---");

#if !DEBUG
                    App.Current.Exit();
#endif
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex);
            }
            //Debug.WriteLine("");
            //Debug.WriteLine("-------------结束读取--------------");
            //Debug.WriteLine("");
        }
        private async void SaveTemp()
        {
            XDocument xDoc = new XDocument(
                new XElement(
                 "root",
                     new XElement("CheckType", App.CurrentCheckKind),
                     new XElement("dayOfWeek", App.CheckDayOfWeek),
                     new XElement("students")
                            )
                                          );
            foreach (var item in App.Stus)
            {
                xDoc.Element("root").Element("students").Add(new XElement("student", new XAttribute("ID", item.Id), new XAttribute("CheckType", item.CType)));
            }
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await storageFolder.CreateFileAsync("temp.xml", CreationCollisionOption.ReplaceExisting);
            using (var stream = await file.OpenStreamForWriteAsync())
            {
                xDoc.Save(stream);
            }
            //Debug.WriteLine("Finish SaveTemp");
        }
        private async void LoadTemp()
        {
            XDocument xDoc = await LoadTempXml();
            var t = xDoc.Element("root").Element("students").Elements();
            int i = 0;
            foreach (var item in t)
            {
                var str = item.Attribute("CheckType").Value;
                App.Stus.ElementAt(i).CType = (CheckType)Enum.Parse(typeof(CheckType), str);
                i++;
            }

        }
        private async Task<bool> CheckIfLoadTempAsync()
        {
            XDocument xDoc = await LoadTempXml();
            if (xDoc == null)
            {
                return false;
            }
            else
            {
                if (xDoc.Element("root").Element("dayOfWeek").Value == App.CheckDayOfWeek.ToString() || xDoc.Element("root").Element("CheckType").Value == App.CurrentCheckKind.ToString())
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
            catch { return null; }
        }
        private static async Task<int> UMessageDialogAsync(string text, params string[] s)
        {
            var dialog = new MessageDialog(text)
            {
                DefaultCommandIndex = 0,
                CancelCommandIndex = 1
            };
            for (int i = 0; i < s.Length; i++)
            {
                dialog.Commands.Add(new UICommand(s[i], cmd => { }, commandId: i));
            }

            //获取返回值
            var result = await dialog.ShowAsync();
            return (int)result.Id;
        }
    }
}
