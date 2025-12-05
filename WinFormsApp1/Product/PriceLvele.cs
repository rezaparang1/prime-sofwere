using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp1.DTO.Product;

namespace WinFormsApp1.Product
{
    public partial class PriceLvele : Form
    {
        private DTO.Product.ProductDto CurrentProduct;
        private class1.Product.PriceLevels _client;
        private Guid _selectedPriceId = Guid.Empty;
        bool update = false;
        int id = 0;
        public PriceLvele(DTO.Product.ProductDto product)
        {
            InitializeComponent();
            CurrentProduct = product;
            _client = new class1.Product.PriceLevels();
            txtbuy.KeyPress += Price_KeyPress;
            txtbuy.TextChanged += Price_TextChanged;

            txtsell.KeyPress += Price_KeyPress;
            txtsell.TextChanged += Price_TextChanged;

            txtprofit.ReadOnly = true;
            txtprofit.FillColor = Color.White;
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

            CalculateProfit();
        }
        private void CalculateProfit()
        {
            long buy = 0, sell = 0;
            long.TryParse((txtbuy.Text ?? "0").Replace(",", ""), out buy);
            long.TryParse((txtsell.Text ?? "0").Replace(",", ""), out sell);

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
            txtbuy.Text = "";
            txtprofit.Text = "";
            txtsell.Text = "";
        }

