using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace thirdproj
{
    public partial class Form1: Form
    {
        public class donHang
        {
            public string tenKhach { get; set; }
            public string tenhang { get; set; }
            public int soluong { get; set; }
            public decimal dongia { get; set; }
            public decimal thanhTien { get; set; }
        }

        private List<donHang> DSdonHang = new List<donHang>();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            textBox1.Text = "";
            textBox2.Text = "";
            numericUpDown1.Value = 1;
            comboBox1.SelectedIndex = -1;
            textBox3.Text = "";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    textBox2.Text = "50,000đ";
                    break;
                case 1:
                    textBox2.Text = "70,000đ";
                    break;
                case 2:
                    textBox2.Text = "20,000đ";
                    break;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng.", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (comboBox1.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn hàng hóa.", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (numericUpDown1.Value <= 0)
            {
                MessageBox.Show("Vui lòng nhập số lượng lớn hơn 0.", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string tenKhach = textBox1.Text;
            string tenhang = comboBox1.SelectedItem.ToString();
            int soluong = (int)numericUpDown1.Value;
            decimal dongia = decimal.Parse(textBox2.Text.Replace("đ", "").Replace(",", ""));
            decimal thanhTien = soluong * dongia;

            donHang dh = new donHang
            {
                tenKhach = tenKhach,
                tenhang = tenhang,
                soluong = soluong,
                dongia = dongia,
                thanhTien = soluong * dongia
            };

            DSdonHang.Add(dh);
            UpdateDGV();
        }

        private void UpdateDGV()
        {
            int noOfcells = 0;
            dataGridView1.Rows.Clear();
            foreach(donHang dh in DSdonHang)
            {
                int index = dataGridView1.Rows.Add();
                DataGridViewRow row = dataGridView1.Rows[index];
                row.Cells["STT"].Value = ++noOfcells;
                row.Cells["tenhang"].Value = dh.tenhang;
                row.Cells["soluong"].Value = dh.soluong;
                row.Cells["dongia"].Value = dh.dongia.ToString("N0") + "đ";
                row.Cells["thanhTien"].Value = dh.thanhTien.ToString("N0") + "đ";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            List<int> indicesToRemove = new List<int>();
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                if (row.Index >= 0 && row.Index < DSdonHang.Count)
                {
                    indicesToRemove.Add(row.Index);
                }
            }

            // Remove the items from the DSdonHang list in reverse order
            indicesToRemove.Sort();
            indicesToRemove.Reverse();
            foreach (int index in indicesToRemove)
            {
                DSdonHang.RemoveAt(index);
            }

            // Update the DataGridView
            UpdateDGV();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            decimal tongTien = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["thanhTien"].Value != null)
                {
                    string cellValue = row.Cells["thanhTien"].Value.ToString();
                    cellValue = cellValue.Replace("đ", "").Replace(",", "");
                    if (decimal.TryParse(cellValue, out decimal value))
                    {
                        tongTien += value;
                    }
                }
            }

            textBox3.Text = tongTien.ToString("N0") + "đ";
        }

        private void doiMauToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(colorDialog1.ShowDialog() == DialogResult.OK) {
                Control control = contextMenuStrip1.SourceControl;
                if (control != null)
                {
                    control.BackColor = colorDialog1.Color;
                }
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }
    }
}
