using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckIn
{
    public partial class FrmMain : Form
    {
        public string[] studentData;

        public string[] studentName = new string[89];//姓名
        public int[] studentID = new int[89];//学号
        public bool[] studentQd = new bool[89];//签到
        public string filedsName = "";//例如 周四早眼
        public int[] studentColor = new int[89];//0 为白色，1为红色，2为灰色
        public Button[] btnStudents = new Button[89];//学生们
        public FrmMain()
        {
            InitializeComponent();
            #region 初始化
            for (int i = 0; i < 89; i++) { studentQd[i] = true; btnStudents[i] = new Button(); this.Controls.Add(btnStudents[i]); btnStudents[i].Visible = false;
                btnStudents[i].BackColor = Color.AliceBlue;
            }
            #endregion
            #region LoadStudent
            int Top = 0;
            int left = 0;
            for (int i = 0; i < 9; i++)
            {
                Top = 0;

                for (int j = 0; j < 9; j++)
                {
                    btnStudents[10 * i + j].Top = Top;
                    btnStudents[10 * i + j].Left = left;
                    btnStudents[10 * i + j].FlatStyle = System.Windows.Forms.FlatStyle.Popup;//Style
                    btnStudents[10 * i + j].Click += new System.EventHandler(this.btnStudents_Click);//注册单击事件
                    Top += 30;
                }
                left += 80;
            }
            #endregion

            #region 读取students.txt
            studentData = System.IO.File.ReadAllLines(System.Environment.CurrentDirectory + "/file/txt/students.txt", Encoding.Default);
            System.Console.WriteLine("读取students.txt");
            foreach (string i in studentData)
            {
                Console.WriteLine(i);
                int index = Int32.Parse(i.Substring(0, 2));
                studentName[index] = i.Substring(2, 4);
                studentID[index] = Int32.Parse(i.Substring(6, 2));
                btnStudents[index].Text = studentName[index];
                btnStudents[index].Visible = true;
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
            System.DateTime currentTime = new System.DateTime();
            int hourint = currentTime.Hour;
            switch (hourint)
            {
                case (6): comboBoxTime.Text = "早"; comboBoxType.Text = "读"; break;
                case (10): comboBoxTime.Text = "早"; comboBoxType.Text = "眼"; break;
                case (12): comboBoxTime.Text = "午"; comboBoxType.Text = "休"; break;
                case (15): comboBoxTime.Text = "午"; comboBoxType.Text = "眼"; break;
                case (5): comboBoxTime.Text = "晚"; comboBoxType.Text = "修"; break;
                case (8): comboBoxTime.Text = "晚"; comboBoxType.Text = "眼"; break;
                default: listBox.Items.Add("[Warn]奇怪的时间"); break;
            }

            #endregion

        }

        private void btnStudents_Click(object sender, System.EventArgs e)
        {
            String[] arrs = ((Button)sender).Text.Split('\n');
            int ID = 0;
            for (int i = 0; i < 89; i++)
            {
                if (arrs[0] == studentName[i]) { ID = i; break; }//查找学号
            }
            Console.WriteLine("当前位置:" + ID);
            Console.WriteLine("当前颜色" + ((Button)sender).BackColor);
            if (((Button)sender).BackColor == Color.Orange)
            {
                ((Button)sender).BackColor = Color.LightSteelBlue;
                studentColor[ID] = 2;
                studentQd[ID] = true;
            }
            else if (studentQd[ID] == true & ((Button)sender).BackColor == Color.LightSteelBlue)
            {
                ((Button)sender).BackColor = Color.AliceBlue;
                studentColor[ID] = 0;
            }
            else
            {
                ((Button)sender).BackColor = Color.Orange;
                studentColor[ID] = 1;
                studentQd[ID] = false;
            }
            Console.WriteLine("当前签到情况" + studentQd[ID]);

            string[] tempstr = new string[89];
            for (int i = 0; i < 89; i++)
            {
                tempstr[i] = studentColor[i].ToString();
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

            for (int j = 0; j < 49; j++)
            {
                for (int i = 0; i < 89; i++)
                {
                    if (studentID[i] != 0 & !studentQd[i] & studentID[i] == j) { listBox.Items.Add(studentID[i] + studentName[i]); break; }
                }
            }
        }

        private void buttonEnd_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            string[] tempReceiver = System.IO.File.ReadAllLines(System.Environment.CurrentDirectory + "/file/txt/temp.txt", Encoding.Default);
            for (int i = 0; i < 89; i++)
            {
                string[] tempstr = tempReceiver[i].Split(';');
                Console.WriteLine(tempstr[0]);
                switch (tempstr[0])
                {
                    case "0": btnStudents[i].BackColor = Color.AliceBlue; studentQd[i] = true; break;
                    case "1": btnStudents[i].BackColor = Color.Orange; studentQd[i] = false; break;
                    case "2": btnStudents[i].BackColor = Color.LightSteelBlue; studentQd[i] = true; break;
                }

            }

        }
    }

}