        private void guna2Button11_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void guna2Button5_Click(object sender, EventArgs e)
        {
            guna2Panel1.Visible = false;
            txtbuy.Text = "";
            txtprofit.Text = "";
            txtsell.Text = "";
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            update = false;
            guna2Panel1.Visible = true;
            Clear();
        }
        private (DTO.Product.UnitsLevel unit, DTO.Product.ProductPrices price)? FindPriceAndUnit(Guid priceId)
        {
            foreach (var unit in CurrentProduct.Units)
            {
                var price = unit.Prices.FirstOrDefault(p => p.TempId == priceId);
                if (price != null)
                    return (unit, price);
            }
            return null;
        }
        public void Creat ()
        {
            if (txtbuy.Text.Trim().Length ==0 && txtsell.Text.Trim().Length ==00 )
            {
                guna2MessageDialog1.Icon = MessageDialogIcon.Error;
                guna2MessageDialog1.Show("لطفا اطلاعات خالی را پرکنید .", "نرم افزار حسابداری و انبارداری ");
            }
            else
            {
                if (update == false)
                {
                    if (_selectedPriceId == Guid.Empty)
                    {
                        if (cmdunit.SelectedValue == null)
                        {
                            MessageBox.Show("لطفاً یک واحد انتخاب کنید.");
                            return;
                        }

                        var selectedUnitId = (Guid)cmdunit.SelectedValue;
                        var selectedUnit = CurrentProduct.Units.FirstOrDefault(u => u.TempId == selectedUnitId);
                        if (selectedUnit == null)
                        {
                            MessageBox.Show("واحد انتخاب شده پیدا نشد.");
                            return;
                        }

                        if (selectedUnit.Prices.Any(p => p.PriceLevelId == Convert.ToInt32(cmdprice.SelectedValue)))
                        {
                            MessageBox.Show("این سطح قیمت برای این واحد قبلاً ثبت شده است.");
                            return;
                        }

                        var newPrice = new DTO.Product.ProductPrices
                        {
                            TempId = Guid.NewGuid(),
                            PriceLevelId = Convert.ToInt32(cmdprice.SelectedValue),
                            BuyPrice = Convert.ToDecimal(txtbuy.Text),
                            Profit = Convert.ToDecimal(txtprofit.Text),
                            SalePrice = Convert.ToDecimal(txtsell.Text)
                        };

                        selectedUnit.Prices.Add(newPrice);
                    }
                }
                else
                {
                    var result = FindPriceAndUnit(_selectedPriceId);
                    if (result == null)
                    {
                        MessageBox.Show("رکورد قیمت برای ویرایش پیدا نشد.");
                        _selectedPriceId = Guid.Empty;
                        return;
                    }

                    var (unit, priceToEdit) = result.Value;

                    priceToEdit.PriceLevelId = Convert.ToInt32(cmdprice.SelectedValue);
                    priceToEdit.BuyPrice = Convert.ToDecimal(txtbuy.Text);
                    priceToEdit.Profit = Convert.ToDecimal(txtprofit.Text);
                    priceToEdit.SalePrice = Convert.ToDecimal(txtsell.Text);

                    _selectedPriceId = Guid.Empty;
                }
            }
            guna2Panel1.Visible = false;
            RefreshPriceGrid();
            Clear();
        }
        private void guna2Button4_Click(object sender, EventArgs e)
        {
            Creat();
        }
        private void RefreshPriceGrid()
        {
            guna2DataGridView1.Rows.Clear();

            foreach (var unit in CurrentProduct.Units)
            {
                foreach (var price in unit.Prices)
                {
                    guna2DataGridView1.Rows.Add(
                        price.TempId,
                        unit.Title,
                        price.PriceLevelId,
                        price.BuyPrice,
                         price.SalePrice,
                        price.Profit           
                    );
                }
            }
        }
        private async void PriceLvele_Load(object sender, EventArgs e)
        {
            //Unit-Product
            var async4 = await _client.GetAll();
            cmdprice.DataSource = async4;
            cmdprice.DisplayMember = "Name";
            cmdprice.ValueMember = "Id";
            cmdprice.Font = new Font("Tahoma", 12, FontStyle.Regular);

            cmdprice.DropDownStyle = ComboBoxStyle.DropDownList;
            cmdunit.DataSource = CurrentProduct.Units; 
            cmdunit.DisplayMember = "Title";         
            cmdunit.ValueMember = "TempId";
            //var firstBarcode = CurrentProduct.Units.FirstOrDefault()?
            //                 .Barcodes.FirstOrDefault()?
            //                 .Barcode;
           // txtname.Text = CurrentProduct.Name;
         //   txtbarcode.Text = firstBarcode;
            RefreshPriceGrid();
        }
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView1.CurrentRow == null)
                return;
            guna2MessageDialog1.Buttons = MessageDialogButtons.YesNo;
            guna2MessageDialog1.Icon = MessageDialogIcon.Warning;
            DialogResult result = guna2MessageDialog1.Show("اطلاعات مد نظر ذخیره شوند ؟", "نرم افزار نکسای");
            if (result == DialogResult.Yes)
            {
                Guid selectedId = (Guid)guna2DataGridView1.CurrentRow.Cells["TempId"].Value;

                var unitWithPrice = CurrentProduct.Units
                    .FirstOrDefault(u => u.Prices.Any(p => p.TempId == selectedId));

                if (unitWithPrice != null)
                {
                    var priceToRemove = unitWithPrice.Prices.First(p => p.TempId == selectedId);
                    unitWithPrice.Prices.Remove(priceToRemove);
                    RefreshPriceGrid();
                }
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            update = true;
            Clear();
            if (guna2DataGridView1.CurrentRow == null) return;

            _selectedPriceId = (Guid)guna2DataGridView1.CurrentRow.Cells["TempId"].Value;

            var selectedUnit = CurrentProduct.Units.First(u => u.Prices.Any(p => p.TempId == _selectedPriceId));
            var priceToEdit = selectedUnit.Prices.First(p => p.TempId == _selectedPriceId);
            guna2Panel1.Visible = true;
           
            cmdunit.SelectedValue = selectedUnit.TempId;
            cmdprice.SelectedValue = priceToEdit.PriceLevelId;
            txtbuy.Text = priceToEdit.BuyPrice.ToString();
            txtprofit.Text = priceToEdit.Profit.ToString();
            txtsell.Text = priceToEdit.SalePrice.ToString();

        }
        private void guna2Button12_Click(object sender, EventArgs e)
        {
            if (txtbuy.Text.Trim().Length != 0 && txtsell.Text.Trim().Length!=0)
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
    }
}
