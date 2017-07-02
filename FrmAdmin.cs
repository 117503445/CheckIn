using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace CheckIn
{
    public partial class FrmAdmin : Form
    {
        public FrmAdmin()
        {
            InitializeComponent();
        }
        private void FrmAdmin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Tools.isFormAdminActive = false;

        }
        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            OleDbConnection conn = new OleDbConnection(@"Provider = Microsoft.Jet.OLEDB.4.0; Data Source =" + Tools.databasePath);
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

            foreach (DataRow myRow in myTable.Rows)
            {
                int score = 0;
                for (int i = 3; i < myTable.Columns.Count; i++)
                {
                    if ((bool)myRow[i]) { score++; }
                }
                myRow[2] = (double)score / 10;
            }
            OleDbCommandBuilder myOleDeCommandBuilder = new OleDbCommandBuilder(myDataAdapter);
            myDataAdapter.Update(myDataSet, "Students");

            myDataSet.Dispose();        // 释放DataSet对象
            myDataAdapter.Dispose();    // 释放SqlDataAdapter对象
            conn.Close();             // 关闭数据库连接
            conn.Dispose();           // 释放数据库连接对象
            listBoxOutput.Items.Add("计算分数" );
        }
        private void buttonSetFalse_Click(object sender, EventArgs e)
        {
            databaseSet(false);
        }
        private void buttonSetTrue_Click(object sender, EventArgs e)
        {
            databaseSet(true);
        }
        private void databaseSet(bool isTrue)
        {

            OleDbConnection conn = new OleDbConnection(@"Provider = Microsoft.Jet.OLEDB.4.0; Data Source =" + Tools.databasePath);
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

            foreach (DataRow myRow in myTable.Rows)
            {
                for (int i = 3; i < myTable.Columns.Count; i++)
                {
                    myRow[i] = isTrue;
                }
            }
            OleDbCommandBuilder myOleDeCommandBuilder = new OleDbCommandBuilder(myDataAdapter);
            myDataAdapter.Update(myDataSet, "Students");

            myDataSet.Dispose();        // 释放DataSet对象
            myDataAdapter.Dispose();    // 释放SqlDataAdapter对象
            conn.Close();             // 关闭数据库连接
            conn.Dispose();           // 释放数据库连接对象
            listBoxOutput.Items.Add("设置数据库所有数据为：" + isTrue);
        }
        private void buttonMoveSeats_Click(object sender, EventArgs e)
        {
            string sourcePath = System.Environment.CurrentDirectory+@"\file\txt\students.txt";
            string targetPath= System.Environment.CurrentDirectory + @"\file\txt\students备份.txt";

            System.IO.File.Copy(sourcePath, targetPath,true);
            string[] strOutput=new string[Tools.studentData.Length];
            int j = 0;
            foreach (string i in Tools.studentData) {
                
                int row = Int32.Parse(i.Substring(0,1));
                row += 2;
                if (row > 8)
                {
                    row -= 8;
                }
                strOutput[j] = row.ToString() + i.Substring(1,i.Length-1);
                j++;
            }

            System.IO.File.WriteAllLines(sourcePath,strOutput, Encoding.Default);
            listBoxOutput.Items.Add("更换座位");
        }
        private void buttonOneKey_Click(object sender, EventArgs e)
        {
            listBoxOutput.Items.Add("这周是" + textBoxGetWeek.Text + "周");
            buttonCalculate_Click(new object(), new EventArgs());
            string targetPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) +@"\"+ textBoxGetWeek.Text + "周签到数据.mdb";
            System.IO.File.Copy(Tools.databasePath, targetPath);
            listBoxOutput.Items.Add("输出至"+ targetPath);
            buttonSetFalse_Click(new object(), new EventArgs());
            buttonCalculate_Click(new object(), new EventArgs());
            listBoxOutput.Items.Add("归档已完成");
        }
    }
}
