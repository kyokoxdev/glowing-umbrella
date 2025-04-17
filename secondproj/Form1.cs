using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace secondproj
{
    public partial class Form1: Form
    {
        private List<Button> DSgheDuocChon = new List<Button>();

        private Button gheDuocChon = null;

        private decimal tongTien = 0;

        private Dictionary<Button, Color> loaigheBanDau = new Dictionary<Button, Color>();

        private Dictionary<string, List<string>> gheDaMua = new Dictionary<string, List<string>>();
        public Form1()
        {
            InitializeComponent();

            foreach (Control control in flowLayoutPanel1.Controls)
            {
                if (control is Button)
                {
                    Button btn = (Button)control;
                    loaigheBanDau.Add(btn, btn.BackColor);
                    btn.MouseHover += new EventHandler(onBtnHover);
                }
            }

            foreach (var film in comboBox1.Items)
            {
                gheDaMua[film.ToString()] = new List<string>();
            }
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn phim trước.", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Button ghe = (Button)sender;
            string phim = comboBox1.SelectedItem.ToString();

            // Check if seat is already purchased for this film
            if (gheDaMua[phim].Contains(ghe.Name))
            {
                MessageBox.Show("Không thể mua ghế này.", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Toggle seat selection
            if (DSgheDuocChon.Contains(ghe))
            {
                // Deselect the seat
                DSgheDuocChon.Remove(ghe);
                ghe.BackColor = loaigheBanDau[ghe];

                // Subtract price from total
                if (decimal.TryParse(TinhGiaVe(ghe).Replace(",", "").Replace(" VND", ""), out decimal giaGhe))
                {
                    tongTien -= giaGhe;
                }
            }
            else
            {
                // Select the seat
                DSgheDuocChon.Add(ghe);
                ghe.BackColor = Color.Green;

                // Add price to total
                if (decimal.TryParse(TinhGiaVe(ghe).Replace(",", "").Replace(" VND", ""), out decimal giaGhe))
                {
                    tongTien += giaGhe;
                }
            }

            // Update total display
            UpdateTotalDisplay();
        }
        private void UpdateTotalDisplay()
        {
            if (comboBox1.SelectedIndex < 0)
            {
                return;
            }
            string phim = comboBox1.SelectedItem.ToString();
            foreach (Control control in flowLayoutPanel1.Controls)
            {
                if (control is Button btn)
                {
                    btn.BackColor = loaigheBanDau[btn];
                }
            }
            foreach (string seatName in gheDaMua[phim])
            {
                Control[] matchingControls = flowLayoutPanel1.Controls.Find(seatName, false);
                if (matchingControls.Length > 0 && matchingControls[0] is Button gheDaMua)
                {
                    gheDaMua.BackColor = Color.Red;
                }
            }
            foreach (Button btn in DSgheDuocChon)
            {
                btn.BackColor = Color.Green;
            }
            label4.Text = tongTien.ToString("N0") + "VND";
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void onBtnHover(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            toolTip1.UseAnimation = true;
            toolTip1.ToolTipTitle = "Giá vé của hàng ghế này là:";
            toolTip1.ToolTipIcon = ToolTipIcon.Info;
            toolTip1.IsBalloon = true;
            toolTip1.SetToolTip(btn, TinhGiaVe(btn));
        }

        private string TinhGiaVeTheoMau(Color color)
        {
            if (color == Color.Cyan)
                return "25,000 VND";
            else if (color == Color.LightGreen)
                return "30,000 VND";
            else if (color == Color.LemonChiffon)
                return "35,000 VND";
            else if (color == Color.Violet)
                return "40,000 VND";
            else if (color == Color.Salmon)
                return "50,000 VND";
            else if (color == Color.MistyRose)
                return "45,000 VND";
            else
                return "FREE";
        }

        private string TinhGiaVe(Button btn)
        { 
            if (btn.BackColor == Color.Cyan)
                return "25,000 VND";
            else if (btn.BackColor == Color.LightGreen)
                return "30,000 VND";
            else if (btn.BackColor == Color.LemonChiffon)
                return "35,000 VND";
            else if (btn.BackColor == Color.Violet)
                return "40,000 VND";
            else if (btn.BackColor == Color.Salmon)
                return "50,000 VND";
            else if (btn.BackColor == Color.MistyRose)
                return "45,000 VND";
            else if (btn.BackColor == Color.Green)  // Return the original price for selected seats
            {
                foreach (var kvp in loaigheBanDau)
                {
                    if (kvp.Key == btn)
                        return TinhGiaVeTheoMau(kvp.Value);
                }
                return "FREE";
            }
            else if (btn.BackColor == Color.Red)  // For purchased seats
                return "SOLD";
            else
                return "FREE";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label2.Text = "Mua vé xem phim: " + comboBox1.SelectedItem.ToString();
            DSgheDuocChon.Clear();
            tongTien = 0;
            UpdateTotalDisplay();
        }

        private void button31_Click(object sender, EventArgs e)
        {
            if (DSgheDuocChon.Count == 0)
            {
                MessageBox.Show("Please select at least one seat.", "No Seats Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (comboBox1.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a film first.", "Selection Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string currentFilm = comboBox1.SelectedItem.ToString();

            // Confirm purchase
            DialogResult result = MessageBox.Show(
                $"Do you want to confirm the purchase of {DSgheDuocChon.Count} seat(s) for {tongTien:N0} VND?",
                "Confirm Purchase",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Mark seats as purchased
                foreach (Button seat in DSgheDuocChon)
                {
                    seat.BackColor = Color.Red;
                    gheDaMua[currentFilm].Add(seat.Name);
                }

                // Clear selection
                DSgheDuocChon.Clear();
                tongTien = 0;
                UpdateTotalDisplay();

                MessageBox.Show("Purchase completed successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
