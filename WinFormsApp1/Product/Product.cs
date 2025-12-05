using Guna.UI2.WinForms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1.Product
{
    public partial class Product : Form
    {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DTO.Product.ProductDto CurrentProduct { get; set; } = new DTO.Product.ProductDto();
        private class1.Product.Type_Product _client1;
        private class1.Product.Section_Product _client2;
        private class1.Product.Storeroom_Product _client3;
        private class1.Product.Unit_Product _client4;
        private class1.Product.Group_Product _client5;
        private class1.Product.Product _client;
        public Product()
        {
            InitializeComponent();
            txtbuyprice.KeyPress += Price_KeyPress;
            txtbuyprice.TextChanged += Price_TextChanged;

            txtseleprice.KeyPress += Price_KeyPress;
            txtseleprice.TextChanged += Price_TextChanged;

            txtprofit.ReadOnly = true;
            txtprofit.FillColor = Color.White;

            txtbarcode.KeyPress += OnlyNumber_KeyPress;
            txtbarcode.TextChanged += OnlyNumber_TextChanged;

            txtvalue.KeyPress += OnlyNumber_KeyPress;
            txtvalue.TextChanged += OnlyNumber_TextChanged;

            _client1 = new class1.Product.Type_Product();
            _client2 = new class1.Product.Section_Product();
            _client3 = new class1.Product.Storeroom_Product();
            _client4 = new class1.Product.Unit_Product();
            _client5 = new class1.Product.Group_Product();
            _client = new class1.Product.Product();
        }
        public void FirstUnit()
        {
            //if (!CurrentProduct.Units.Any())
            //{
            var defaultUnit = new DTO.Product.UnitsLevel
            {
                Title = "عدد",
                UnitProductId = (int)cmdunitproduct.SelectedValue,
                ConversionFactor = 1
            };

            //defaultUnit.Prices.Add(new DTO.Product.ProductPrices
            //{
            //    PriceLevelId = 1,
            //    BuyPrice = Convert.ToDecimal(txtbuyprice.Text),
            //    Profit = Convert.ToDecimal(txtprofit.Text),
            //    SalePrice = Convert.ToDecimal(txtseleprice.Text)
            //});

            //defaultUnit.Barcodes.Add(new DTO.Product.ProductBarcodes
            //{
            //    Barcode = txtbarcode.Text
            //});
            CurrentProduct.Units.Add(defaultUnit);
            // }

        }
        public void FirstPrice()
        {
            CurrentProduct = new DTO.Product.ProductDto();
            if (!CurrentProduct.Units.Any())
            {
                var defaultUnit = new DTO.Product.UnitsLevel
                {
                    Title = "عدد",
                    UnitProductId = (int)cmdunitproduct.SelectedValue,
                    ConversionFactor = 1
                };

                defaultUnit.Prices.Add(new DTO.Product.ProductPrices
                {
                    PriceLevelId = 1,
                    BuyPrice = Convert.ToDecimal(txtbuyprice.Text),
                    Profit = Convert.ToDecimal(txtprofit.Text),
                    SalePrice = Convert.ToDecimal(txtseleprice.Text)
                });

                defaultUnit.Barcodes.Add(new DTO.Product.ProductBarcodes
                {
                    Barcode = txtbarcode.Text
                });
                CurrentProduct.Units.Add(defaultUnit);
            }
        }
        public bool update = false;
        public int id = 0;
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

            CalculateProfit();
        }
        private void CalculateProfit()
        {
            long buy = 0, sell = 0;
            long.TryParse((txtbuyprice.Text ?? "0").Replace(",", ""), out buy);
            long.TryParse((txtseleprice.Text ?? "0").Replace(",", ""), out sell);

            long profit = sell - buy;
            txtprofit.Text = string.Format(CultureInfo.InvariantCulture, "{0:N0}", profit);

            txtprofit.FillColor = profit < 0 ? Color.LightCoral : Color.LightGreen;
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
        public void Clear()
        {
            txtbarcode.Text = "";
            txtname.Text = "";
            txtbuyprice.Text = "";
            txtseleprice.Text = "";
            txtprofit.Text = "";
            txttax.Text = "";
            txtbutton.Text = "";
            txtmax.Text = "";
            txtmin.Text = "";
            txtdescription.Text = "";
            guna2PictureBox1.Image = null;
            selectedImagePath = "";
        }
        private async void Product_Load(object sender, EventArgs e)
        {
            //Type-Product
            var async1 = await _client1.GetAll();
            cmdtypeproduct.DataSource = async1;
            cmdtypeproduct.DisplayMember = "Name";
            cmdtypeproduct.ValueMember = "Id";
            cmdtypeproduct.Font = new Font("Tahoma", 12, FontStyle.Regular);
            cmdtypeproduct.DropDownStyle = ComboBoxStyle.DropDownList;
            //Group-Product
            var async2 = await _client5.GetAll();
            cmdgroupproduct.DataSource = async2;
            cmdgroupproduct.DisplayMember = "Name";
            cmdgroupproduct.ValueMember = "Id";
            cmdgroupproduct.Font = new Font("Tahoma", 12, FontStyle.Regular);
            cmdgroupproduct.DropDownStyle = ComboBoxStyle.DropDownList;
            //Anbar-Prodduct
            var async3 = await _client3.GetAllAsync();
            cmdanbar.DataSource = async3;
            cmdanbar.DisplayMember = "Name";
            cmdanbar.ValueMember = "Id";
            cmdanbar.Font = new Font("Tahoma", 12, FontStyle.Regular);
            cmdanbar.DropDownStyle = ComboBoxStyle.DropDownList;
            //Unit-Product
            var async4 = await _client4.GetAll();
            cmdunitproduct.DataSource = async4;
            cmdunitproduct.DisplayMember = "Name";
            cmdunitproduct.ValueMember = "Id";
            cmdunitproduct.Font = new Font("Tahoma", 12, FontStyle.Regular);
            cmdunitproduct.DropDownStyle = ComboBoxStyle.DropDownList;
            //Section-Product
            var async5 = await _client2.GetAll();
            cmdsection.DataSource = async5;
            cmdsection.DisplayMember = "Name";
            cmdsection.ValueMember = "Id";
            cmdsection.Font = new Font("Tahoma", 12, FontStyle.Regular);
            cmdsection.DropDownStyle = ComboBoxStyle.DropDownList;

            grOperation.Visible = true;
            grSearch.Visible = false;
            guna2Panel1.BackColor = ColorTranslator.FromHtml("#374151");
            guna2Panel2.BackColor = ColorTranslator.FromHtml("#111827");
            FirstUnit();
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
            CurrentProduct = new DTO.Product.ProductDto();
            id = 0;
            update = false;
        }
        private void guna2Button7_Click(object sender, EventArgs e)
        {
            if (CurrentProduct != null)
            {
                guna2MessageDialog1.Buttons = MessageDialogButtons.YesNo;
                guna2MessageDialog1.Icon = MessageDialogIcon.Warning;
                DialogResult result = guna2MessageDialog1.Show("اطلاعات مد نظر ذخیره شوند ؟", "نرم افزار نکسای");
                if (result == DialogResult.Yes)
                {
                    Creat();
                }
                else
                {
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }
        }
        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if (txtbarcode.Text.Trim().Length == 0)
            {
                guna2MessageDialog3.Icon = MessageDialogIcon.Error;
                guna2MessageDialog3.Show("بارکد کالا نمیتواند خالی باشد .", "نرم افزار حسابداری و انبارداری ");
            }
            else if (txtname.Text.Trim().Length == 0)
            {
                guna2MessageDialog3.Icon = MessageDialogIcon.Error;
                guna2MessageDialog3.Show("نام کالا نمیتواند خالی باشد .", "نرم افزار حسابداری و انبارداری ");
            }
            else
            {
                UnitLevel frm = new UnitLevel(CurrentProduct);
                frm.txtbarcode.Text = txtbarcode.Text;
                frm.ShowDialog();
            }
        }
        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (txtbuyprice.Text.Trim().Length == 0)
            {
                guna2MessageDialog3.Icon = MessageDialogIcon.Error;
                guna2MessageDialog3.Show("قیمت خرید کالا نمیتواند خالی باشد .", "نرم افزار حسابداری و انبارداری ");
            }
            else if (txtseleprice.Text.Trim().Length == 0)
            {
                guna2MessageDialog3.Icon = MessageDialogIcon.Error;
                guna2MessageDialog3.Show("قیمت فروش کالا نمیتواند خالی باشد .", "نرم افزار حسابداری و انبارداری ");
            }
            else
            {
                if (CurrentProduct.Units.Count == 2)
                {
                    PriceLvele frm = new PriceLvele(CurrentProduct);
                    frm.txtbarcode.Text = txtbarcode.Text;
                    frm.txtname.Text = txtname.Text;
                    frm.ShowDialog();
                }
                else
                {
                    FirstPrice();
                    PriceLvele frm = new PriceLvele(CurrentProduct);
                    frm.txtbarcode.Text = txtbarcode.Text;
                    frm.txtname.Text = txtname.Text;
                    frm.ShowDialog();
                }

            }
        }
        private void guna2Button6_Click(object sender, EventArgs e)
        {
            if (txtbarcode.Text.Trim().Length == 0)
            {
                guna2MessageDialog3.Icon = MessageDialogIcon.Error;
                guna2MessageDialog3.Show("بارکد کالا نمیتواند خالی باشد .", "نرم افزار حسابداری و انبارداری ");
            }
            else if (txtname.Text.Trim().Length == 0)
            {
                guna2MessageDialog3.Icon = MessageDialogIcon.Error;
                guna2MessageDialog3.Show("نام کالا نمیتواند خالی باشد .", "نرم افزار حسابداری و انبارداری ");
            }
            else
            {
                Barcodes frm = new Barcodes(CurrentProduct);
                frm.ShowDialog();
            }
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (txtname.Text.Trim().Length == 0 && txtbarcode.Text.Trim().Length == 0)
            {
                label24.Text = "لطفا نام و کد کالا را وارد کنید .";
                label24.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                label24.Visible = true;
            }
            else
            {
                ShortKey frm = new ShortKey(CurrentProduct);
                frm.ShowDialog();
            }
        }
        private void guna2Button11_Click(object sender, EventArgs e)
        {
            Process.Start("calc.exe");
        }
        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (chdtax.Checked)
            {
                txttax.Visible = true;
            }
            else
            {
                txttax.Visible = false; ;
            }
        }
        private void guna2CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (chdbutton.Checked)
            {
                txtbutton.Visible = true;
            }
            else
            {
                txtbutton.Visible = false; ;
            }
        }
        private string selectedImagePath = "";
        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select an image";
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                selectedImagePath = ofd.FileName;

                guna2PictureBox1.Image = Image.FromFile(selectedImagePath);

                guna2PictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }
        private ProductDtoForApi MapToApi(DTO.Product.ProductDto product)
        {
            return new ProductDtoForApi
            {
                Name = product.Name,
                TypeProductId = product.TypeProductId,
                UnitProductId = product.UnitProductId,
                SectionProductId = product.SectionProductId,
                StoreroomProductId = product.StoreroomProductId,
                GroupProductId = product.GroupProductId,

                BuyPrice = product.BuyPrice,    // ✅ حالا decimal هست
                Profit = product.Profit,
                SalePrice = product.SalePrice,

                IsActive = product.IsActive,
                IsTax = product.IsTax,
                Tax = product.Tax,
                IsWeighty = product.IsWeighty,

                Inventory = product.Inventory,
                MinInventory = product.MinInventory,
                MaxInventory = product.MaxInventory,

                Description = product.Description,

                Units = product.Units.Select(u => new UnitsLevelDtoForApi
                {
                    Title = u.Title,
                    UnitProductId = u.UnitProductId,
                    ConversionFactor = u.ConversionFactor,

                    Prices = u.Prices.Select(p => new ProductPriceDtoForApi
                    {
                        PriceLevelId = p.PriceLevelId,
                        BuyPrice = p.BuyPrice,   // ✅ مستقیم می‌فرسته به API
                        Profit = p.Profit,
                        SalePrice = p.SalePrice
                    }).ToList(),

                    Barcodes = u.Barcodes.Select(b => new ProductBarcodeDtoForApi
                    {
                        Barcode = b.Barcode
                    }).ToList()
                }).ToList()
            };
        }
        public void SetValue()
        {
            CurrentProduct.Name = txtname.Text.Trim();
            CurrentProduct.TypeProductId = (int)cmdtypeproduct.SelectedValue;   // نباید صفر باشد
            CurrentProduct.UnitProductId = (int)cmdunitproduct.SelectedValue;
            CurrentProduct.SectionProductId = (int)cmdsection.SelectedValue;
            CurrentProduct.StoreroomProductId = (int)cmdanbar.SelectedValue;
            CurrentProduct.GroupProductId = (int)cmdgroupproduct.SelectedValue;
            CurrentProduct.BuyPrice = Convert.ToDecimal(txtbuyprice.Text);
            CurrentProduct.Profit = Convert.ToDecimal(txtprofit.Text);
            CurrentProduct.SalePrice = Convert.ToDecimal(txtseleprice.Text);
            CurrentProduct.IsActive = radactive.Checked;
            CurrentProduct.IsTax = chdtax.Checked;
            CurrentProduct.Tax = Convert.ToInt16(txttax.Text);
            CurrentProduct.IsWeighty = chdweighty.Checked;
            CurrentProduct.ImagePath = selectedImagePath;
            CurrentProduct.Inventory = Convert.ToInt16(txtvalue.Text);
            CurrentProduct.MinInventory = Convert.ToInt16(txtmin.Text);
            CurrentProduct.MaxInventory = Convert.ToInt16(txtmax.Text);
            CurrentProduct.Description = txtdescription.Text.Trim();
        }
        public async void Creat()
        {
            if (txtname.Text.Trim().Length == 0 && txtbarcode.Text.Trim().Length == 0 && txtbuyprice.Text.Trim().Length == 0)
            {
                label24.Text = "لطفا نام کالا را وارد کنید .";
                label24.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                label24.Visible = true;
            }
            else if (txtprofit.FillColor == Color.LightCoral)
            {
                guna2MessageDialog3.Icon = MessageDialogIcon.Error;
                guna2MessageDialog3.Show("سود شما از این محصول در ضرر است .", "نرم افزار حسابداری و انبارداری ");
            }
            else
            {
                if (update == false)
                {
                    SetValue();
                    FirstPrice();
                    var apiProduct = MapToApi(CurrentProduct);
                    var result = await _client.AddAsync(apiProduct);
                    if (result.Contains("موفقیت"))
                    {
                        label24.Text = "ثبت کالا با موفقیت انجام شد .";
                        label24.ForeColor = ColorTranslator.FromHtml("#A1E3B2");
                        label24.Visible = true;
                    }
                    else
                    {
                        label24.Text = $"خطایی در ثبت اطلاعات ایجاد شده است مجددا تلاش کنید .{result}";
                        label24.ForeColor = ColorTranslator.FromHtml("#F15B5B");
                        label24.Visible = true;
                    }
                }
                else
                {

                }
                Clear();
                id = 0;
                update = false;
                CurrentProduct = new DTO.Product.ProductDto();
            }
        }
        private void guna2Button9_Click(object sender, EventArgs e)
        {
            Creat();
        }
        private void txtbuyprice_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtbuyprice.Text, out var price))
            {
                CurrentProduct.SalePrice = price;
            }
        }
        private void txtseleprice_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtseleprice.Text, out var price))
            {
                CurrentProduct.SalePrice = price;
            }
        }
        private void txtprofit_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtprofit.Text, out var price))
            {
                CurrentProduct.SalePrice = price;
            }
        }
        private void txtbarcode_TextChanged(object sender, EventArgs e)
        {
            //string barcode = CurrentProduct.Units.FirstOrDefault()?.Barcodes.FirstOrDefault()?.Barcode;
            //txtbarcode.Text = barcode;
        }
        private void txtname_TextChanged(object sender, EventArgs e)
        {
            CurrentProduct.Name = txtname.Text.Trim();
        }
        private void guna2Button20_Click(object sender, EventArgs e)
        {
            guna2Panel3.Visible = false;
            label24.Visible = false;
        }
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (txtname.Text.Trim().Length == 0 && txtbarcode.Text.Trim().Length == 0 && txtseleprice.Text.Trim().Length == 0)
            {
                label24.Text = "لطفا اطلاعات کالا رو تکمیل کنید .";
                label24.ForeColor = ColorTranslator.FromHtml("#A1E3B2");
                label24.Visible = true;
            }
            else
            {
                guna2Panel3.Visible = true;
                lblname.Text = txtname.Text;
                lblanbar.Text = cmdanbar.Text;
                lblprice.Text = txtseleprice.Text;
                lblsection.Text = cmdsection.Text;
                lblunit.Text = cmdunitproduct.Text;
                label24.Visible = false;
            }
        }
        private void guna2Button19_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button17_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
