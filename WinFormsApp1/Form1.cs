namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Bank.bankt frm = new Bank.bankt();
            //frm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Bank.bank frm = new Bank.bank();
            //frm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Fund.fund frm = new Fund.fund();
            //frm.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Fund.fundtouser frm = new Fund.fundtouser();
            //frm.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //Fund.shift frm = new Fund.shift();
            //frm.ShowDialog();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2Button47_Click(object sender, EventArgs e)
        {
            Settings.Basic_Data basic = new Settings.Basic_Data();
            basic.ShowDialog();
        }

        private void guna2Button14_Click(object sender, EventArgs e)
        {
            Product.Product frm = new Product.Product();
            frm.ShowDialog();
        }

        private void guna2Button15_Click(object sender, EventArgs e)
        {
            Product.Storeroom_Product frm = new Product.Storeroom_Product();
            frm.ShowDialog();
        }

        private void guna2Button35_Click(object sender, EventArgs e)
        {
            Bank.Bnak frm = new Bank.Bnak();
            frm.ShowDialog();
        }

        private void guna2Button40_Click(object sender, EventArgs e)
        {
            Fund.Fund frm = new Fund.Fund();
            frm.ShowDialog();
        }

        private void guna2Button43_Click(object sender, EventArgs e)
        {
            Fund.Cash_Register_To_The_User frm = new Fund.Cash_Register_To_The_User();
            frm.ShowDialog();
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            Fund.Work_Shift frm = new Fund.Work_Shift();
            frm.ShowDialog();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Invoices.Sales_invoice frm = new Invoices.Sales_invoice();
            frm.ShowDialog();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Invoices.purchase_invoice frm = new Invoices.purchase_invoice();
            frm.ShowDialog();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            Invoices.Sales_return_invoice frm = new Invoices.Sales_return_invoice();
            frm.ShowDialog();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            Invoices.Purchase_return_invoice frm = new Invoices.Purchase_return_invoice();
            frm.ShowDialog();
        }

        private void guna2Button24_Click(object sender, EventArgs e)
        {
            People.People frm = new People.People();
            frm.ShowDialog();
        }

        private void guna2Button16_Click(object sender, EventArgs e)
        {
            Product.Product_Batch_Changes frm = new Product.Product_Batch_Changes();
            frm.ShowDialog();
        }

        private void guna2Button48_Click(object sender, EventArgs e)
        {
            Settings.User frm = new Settings.User();
            frm.ShowDialog();
        }

        private void guna2Button49_Click(object sender, EventArgs e)
        {
            Settings.Access_Level frm = new Settings.Access_Level();
            frm.ShowDialog();
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            Invoices.Pre_Invoice frm = new Invoices.Pre_Invoice();
            frm.ShowDialog();
        }

        private void guna2Button11_Click(object sender, EventArgs e)
        {
            Invoices.Order frm = new Invoices.Order();
            frm.ShowDialog();
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            Invoices.Sales_Invoice_R frm = new Invoices.Sales_Invoice_R();
            frm.ShowDialog();
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            Invoices.Purchase_Invoice_R frm = new Invoices.Purchase_Invoice_R();
            frm.ShowDialog();
        }

        private void guna2Button9_Click(object sender, EventArgs e)
        {
            Invoices.Sales_Return_Invoice_R frm = new Invoices.Sales_Return_Invoice_R();
            frm.ShowDialog();
        }

        private void guna2Button10_Click(object sender, EventArgs e)
        {
            Invoices.Purchase_Return_Invoice_R frm = new Invoices.Purchase_Return_Invoice_R();
            frm.ShowDialog();
        }

        private void guna2Button13_Click(object sender, EventArgs e)
        {
            Invoices.Pre_Invoice_R frm = new Invoices.Pre_Invoice_R();
            frm.ShowDialog();
        }

        private void guna2Button19_Click(object sender, EventArgs e)
        {
            Invoices.Order_R frm = new Invoices.Order_R();
            frm.ShowDialog();
        }

        private void guna2Button12_Click(object sender, EventArgs e)
        {
            Invoices.Receipts_and_Payments frm = new Invoices.Receipts_and_Payments();
            frm.ShowDialog();
        }

        private void guna2Button26_Click(object sender, EventArgs e)
        {
            Invoices.Daily_Report_Summary frm = new Invoices.Daily_Report_Summary();
            frm.ShowDialog();
        }

        private void guna2Button27_Click(object sender, EventArgs e)
        {
            Product.Off_Product frm = new Product.Off_Product();
            frm.ShowDialog();
        }

        private void guna2Button61_Click(object sender, EventArgs e)
        {
            Product.Product_Failure frm = new Product.Product_Failure();
            frm.ShowDialog();
        }

        private void guna2Button17_Click(object sender, EventArgs e)
        {
            Product.Inventory_Report frm = new Product.Inventory_Report();
            frm.ShowDialog();
        }

        private void guna2Button18_Click(object sender, EventArgs e)
        {
            Product.Kardex_Product_Report frm = new Product.Kardex_Product_Report();
            frm.ShowDialog();
        }

        private void guna2Button20_Click(object sender, EventArgs e)
        {
            Product.Product_Order_Point_R frm = new Product.Product_Order_Point_R();
            frm.ShowDialog();
        }

        private void guna2Button21_Click(object sender, EventArgs e)
        {
            Product.Product_Transaction_R frm = new Product.Product_Transaction_R();
            frm.ShowDialog();
        }

        private void guna2Button22_Click(object sender, EventArgs e)
        {
            Product.Monthly_Product_R frm = new Product.Monthly_Product_R();
            frm.ShowDialog();
        }

        private void guna2Button23_Click(object sender, EventArgs e)
        {
            Product.Quarterly_Product_Report frm = new Product.Quarterly_Product_Report();
            frm.ShowDialog();
        }

        private void guna2Button53_Click(object sender, EventArgs e)
        {
            Product.Detailed_Product_Discount_R frm = new Product.Detailed_Product_Discount_R();
            frm.ShowDialog();
        }

        private void guna2Button62_Click(object sender, EventArgs e)
        {
            Product.Product_Return_Report frm = new Product.Product_Return_Report();
            frm.ShowDialog();
        }

        private void guna2Button54_Click(object sender, EventArgs e)
        {
            People.Send_SMS frm = new People.Send_SMS();
            frm.ShowDialog();
        }

        private void guna2Button28_Click(object sender, EventArgs e)
        {
          
        }

        private void guna2Button25_Click(object sender, EventArgs e)
        {
            People.Settlement_People frm = new People.Settlement_People();
            frm.ShowDialog();
        }

        private void guna2Button29_Click(object sender, EventArgs e)
        {
            People.SMS_Management frm = new People.SMS_Management();
            frm.ShowDialog();
        }

        private void guna2Button30_Click(object sender, EventArgs e)
        {
            People.People_Report frm = new People.People_Report();
            frm.ShowDialog();
        }

        private void guna2Button31_Click(object sender, EventArgs e)
        {
            People.Detailed_Report_People frm = new People.Detailed_Report_People();
            frm.ShowDialog();
        }

        private void guna2Button36_Click(object sender, EventArgs e)
        {
            Bank.Bank_To_Bank frm = new Bank.Bank_To_Bank();
            frm.ShowDialog();
        }

        private void guna2Button37_Click(object sender, EventArgs e)
        {
            Bank.Pay_To_Bank frm = new Bank.Pay_To_Bank();
            frm.ShowDialog();
        }

        private void guna2Button38_Click(object sender, EventArgs e)
        {
            Bank.Detailed_Bnak_R frm = new Bank.Detailed_Bnak_R();
            frm.ShowDialog();
        }

        private void guna2Button39_Click(object sender, EventArgs e)
        {
            Bank.Deposit_Slip_R frm = new Bank.Deposit_Slip_R();
            frm.ShowDialog();
        }

        private void guna2Button57_Click(object sender, EventArgs e)
        {
            Bank.Card_Reader__R frm = new Bank.Card_Reader__R();
            frm.ShowDialog();
        }

        private void guna2Button46_Click(object sender, EventArgs e)
        {
            Settings.System_Settings frm = new Settings.System_Settings();
            frm.ShowDialog();
        }

        private void guna2Button45_Click(object sender, EventArgs e)
        {
            Fund.Bank_and_Fund_Inventory frm = new Fund.Bank_and_Fund_Inventory();
            frm.ShowDialog();
        }

        private void guna2Button32_Click(object sender, EventArgs e)
        {
            People.Sales_People_R frm = new People.Sales_People_R();
            frm.ShowDialog();
        }

        private void guna2Button33_Click(object sender, EventArgs e)
        {
            People.Purchase_Peopel_R frm = new People.Purchase_Peopel_R();
            frm.ShowDialog();
        }

        private void guna2Button34_Click(object sender, EventArgs e)
        {
            People.Summary__People_R frm = new People.Summary__People_R();
            frm.ShowDialog();
        }

        private void guna2Button55_Click(object sender, EventArgs e)
        {
            People.Send_SMS_R frm = new People.Send_SMS_R();
            frm.ShowDialog();
        }

        private void guna2Button56_Click(object sender, EventArgs e)
        {
            People.Logs_SMS frm = new People.Logs_SMS();
            frm.ShowDialog();
        }

        private void guna2Button41_Click(object sender, EventArgs e)
        {
            Fund.Fund_To_Fund frm = new Fund.Fund_To_Fund();
            frm.ShowDialog();
        }

        private void guna2Button42_Click(object sender, EventArgs e)
        {
            Fund.Bank_To_Fund frm = new Fund.Bank_To_Fund();
            frm.ShowDialog();
        }

        private void guna2Button44_Click(object sender, EventArgs e)
        {
            Fund.Fund_Detailed_R frm = new Fund.Fund_Detailed_R();
            frm.ShowDialog();
        }

        private void guna2Button50_Click(object sender, EventArgs e)
        {
            Settings.Access_Level frm = new Settings.Access_Level();
            frm.ShowDialog();
        }

        private void guna2Button51_Click(object sender, EventArgs e)
        {
            Settings.Print_line_Settings frm = new Settings.Print_line_Settings();
            frm.ShowDialog();
        }

        private void guna2Button52_Click(object sender, EventArgs e)
        {
            Settings.Backup frm = new Settings.Backup();
            frm.ShowDialog();
        }

        private void guna2Button58_Click(object sender, EventArgs e)
        {
            Settings.Recovery frm = new Settings.Recovery();
            frm.ShowDialog();
        }

        private void guna2Button59_Click(object sender, EventArgs e)
        {
            Settings.Computer_Information frm = new Settings.Computer_Information();
            frm.ShowDialog();
        }

        private void guna2Button60_Click(object sender, EventArgs e)
        {
            Settings.System_Operational_R frm = new Settings.System_Operational_R();
            frm.ShowDialog();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
