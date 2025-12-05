using Guna.UI2.WinForms;
using Guna.UI2.WinForms.Suite;
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
//using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WinFormsApp1.Settings
{
    public partial class Basic_Data : Form
    {
        private class1.Bank.Definition_Bank _client1;
        private class1.Product.Type_Product _client2;
        private class1.Product.Section_Product _client3;
        private class1.Product.Unit_Product _client4;
        private class1.Product.PriceLevels _client5;
        private class1.Product.Group_Product _client6;
        private class1.People.Group_People _client7;
        private class1.People.Type_People _client8;
        private class1.Settings.Group_User _client9;
        public Basic_Data()
        {
            InitializeComponent();
            _client1 = new class1.Bank.Definition_Bank();
            _client2 = new class1.Product.Type_Product();
            _client3 = new class1.Product.Section_Product();
            _client4 = new class1.Product.Unit_Product();
            _client5 = new class1.Product.PriceLevels();
            _client6 = new class1.Product.Group_Product();
            _client7 = new class1.People.Group_People();
            _client8 = new class1.People.Type_People();
            _client9 = new class1.Settings.Group_User();
        }

        void DataGrid1()
        {
            guna2DataGridView1.Columns[1].HeaderText = "نام";
            //************************************************
            guna2DataGridView1.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //***************************************************************
            guna2DataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //****************************************************
            guna2DataGridView1.Columns[0].Visible = false;
           
        }
        void DataGrid2()
        {
            guna2DataGridView2.Columns[1].HeaderText = "نام";
            //************************************************
            guna2DataGridView2.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //***************************************************************
            guna2DataGridView2.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //****************************************************
            guna2DataGridView2.Columns[0].Visible = false;
       //     guna2DataGridView2.Columns[2].Visible = false;
        }
        void DataGrid3()
        {
            guna2DataGridView3.Columns[1].HeaderText = "نام";
            //************************************************
            guna2DataGridView3.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //***************************************************************
            guna2DataGridView3.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //****************************************************
            guna2DataGridView3.Columns[0].Visible = false;
           // guna2DataGridView3.Columns[2].Visible = false;
           // guna2DataGridView3.Columns[3].Visible = false;
        }
        void DataGrid4()
        {
            guna2DataGridView4.Columns[1].HeaderText = "نام";
            //************************************************
            guna2DataGridView4.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //***************************************************************
            guna2DataGridView4.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //****************************************************
            guna2DataGridView4.Columns[0].Visible = false;
          //  guna2DataGridView4.Columns[2].Visible = false;
          //  guna2DataGridView4.Columns[3].Visible = false;
        }
        void DataGrid5()
        {
            guna2DataGridView5.Columns[1].HeaderText = "نام";
            //************************************************
            guna2DataGridView5.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //***************************************************************
            guna2DataGridView5.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //****************************************************
            guna2DataGridView5.Columns[0].Visible = false;
          //  guna2DataGridView5.Columns[2].Visible = false;
          //  guna2DataGridView5.Columns[3].Visible = false;
        }
        void DataGrid6()
        {
            guna2DataGridView6.Columns[1].HeaderText = "نام";
            //************************************************
            guna2DataGridView6.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //***************************************************************
            guna2DataGridView6.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //****************************************************
            guna2DataGridView6.Columns[0].Visible = false;
           // guna2DataGridView6.Columns[2].Visible = false;
        }
        void DataGrid7()
        {
            guna2DataGridView7.Columns[1].HeaderText = "نام";
            //************************************************
            guna2DataGridView7.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //***************************************************************
            guna2DataGridView7.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //****************************************************
            guna2DataGridView7.Columns[0].Visible = false;
          //  guna2DataGridView7.Columns[2].Visible = false;
        }
        void DataGrid8()
        {
            guna2DataGridView8.Columns[1].HeaderText = "نام";
            //************************************************
            guna2DataGridView8.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //***************************************************************
            guna2DataGridView8.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //****************************************************
            guna2DataGridView8.Columns[0].Visible = false;
           // guna2DataGridView8.Columns[2].Visible = false;
        }
        int id = 0;
        bool update = false;
        private void guna2Button33_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void guna2Button29_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void guna2Button25_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void guna2Button21_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void guna2Button17_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void guna2Button13_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void guna2Button9_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void guna2Button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void guna2Button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private async void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (guna2TextBox1.Text.Trim().Length == 0)
                {
                    labelX3.Text = "نام نمیتواند خالی باشد . خالی بودن نام باعت ایجاد تداخل در نرم افزار میگردد .";
                    labelX3.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                    labelX3.Visible = true;
                }
                else
                {
                    if (update == false)
                    {
                        var async = new DTO.Bank.Definition_Bank
                        {
                            Name = guna2TextBox1.Text,
                            IsDelete = true
                        };
                        var result = await _client1.Add(async);
                        if (result.Contains("موفق"))
                        {
                            labelX3.Text = result;
                            labelX3.ForeColor = ColorTranslator.FromHtml("#A1E3B2");
                            labelX3.Visible = true;
                        }
                        else
                        {
                            labelX3.Text = result;
                            labelX3.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                            labelX3.Visible = true;

                        }
                        guna2TextBox1.Text = "";
                        var async1 = await _client1.GetAll();
                        guna2DataGridView1.DataSource = async1;
                        DataGrid1();
                    }
                    else
                    {
                        var async2 = new DTO.Bank.Definition_Bank
                        {
                            Name = guna2TextBox1.Text,
                            Id = id
                        };
                        var result = await _client1.Update(async2);
                        if (result.Contains("موفق"))
                        {
                            labelX3.Text = result;
                            labelX3.ForeColor = ColorTranslator.FromHtml("#A1E3B2");
                            labelX3.Visible = true;
                        }
                        else
                        {
                            labelX3.Text = result;
                            labelX3.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                            labelX3.Visible = true;

                        }
                        guna2TextBox1.Text = "";
                        var async3 = await _client1.GetAll();
                        guna2DataGridView1.DataSource = async3;
                        DataGrid1();
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
        private async void guna2Button8_Click(object sender, EventArgs e)
        {
            try
            {
                if (guna2TextBox2.Text.Trim().Length == 0)
                {
                    label10.Text = "نام نمیتواند خالی باشد . خالی بودن نام باعت ایجاد تداخل در نرم افزار میگردد .";
                    label10.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                    label10.Visible = true;
                }
                else
                {
                    if (update == false)
                    {
                        var async = new DTO.Product.Type_Product
                        {
                            Name = guna2TextBox2.Text,
                            IsDelete = true
                        };
                        var result = await _client2.Add(async);
                        if (result.Contains("موفق"))
                        {
                            label10.Text = result;
                            label10.ForeColor = ColorTranslator.FromHtml("#A1E3B2");
                            label10.Visible = true;
                        }
                        else
                        {
                            label10.Text = result;
                            label10.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                            label10.Visible = true;

                        }
                        var async1 = await _client2.GetAll();
                        guna2DataGridView2.DataSource = async1;
                        DataGrid2();
                    }
                    else
                    {
                        var async2 = new DTO.Product.Type_Product
                        {
                            Name = guna2TextBox2.Text,
                            Id = id
                        };
                        var result = await _client2.Update(async2);
                        if (result.Contains("موفق"))
                        {
                            label10.Text = result;
                            label10.ForeColor = ColorTranslator.FromHtml("#A1E3B2");
                            label10.Visible = true;
                        }
                        else
                        {
                            label10.Text = result;
                            label10.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                            label10.Visible = true;

                        }
                        guna2TextBox2.Text = "";
                        var async3 = await _client2.GetAll();
                        guna2DataGridView2.DataSource = async3;
                        DataGrid2();
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
        private async void guna2Button12_Click(object sender, EventArgs e)
        {
            try
            {
                if (guna2TextBox3.Text.Trim().Length == 0)
                {
                    label11.Text = "نام نمیتواند خالی باشد . خالی بودن نام باعت ایجاد تداخل در نرم افزار میگردد .";
                    label11.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                    label11.Visible = true;
                }
                else
                {
                    if (update == false)
                    {
                        var async = new DTO.Product.Section_Product
                        {
                            Name = guna2TextBox3.Text,
                            IsDelete = true
                        };
                        var result = await _client3.Add(async);
                        if (result == "عملیات با موفقیت انجام شد .")
                        {
                            label11.Text = result;
                            label11.ForeColor = ColorTranslator.FromHtml("#A1E3B2");
                            label11.Visible = true;
                        }
                        else
                        {
                            label11.Text = result;
                            label11.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                            label11.Visible = true;

                        }
                        guna2TextBox3.Text = "";
                        var async1 = await _client3.GetAll();
                        guna2DataGridView3.DataSource = async1;
                        DataGrid3();
                    }
                    else
                    {
                        var async2 = new DTO.Product.Section_Product
                        {
                            Name = guna2TextBox3.Text,
                            Id = id
                        };
                        var result = await _client3.Update(async2);
                        if (result == "عملیات با موفقیت انجام شد .")
                        {
                            label11.Text = result;
                            label11.ForeColor = ColorTranslator.FromHtml("#A1E3B2");
                            label11.Visible = true;
                        }
                        else
                        {
                            label11.Text = result;
                            label11.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                            label11.Visible = true;

                        }
                        guna2TextBox3.Text = "";
                        var async3 = await _client3.GetAll();
                        guna2DataGridView3.DataSource = async3;
                        DataGrid3();
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
        private async void guna2Button16_Click(object sender, EventArgs e)
        {
            try
            {
                if (guna2TextBox4.Text.Trim().Length == 0)
                {
                    label12.Text = "نام نمیتواند خالی باشد . خالی بودن نام باعت ایجاد تداخل در نرم افزار میگردد .";
                    label12.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                    label12.Visible = true;
                }
                else
                {
                    if (update == false)
                    {
                        var async = new DTO.Product.Unit_Product
                        {
                            Name = guna2TextBox4.Text,
                            IsDelete = true
                        };
                        var result = await _client4.Add(async);
                        if (result == "عملیات با موفقیت انجام شد .")
                        {
                            label12.Text = result;
                            label12.ForeColor = ColorTranslator.FromHtml("#A1E3B2");
                            label12.Visible = true;
                        }
                        else
                        {
                            label12.Text = result;
                            label12.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                            label12.Visible = true;

                        }
                        guna2TextBox4.Text = "";
                        var async1 = await _client4.GetAll();
                        guna2DataGridView4.DataSource = async1;
                        DataGrid4();
                    }
                    else
                    {
                        var async2 = new DTO.Product.Unit_Product
                        {
                            Name = guna2TextBox4.Text,
                            Id = id
                        };
                        var result = await _client4.Update(async2);
                        if (result == "عملیات با موفقیت انجام شد .")
                        {
                            label12.Text = result;
                            label12.ForeColor = ColorTranslator.FromHtml("#A1E3B2");
                            label12.Visible = true;
                        }
                        else
                        {
                            label12.Text = result;
                            label12.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                            label12.Visible = true;

                        }
                        guna2TextBox4.Text = "";
                        var async3 = await _client4.GetAll();
                        guna2DataGridView4.DataSource = async3;
                        DataGrid4();
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
        private async void guna2Button20_Click(object sender, EventArgs e)
        {
            try
            {
                if (guna2TextBox5.Text.Trim().Length == 0)
                {
                    label13.Text = "نام نمیتواند خالی باشد . خالی بودن نام باعت ایجاد تداخل در نرم افزار میگردد .";
                    label13.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                    label13.Visible = true;
                }
                else
                {
                    if (update == false)
                    {
                        var async = new DTO.Product.PriceLevels
                        {
                            Name = guna2TextBox5.Text,
                            IsDelete = true
                        };
                        var result = await _client5.Add(async);
                        if (result == "عملیات با موفقیت انجام شد .")
                        {
                            label13.Text = result;
                            label13.ForeColor = ColorTranslator.FromHtml("#A1E3B2");
                            label13.Visible = true;
                        }
                        else
                        {
                            label13.Text = result;
                            label13.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                            label13.Visible = true;

                        }
                        guna2TextBox5.Text = "";
                        var async1 = await _client5.GetAll();
                        guna2DataGridView5.DataSource = async1;
                        DataGrid5();
                    }
                    else
                    {
                        var async2 = new DTO.Product.PriceLevels
                        {
                            Name = guna2TextBox5.Text,
                            Id = id
                        };
                        var result = await _client5.Update(async2);
                        if (result == "عملیات با موفقیت انجام شد .")
                        {
                            label13.Text = result;
                            label13.ForeColor = ColorTranslator.FromHtml("#A1E3B2");
                            label13.Visible = true;
                        }
                        else
                        {
                            label13.Text = result;
                            label13.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                            label13.Visible = true;

                        }
                        guna2TextBox5.Text = "";
                        var async3 = await _client5.GetAll();
                        guna2DataGridView5.DataSource = async3;
                        DataGrid5();
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
        private async void guna2Button24_Click(object sender, EventArgs e)
        {
            try
            {
                if (guna2TextBox6.Text.Trim().Length == 0)
                {
                    label14.Text = "نام نمیتواند خالی باشد . خالی بودن نام باعت ایجاد تداخل در نرم افزار میگردد .";
                    label14.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                    label14.Visible = true;
                }
                else
                {
                    if (update == false)
                    {
                        var async = new DTO.Product.Group_Product
                        {
                            Name = guna2TextBox6.Text,
                            IsDelete = true
                        };
                        var result = await _client6.Add(async);
                        if (result == "عملیات با موفقیت انجام شد .")
                        {
                            label14.Text = result;
                            label14.ForeColor = ColorTranslator.FromHtml("#A1E3B2");
                            label14.Visible = true;
                        }
                        else
                        {
                            label14.Text = result;
                            label14.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                            label14.Visible = true;

                        }
                        guna2TextBox6.Text = "";
                        var async1 = await _client6.GetAll();
                        guna2DataGridView6.DataSource = async1;
                        DataGrid6();
                    }
                    else
                    {
                        var async2 = new DTO.Product.Group_Product
                        {
                            Name = guna2TextBox6.Text,
                            Id = id
                        };
                        var result = await _client6.Update(async2);
                        if (result == "عملیات با موفقیت انجام شد .")
                        {
                            label14.Text = result;
                            label14.ForeColor = ColorTranslator.FromHtml("#A1E3B2");
                            label14.Visible = true;
                        }
                        else
                        {
                            label14.Text = result;
                            label14.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                            label14.Visible = true;

                        }
                        guna2TextBox6.Text = "";
                        var async3 = await _client6.GetAll();
                        guna2DataGridView6.DataSource = async3;
                        DataGrid6();
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
        private async void guna2Button28_Click(object sender, EventArgs e)
        {
            try
            {
                if (guna2TextBox7.Text.Trim().Length == 0)
                {
                    label15.Text = "نام نمیتواند خالی باشد . خالی بودن نام باعت ایجاد تداخل در نرم افزار میگردد .";
                    label15.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                    label15.Visible = true;
                }
                else
                {
                    if (update == false)
                    {
                        var async = new DTO.People.Group_People
                        {
                            Name = guna2TextBox7.Text,
                            IsDelete = true
                        };
                        var result = await _client7.Add(async);
                        if (result == "عملیات با موفقیت انجام شد .")
                        {
                            label15.Text = result;
                            label15.ForeColor = ColorTranslator.FromHtml("#A1E3B2");
                            label15.Visible = true;
                        }
                        else
                        {
                            label15.Text = result;
                            label15.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                            label15.Visible = true;

                        }
                        guna2TextBox7.Text = "";
                        var async1 = await _client7.GetAll();
                        guna2DataGridView7.DataSource = async1;
                        DataGrid7();
                    }
                    else
                    {
                        var async2 = new DTO.People.Group_People
                        {
                            Name = guna2TextBox7.Text,
                            Id = id
                        };
                        var result = await _client7.Update(async2);
                        if (result == "عملیات با موفقیت انجام شد .")
                        {
                            label15.Text = result;
                            label15.ForeColor = ColorTranslator.FromHtml("#A1E3B2");
                            label15.Visible = true;
                        }
                        else
                        {
                            label15.Text = result;
                            label15.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                            label15.Visible = true;

                        }
                        guna2TextBox7.Text = "";
                        var async3 = await _client7.GetAll();
                        guna2DataGridView7.DataSource = async3;
                        DataGrid7();
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
        private async void guna2Button32_Click(object sender, EventArgs e)
        {
            try
            {
                if (guna2TextBox8.Text.Trim().Length == 0)
                {
                    label16.Text = "نام نمیتواند خالی باشد . خالی بودن نام باعت ایجاد تداخل در نرم افزار میگردد .";
                    label16.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                    label16.Visible = true;
                }
                else
                {
                    if (update == false)
                    {
                        var async = new DTO.People.Type_People
                        {
                            Name = guna2TextBox8.Text,
                            IsDelete = true
                        };
                        var result = await _client8.Add(async);
                        if (result == "عملیات با موفقیت انجام شد .")
                        {
                            label16.Text = result;
                            label16.ForeColor = ColorTranslator.FromHtml("#A1E3B2");
                            label16.Visible = true;
                        }
                        else
                        {
                            label16.Text = result;
                            label16.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                            label16.Visible = true;

                        }
                        guna2TextBox8.Text = "";
                        var async1 = await _client8.GetAll();
                        guna2DataGridView8.DataSource = async1;
                        DataGrid8();
                    }
                    else
                    {
                        var async2 = new DTO.People.Type_People
                        {
                            Name = guna2TextBox8.Text,
                            Id = id
                        };
                        var result = await _client8.Update(async2);
                        if (result == "عملیات با موفقیت انجام شد .")
                        {
                            label16.Text = result;
                            label16.ForeColor = ColorTranslator.FromHtml("#A1E3B2");
                            label16.Visible = true;
                        }
                        else
                        {
                            label16.Text = result;
                            label16.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                            label16.Visible = true;

                        }
                        guna2TextBox8.Text = "";
                        var async3 = await _client8.GetAll();
                        guna2DataGridView8.DataSource = async3;
                        DataGrid8();
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
        private void guna2TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            id = 0;
            update = false;
        }
        private async void guna2Button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (guna2DataGridView1.SelectedRows.Count > 0)
                {
                    guna2TextBox1.Text = "";
                    var async5 = await _client1.GetById(id);
                    guna2TextBox1.Text = async5?.Name;
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
        private async void guna2Button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (guna2DataGridView2.SelectedRows.Count > 0)
                {
                    guna2TextBox2.Text = "";
                    var async5 = await _client2.GetById(id);
                    guna2TextBox2.Text = async5?.Name;
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
        private async void guna2Button11_Click(object sender, EventArgs e)
        {
            try
            {
                if (guna2DataGridView3.SelectedRows.Count > 0)
                {
                    guna2TextBox3.Text = "";
                    var async5 = await _client3.GetById(id);
                    guna2TextBox3.Text = async5?.Name;
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
        private async void guna2Button15_Click(object sender, EventArgs e)
        {
            try
            {
                if (guna2DataGridView4.SelectedRows.Count > 0)
                {
                    guna2TextBox4.Text = "";
                    var async5 = await _client4.GetById(id);
                    guna2TextBox4.Text = async5?.Name;
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
        private async void guna2Button19_Click(object sender, EventArgs e)
        {
            try
            {
                if (guna2DataGridView5.SelectedRows.Count > 0)
                {
                    guna2TextBox5.Text = "";
                    var async5 = await _client5.GetById(id);
                    guna2TextBox5.Text = async5?.Name;
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
        private async void guna2Button23_Click(object sender, EventArgs e)
        {
            try
            {
                if (guna2DataGridView6.SelectedRows.Count > 0)
                {
                    guna2TextBox6.Text = "";
                    var async5 = await _client6.GetById(id);
                    guna2TextBox6.Text = async5?.Name;
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
        private async void guna2Button27_Click(object sender, EventArgs e)
        {
            try
            {
                if (guna2DataGridView7.SelectedRows.Count > 0)
                {
                    guna2TextBox7.Text = "";
                    var async5 = await _client7.GetById(id);
                    guna2TextBox7.Text = async5?.Name;
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
        private async void guna2Button31_Click(object sender, EventArgs e)
        {
            try
            {
                if (guna2DataGridView8.SelectedRows.Count > 0)
                {
                    guna2TextBox8.Text = "";
                    var async5 = await _client8.GetById(id);
                    guna2TextBox8.Text = async5?.Name;
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

        private async void guna2Button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (id == 0)
                {
                    labelX3.Text = "موردی برای حذف وجود ندارد.";
                    labelX3.Visible = true;
                }
                else
                {
                    guna2MessageDialog1.Buttons = MessageDialogButtons.YesNo;
                    guna2MessageDialog1.Icon = MessageDialogIcon.Warning;
                    DialogResult result = guna2MessageDialog1.Show("آیا این بانک حذف شود ؟", "نرم افزار حسابداری و انبارداری کارن");
                    if (result == DialogResult.Yes)
                    {
                        var result1 = await _client1.Delete(id);
                        if (result1.Contains("موفق"))
                        {
                            labelX3.Text = result1;
                            labelX3.ForeColor = ColorTranslator.FromHtml("#A1E3B2");
                            labelX3.Visible = true;
                        }
                        else
                        {
                            labelX3.Text = result1;
                            labelX3.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                            labelX3.Visible = true;
                        }
                        var async = await _client1.GetAll();
                        guna2DataGridView1.DataSource = async;
                        guna2TextBox1.Text = "";
                        DataGrid1();
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
        private async void guna2Button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (id == 0)
                {
                    label10.Text = "موردی برای حذف وجود ندارد.";
                    label10.Visible = true;
                }
                else
                {
                    guna2MessageDialog1.Buttons = MessageDialogButtons.YesNo;
                    guna2MessageDialog1.Icon = MessageDialogIcon.Warning;
                    DialogResult result = guna2MessageDialog1.Show("آیا این بانک حذف شود ؟", "نرم افزار حسابداری و انبارداری کارن");
                    if (result == DialogResult.Yes)
                    {
                        var result1 = await _client2.Delete(id);
                        if (result1 == "عملیات با موفقیت انجام شد .")
                        {
                            label10.Text = result1;
                            label10.ForeColor = ColorTranslator.FromHtml("#A1E3B2");
                            label10.Visible = true;
                        }
                        else
                        {
                            label10.Text = result1;
                            label10.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                            label10.Visible = true;
                        }
                        var async = await _client2.GetAll();
                        guna2DataGridView2.DataSource = async;
                        guna2TextBox2.Text = "";
                        DataGrid2();
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
        private async void guna2Button10_Click(object sender, EventArgs e)
        {
            try
            {
                if (id == 0)
                {
                    label11.Text = "موردی برای حذف وجود ندارد.";
                    label11.Visible = true;
                }
                else
                {
                    guna2MessageDialog1.Buttons = MessageDialogButtons.YesNo;
                    guna2MessageDialog1.Icon = MessageDialogIcon.Warning;
                    DialogResult result = guna2MessageDialog1.Show("آیا این بانک حذف شود ؟", "نرم افزار حسابداری و انبارداری کارن");
                    if (result == DialogResult.Yes)
                    {
                        var result1 = await _client3.Delete(id);
                        if (result1 == "عملیات با موفقیت انجام شد .")
                        {
                            label11.Text = result1;
                            label11.ForeColor = ColorTranslator.FromHtml("#A1E3B2");
                            label11.Visible = true;
                        }
                        else
                        {
                            label11.Text = result1;
                            label11.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                            label11.Visible = true;
                        }
                        var async = await _client3.GetAll();
                        guna2DataGridView3.DataSource = async;
                        guna2TextBox3.Text = "";
                        DataGrid3();
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
                if (id == 0)
                {
                    label12.Text = "موردی برای حذف وجود ندارد.";
                    label12.Visible = true;
                }
                else
                {
                    guna2MessageDialog1.Buttons = MessageDialogButtons.YesNo;
                    guna2MessageDialog1.Icon = MessageDialogIcon.Warning;
                    DialogResult result = guna2MessageDialog1.Show("آیا این بانک حذف شود ؟", "نرم افزار حسابداری و انبارداری کارن");
                    if (result == DialogResult.Yes)
                    {
                        var result1 = await _client4.Delete(id);
                        if (result1 == "عملیات با موفقیت انجام شد .")
                        {
                            label12.Text = result1;
                            label12.ForeColor = ColorTranslator.FromHtml("#A1E3B2");
                            label12.Visible = true;
                        }
                        else
                        {
                            label12.Text = result1;
                            label12.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                            label12.Visible = true;
                        }
                        var async = await _client4.GetAll();
                        guna2DataGridView4.DataSource = async;
                        guna2TextBox4.Text = "";
                        DataGrid4();
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
        private async void guna2Button18_Click(object sender, EventArgs e)
        {
            try
            {
                if (id == 0)
                {
                    label13.Text = "موردی برای حذف وجود ندارد.";
                    label13.Visible = true;
                }
                else
                {
                    guna2MessageDialog1.Buttons = MessageDialogButtons.YesNo;
                    guna2MessageDialog1.Icon = MessageDialogIcon.Warning;
                    DialogResult result = guna2MessageDialog1.Show("آیا این بانک حذف شود ؟", "نرم افزار حسابداری و انبارداری کارن");
                    if (result == DialogResult.Yes)
                    {
                        var result1 = await _client5.Delete(id);
                        if (result1 == "عملیات با موفقیت انجام شد .")
                        {
                            label13.Text = result1;
                            label13.ForeColor = ColorTranslator.FromHtml("#A1E3B2");
                            label13.Visible = true;
                        }
                        else
                        {
                            label13.Text = result1;
                            label13.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                            label13.Visible = true;
                        }
                        var async = await _client5.GetAll();
                        guna2DataGridView5.DataSource = async;
                        guna2TextBox5.Text = "";
                        DataGrid5();
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
        private async void guna2Button22_Click(object sender, EventArgs e)
        {
            try
            {
                if (id == 0)
                {
                    label14.Text = "موردی برای حذف وجود ندارد.";
                    label14.Visible = true;
                }
                else
                {
                    guna2MessageDialog1.Buttons = MessageDialogButtons.YesNo;
                    guna2MessageDialog1.Icon = MessageDialogIcon.Warning;
                    DialogResult result = guna2MessageDialog1.Show("آیا این بانک حذف شود ؟", "نرم افزار حسابداری و انبارداری کارن");
                    if (result == DialogResult.Yes)
                    {
                        var result1 = await _client6.Delete(id);
                        if (result1 == "عملیات با موفقیت انجام شد .")
                        {
                            label14.Text = result1;
                            label14.ForeColor = ColorTranslator.FromHtml("#A1E3B2");
                            label14.Visible = true;
                        }
                        else
                        {
                            label14.Text = result1;
                            label14.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                            label14.Visible = true;
                        }
                        var async = await _client6.GetAll();
                        guna2DataGridView6.DataSource = async;
                        guna2TextBox6.Text = "";
                        DataGrid6();
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
        private async void guna2Button26_Click(object sender, EventArgs e)
        {
            try
            {
                if (id == 0)
                {
                    label15.Text = "موردی برای حذف وجود ندارد.";
                    label15.Visible = true;
                }
                else
                {
                    guna2MessageDialog1.Buttons = MessageDialogButtons.YesNo;
                    guna2MessageDialog1.Icon = MessageDialogIcon.Warning;
                    DialogResult result = guna2MessageDialog1.Show("آیا این بانک حذف شود ؟", "نرم افزار حسابداری و انبارداری کارن");
                    if (result == DialogResult.Yes)
                    {
                        var result1 = await _client7.Delete(id);
                        if (result1 == "عملیات با موفقیت انجام شد .")
                        {
                            label15.Text = result1;
                            label15.ForeColor = ColorTranslator.FromHtml("#A1E3B2");
                            label15.Visible = true;
                        }
                        else
                        {
                            label15.Text = result1;
                            label15.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                            label15.Visible = true;
                        }
                        var async = await _client7.GetAll();
                        guna2DataGridView7.DataSource = async;
                        guna2TextBox7.Text = "";
                        DataGrid7();
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
        private async void guna2Button30_Click(object sender, EventArgs e)
        {
            try
            {
                if (id == 0)
                {
                    label16.Text = "موردی برای حذف وجود ندارد.";
                    label16.Visible = true;
                }
                else
                {
                    guna2MessageDialog1.Buttons = MessageDialogButtons.YesNo;
                    guna2MessageDialog1.Icon = MessageDialogIcon.Warning;
                    DialogResult result = guna2MessageDialog1.Show("آیا این بانک حذف شود ؟", "نرم افزار حسابداری و انبارداری کارن");
                    if (result == DialogResult.Yes)
                    {
                        var result1 = await _client8.Delete(id);
                        if (result1 == "عملیات با موفقیت انجام شد .")
                        {
                            label16.Text = result1;
                            label16.ForeColor = ColorTranslator.FromHtml("#A1E3B2");
                            label16.Visible = true;
                        }
                        else
                        {
                            label16.Text = result1;
                            label16.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                            label16.Visible = true;
                        }
                        var async = await _client8.GetAll();
                        guna2DataGridView8.DataSource = async;
                        guna2TextBox8.Text = "";
                        DataGrid8();
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

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var cellValue = guna2DataGridView1.Rows[e.RowIndex].Cells[0].Value;
                if (cellValue != null && int.TryParse(cellValue.ToString(), out int parsedId))
                {
                    id = parsedId;
                }
                else
                {
                    guna2MessageDialog1.Icon = MessageDialogIcon.Warning;
                    guna2MessageDialog1.Show("مقدار سلول معتبر نیست یا خالی است", "نرم افزار حسابداری و انبارداری کارن");
                    return;
                }
            }
            catch (Exception ex)
            {
                guna2MessageDialog1.Icon = MessageDialogIcon.Error;
                guna2MessageDialog1.Show($"خطایی در انجام عملیات مورد نظر رخ داده است . {ex.Message}", "نرم افزار حسابداری و انبارداری کارن");
            }
        }
        private void guna2DataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var cellValue = guna2DataGridView2.Rows[e.RowIndex].Cells[0].Value;
                if (cellValue != null && int.TryParse(cellValue.ToString(), out int parsedId))
                {
                    id = parsedId;
                }
                else
                {
                    guna2MessageDialog1.Icon = MessageDialogIcon.Warning;
                    guna2MessageDialog1.Show("مقدار سلول معتبر نیست یا خالی است", "نرم افزار حسابداری و انبارداری کارن");
                    return;
                }
            }
            catch (Exception ex)
            {
                guna2MessageDialog1.Icon = MessageDialogIcon.Error;
                guna2MessageDialog1.Show($"خطایی در انجام عملیات مورد نظر رخ داده است . {ex.Message}", "نرم افزار حسابداری و انبارداری کارن");
            }
        }
        private void guna2DataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var cellValue = guna2DataGridView3.Rows[e.RowIndex].Cells[0].Value;
                if (cellValue != null && int.TryParse(cellValue.ToString(), out int parsedId))
                {
                    id = parsedId;
                }
                else
                {
                    guna2MessageDialog1.Icon = MessageDialogIcon.Warning;
                    guna2MessageDialog1.Show("مقدار سلول معتبر نیست یا خالی است", "نرم افزار حسابداری و انبارداری کارن");
                    return;
                }
            }
            catch (Exception ex)
            {
                guna2MessageDialog1.Icon = MessageDialogIcon.Error;
                guna2MessageDialog1.Show($"خطایی در انجام عملیات مورد نظر رخ داده است . {ex.Message}", "نرم افزار حسابداری و انبارداری کارن");
            }
        }
        private void guna2DataGridView4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var cellValue = guna2DataGridView4.Rows[e.RowIndex].Cells[0].Value;
                if (cellValue != null && int.TryParse(cellValue.ToString(), out int parsedId))
                {
                    id = parsedId;
                }
                else
                {
                    guna2MessageDialog1.Icon = MessageDialogIcon.Warning;
                    guna2MessageDialog1.Show("مقدار سلول معتبر نیست یا خالی است", "نرم افزار حسابداری و انبارداری کارن");
                    return;
                }
            }
            catch (Exception ex)
            {
                guna2MessageDialog1.Icon = MessageDialogIcon.Error;
                guna2MessageDialog1.Show($"خطایی در انجام عملیات مورد نظر رخ داده است . {ex.Message}", "نرم افزار حسابداری و انبارداری کارن");
            }
        }
        private void guna2DataGridView5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var cellValue = guna2DataGridView5.Rows[e.RowIndex].Cells[0].Value;
                if (cellValue != null && int.TryParse(cellValue.ToString(), out int parsedId))
                {
                    id = parsedId;
                }
                else
                {
                    guna2MessageDialog1.Icon = MessageDialogIcon.Warning;
                    guna2MessageDialog1.Show("مقدار سلول معتبر نیست یا خالی است", "نرم افزار حسابداری و انبارداری کارن");
                    return;
                }
            }
            catch (Exception ex)
            {
                guna2MessageDialog1.Icon = MessageDialogIcon.Error;
                guna2MessageDialog1.Show($"خطایی در انجام عملیات مورد نظر رخ داده است . {ex.Message}", "نرم افزار حسابداری و انبارداری کارن");
            }
        }
        private void guna2DataGridView6_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var cellValue = guna2DataGridView6.Rows[e.RowIndex].Cells[0].Value;
                if (cellValue != null && int.TryParse(cellValue.ToString(), out int parsedId))
                {
                    id = parsedId;
                }
                else
                {
                    guna2MessageDialog1.Icon = MessageDialogIcon.Warning;
                    guna2MessageDialog1.Show("مقدار سلول معتبر نیست یا خالی است", "نرم افزار حسابداری و انبارداری کارن");
                    return;
                }
            }
            catch (Exception ex)
            {
                guna2MessageDialog1.Icon = MessageDialogIcon.Error;
                guna2MessageDialog1.Show($"خطایی در انجام عملیات مورد نظر رخ داده است . {ex.Message}", "نرم افزار حسابداری و انبارداری کارن");
            }
        }
        private void guna2DataGridView7_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var cellValue = guna2DataGridView7.Rows[e.RowIndex].Cells[0].Value;
                if (cellValue != null && int.TryParse(cellValue.ToString(), out int parsedId))
                {
                    id = parsedId;
                }
                else
                {
                    guna2MessageDialog1.Icon = MessageDialogIcon.Warning;
                    guna2MessageDialog1.Show("مقدار سلول معتبر نیست یا خالی است", "نرم افزار حسابداری و انبارداری کارن");
                    return;
                }
            }
            catch (Exception ex)
            {
                guna2MessageDialog1.Icon = MessageDialogIcon.Error;
                guna2MessageDialog1.Show($"خطایی در انجام عملیات مورد نظر رخ داده است . {ex.Message}", "نرم افزار حسابداری و انبارداری کارن");
            }
        }
        private void guna2DataGridView8_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var cellValue = guna2DataGridView8.Rows[e.RowIndex].Cells[0].Value;
                if (cellValue != null && int.TryParse(cellValue.ToString(), out int parsedId))
                {
                    id = parsedId;
                }
                else
                {
                    guna2MessageDialog1.Icon = MessageDialogIcon.Warning;
                    guna2MessageDialog1.Show("مقدار سلول معتبر نیست یا خالی است", "نرم افزار حسابداری و انبارداری کارن");
                    return;
                }
            }
            catch (Exception ex)
            {
                guna2MessageDialog1.Icon = MessageDialogIcon.Error;
                guna2MessageDialog1.Show($"خطایی در انجام عملیات مورد نظر رخ داده است . {ex.Message}", "نرم افزار حسابداری و انبارداری کارن");
            }
        }
        private async void Basic_Data_Load(object sender, EventArgs e)
        {
            var async1 = await _client1.GetAll();
            guna2DataGridView1.DataSource = async1;
            DataGrid1();

            var async2 = await _client2.GetAll();
            guna2DataGridView2.DataSource = async2;
            DataGrid2();

            var async3 = await _client3.GetAll();
            guna2DataGridView3.DataSource = async3;
            DataGrid3();

            var async4 = await _client4.GetAll();
            guna2DataGridView4.DataSource = async4;
            DataGrid4();

            var async5 = await _client5.GetAll();
            guna2DataGridView5.DataSource = async5;
            DataGrid5();

            var async6 = await _client6.GetAll();
            guna2DataGridView6.DataSource = async6;
            DataGrid6();

            var async7 = await _client7.GetAll();
            guna2DataGridView7.DataSource = async7;
            DataGrid7();

            var async8 = await _client8.GetAll();
            guna2DataGridView8.DataSource = async8;
            DataGrid8();
        }

        private void guna2TabControl1_Click(object sender, EventArgs e)
        {
            update = false;
            id = 0;
        }
    }
}
