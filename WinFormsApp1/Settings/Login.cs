using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1.Settings
{
    public partial class Login : Form
    {
        private class1.Settings.Login _client;
        public Login()
        {
            InitializeComponent();
            _client = new class1.Settings.Login();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private async void guna2Button1_Click(object sender, EventArgs e)
        {
            guna2Button1.Enabled = false;
            try
            {
                var result = await _client.LoginAsync(guna2TextBox1.Text.Trim(), guna2TextBox2.Text);
                if (result == null)
                {
                    MessageBox.Show("نام کاربری یا رمز عبور نادرست است.", "خطا",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                new Form1().Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطا در اتصال به سرور:\n{ex.Message}", "خطا",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                guna2Button1.Enabled = true;
            }
        }
    }
}
