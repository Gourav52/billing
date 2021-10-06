using AnyStore7.BLL;
using AnyStore7.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyStore7.UI
{
    public partial class frmCategories : Form
    {
        public frmCategories()
        {
            InitializeComponent();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        categoriesBLL c = new categoriesBLL();
        categoriesDAL dal = new categoriesDAL();
        userDAL udal = new userDAL();

        private void btnADD_Click(object sender, EventArgs e)
        {
            //Get The Values from Category form
            c.title = txtTitle.Text;
            c.description = txtDescription.Text;
            c.added_date = DateTime.Now;

            //Getting ID in Added by field
            string loggedUser = frmLogin.loggedIn;
            userBLL usr = udal.GetIDFromUsername(loggedUser);
            //passing the id of Logged in user in added by field
            c.added_by = usr.id;

            //Creating Boolean Method To insert Data Into Database
            bool success = dal.Insert(c);

            //if the category is inserted successfully then the value of the success will be true else it will be false
            if (success == true)
            {
                //NewCategories Inserted Successfully
                MessageBox.Show("New Category Inserted Successfully.");
                Clear();
                //Refreash data Grid View
                DataTable dt = dal.Select();
                dgvCategories.DataSource = dt;
            }
            else
            {
                //Failed to Insert New Category
                MessageBox.Show("Failed To Insert New Category.");
            }
        }
        public void Clear()
        {
            txtCategoryID.Text = "";
            txtTitle.Text = "";
            txtDescription.Text = "";
            txtSearch.Text = "";
        }

        private void frmCategories_Load(object sender, EventArgs e)
        {
            //Here Write the code to display all The categories in Data Grid View
            DataTable dt = dal.Select();
            dgvCategories.DataSource = dt;
        }

        private void dgvCategories_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //Finding the row index Of the row clicked on DataGridView
            int RowIndex = e.RowIndex;
            txtCategoryID.Text = dgvCategories.Rows[RowIndex].Cells[0].Value.ToString();
            txtTitle.Text = dgvCategories.Rows[RowIndex].Cells[1].Value.ToString();
            txtDescription.Text = dgvCategories.Rows[RowIndex].Cells[2].Value.ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //Get the values From the Catogory form
            c.id = int.Parse(txtCategoryID.Text);
            c.title = txtTitle.Text;
            c.description = txtDescription.Text;
            c.added_date = DateTime.Now;
            //Getting ID in Added by field
            string loggedUser = frmLogin.loggedIn;
            userBLL usr = udal.GetIDFromUsername(loggedUser);
            //passing the id of Logged in user in added by field
            c.added_by = usr.id;

            //Creating Boolean variable To update categories and Check
            bool success = dal.Update(c);
            //If the Category is updated successfully then the value of Success will be true else it will be false
            if (success == true)
            {
                //Category Updated Successfully
                MessageBox.Show("Category Updated Successfully.");
                Clear();
                //Refresh DGridView
                DataTable dt = dal.Select();
                dgvCategories.DataSource = dt;
            }
            else
            {
                //Failed To update Category
                MessageBox.Show("Failed To Update Category");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Get the Id of Category Which we want to Delete
            c.id = int.Parse(txtCategoryID.Text);

            //Creating Boolean variable to Delete the Category
            bool success = dal.Delete(c);

            //If the category is Deleted Successfully then the value of  success will be true else it will be false
            if (success==true)
            {
                //Category deleted Successfully
                MessageBox.Show("Category Deleted Successfully");
                Clear();
                //Refreshing DataGView
                DataTable dt = dal.Select();
                dgvCategories.DataSource = dt;
            }
            else
            {
                //Failed to Delete Category
                MessageBox.Show("Failed To Delete Category");
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //Get the Keywords
            string keywords = txtSearch.Text;

            //Filters the Categories based on Keywords
            if (keywords != null)
            {
                //Use Search Method to Display Categories
                DataTable dt = dal.Search(keywords);
                dgvCategories.DataSource = dt;

            }
            else
            {
                //Use Select Method To Display all Categories
                DataTable dt = dal.Select();
                dgvCategories.DataSource = dt;
            }
        }
    }
}
