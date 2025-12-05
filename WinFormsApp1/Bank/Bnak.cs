using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1.Bank
{
    public partial class Bnak : Form
    {
        private class1.Bank.Definition_Bank _client;
        public Bnak()
        {
            InitializeComponent();

            txtheasb.KeyPress += OnlyNumber_KeyPress;
            txtheasb.TextChanged += OnlyNumber_TextChanged;

            txtIdcart.KeyPress += OnlyNumber_KeyPress;
            txtIdcart.TextChanged += OnlyNumber_TextChanged;

            txtphonebranch.KeyPress += OnlyNumber_KeyPress;
            txtphonebranch.TextChanged += OnlyNumber_TextChanged;

            txtidbranch.KeyPress += OnlyNumber_KeyPress;
            txtidbranch.TextChanged += OnlyNumber_TextChanged;

            txtheasbs.KeyPress += OnlyNumber_KeyPress;
            txtheasbs.TextChanged += OnlyNumber_TextChanged;

            txtphonebranchs.KeyPress += OnlyNumber_KeyPress;
            txtphonebranchs.TextChanged += OnlyNumber_TextChanged;

            txtphonebranchs.KeyPress += OnlyNumber_KeyPress;
            txtphonebranchs.TextChanged += OnlyNumber_TextChanged;

            txtidcarts.KeyPress += OnlyNumber_KeyPress;
            txtidcarts.TextChanged += OnlyNumber_TextChanged;

            txtvalue.KeyPress += Price_KeyPress;
            txtvalue.TextChanged += Price_TextChanged;

            _client = new class1.Bank.Definition_Bank();
        }
        public void Clear()
        {
            txtheasb.Text = "";
            txtpeople.Text = "";
            txtIdcart.Text = "";
            txtnamebranch.Text = "";
            txtphonebranch.Text = "";
            txtidbranch.Text = "";
            txtaddress.Text = "";
            txtvalue.Text = "";
            chdcardreder.Checked = false;
            radyes.Checked = false;
            radno.Checked = false;
            radmessage.Checked = false;
        }
        private void Price_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar)) return;
            if (!char.IsDigit(e.KeyChar)) e.Handled = true;
        }
        private void Price_TextChanged(object? sender, EventArgs e)
        {
            if (sender is not Guna2TextBox tb) return;
            int selStart = tb.SelectionStart;

            string raw = Regex.Replace(tb.Text ?? "", @"\D", "");
            if (raw.Length == 0)
            {
                tb.Text = "";
                return;
            }

            if (long.TryParse(raw, out long value))
            {
                tb.TextChanged -= Price_TextChanged;
                tb.Text = string.Format(CultureInfo.InvariantCulture, "{0:N0}", value);
                tb.SelectionStart = Math.Max(tb.Text.Length, selStart);
                tb.TextChanged += Price_TextChanged;
            }

        }
        private void OnlyNumber_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (sender is not Guna.UI2.WinForms.Guna2TextBox tb) return;

            if (char.IsControl(e.KeyChar)) return;

            if (!char.IsDigit(e.KeyChar))
                e.Handled = true;
        }
        private void OnlyNumber_TextChanged(object? sender, EventArgs e)
        {
            if (sender is not Guna.UI2.WinForms.Guna2TextBox tb) return;

            string raw = Regex.Replace(tb.Text ?? "", @"\D", "");
            if (tb.Text != raw)
            {
                int selStart = tb.SelectionStart;
                tb.Text = raw;
                tb.SelectionStart = Math.Min(selStart, tb.Text.Length);
            }
        }
        private void label2_Click(object sender, EventArgs e)
        {
            grOperation.Visible = true;
            grSearch.Visible = false;
            guna2Panel1.BackColor = ColorTranslator.FromHtml("#374151");
            guna2Panel2.BackColor = ColorTranslator.FromHtml("#111827");
        }
        private void label1_Click(object sender, EventArgs e)
        {
            grOperation.Visible = false;
            grSearch.Visible = true;
            guna2Panel1.BackColor = ColorTranslator.FromHtml("#111827");
            guna2Panel2.BackColor = ColorTranslator.FromHtml("#374151");
        }
        private void guna2Panel1_Click(object sender, EventArgs e)
        {
            grOperation.Visible = true;
            grSearch.Visible = false;
            guna2Panel1.BackColor = ColorTranslator.FromHtml("#374151");
            guna2Panel2.BackColor = ColorTranslator.FromHtml("#111827");
        }
        private void guna2Panel2_Click(object sender, EventArgs e)
        {
            grOperation.Visible = false;
            grSearch.Visible = true;
            guna2Panel1.BackColor = ColorTranslator.FromHtml("#111827");
            guna2Panel2.BackColor = ColorTranslator.FromHtml("#374151");
        }
        private async void Bnak_Load(object sender, EventArgs e)
        {
            grOperation.Visible = true;
            grSearch.Visible = false;
            guna2Panel1.BackColor = ColorTranslator.FromHtml("#374151");
            guna2Panel2.BackColor = ColorTranslator.FromHtml("#111827");
            //Bank
            var async1 = await _client.GetAll();
            guna2ComboBox1.DataSource = async1;
            guna2ComboBox1.DisplayMember = "Name";
            guna2ComboBox1.ValueMember = "Id";
            guna2ComboBox1.Font = new Font("Tahoma", 12, FontStyle.Regular);
            guna2ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        private void guna2Button8_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void guna2Button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            txtheasbs.Text = "";
            txtbranchnames.Text = "";
            txtaddresss.Text = "";
            txtidcarts.Text = "";
            txtidbranchs.Text = "";
            txtphonebranchs.Text = "";
        }
        private void guna2Button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void guna2Button9_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }
    }
}
