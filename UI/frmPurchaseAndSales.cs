using AnyStore7.BLL;
using AnyStore7.DAL;
using DGVPrinterHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace AnyStore7.UI
{
    public partial class frmPurchaseAndSales : Form
    {
        public frmPurchaseAndSales()
        {
            InitializeComponent();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        DeaCustDAL dcDAL = new DeaCustDAL();
        productsDAL pDAL = new productsDAL();
        userDAL uDAl = new userDAL();
        transactionDAL tDAL = new transactionDAL();
        transactionDetailDAL tdDAL = new transactionDetailDAL();

        DataTable transactionDT = new DataTable();
        private void frmPurchaseAndSales_Load(object sender, EventArgs e)
        {
            //Get the Transaction Type value from UserDashboard
            string type = frmUserDashboard.transactionType;
            //set the val on lblTop
            lblTop.Text = type;

            //Specify Colums for our TransactionDataTable
            transactionDT.Columns.Add("Product Name");
            transactionDT.Columns.Add("Rate");
            transactionDT.Columns.Add("Quantity");
            transactionDT.Columns.Add("Total");
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //Get the keyword frm the txtbox
            string keyword = txtSearch.Text;

            if (keyword == "")
            {
                //Clear all the txtboxes
                txtName.Text = "";
                txtEmail.Text = "";
                txtContact.Text = "";
                txtAddress.Text = "";
                return;
            }

            //Write the code to get details and set the val on txtboxes
            DeaCustBLL dc = dcDAL.SearchDealerCustomerForTransaction(keyword);

            //Now transfer or set the val from deacustBLL to txtbxes
            txtName.Text = dc.name;
            txtEmail.Text = dc.email;
            txtContact.Text = dc.contact;
            txtAddress.Text = dc.address;
        }

        private void txtSearchProduct_TextChanged(object sender, EventArgs e)
        {
            //Get the keyword from productSearchtxtbox
            string keyword = txtSearchProduct.Text;

            //Check if we have val on txtsearch pro or not
            if (keyword == "")
            {
                txtProductName.Text = "";
                txtInventory.Text = "";
                txtRate.Text = "";
                txtQty.Text = "";
                return;
            }

            //search the pro and display on respective txtboxes
            productsBLL p = pDAL.GetProductsForTransaction(keyword);

            //Set the val on txt boxes based on p obj
            txtProductName.Text = p.name;
            txtInventory.Text = p.qty.ToString();
            txtRate.Text = p.rate.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //get product name,rate,qty cus wants to buy
            string productName = txtProductName.Text;
            decimal Rate=decimal.Parse(txtRate.Text);
            decimal Qty = decimal.Parse(txtQty.Text);

            decimal Total = Rate * Qty; // Total=Rate x Qty

            //Display the Sub Total in textBox
            //Get the Total Val from txtBox
            decimal subTotal = decimal.Parse(txtSubTotal.Text);
            subTotal = subTotal + Total;

            //Check wheather the pro is selected or not
            if (productName == "")
            {
                //Display a Error Message
                MessageBox.Show("Select The Product First. Try Again..");
            }
            else
            {
                //Add pro to the dgView
                transactionDT.Rows.Add(productName,Rate,Qty,Total);

                //Show in dgv
                dgvAddedProducts.DataSource = transactionDT;
                //Display the Subtotal In Textbox
                txtSubTotal.Text = subTotal.ToString();

                //Clear th txtbox
                txtSearchProduct.Text = "";
                txtProductName.Text = "";
                txtInventory.Text = "0.00";
                txtRate.Text = "0.00";
                txtQty.Text = "0.00";

            }
        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            //Get the val From discount txtb
            string value = txtDiscount.Text;

            if (value == "")
            {
                //Dispalay Error Message
                MessageBox.Show("Please Add Discount First");
            }
            else
            {
                //Get the Discount in decimal val
                decimal subTotal = decimal.Parse(txtSubTotal.Text);
                decimal discount = decimal.Parse(txtDiscount.Text);

                //Calculate The GrandTotal BAsed on discount
                decimal grandTotal = ((100 - discount) / 100) * subTotal;

                //display the grand total in txtbox
                txtGrandTotal.Text = grandTotal.ToString();
            }
        }

        private void txtVat_TextChanged(object sender, EventArgs e)
        {
            //Check if the Grand Total Has Val or Not if it has not val then calcu the disc 1st
            string check = txtGrandTotal.Text;
            if (check == "")
            {
                //Display the error message to calculate dis
                MessageBox.Show("Calculate the Discount and Set the Grand Total First.");
            }
            else
            {
                //Calculate vat
                //Getting the Vat % first
                decimal previousGT = decimal.Parse(txtGrandTotal.Text);
                decimal vat = decimal.Parse(txtVat.Text);
                decimal grandTotalWithVAT=((100+vat)/100)*previousGT;

                //Display new GT wth VAT
                txtGrandTotal.Text = grandTotalWithVAT.ToString();
            }
        }

        private void txtPaidAmount_TextChanged(object sender, EventArgs e)
        {
            //Get the Paid amount And grandT
            decimal grandTotal = decimal.Parse(txtGrandTotal.Text);
            decimal paidAmount = decimal.Parse(txtPaidAmount.Text);

            decimal returnAmount = paidAmount - grandTotal;

            //Display the return amt as well
            txtReturnAmount.Text = returnAmount.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //Get The Val From purchaseSales form 1st
            transactionsBLL transaction = new transactionsBLL();

            transaction.type = lblTop.Text;

            //Get the id of dealer or customer Here
            //Lets get name of the Dealer or customer 1st
            string deaCustName = txtName.Text;
            DeaCustBLL dc = dcDAL.GetDeaCustIDFromName(deaCustName);

            transaction.dea_cust_id = dc.id;
            transaction.grandTotal = Math.Round(decimal.Parse(txtGrandTotal.Text),2);
            transaction.transaction_date = DateTime.Now;
            transaction.tax=decimal.Parse(txtVat.Text);
            transaction.discount = decimal.Parse(txtDiscount.Text);

            //Get the userName of Logged in user
            string username = frmLogin.loggedIn;
            userBLL u = uDAl.GetIDFromUsername(username);

            transaction.added_by = u.id;
            transaction.transactionDetails = transactionDT;

            //Lets Create a boolean var and set its val to false
            bool success = false;

            //Actual code to insert Transaction and transaction Details
            using(TransactionScope scope=new TransactionScope())
            {
                int transactionID = -1;
                //Create a boolean val and insert transaction
                bool w = tDAL.Insert_Transaction(transaction, out transactionID);

                //Use 4loop to insert transactiondetails
                for(int i=0; i<transactionDT.Rows.Count; i++)
                {
                    //Get all the Details of the product
                    transactionDetailBLL transactionDetail = new transactionDetailBLL();
                    //Get the product name and convert it to id
                    string ProductName = transactionDT.Rows[i][0].ToString();
                    productsBLL p = pDAL.GetProductIDFromName(ProductName);

                    transactionDetail.product_id = p.id;
                    transactionDetail.rate = decimal.Parse(transactionDT.Rows[i][1].ToString());
                    transactionDetail.qty = decimal.Parse(transactionDT.Rows[i][2].ToString());
                    transactionDetail.total = Math.Round(decimal.Parse(transactionDT.Rows[i][3].ToString()),2);
                    transactionDetail.dea_cust_id= dc.id;
                    transactionDetail.added_date = DateTime.Now;
                    transactionDetail.added_by = u.id;

                    //Here Increase or Decrease product qty Based on purchase or sales
                    string transactionType = lblTop.Text;

                    //Lets check we are on purchase or sales
                    bool x=false;
                    if (transactionType == "Purchase")
                    {
                        //Increase the prod
                         x = pDAL.IncreaseProduct(transactionDetail.product_id, transactionDetail.qty);
                    }
                    else if (transactionType == "Sales")
                    {
                        //Decrease the pro qty
                        x = pDAL.DecreaseProduct(transactionDetail.product_id, transactionDetail.qty);
                    }

                    //Insert transaction detail inside db
                    bool y = tdDAL.InsertTransactionDetail(transactionDetail);
                    success = w && x && y;
                }
                
                if (success == true)
                {
                    //Transaction Complete
                    scope.Complete();

                    //Code to Print Bill
                    DGVPrinter printer = new DGVPrinter();

                    printer.Title = "\r\n\r\n\r\n TINOS SOFTWARE AND SECURITY SOLUTIONS \r\n\r\n";
                    printer.SubTitle = "ERNAKULAM, Kerala \r\n Phone: 01-045XXXX \r\n\r\n";
                    printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
                    printer.PageNumbers = true;
                    printer.PageNumberInHeader = false;
                    printer.PorportionalColumns = true;
                    printer.HeaderCellAlignment = StringAlignment.Near;
                    printer.Footer = "Discount: " + txtDiscount.Text + "% \r\n" + "VAT: " + txtVat.Text + "% \r\n" + "Grand Total: " + txtGrandTotal.Text + "\r\n\r\n" + "Thank you for doing business with us.";
                    printer.FooterSpacing = 15;
                    printer.PrintDataGridView(dgvAddedProducts);

                    MessageBox.Show("Transaction Completed Succcessfully");
                    //Clear the gdv and Clear all the txtboxes
                    dgvAddedProducts.DataSource = null;
                    dgvAddedProducts.Rows.Clear();

                    txtSearch.Text = "";
                    txtName.Text = "";
                    txtEmail.Text = "";
                    txtContact.Text = "";
                    txtAddress.Text = "";
                    txtSearchProduct.Text = "";
                    txtProductName.Text = "";
                    txtInventory.Text = "0";
                    txtRate.Text = "0";
                    txtQty.Text = "0";
                    txtSubTotal.Text = "0";
                    txtDiscount.Text = "0";
                    txtVat.Text = "0";
                    txtGrandTotal.Text="0";
                    txtPaidAmount.Text = "0";
                    txtReturnAmount.Text = "0";
                }
                else
                {
                    //Transaction Failed
                    MessageBox.Show("Transaction Failed");
                }
            }
        }
    }
}
