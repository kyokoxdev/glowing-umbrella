using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testmarkup
{
    public partial class Form1: Form
    {
        public class VeXe
        {
            public string tenKhach { get; set; }
            public string viTriNgoi { get; set; }
            public int tuoiKH { get; set; }
            public decimal tongTien { get; set; }
        }
        
        private List<VeXe> DSveXe = new List<VeXe>();
        private Button gheDuocChon = null;
        private const decimal giaVe = 50000;
        public Form1()
        {
            InitializeComponent();

            foreach (Control control in this.Controls)
            {
                   control.ContextMenuStrip = contextMenuStrip1;
            }
        }

        private void ghe_Click(object sender, EventArgs e)
        {
            Button ghe = (Button)sender;
            if (ghe.BackColor == Color.Red)
            {
                MessageBox.Show("Ghế đã được chọn");
                return;
            }
            if (ghe.BackColor == Color.Orange)
            {
                ghe.BackColor = Color.Transparent;
                gheDuocChon = null;
                return;
            }
            if (gheDuocChon != null)
            {
                if (gheDuocChon.BackColor != Color.Red) {
                    gheDuocChon.BackColor = Color.Transparent;
                }
            }
            ghe.BackColor = Color.Orange;
            gheDuocChon = ghe;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int tuoi = (int)numericUpDown1.Value;
            decimal tongTien = giaVe;

            if (tuoi < 18)
            {
                tongTien *= 0.8m; // giam 20%
            }
            else if (tuoi >= 50)
            {
                tongTien *= 0.5m; // giam 50%
            }
            textBox2.Text = tongTien.ToString("N0") + "VND";
        }

        private void button47_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (gheDuocChon == null)
            {
                MessageBox.Show("Vui lòng chọn vị trí", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string priceText = textBox2.Text.Replace("VND", "").Replace(",", "");
            decimal price = decimal.Parse(priceText);

            VeXe ve = new VeXe
            {
                tenKhach = textBox1.Text,
                tuoiKH = (int)numericUpDown1.Value,
                tongTien = price,
                viTriNgoi = gheDuocChon.Text,
            };

            DSveXe.Add(ve);

            UpdateDataGridView();

            gheDuocChon.BackColor = Color.Red;
            gheDuocChon.Enabled = true;

            textBox1.Text = "";
            numericUpDown1.Value = 1;
            gheDuocChon = null;
            MessageBox.Show("Thanh toán thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateDataGridView()
        {
            dataGridView1.Rows.Clear();
            foreach (var ve in DSveXe)
            {
                int index = dataGridView1.Rows.Add();
                DataGridViewRow row = dataGridView1.Rows[index];
                row.Cells["tenKhach"].Value = ve.tenKhach;
                row.Cells["viTriNgoi"].Value = ve.viTriNgoi;
                row.Cells["tuoiKH"].Value = ve.tuoiKH;
                row.Cells["tongTien"].Value = ve.tongTien.ToString("N0") + "VND";
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Get the control to which the context menu belongs
            if (contextMenuStrip1.SourceControl != null) {
                colorDialog1.AllowFullOpen = true;
                colorDialog1.AnyColor = true;

                // Set initial color from the control's current background
                colorDialog1.Color = contextMenuStrip1.SourceControl.BackColor;

                // Show the dialog and change the color if user clicks OK
                if (colorDialog1.ShowDialog() == DialogResult.OK)
                {
                    // Check if we're changing a selected seat
                    if (contextMenuStrip1.SourceControl is Button button)
                    {
                        // Apply the color to the control
                        button.BackColor = colorDialog1.Color;

                        // Handle selection logic
                        if (button == gheDuocChon && colorDialog1.Color != Color.Orange)
                        {
                            // If we're changing the selected seat to a non-orange color, deselect it
                            gheDuocChon = null;
                        }
                        else if (colorDialog1.Color == Color.Orange)
                        {
                            // If we're changing the color to Orange (selection color)
                            if (gheDuocChon != null && gheDuocChon != button && gheDuocChon.BackColor != Color.Red)
                            {
                                // Clear the previous selection
                                gheDuocChon.BackColor = Color.Transparent;
                            }
                            gheDuocChon = button;
                        }
                    }
                    else
                    {
                        // For non-button controls, just change the color
                        contextMenuStrip1.SourceControl.BackColor = colorDialog1.Color;
                    }
                }
            }
        }
    }
}
