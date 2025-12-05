using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1.People
{
    public partial class People : Form
    {
        Random rnd = new Random();
        public People()
        {
            InitializeComponent();
        }
        public bool update = false;
        public int id = 0;
        public void Clear()
        {
            txtname.Text = "";
            txtfamily.Text = "";
            txtphone.Text = "";
            txtcodemeli.Text = "";
            txtlimit.Text = "";
            txtoff.Text = "";
            txtide.Text = "";
            txtaddress.Text = "";
            txtdec.Text = "";
            chdtax.Checked = false;
            guna2CheckBox1.Checked = false;
            guna2CheckBox2.Checked = false;
            guna2CheckBox3.Checked = false;
            guna2CheckBox4.Checked = false;
            guna2CheckBox5.Checked = false;
        }
        private void guna2Button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void guna2Button7_Click(object sender, EventArgs e)
        {
            this.Close();
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
        private void guna2Button8_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void guna2Button9_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (txtname.Text.Trim().Length == 0)
            //    {
            //        label29.Text = "نام نمیتواند خالی باشد . خالی بودن نام باعت ایجاد تداخل در نرم افزار میگردد .";
            //        label29.ForeColor = ColorTranslator.FromHtml("#F15B5B");
            //        label29.Visible = true;
            //    }
            //    else if (txtfamily.Text.Trim().Length == 0)
            //    {
            //        label29.Text = "نام خانوادگی نمیتواند خالی باشد . خالی بودن نام  خانوادگی باعت ایجاد تداخل در نرم افزار میگردد .";
            //        label29.ForeColor = ColorTranslator.FromHtml("#F15B5B");
            //        label29.Visible = true;
            //    }
            //    else
            //    {
            //        if (update == false)
            //        {
            //            string idpeople = "";
            //            if (txtid.Text.Trim().Length ==0 )
            //            {
            //                int num = rnd.Next(100, 1000);

            //                idpeople = Convert.ToString(num);
            //            }
            //            else
            //            {
            //                idpeople = txtid.Text;
            //            }
            //            var async = new DTO.People.People
            //            {
            //                IdPeople = idpeople,

            //            };
            //            var result = await _client1.AddAsync(async);
            //            if (result == "عملیات با موفقیت انجام شد .")
            //            {
            //                label29.Text = result;
            //                label29.ForeColor = ColorTranslator.FromHtml("#A1E3B2");
            //                label29.Visible = true;
            //            }
            //            else
            //            {
            //                label29.Text = result;
            //                label29.ForeColor = ColorTranslator.FromHtml("#F15B5B");
            //                label29.Visible = true;

            //            }
            //            guna2TextBox1.Text = "";
            //            var async1 = await _client1.GetAllAsync();
            //            guna2DataGridView1.DataSource = async1;
            //            DataGrid1();
            //        }
            //        else
            //        {
            //            var async2 = new DTO.Bank.Definition_Bank
            //            {
            //                Name = guna2TextBox1.Text,
            //                Id = id
            //            };
            //            var result = await _client1.UpdateAsync(async2);
            //            if (result == "عملیات با موفقیت انجام شد .")
            //            {
            //                label29.Text = result;
            //                label29.ForeColor = ColorTranslator.FromHtml("#A1E3B2");
            //                label29.Visible = true;
            //            }
            //            else
            //            {
            //                label29.Text = result;
            //                label29.ForeColor = ColorTranslator.FromHtml("#F15B5B");
            //                label29.Visible = true;

            //            }
            //            guna2TextBox1.Text = "";
            //            var async3 = await _client1.GetAllAsync();
            //            guna2DataGridView1.DataSource = async3;
            //            DataGrid1();
            //            id = 0;
            //            update = false;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    guna2MessageDialog1.Icon = MessageDialogIcon.Error;
            //    guna2MessageDialog1.Show($"خطایی در انجام عملیات مورد نظر رخ داده است . {ex.Message}", "نرم افزار حسابداری و انبارداری کارن");
            //}
        }
    }
}
