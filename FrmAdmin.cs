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

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void buttonCalculate_Click(object sender, EventArgs e)
        {

        }
    }
}
