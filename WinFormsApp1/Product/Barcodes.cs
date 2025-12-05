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
    public partial class Barcodes : Form
    {
        private DTO.Product.ProductDto CurrentProduct;
        private Guid _selectedBarcodeId = Guid.Empty;
        public Barcodes(DTO.Product.ProductDto product)
        {
            InitializeComponent();
            CurrentProduct = product;
        }
        bool update = false;
        private void guna2Button11_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void Creat ()
        {
            DTO.Product.UnitsLevel selectedUnit;
            if (txtbarcode.Text.Trim().Length == 0)
            {
                guna2MessageDialog1.Icon = MessageDialogIcon.Error;
                guna2MessageDialog1.Show("لطفا اطلاعات خالی را پرکنید .", "نرم افزار حسابداری و انبارداری ");
            }
            else
            {
                if(update == false)
                {
                    //if (!string.IsNullOrWhiteSpace(lblid.Text) && Guid.TryParse(lblid.Text, out Guid unitId))
                    //{
                       
                    //        MessageBox.Show("واحد انتخاب شده معتبر نیست.");
                    //        return;
                        
                    //}
                    //else
                    //{
                        selectedUnit = CurrentProduct.Units.FirstOrDefault();
                        if (selectedUnit == null)
                        {
                            MessageBox.Show("واحد پیش‌فرض موجود نیست.");
                            return;
                        }
                   // }
                    if (CurrentProduct.Units.Any(u => u.Barcodes.Any(b => b.Barcode == txtbarcode.Text)))
                    {
                        MessageBox.Show("این بارکد قبلاً ثبت شده است.");
                        return;
                    }
                    if (_selectedBarcodeId == Guid.Empty)
                    {
                        selectedUnit.Barcodes.Add(new DTO.Product.ProductBarcodes
                        {
                            TempId = Guid.NewGuid(),
                            Barcode = txtbarcode.Text
                        });
                    }
                }
                else
                {
                    var result = CurrentProduct.Units
                  .SelectMany(u => u.Barcodes, (unit, barcode) => new { unit, barcode })
                  .FirstOrDefault(x => x.barcode.TempId == _selectedBarcodeId);
                    if (result == null)
                    {
                        MessageBox.Show("رکورد بارکد برای ویرایش پیدا نشد.");
                        _selectedBarcodeId = Guid.Empty;
                        return;
                    }
                    if (CurrentProduct.Units.Any(u => u.Barcodes.Any(b => b.Barcode == txtbarcode.Text && b.TempId != _selectedBarcodeId)))
                    {
                        MessageBox.Show("این بارکد قبلاً ثبت شده است.");
                        return;
                    }
                    result.barcode.Barcode = txtbarcode.Text;
                    _selectedBarcodeId = Guid.Empty;
                }

            }
            txtbarcode.Clear();
            RefreshBarcodeGrid();
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Creat();   
        }
        private void RefreshBarcodeGrid()
        {
            guna2DataGridView1.Rows.Clear();

            foreach (var unit in CurrentProduct.Units)
            {
                foreach (var barcode in unit.Barcodes)
                {
                    guna2DataGridView1.Rows.Add(
                        barcode.TempId,
                        barcode.Barcode,
                        unit.Title
                    );
                }
            }
        }
        private void guna2Button12_Click(object sender, EventArgs e)
        {
            if (txtbarcode.Text.Trim().Length != 0)
            {
                Creat();
            }
            else
            {
                this.Close();
            }      
        }
        private void Barcodes_Load(object sender, EventArgs e)
        {
            RefreshBarcodeGrid();
        }
        private void guna2Button3_Click(object sender, EventArgs e)
        {
            update = true;
            if (guna2DataGridView1.CurrentRow == null) return;

            _selectedBarcodeId = (Guid)guna2DataGridView1.CurrentRow.Cells["TempId"].Value;

            var selectedUnit = CurrentProduct.Units.First(u => u.Barcodes.Any(p => p.TempId == _selectedBarcodeId));
            var priceToEdit = selectedUnit.Barcodes.First(p => p.TempId == _selectedBarcodeId);
            txtbarcode.Text = priceToEdit.Barcode;
        }
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView1.CurrentRow == null)
                return;

            Guid selectedId = (Guid)guna2DataGridView1.CurrentRow.Cells["TempId"].Value;

            var unitWithPrice = CurrentProduct.Units
                .FirstOrDefault(u => u.Barcodes.Any(p => p.TempId == selectedId));

            if (unitWithPrice != null)
            {
                var priceToRemove = unitWithPrice.Barcodes.First(p => p.TempId == selectedId);
                unitWithPrice.Barcodes.Remove(priceToRemove);

                RefreshBarcodeGrid();
            }
        }
    }
}
