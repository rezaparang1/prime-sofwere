using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1.Product
{
    public partial class ShortKey : Form
    {
        private DTO.Product.ProductDto CurrentProduct;
        public ShortKey(DTO.Product.ProductDto product)
        {
            InitializeComponent();
            CurrentProduct = product;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            var btn = sender as Guna.UI2.WinForms.Guna2Button;
            if (btn != null)
            {
                if (!string.IsNullOrWhiteSpace(txtshortkey.Text))
                {
                    txtshortkey.Text += " + ";
                }

                txtshortkey.Text += btn.Text;
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            var btn = sender as Guna.UI2.WinForms.Guna2Button;
            if (btn != null)
            {
                if (!string.IsNullOrWhiteSpace(txtshortkey.Text))
                {
                    txtshortkey.Text += " + ";
                }

                txtshortkey.Text += btn.Text;
            }
        }

        private void guna2Button17_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2Button16_Click(object sender, EventArgs e)
        {
            txtshortkey.Text = CurrentProduct.ShortcutKey;
            this.Close();
        }

        private void ShortKey_Load(object sender, EventArgs e)
        {
            txtshortkey.Text = CurrentProduct.ShortcutKey;
        }
    }
}
