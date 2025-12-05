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
    public partial class Storeroom_Product : Form
    {
        public Storeroom_Product()
        {
            InitializeComponent();
        }

        private void guna2ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {
            grOperation.Visible = true;
            grSearch.Visible = false;
            guna2Panel1.BackColor = ColorTranslator.FromHtml("#374151");
            guna2Panel2.BackColor = ColorTranslator.FromHtml("#111827");
        }

        private void guna2Panel1_Click(object sender, EventArgs e)
        {
            grOperation.Visible = true;
            grSearch.Visible = false;
            guna2Panel1.BackColor = ColorTranslator.FromHtml("#374151");
            guna2Panel2.BackColor = ColorTranslator.FromHtml("#111827");
        }

        private void label27_Click(object sender, EventArgs e)
        {
            grOperation.Visible = false;
            grSearch.Visible = true;
            guna2Panel1.BackColor = ColorTranslator.FromHtml("#111827");
            guna2Panel2.BackColor = ColorTranslator.FromHtml("#374151");
        }

        private void guna2Panel2_Click(object sender, EventArgs e)
        {
            grOperation.Visible = false;
            grSearch.Visible = true;
            guna2Panel1.BackColor = ColorTranslator.FromHtml("#111827");
            guna2Panel2.BackColor = ColorTranslator.FromHtml("#374151");
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
