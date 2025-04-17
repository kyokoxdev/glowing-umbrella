using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace finalproj
{
    public partial class Form2: Form
    {
        public string filterID = "";
        public string filterPOB = "";
        public bool filterGD = true;

        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            filterID = textBox1.Text;
            filterPOB = comboBox1.SelectedItem?.ToString() ?? "";
            filterGD = radioButton1.Checked;
            this.Close();
        }
    }
}
