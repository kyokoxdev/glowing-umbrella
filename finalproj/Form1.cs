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
    public partial class Form1: Form
    {

        public class sinhVien
        {
            public string ten { get; set; }
            public string maSV { get; set; }
            public DateTime ngaysinh { get; set; }
            public string noisinh { get; set; }
            public bool gioitinh {  get; set; }
        }

        private List<sinhVien> DSSV = new List<sinhVien>();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Vui lòng nhập tên sinh viên", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Vui lòng nhập mã sinh viên", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (comboBox1.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng nhập nơi sinh", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dateTimePicker1.Value > DateTime.Now)
            {
                MessageBox.Show("Ngày sinh không hợp lệ", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            sinhVien sv = new sinhVien();
            sv.ten = textBox1.Text;
            sv.maSV = textBox2.Text;
            sv.ngaysinh = dateTimePicker1.Value;
            sv.noisinh = comboBox1.SelectedItem.ToString();
            sv.gioitinh = radioButton1.Checked;

            DSSV.Add(sv);
            UpdateDGV();
        }

        private void UpdateDGV()
        {
            dataGridView1.Rows.Clear();
            foreach (var sv in DSSV)
            {
                int index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells[0].Value = sv.ten;
                dataGridView1.Rows[index].Cells[1].Value = sv.maSV;
                dataGridView1.Rows[index].Cells[2].Value = sv.ngaysinh.ToString("dd/MM/yyyy");
                dataGridView1.Rows[index].Cells[3].Value = sv.noisinh;
                dataGridView1.Rows[index].Cells[4].Value = (sv.gioitinh) ? "Nam" : "Nữ";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                row.Cells[0].Value = textBox1.Text;
                row.Cells[1].Value = textBox2.Text;
                row.Cells[2].Value = dateTimePicker1.Value.ToString("dd/MM/yyyy");
                row.Cells[3].Value = comboBox1.SelectedItem.ToString();
                row.Cells[4].Value = (radioButton1.Checked) ? "Nam" : "Nữ";
                int index = row.Index;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<int> indicesToRemove = new List<int>();
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                if (row.Index >= 0 && row.Index < DSSV.Count)
                {
                    indicesToRemove.Add(row.Index);
                }
            }

            // Remove the items from the DSdonHang list in reverse order
            indicesToRemove.Sort();
            indicesToRemove.Reverse();
            foreach (int index in indicesToRemove)
            {
                DSSV.RemoveAt(index);
            }

            // Update the DataGridView
            UpdateDGV();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                textBox1.Text = row.Cells[0].Value.ToString();
                textBox2.Text = row.Cells[1].Value.ToString();
                dateTimePicker1.Value = DateTime.Parse(row.Cells[2].Value.ToString());
                comboBox1.SelectedItem = row.Cells[3].Value.ToString();
                if (row.Cells[4].Value.ToString() == "Nam")
                {
                    radioButton1.Checked = true;
                    radioButton2.Checked = false;
                }
                else
                {
                    radioButton2.Checked = true;
                    radioButton1.Checked = false;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            form2.FormClosing += (s, eArgs) =>
            {

                    string filterID = form2.filterID;
                    string filterPOB = form2.filterPOB;
                    bool filterGD = form2.filterGD;
                    var filteredList = DSSV.Where(sv =>
                        (string.IsNullOrEmpty(filterID) || sv.maSV.Contains(filterID)) &&
                        (string.IsNullOrEmpty(filterPOB) || sv.noisinh == filterPOB) &&
                        (filterGD == true || sv.gioitinh == filterGD)).ToList();
                    dataGridView1.Rows.Clear();
                    foreach (var sv in filteredList)
                    {
                        int index = dataGridView1.Rows.Add();
                        dataGridView1.Rows[index].Cells[0].Value = sv.ten;
                        dataGridView1.Rows[index].Cells[1].Value = sv.maSV;
                        dataGridView1.Rows[index].Cells[2].Value = sv.ngaysinh.ToString("dd/MM/yyyy");
                        dataGridView1.Rows[index].Cells[3].Value = sv.noisinh;
                        dataGridView1.Rows[index].Cells[4].Value = (sv.gioitinh) ? "Nam" : "Nữ";
                    }
                
            };
        }
    }
}
