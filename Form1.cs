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
    public partial class Form1 : Form
    {
        public string[] studentData;

        public string[] studentName = new string[89];
        public int[] studentID = new int[89];

        public Form1()
        {
            #region Load
            InitializeComponent();
            Button[] btnStudents = new Button[89];
            int Top = 0;
            int left = 0;
            for (int i = 0; i < 9; i++)
            {
                Top = 0;

                for (int j = 0; j < 9; j++)
                {
                    Console.WriteLine(10 * i + j);
                    btnStudents[10 * i + j] = new Button();
                    btnStudents[10 * i + j].Top = Top;
                    btnStudents[10 * i + j].Left = left;
                    // btnStudents[10*i+j].Text = i.ToString() + j.ToString();
                    btnStudents[10 * i + j].FlatStyle = System.Windows.Forms.FlatStyle.Popup;
                    this.Controls.Add(btnStudents[10 * i + j]);
                    btnStudents[10 * i + j].Click += new System.EventHandler(this.btnStudents_Click);
                    btnStudents[10 * i + j].Visible = false;
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
                studentID[index] = Int32.Parse(i.Substring(0, 2));

                btnStudents[studentID[index]].Text = studentName[index];
                btnStudents[studentID[index]].Visible = true;

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
            // if((button)sender[])
        }
    }

}
