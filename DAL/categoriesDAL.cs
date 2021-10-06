using AnyStore7.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyStore7.DAL
{
    class categoriesDAL
    {
        //Static String Method for Db Connection String
        static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

        #region Select Method
        public DataTable Select()
        {
            //Creating Database Connection
            SqlConnection conn = new SqlConnection(myconnstrng);

            DataTable dt = new DataTable();

            try
            {
                //Writing SQL Query To GET All The Data from Database
                string sql = "SELECT * FROM tbl_categories";

                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //open Db Connection
                conn.Open();
                //Adding the value from adapter to data table DT
                adapter.Fill(dt);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return dt;
        }
        #endregion
        #region Insert New Category
        public bool Insert(categoriesBLL c)
        {
            //Creating A Boolean Variable and set its default value to false
            bool isSuccess = false;

            //Creating to Database
            SqlConnection conn = new SqlConnection(myconnstrng);

            try
            {
                //Writing Query to Add new Category
                string sql = "INSERT INTO tbl_categories (title, description, added_date, added_by) VALUES (@title, @description, @added_date, @added_by)";

                //Creating SQL Command to pass values in our query
                SqlCommand cmd = new SqlCommand(sql, conn);
                //passing values through Parameter
                cmd.Parameters.AddWithValue("@title", c.title);
                cmd.Parameters.AddWithValue("@description", c.description);
                cmd.Parameters.AddWithValue("@added_date", c.added_date);
                cmd.Parameters.AddWithValue("@added_by", c.added_by);

                //Open DB connection
                conn.Open();

                //Creating the int Variable to execute query
                int rows = cmd.ExecuteNonQuery();

                //If the Query is Executed successfully then its value will be greater than 0 else it will be less than 0
                if (rows>0)
                {
                    //Query executed Successfully
                    isSuccess = true;
                }
                else
                {
                    //Failed to Execute Query
                    isSuccess = false;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //Closing the Db Connection
                conn.Close();
            }
            return isSuccess;
        }
        #endregion
        #region Update Method
        public bool Update(categoriesBLL c)
        {
            //Creating Boolean variable and set it deafault value to false
            bool isSuccess = false;

            //Creating Sql Connection
            SqlConnection conn = new SqlConnection(myconnstrng);

            try
            {
                //Query to update Category
                string sql = "UPDATE tbl_categories SET title=@title, description=@description, added_date=@added_date, added_by=@added_by WHERE id=@id";

                //sql command to pass the Value on sql Query
                SqlCommand cmd = new SqlCommand(sql, conn);

                //Passing value using cmd
                cmd.Parameters.AddWithValue("@title", c.title);
                cmd.Parameters.AddWithValue("@description", c.description);
                cmd.Parameters.AddWithValue("@added_date", c.added_date);
                cmd.Parameters.AddWithValue("@added_by", c.added_by);
                cmd.Parameters.AddWithValue("@id", c.id);

                //Open DB Connection
                conn.Open();

                //Create int variable to execute Query
                int rows = cmd.ExecuteNonQuery();

                //if the Query is successfully executed then the value will be greater than zero
                if (rows > 0)
                {
                    //Query Executed Successfully
                    isSuccess = true;
                }
                else
                {
                    //Failed to execute Query
                    isSuccess = false;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return isSuccess;
        }
        #endregion
        #region Delete Category Method
        public bool Delete(categoriesBLL c)
        {
            //Creating a boolean variable and set its value to false
            bool isSuccess = false;

            SqlConnection conn = new SqlConnection(myconnstrng);

            try
            {
                //SQL Query to Delete from DB
                string sql = "DELETE FROM tbl_categories WHERE id=@id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                //Passing the value using cmd
                cmd.Parameters.AddWithValue("@id", c.id);

                //open SqlConnection
                conn.Open();

                int rows = cmd.ExecuteNonQuery();

                //if the query is executed Successfully then the value of rows will be greater than zero else it will be less than 0
                if (rows > 0)
                {
                    //Query Executed Successfully
                    isSuccess = true;
                }
                else
                {
                    //Failed to Execute Query
                    isSuccess = false;

                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return isSuccess;
        }
        #endregion
        #region Method For Search Functionality
        public DataTable Search(string keywords)
        {
            //Sql Connection for Database Connection
            SqlConnection conn = new SqlConnection(myconnstrng);

            //Creating Datatable to hold the data from db temporarily
            DataTable dt = new DataTable();

            try
            {
                //Sql Query to Search Categories from DB
                String sql = "SELECT * FROM tbl_categories WHERE id LIKE'%"+keywords+"%' OR title LIKE '%"+keywords+"%' OR description LIKE '%"+keywords+"%'";
                //Creating SQl Command to Execute the Query
                SqlCommand cmd = new SqlCommand(sql, conn);

                //Getting Data From Database
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                //Open DataBaseConnection
                conn.Open();
                //Passing Values From adapter to Datatable dt
                adapter.Fill(dt);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
           

            return dt;
        }
        #endregion
    }
}
