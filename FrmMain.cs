using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace CheckIn
{
    public partial class FrmMain : Form
    {
        public string filedsName = "";//例如 周四早眼
        
        public Students[] students = new Students[51];
        public Button[] btnStudents = new Button[51];//学生按钮
        Point mouseOff;//鼠标移动位置变量
        bool leftFlag;//标签是否为左键
        public FrmMain()
        {
            InitializeComponent();
            #region 初始化
            for (int i = 0; i < students.Length; i++) {
                students[i] = new Students();
                btnStudents[i] = new Button();
                this.Controls.Add(btnStudents[i]);//注册按钮
                btnStudents[i].Visible = false;
                btnStudents[i].BackColor = Color.AliceBlue;
                btnStudents[i].Width = 250;
                btnStudents[i].Height = 120;
                btnStudents[i].Font = new Font(btnStudents[i].Font.FontFamily, 17, btnStudents[i].Font.Style); 
                btnStudents[i].FlatStyle = System.Windows.Forms.FlatStyle.Popup;//Style
                btnStudents[i].Click += new System.EventHandler(this.btnStudents_Click);//注册单击事件
            }
            #endregion
            #region 读取students.txt
         string[] studentData;
        studentData = System.IO.File.ReadAllLines(System.Environment.CurrentDirectory +
                "/file/txt/students.txt", Encoding.Default);//读取students.txt
            System.Console.WriteLine("读取students.txt");
            foreach (string i in studentData)
            {
                int id = Int32.Parse(i.Substring(6, 2));
                students[id].Name = i.Substring(2, 4);
                if (students[id].Name.Substring(3,1)!=" ") { btnStudents[id].Font = new Font(btnStudents[id].Font.FontFamily, 13, btnStudents[id].Font.Style); }
                students[id].Enabled = true;
                students[id].PositionX = Int32.Parse( i.Substring(1,1));
                students[id].PositionY = Int32.Parse(i.Substring(0,1));
                btnStudents[id].Text = students[id].Name;
                btnStudents[id].Visible = true;
                
                //TEMP
                btnStudents[id].Top = 130 * students[id].PositionX;
                btnStudents[id].Left = 260 * students[id].PositionY;

            }
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
            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;
            int hourint = currentTime.Hour;
            switch (hourint)
            {
                case 6: comboBoxTime.Text = "早"; comboBoxType.Text = "读"; comboBoxType.Items.Add("读"); break;
                case 10: comboBoxTime.Text = "早"; comboBoxType.Text = "眼"; comboBoxType.Items.Add("读"); break;
                case 12: comboBoxTime.Text = "午"; comboBoxType.Text = "休"; comboBoxType.Items.Add("休"); break;
                case 15: comboBoxTime.Text = "午"; comboBoxType.Text = "眼"; comboBoxType.Items.Add("休"); break;
                case 5: comboBoxTime.Text = "晚"; comboBoxType.Text = "修"; comboBoxType.Items.Add("修"); break;
                case 8: comboBoxTime.Text = "晚"; comboBoxType.Text = "眼"; comboBoxType.Items.Add("修"); break;
                default: listBox.Items.Add("[Warn]奇怪的时间"); break;
            }
            comboBoxType.Items.Add("眼");

            #endregion

        }
        private void btnStudents_Click(object sender, System.EventArgs e)
        {
            String[] arrs = ((Button)sender).Text.Split('\n');
            int ID = 0;
            for (int i = 0; i < students.Length; i++)
            {
                if (arrs[0] == students[i].Name) { ID = i; break; }//查找学号
            }
            Console.WriteLine("当前学号:" + ID);
            Console.WriteLine("当前姓名"+students[ID].Name);
            Console.WriteLine("当前颜色" + ((Button)sender).BackColor);
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

            Console.WriteLine("当前签到情况" + students[ID].IsCheck); 

            string[] tempstr = new string[students.Length];

            for (int i = 0; i < students.Length; i++)
            {
                tempstr[i] = students[i].color.ToString();
                
            }
            System.IO.File.WriteAllLines(System.Environment.CurrentDirectory + "/file/txt/temp.txt", tempstr);


        }       
        private void comboBoxTime_SelectedIndexChanged(object sender, EventArgs e)


        {
            comboBoxType.Items.Clear();

            switch (comboBoxTime.Text)
            {

                case "早": comboBoxType.Items.Add("读"); comboBoxType.Text = "读"; break;
                case "午": comboBoxType.Items.Add("休"); comboBoxType.Text = "休"; break;
                case "晚": comboBoxType.Items.Add("修"); comboBoxType.Text = "修"; break;

            }

            comboBoxType.Items.Add("眼");

        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            filedsName = comboBoxDay.Text + comboBoxTime.Text + comboBoxType.Text;
            Console.WriteLine("当前fieldsname" + filedsName);
            listBox.Items.Clear();
            for (int i = 0; i < students.Length;i++)
            {
                if (students[i].Enabled & !students[i].IsCheck) { listBox.Items.Add(i+students[i].Name); }
            }

            OleDbConnection conn = new OleDbConnection(@"Provider = Microsoft.Jet.OLEDB.4.0; Data Source ="+ System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+@"file/mdb/log.mdb");
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
                if ((bool)myrow[filedsName] == true) { isChecked = true;break; }
            }
            if (isChecked)
            {
                DialogResult receiver = MessageBox.Show("在" + filedsName + "中检测到已有的数据,是否继续签到？", "疑惑", MessageBoxButtons.OKCancel);
                if (receiver == DialogResult.OK) {
                    Console.WriteLine("用户OK");       
                } else
                {
                    Console.WriteLine("用户NO");
                    listBox.Items.Clear();
                    listBox.Items.Add("取消成功");
                    return;
                }}
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
            listBox.Items.Add("成功签到");
            string[] strBackups;
            strBackups = System.IO.File.ReadAllLines(System.Environment.CurrentDirectory +
                    "/file/txt/backup.txt", Encoding.Default);//读取backup.txt
        }

        private void buttonEnd_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }
        private void buttonLoad_Click(object sender, EventArgs e)
        {
            string[] tempReceiver = System.IO.File.ReadAllLines(System.Environment.CurrentDirectory + "/file/txt/temp.txt", Encoding.Default);
            for (int i = 0; i < students.Length; i++)
            {
                string[] tempstr = tempReceiver[i].Split(';');
                Console.WriteLine(tempstr[0]);
                switch (tempstr[0])
                {
                    case "0": btnStudents[i].BackColor = Color.AliceBlue; students[i].IsCheck= true; break;
                    case "1": btnStudents[i].BackColor = Color.Orange; students[i].IsCheck = false; break;
                    case "2": btnStudents[i].BackColor = Color.LightSteelBlue; students[i].IsCheck = true; break;
                }

            }

        }
        private void FrmMain_Load(object sender, EventArgs e)
        {
           Top = Screen.PrimaryScreen.Bounds.Height * 2 / 9;
            Left = Screen.PrimaryScreen.Bounds.Width /8;
        }
        private void FrmMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseOff = new Point(-e.X, -e.Y); //得到变量的值
                leftFlag = true;                  //点击左键按下时标注为true;
            }
        }
        private void FrmMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                Point mouseSet = Control.MousePosition;
                mouseSet.Offset(mouseOff.X, mouseOff.Y);  //设置移动后的位置
                Location = mouseSet;
            }
        }
        private void FrmMain_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                leftFlag = false;//释放鼠标后标注为false;
            }
        }
    }
    public class Students {
        private string name = "";
        private int positionX = 0;
        private int positionY = 0;
        public int color = 0;
        private bool isCheck = true;
        private bool enabled = false;
        public string Name { get => name; set => name = value; }
        public int PositionX { get => positionX; set => positionX = value; }
        public int PositionY { get => positionY; set => positionY = value; }

        public bool IsCheck { get => isCheck; set => isCheck = value; }
        public bool Enabled { get => enabled; set => enabled = value; }
    }
}
