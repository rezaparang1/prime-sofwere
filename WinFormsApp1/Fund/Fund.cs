using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheArtOfDevHtmlRenderer.Adapters;

namespace WinFormsApp1.Fund
{
    public partial class Fund : Form
    {
        private class1.Fund.Fund _client;
        public Fund()
        {
            InitializeComponent();
            _client = new class1.Fund.Fund();

            txtvalue.KeyPress += OnlyNumber_KeyPress;
            txtvalue.TextChanged += OnlyNumber_TextChanged;
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
        int id = 0;
        bool update = false;
        void DataGrid()
        {
            guna2DataGridView1.Columns[1].HeaderText = "نام";
            guna2DataGridView1.Columns[2].HeaderText = "توضیحات";
            //************************************************
            guna2DataGridView1.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //***************************************************************
            guna2DataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //****************************************************
            guna2DataGridView1.Columns[0].Visible = false;
            guna2DataGridView1.Columns[3].Visible = false;
            guna2DataGridView1.Columns[4].Visible = false;
            guna2DataGridView1.Columns[5].Visible = false;
            guna2DataGridView1.Columns[6].Visible = false;
            guna2DataGridView1.Columns[7].Visible = false;
        }
        public void Clear()
        {
            txtname.Text = "";
            txtdes.Text = "";
            txtvalue.Text = "";
            radmessage.Checked = false;
            radno.Checked = false;
            radyes.Checked = false;
        }
        private void guna2Button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void guna2Button3_Click(object sender, EventArgs e)
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
        private void label1_Click(object sender, EventArgs e)
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
        private async void guna2Button9_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtname.Text.Trim().Length == 0)
                {
                    label3.Text = "نام نمیتواند خالی باشد . خالی بودن نام باعت ایجاد تداخل در نرم افزار میگردد .";
                    label3.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                    label3.Visible = true;
                }
                if (txtvalue.Text.Trim().Length == 0)
                {
                    label3.Text = "موجودی نمیتواند خالی باشد . خالی بودن نام باعت ایجاد تداخل در نرم افزار میگردد .";
                    label3.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                    label3.Visible = true;
                }
                else
                {
                    DTO.Fund.NegativeBalancePolicy policy;

                    if (radyes.Checked)
                        policy = DTO.Fund.NegativeBalancePolicy.Yes;
                    else if (radno.Checked)
                        policy = DTO.Fund.NegativeBalancePolicy.No;
                    else if (radmessage.Checked)
                        policy = DTO.Fund.NegativeBalancePolicy.Message;
                    else
                        policy = DTO.Fund.NegativeBalancePolicy.No;
                    if (update == false)
                    {
                        var async = new DTO.Fund.Fund
                        {
                            Name = txtname.Text,
                            Description = txtdes.Text,
                            IsDelete = true,
                            NegativeBalancePolicy = policy,
                            FirstInventory = long.Parse(txtvalue.Text)
                        };
                        var result = await _client.Add(async);
                        if (result.Contains("موفق"))
                        {
                            label3.Text = result;
                            label3.ForeColor = ColorTranslator.FromHtml("#A1E3B2");
                            label3.Visible = true;
                        }
                        else
                        {
                            label3.Text = result;
                            label3.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                            label3.Visible = true;

                        }
                        Clear();
                        var async1 = await _client.GetAll();
                        guna2DataGridView1.DataSource = async1;
                        DataGrid();
                    }
                    else
                    {
                        var async2 = new DTO.Fund.Fund
                        {
                            Name = txtname.Text,
                            Description = txtdes.Text,
                            IsDelete = true,
                            NegativeBalancePolicy = policy,
                        };
                        var result = await _client.Update(async2);
                        if (result.Contains("موفق"))
                        {
                            label3.Text = result;
                            label3.ForeColor = ColorTranslator.FromHtml("#A1E3B2");
                            label3.Visible = true;
                        }
                        else
                        {
                            label3.Text = result;
                            label3.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                            label3.Visible = true;

                        }
                        Clear();
                        var async3 = await _client.GetAll();
                        guna2DataGridView1.DataSource = async3;
                        DataGrid();
                        id = 0;
                        update = false;
                    }
                }
            }
            catch (Exception ex)
            {
                guna2MessageDialog1.Icon = MessageDialogIcon.Error;
                guna2MessageDialog1.Show($"خطایی در انجام عملیات مورد نظر رخ داده است . {ex.Message}", "نرم افزار حسابداری و انبارداری کارن");
            }
        }
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            txtnames.Text = "";
        }
        private async void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txtnames.Text.Trim();
                List<DTO.Fund.Fund> Fund = await _client.SearchAsync(name);
                guna2DataGridView1.DataSource = Fund;
                DataGrid();
            }
            catch (Exception ex)
            {
                guna2MessageDialog1.Icon = MessageDialogIcon.Error;
                guna2MessageDialog1.Show($"خطایی در انجام عملیات مورد نظر رخ داده است . {ex.Message}", "نرم افزار حسابداری و انبارداری کارن");
            }
        }

        private async void guna2Button15_Click(object sender, EventArgs e)
        {
            try
            {
                if (id == 0)
                {
                    label5.Text = "موردی برای حذف وجود ندارد.";
                    label5.Visible = true;
                }
                else
                {
                    guna2MessageDialog1.Buttons = MessageDialogButtons.YesNo;
                    guna2MessageDialog1.Icon = MessageDialogIcon.Warning;
                    DialogResult result = guna2MessageDialog1.Show("آیا این بانک حذف شود ؟", "نرم افزار حسابداری و انبارداری کارن");
                    if (result == DialogResult.Yes)
                    {
                        var result1 = await _client.Delete(id);
                        if (result1 == "عملیات با موفقیت انجام شد .")
                        {
                            label5.Text = result1;
                            label5.ForeColor = ColorTranslator.FromHtml("#A1E3B2");
                            label5.Visible = true;
                        }
                        else
                        {
                            label5.Text = result1;
                            label5.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                            label5.Visible = true;
                        }
                        var async = await _client.GetAll();
                        guna2DataGridView1.DataSource = async;
                        Clear();
                        DataGrid();
                        id = 0;
                        update = false;
                    }
                }
            }
            catch (Exception ex)
            {
                guna2MessageDialog1.Icon = MessageDialogIcon.Error;
                guna2MessageDialog1.Show($"خطایی در انجام عملیات مورد نظر رخ داده است . {ex.Message}", "نرم افزار حسابداری و انبارداری کارن");
            }
        }

        private async void guna2Button14_Click(object sender, EventArgs e)
        {
            try
            {
                if (guna2DataGridView1.SelectedRows.Count > 0)
                {
                    grOperation.Visible = true;
                    grSearch.Visible = false;
                    guna2Panel1.BackColor = ColorTranslator.FromHtml("#374151");
                    guna2Panel2.BackColor = ColorTranslator.FromHtml("#111827");
                    Clear();
                    var async = await _client.GetById(id);
                    txtname.Text = async.Name;
                    txtdes.Text = async.Description;
                    txtvalue.Text = Convert.ToString(txtvalue.Text);
                    update = true;
                }
                else
                {
                    guna2MessageDialog1.Icon = MessageDialogIcon.Error;
                    guna2MessageDialog1.Show("موردی برای ویرایش انتخاب نشده است .", "نرم افزار حسابداری و انبارداری کارن");
                }
            }
            catch (Exception ex)
            {
                guna2MessageDialog1.Icon = MessageDialogIcon.Error;
                guna2MessageDialog1.Show($"خطایی در انجام عملیات مورد نظر رخ داده است . {ex.Message}", "نرم افزار حسابداری و انبارداری کارن");
            }
        }

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                id = (int)(guna2DataGridView1.Rows[e.RowIndex].Cells[0].Value);
            }
            catch (Exception ex)
            {
                guna2MessageDialog1.Icon = MessageDialogIcon.Error;
                guna2MessageDialog1.Show($"خطایی در انجام عملیات مورد نظر رخ داده است . {ex.Message}", "نرم افزار حسابداری و انبارداری کارن");
            }
        }
    }
}
