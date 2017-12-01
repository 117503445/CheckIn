using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;

namespace CheckIn
{
    public partial class FrmMain : Form
    {
        public string filedsName = "";//例如 周四早眼
        public Students[] students = new Students[51];
        public Button[] btnStudents = new Button[51];//学生按钮

        bool mov = false;//初始化
        int xpos;
        int ypos;


        public FrmMain()
        {
            InitializeComponent();

            Console.WriteLine("程序版本" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
            Console.WriteLine("屏幕高度" + Screen.PrimaryScreen.Bounds.Height);
            Console.WriteLine("屏幕宽度" + Screen.PrimaryScreen.Bounds.Width);
            this.Text += System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
#if DEBUG
            this.Text += "_DEBUG";
#endif
            #region 初始化
            for (int i = 0; i < students.Length; i++)
            {
                students[i] = new Students();
                btnStudents[i] = new Button();
                this.Controls.Add(btnStudents[i]);//注册按钮
                btnStudents[i].Visible = false;
                btnStudents[i].BackColor = Color.AliceBlue;

                btnStudents[i].Width = 250 * Screen.PrimaryScreen.Bounds.Width / 3840;
                btnStudents[i].Height = 120 * Screen.PrimaryScreen.Bounds.Height / 2160;
                switch (Screen.PrimaryScreen.Bounds.Width)
                {
                    case 1280: btnStudents[i].Font = new Font(btnStudents[i].Font.FontFamily, 13, btnStudents[i].Font.Style); break;
                    case 3840: btnStudents[i].Font = new Font(btnStudents[i].Font.FontFamily, 17, btnStudents[i].Font.Style); break;

                }
                btnStudents[i].FlatStyle = System.Windows.Forms.FlatStyle.Popup;//Style
                btnStudents[i].Click += new System.EventHandler(this.BtnStudents_Click);//注册单击事件
            }
            #endregion
            #region 读取students.txt
            string txtStudents = Application.StartupPath +
                    "/file/txt/students.txt";
            if (!File.Exists(txtStudents))
            {
                MessageBox.Show("无法找到students.txt,已停止继续加载", "错误");
                MessageBox.Show(txtStudents);


                return;
            }
            try
            {
                Tools.studentData = System.IO.File.ReadAllLines(txtStudents, Encoding.Default);//读取students.txt
            }
            catch
            {
                MessageBox.Show("读取students.txt时失败,已停止继续加载", "错误");
                return;
            }
            Console.WriteLine("Loading students.txt");
            foreach (string i in Tools.studentData)
            {
                string[] strTemp = i.Split(';');
                int ID = 0;
                string name = "";
                int PositionX = 0;
                int PositionY = 0;
                bool isEnabled = true;//判断是否继续向下运行
                try
                {
                    ID = Int32.Parse(strTemp[3]);
                    name = strTemp[2];
                    PositionX = Int32.Parse(strTemp[1]);
                    PositionY = Int32.Parse(strTemp[0]);
                }
                catch
                {
                    MessageBox.Show("读取学生数据" + i + "时出现了问题", "警告");
                    isEnabled = false;
                }
                if (isEnabled)
                {
                    if (ID > 50)
                    {
                        MessageBox.Show("读取学生数据" + i + "时出现了问题,请检查ID", "警告");
                    }
                    else if (PositionX > 8)
                    {
                        MessageBox.Show("读取学生数据" + i + "时出现了问题,请检查列", "警告");
                    }
                    else if (PositionY > 8)
                    {
                        MessageBox.Show("读取学生数据" + i + "时出现了问题,请检查行", "警告");
                    }
                    else
                    {
                        students[ID].Enabled = true;

                        students[ID].Name = name;
                        if (students[ID].Name.Length > 3) { btnStudents[ID].Font = new Font(btnStudents[ID].Font.FontFamily, 13, btnStudents[ID].Font.Style); }

                        students[ID].PositionX = PositionX;
                        students[ID].PositionY = PositionY;

                        btnStudents[ID].Text = students[ID].Name;
                        btnStudents[ID].Visible = true;
                        btnStudents[ID].Top = (int)(0.035 * Screen.PrimaryScreen.Bounds.Width * students[ID].PositionX);
                        btnStudents[ID].Left = (int)(0.07 * Screen.PrimaryScreen.Bounds.Width * students[ID].PositionY);
                    }
                }
            }

            Console.WriteLine("Finish Loading");
            #endregion
            #region 填充comboBox
            string weekstr = DateTime.Now.DayOfWeek.ToString();
            switch (weekstr)
            {
                case "Monday": comboBoxDay.Text = "周一"; break;
                case "Tuesday": comboBoxDay.Text = "周二"; break;
                case "Wednesday": comboBoxDay.Text = "周三"; break;
                case "Thursday": comboBoxDay.Text = "周四"; break;
                case "Friday": comboBoxDay.Text = "周五"; break;
                case "Saturday": comboBoxDay.Text = "周六"; break;
                case "Sunday": comboBoxDay.Text = "周日"; break;
            }
            comboBoxType.Items.Clear();
            comboBoxDay.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxTime.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxType.DropDownStyle = ComboBoxStyle.DropDownList;
            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;
            int hourint = currentTime.Hour;
            comboBoxType.Items.Add("眼");
            switch (hourint)
            {
                case 6: comboBoxType.Items.Add("读"); comboBoxType.Items.Add("操"); comboBoxTime.Text = "早"; comboBoxType.Text = "读"; break;
                case 9:

                    comboBoxType.Items.Add("读");
                    comboBoxType.Items.Add("操"); comboBoxTime.Text = "早";
                    comboBoxType.Text = "操";
                    break;
                case 10: comboBoxTime.Text = "早"; comboBoxType.Items.Add("读"); comboBoxType.Items.Add("操"); comboBoxType.Text = "眼"; break;
                case 12: comboBoxTime.Text = "午"; comboBoxType.Items.Add("休"); comboBoxType.Text = "休"; break;
                case 15: comboBoxTime.Text = "午"; comboBoxType.Items.Add("休"); comboBoxType.Text = "眼"; break;
                case 17: comboBoxTime.Text = "晚"; comboBoxType.Items.Add("修"); comboBoxType.Text = "修"; break;
                case 20: comboBoxTime.Text = "晚"; comboBoxType.Items.Add("修"); comboBoxType.Text = "眼"; break;
                default: listBox.Items.Add("[Warn]奇怪的时间"); comboBoxType.Items.Add("读"); break;
            }


            #endregion


        }
        private void BtnStudents_Click(object sender, EventArgs e)
        {
            String[] arrs = ((Button)sender).Text.Split('\n');
            int ID = 0;
            for (int i = 0; i < students.Length; i++)
            {
                if (arrs[0] == students[i].Name) { ID = i; break; }//查找学号
            }

            if (((Button)sender).BackColor == Color.Orange)
            {
                ((Button)sender).BackColor = Color.LightSteelBlue;
                students[ID].color = 2;
                students[ID].IsCheck = true;
            }
            else if (students[ID].IsCheck == true & ((Button)sender).BackColor == Color.LightSteelBlue)
            {
                ((Button)sender).BackColor = Color.AliceBlue;
                students[ID].color = 0;
            }
            else
            {
                ((Button)sender).BackColor = Color.Orange;
                students[ID].color = 1;
                students[ID].IsCheck = false;
            }
            Console.WriteLine("当前学号:" + ID);
            Console.WriteLine("当前姓名" + students[ID].Name);
            Console.WriteLine("当前颜色" + ((Button)sender).BackColor);
            Console.WriteLine("当前签到情况" + students[ID].IsCheck);

            string[] strTemp = new string[students.Length];

            for (int i = 0; i < students.Length; i++)
            {
                strTemp[i] = students[i].color.ToString();

            }
            string txtTemp = Application.StartupPath + "/file/txt/temp.txt";
            if (File.Exists(txtTemp))
            {
                try { System.IO.File.WriteAllLines(txtTemp, strTemp); }
                catch
                {
                    MessageBox.Show("写入temp.txt时失败,无法记录当前签到情况", "警告");
                }

            }
            else
            {
                MessageBox.Show("无法找到temp.txt,无法记录当前签到情况", "警告");
            }


        }
        private void ComboBoxTime_SelectedIndexChanged(object sender, EventArgs e)


        {
            comboBoxType.Items.Clear();

            switch (comboBoxTime.Text)
            {

                case "早": comboBoxType.Items.Add("读"); comboBoxType.Items.Add("操"); comboBoxType.Text = "读"; break;
                case "午": comboBoxType.Items.Add("休"); comboBoxType.Text = "休"; break;
                case "晚": comboBoxType.Items.Add("修"); comboBoxType.Text = "修"; break;

            }

            comboBoxType.Items.Add("眼");

        }
        private void ButtonSave_Click(object sender, EventArgs e)
        {
            buttonEnd.Enabled = false;
            Application.DoEvents();//禁用BtnEnd

            filedsName = comboBoxDay.Text + comboBoxTime.Text + comboBoxType.Text;
            Console.WriteLine("当前fieldsname" + filedsName);
            listBox.Items.Clear();
            for (int i = 0; i < students.Length; i++)
            {
                if (students[i].Enabled & !students[i].IsCheck) { listBox.Items.Add(i + students[i].Name); }
            }
            try
            {
                #region 数据库连接
                OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; Data Source =" + Tools.databasePath);
                conn.Open();
                OleDbCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select * from Students";
                OleDbDataAdapter myDataAdapter = new OleDbDataAdapter()
                {
                    SelectCommand = cmd
                };
                DataSet myDataSet = new DataSet();
                myDataAdapter.Fill(myDataSet, "Students");
                DataTable myTable = myDataSet.Tables["Students"];
                bool isChecked = false;
                foreach (DataRow myrow in myTable.Rows)
                {
                    if ((bool)myrow[filedsName] == true) { isChecked = true; break; }
                }
                if (isChecked)
                {
                    DialogResult receiver = MessageBox.Show("在" + filedsName + "中检测到已有的数据,是否继续签到？", "疑惑", MessageBoxButtons.OKCancel);
                    if (receiver == DialogResult.OK)
                    {
                        Console.WriteLine("用户OK");
                    }
                    else
                    {
                        Console.WriteLine("用户NO");
                        listBox.Items.Clear();
                        listBox.Items.Add("取消成功");
                        buttonEnd.Enabled = true;
                        return;
                    }
                }
                foreach (DataRow myrow in myTable.Rows)
                {
                    myrow[filedsName] = students[(int)myrow["ID"]].IsCheck;
                }
                OleDbCommandBuilder myOleDeCommandBuilder = new OleDbCommandBuilder(myDataAdapter);
                myDataAdapter.Update(myDataSet, "Students");

                myDataSet.Dispose();        // 释放DataSet对象
                myDataAdapter.Dispose();    // 释放SqlDataAdapter对象
                conn.Close();             // 关闭数据库连接
                conn.Dispose();           // 释放数据库连接对象
                #endregion
            }
            catch
            {
                MessageBox.Show("无法将写入数据库", "严重警告");
                return;
            }
            listBox.Items.Add("签到成功");
            Console.WriteLine("签到成功");
            string[] strBackups;
            string txtbackup = Application.StartupPath +
                    "/file/txt/backup.txt";
            if (File.Exists(txtbackup))
            {


                strBackups = System.IO.File.ReadAllLines(txtbackup, Encoding.Default);//读取backup.txt
                string BpVersion = "";
                System.DateTime currentTime = new System.DateTime();
                currentTime = System.DateTime.Now;
                string timeName = currentTime.Month.ToString() + "_" +
                        currentTime.Day.ToString() + "_" +
                        currentTime.Hour.ToString() + "_" +
                        currentTime.Minute.ToString() + "_" +
                        currentTime.Second.ToString() + "_" +
                        filedsName;

                Console.WriteLine("timeName=" + timeName);
                foreach (string s in strBackups)
                {
                    string[] strTemp = s.Split(';');
                    string sourcePath = Tools.databasePath;
                    string targetPath = "";
                    switch (strTemp[0])
                    {
                        case "BpVersion":
                            BpVersion = strTemp[1];
                            Console.WriteLine("BpVersion=" + BpVersion);
                            break;
                        case "default":
                            Console.WriteLine("Type=default");
                            targetPath = strTemp[1] + "/" + timeName + @".mdb";
                            Console.WriteLine("  sourcePath=" + sourcePath);
                            Console.WriteLine("  targetPath=" + targetPath);
                            if (Directory.Exists(strTemp[1]))
                            {

                                System.IO.File.Copy(sourcePath, targetPath, true);
                                Console.WriteLine("BackupFinished");
                                listBox.Items.Add("备份成功");
                            }
                            else
                            {
                                listBox.Items.Add("未找到default型数据" + strTemp[1]);
                            }

                            break;
                        case "AppPath":
                            Console.WriteLine("Type:AppPath");
                            Console.WriteLine("功能没做");
                            break;
                        case "NoDisk":
                            Console.WriteLine("Type:NoDisk");
                            bool isFind = false;
                            for (char i = 'A'; i <= 'Z'; i++)
                            {

                                if (Directory.Exists(i + strTemp[1]))
                                {
                                    targetPath = i + strTemp[1] + "/" + timeName + @".mdb";
                                    Console.WriteLine("  sourcePath=" + sourcePath);
                                    Console.WriteLine("  targetPath=" + targetPath);
                                    System.IO.File.Copy(sourcePath, targetPath, true);
                                    Console.WriteLine("BackupFinished");
                                    isFind = true;
                                    break;
                                }
                            }
                            if (!isFind) { listBox.Items.Add("未找到NoDisk型备份路径" + strTemp[1]); }
                            break;
                    }
                }
            }
            else
            {
                MessageBox.Show("无法找到backup.txt,无法备份", "严重警告");
            }
            buttonEnd.Enabled = true;
        }
        private void ButtonEnd_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        private void ButtonLoad_Click(object sender, EventArgs e)
        {
            string[] tempReceiver = System.IO.File.ReadAllLines(Application.StartupPath + "/file/txt/temp.txt", Encoding.Default);
            for (int i = 0; i < students.Length; i++)
            {
                string[] tempstr = tempReceiver[i].Split(';');
                Console.WriteLine(tempstr[0]);
                switch (tempstr[0])
                {
                    case "0": btnStudents[i].BackColor = Color.AliceBlue; students[i].color = 0; students[i].IsCheck = true; break;
                    case "1": btnStudents[i].BackColor = Color.Orange; students[i].color = 1; students[i].IsCheck = false; break;
                    case "2": btnStudents[i].BackColor = Color.LightSteelBlue; students[i].color = 2; students[i].IsCheck = true; break;
                }

            }

        }
        private void FrmMain_Load(object sender, EventArgs e)
        {
            Height = Screen.PrimaryScreen.Bounds.Height * 7 / 9;
            Top = Screen.PrimaryScreen.Bounds.Height * 2 / 9;
            Width = Screen.PrimaryScreen.Bounds.Width * 6 / 8;
            Left = Screen.PrimaryScreen.Bounds.Width / 8;

            switch (Screen.PrimaryScreen.Bounds.Width)
            {
                case 1920:
                    listBox.Left = this.Width - 150;
                    listBox.Top = 0;
                    listBox.Height = Height;
                    #region buttonLoad
                    buttonLoad.Width = btnStudents[1].Width;
                    buttonLoad.Height = btnStudents[1].Height;
                    buttonLoad.Left = listBox.Left - buttonLoad.Width - 10;
                    buttonLoad.Top = 550;
                    #endregion               
                    #region buttonSave
                    buttonSave.Width = btnStudents[1].Width;
                    buttonSave.Height = btnStudents[1].Height;
                    buttonSave.Left = listBox.Left - buttonSave.Width - 10;
                    buttonSave.Top = 600;
                    #endregion
                    #region buttonEnd
                    buttonEnd.Width = btnStudents[1].Width;
                    buttonEnd.Height = btnStudents[1].Height;
                    buttonEnd.Left = listBox.Left - buttonEnd.Width - 10;
                    buttonEnd.Top = 650;
                    #endregion
                    comboBoxDay.Left = buttonLoad.Left - comboBoxDay.Width - 20;
                    comboBoxDay.Top = buttonLoad.Top + 1;

                    comboBoxTime.Left = buttonSave.Left - comboBoxTime.Width - 20;
                    comboBoxTime.Top = buttonSave.Top + 1;

                    comboBoxType.Left = buttonEnd.Left - comboBoxType.Width - 20;
                    comboBoxType.Top = buttonEnd.Top + 1;
                    break;
                case 1280:
                    listBox.Left = this.Width - 150;
                    listBox.Top = 0;
                    listBox.Height = Height;
                    #region buttonLoad
                    buttonLoad.Width = btnStudents[1].Width;
                    buttonLoad.Height = btnStudents[1].Height;
                    buttonLoad.Left = listBox.Left - buttonLoad.Width - 10;
                    buttonLoad.Top = 350;
                    #endregion               
                    #region buttonSave
                    buttonSave.Width = btnStudents[1].Width;
                    buttonSave.Height = btnStudents[1].Height;
                    buttonSave.Left = listBox.Left - buttonSave.Width - 10;
                    buttonSave.Top = 400;
                    #endregion
                    #region buttonEnd
                    buttonEnd.Width = btnStudents[1].Width;
                    buttonEnd.Height = btnStudents[1].Height;
                    buttonEnd.Left = listBox.Left - buttonEnd.Width - 10;
                    buttonEnd.Top = 450;
                    #endregion
                    comboBoxDay.Left = buttonLoad.Left - comboBoxDay.Width - 20;
                    comboBoxDay.Top = buttonLoad.Top + 1;

                    comboBoxTime.Left = buttonSave.Left - comboBoxTime.Width - 20;
                    comboBoxTime.Top = buttonSave.Top + 1;

                    comboBoxType.Left = buttonEnd.Left - comboBoxType.Width - 20;
                    comboBoxType.Top = buttonEnd.Top + 1;
                    break;

                case 3840:
                default: break;
            }

            if (Tools.tDEBUG)
            {
                FrmAdmin frmAdmin = new FrmAdmin();
                frmAdmin.Show();
            }
        }
        private void FrmMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (!Tools.isFormAdminActive)
                {
                    FrmAdmin frmAdmin = new FrmAdmin();
                    frmAdmin.Show();
                    Tools.isFormAdminActive = true;
                }


            }
            if (e.Button == MouseButtons.Left)
            {
                mov = true;
                xpos = MousePosition.X;//鼠标的x坐标为当前窗体左上角x坐标
                ypos = MousePosition.Y;//鼠标的y坐标为当前窗体左上角y坐标
            }

        }
        private void FrmMain_MouseMove(object sender, MouseEventArgs e)
        {

            if (mov)
            {
                this.Left += MousePosition.X - xpos;//根据鼠标x坐标确定窗体的左边坐标x
                this.Top += MousePosition.Y - ypos;//根据鼠标的y坐标窗体的顶部，即Y坐标
                xpos = MousePosition.X;
                ypos = MousePosition.Y;
            }
        }
        private void FrmMain_MouseUp(object sender, MouseEventArgs e)
        {
            mov = false;//停止移动

        }
    }

}
