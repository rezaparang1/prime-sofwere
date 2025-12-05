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
    public partial class UnitLevel : Form
    {
        private Guid? currentTempUnitId;
        private List<DTO.Product.ProductBarcodes> tempBarcodes = new();
        private DTO.Product.ProductDto _product;
        private DTO.Product.ProductDto CurrentProduct;
        private class1.Product.Unit_Product _client;
        public UnitLevel(DTO.Product.ProductDto product)
        {
            InitializeComponent();
            CurrentProduct = product;
            _product = product;
            _client = new class1.Product.Unit_Product();
        }
        bool update = false;
        int id = 0;
        private void guna2Button11_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            update = false;
            guna2Panel1.Visible = true;
            txtonvan.Text = "";
            txtzarib.Text = "";
        }
        private void guna2Button5_Click(object sender, EventArgs e)
        {
            guna2Panel1.Visible = false;
            txtonvan.Text = "";
            txtzarib.Text = "";
        }
        public void Creat ()
        {
            if(txtonvan.Text.Trim().Length==0 && txtzarib.Text.Trim().Length==0)
            {
                guna2MessageDialog1.Icon = MessageDialogIcon.Error;
                guna2MessageDialog1.Show("لطفا اطلاعات خالی را پرکنید .", "نرم افزار حسابداری و انبارداری ");
            }
            else
            {
                if (update == false)
                {
                    guna2Panel1.Visible = true;
                    var newUnit = new DTO.Product.UnitsLevel
                    {
                        TempId = currentTempUnitId ?? Guid.NewGuid(),
                        Title = txtonvan.Text,
                        UnitProductId = (int)cmdunit.SelectedValue,
                        ConversionFactor = Convert.ToDecimal(txtzarib.Text),
                        Prices = new List<DTO.Product.ProductPrices>(),
                        Barcodes = new List<DTO.Product.ProductBarcodes>() // خالی بگذارید
                    };
                    newUnit.Prices.Add(new DTO.Product.ProductPrices
                    {
                        TempId = Guid.NewGuid(),
                        PriceLevelId = 1,
                        BuyPrice = CurrentProduct.BuyPrice * Convert.ToDecimal(txtzarib.Text),
                        Profit = CurrentProduct.Profit * Convert.ToDecimal(txtzarib.Text),
                        SalePrice = CurrentProduct.SalePrice * Convert.ToDecimal(txtzarib.Text),
                    });
                    CurrentProduct.Units.Add(newUnit);
                    guna2Panel1.Visible = false;
                }
                else
                {
                    if (guna2Button4.Tag is Guid editId)
                    {
                        var unit = CurrentProduct.Units.First(u => u.TempId == editId);
                        unit.UnitProductId = (int)cmdunit.SelectedValue;
                        unit.ConversionFactor = Convert.ToDecimal(txtzarib.Text);
                        unit.Title = txtonvan.Text;
                    }
                }
            }           
        }
        private void guna2Button4_Click(object sender, EventArgs e)
        {
           
                Creat();
            
            guna2Panel1.Visible = false;
            txtonvan.Text = "";
            txtzarib.Text = "";
            guna2Button4.Tag = null;
            RefreshGrid();
        }
        private async void UnitLevel_Load(object sender, EventArgs e)
        {
            //var firstBarcode = CurrentProduct.Units.FirstOrDefault()?
            //                 .Barcodes.FirstOrDefault()?
            //                 .Barcode;
            txtname.Text = CurrentProduct.Name;
           // txtbarcode.Text = firstBarcode;
            RefreshGrid();
            //Unit-Product
            var async4 = await _client.GetAll();
            cmdunit.DataSource = async4;
            cmdunit.DisplayMember = "Name";
            cmdunit.ValueMember = "Id";
            cmdunit.Font = new Font("Tahoma", 12, FontStyle.Regular);
            cmdunit.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        private void RefreshGrid()
        {
            guna2DataGridView1.Rows.Clear();

            foreach (var unit in _product.Units)
            {
                guna2DataGridView1.Rows.Add(
                    unit.TempId,
                    unit.Title,
                    unit.UnitProductId,
                    unit.ConversionFactor
                );
            }
        }
        private void guna2Button12_Click(object sender, EventArgs e)
        {
            if (txtonvan.Text.Trim().Length != 0 && txtzarib.Text.Trim().Length != 0)
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
        private DTO.Product.UnitsLevel? GetSelectedUnit()
        {
            if (guna2DataGridView1.SelectedRows.Count == 0)
                return null;

            var selectedId = (Guid)guna2DataGridView1.SelectedRows[0].Cells["TempId"].Value;
            return CurrentProduct.Units.FirstOrDefault(u => u.TempId == selectedId);
        }
        private void guna2Button3_Click(object sender, EventArgs e)
        {
            update = true;
            txtonvan.Text = "";
            txtzarib.Text = "";
            var unit = GetSelectedUnit();
            if (unit == null)
            {
                MessageBox.Show("هیچ ردیفی انتخاب نشده است.");
                return;
            }
            cmdunit.SelectedValue = unit.UnitProductId;
            txtzarib.Text = Convert.ToString(unit.ConversionFactor);
            txtonvan.Text = unit.Title;
            guna2Panel1.Visible = true;
            guna2Button4.Tag = unit.TempId;
        }
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            var unit = GetSelectedUnit();
            if (unit == null)
            {
                guna2MessageDialog1.Icon = MessageDialogIcon.Error;
                guna2MessageDialog1.Show("هیچ ردیفی انتخاب نشده است .", "نرم افزار حسابداری و انبارداری ");
                return;
            }

            guna2MessageDialog1.Buttons = MessageDialogButtons.YesNo;
            guna2MessageDialog1.Icon = MessageDialogIcon.Warning;
            DialogResult result = guna2MessageDialog1.Show("اطلاعات مد نظر لغو شود ؟", "نرم افزار نکسای");
            if (result == DialogResult.Yes)
            {
                CurrentProduct.Units.Remove(unit);
                RefreshGrid();
            }
        }
        private void guna2Button6_Click(object sender, EventArgs e)
        {
            if (update == false)
            {
                currentTempUnitId = Guid.NewGuid();
                Barcodes frm = new Barcodes(CurrentProduct);
                frm.lblid.Text = Convert.ToString(currentTempUnitId);
                frm.ShowDialog();
            }
            else
            {
                var selectedId = (Guid)guna2DataGridView1.SelectedRows[0].Cells["TempId"].Value;
                Barcodes frm = new Barcodes(CurrentProduct);
                frm.lblid.Text = Convert.ToString(selectedId);
                frm.ShowDialog();
            }
        }
    }
}
