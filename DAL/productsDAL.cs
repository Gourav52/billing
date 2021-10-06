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
    class productsDAL
    {
        //Creating Static String Method for DB Connection
        static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

        #region Select Method For Product Module
        public DataTable Select()
        {
            //Create Sql Connection to Connect Db
            SqlConnection conn = new SqlConnection(myconnstrng);

            //DataTable to hold the data From DB
            DataTable dt = new DataTable();

            try
            {
                //Writing The Query to Select all the Products From DB
                String sql = "SELECT * FROM tbl_products";

                //Creating A Command to Execute Query
                SqlCommand cmd = new SqlCommand(sql, conn);

                //SQL Data Adapter to Hold the value  from DataBase temporarily
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                //Open Db Connection
                conn.Open();

                adapter.Fill(dt);
            }
            catch (Exception ex)
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
        #region Method To INSERT product In DB
        public bool Insert(productsBLL p)
        {
            //Creating Boolean Variable and Set its Default Value to false
            bool isSuccess = false;

            //SQL Connection for DB
            SqlConnection conn = new SqlConnection(myconnstrng);

            try
            {
                //Sql Query to insert products to db
                String sql = "INSERT INTO tbl_products (name, category, description, rate, qty, added_date, added_by) VALUES (@name, @category, @description, @rate, @qty, @added_date, @added_by)";

                //Creating Sql Command To pass The Values
                SqlCommand cmd = new SqlCommand(sql, conn);

                //Passing The Values Through Parameters
                cmd.Parameters.AddWithValue("@name", p.name);
                cmd.Parameters.AddWithValue("@category", p.category);
                cmd.Parameters.AddWithValue("@description", p.description);
                cmd.Parameters.AddWithValue("@rate", p.rate);
                cmd.Parameters.AddWithValue("@qty", p.qty);
                cmd.Parameters.AddWithValue("@added_date", p.added_date);
                cmd.Parameters.AddWithValue("@added_by", p.added_by);

                //opening Db Connection
                conn.Open();

                int rows = cmd.ExecuteNonQuery();

                //If the query Is Executed Successfully Then the Value of Rows will be greater than 0 else it will be less than 0
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
        #region Method to Update Product in Database
        public bool Update(productsBLL p)
        {
            //Create a Boolean variable and set its initial value false
            bool isSuccess = false;

            //Create Sql Connection for DB
            SqlConnection conn = new SqlConnection(myconnstrng);


            try
            {
                //Sql Query to Update data in Db
                String sql = "UPDATE tbl_products SET name=@name, category=@category, description=@description, rate=@rate, added_date=@added_date, added_by=@added_by WHERE id=@id";

                //Create Sql Command to Pass the value to Query
                SqlCommand cmd = new SqlCommand(sql, conn);
                //Passing The Values Using parameters and Cmd
                cmd.Parameters.AddWithValue("@name", p.name);
                cmd.Parameters.AddWithValue("@category", p.category);
                cmd.Parameters.AddWithValue("@description", p.description);
                cmd.Parameters.AddWithValue("@rate", p.rate);
                cmd.Parameters.AddWithValue("@qty", p.qty);
                cmd.Parameters.AddWithValue("@added_date",p.added_date);
                cmd.Parameters.AddWithValue("@added_by", p.added_by);
                cmd.Parameters.AddWithValue("@id", p.id);

                //Open the db connection
                conn.Open();

                //Create INt Var To check if the query is executed Successfully or not
                int rows = cmd.ExecuteNonQuery();

                //if the query is executed Successfully then the value of rows will be greater than 0 else it will be less than 0
                if (rows > 0)
                {
                    //Query ExecutedSuccessfully
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
        #region Method To Delete Product from DB
        public bool Delete(productsBLL p)
        {
            //Create Boolean Var and Set its default Value to false
            bool isSuccess = false;

            //Sql conn for DB Connection
            SqlConnection conn = new SqlConnection(myconnstrng);

            try
            {
                //Write Query product from Db
                String sql = "DELETE tbl_products WHERE id=@id";

                //sql Command to pass the value
                SqlCommand cmd = new SqlCommand(sql, conn);

                //passing the values using cmd
                cmd.Parameters.AddWithValue("@id", p.id);

                //open DB connection
                conn.Open();

                int rows = cmd.ExecuteNonQuery();
                //if the query is executed successfully then the value of rows will be Greater than 0 else it will be less than 0
                if (rows > 0)
                {
                    //Query Executed Successfully
                    isSuccess = true;
                }
                else
                {
                    //Failed To Execute Query
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
        #region Search Method for product Module
        public DataTable Search(string keywords)
        {
            //Sql Conn 4 db conn
            SqlConnection conn = new SqlConnection(myconnstrng);
            //Creating DataTable To hold value from db
            DataTable dt = new DataTable();

            try
            {
                //Sql Query to Search the products
                string sql = "SELECT * FROM tbl_products WHERE id LIKE '%"+keywords+"%' OR name LIKE '%"+keywords+"%' OR category LIKE '%"+keywords+"%'";
                SqlCommand cmd = new SqlCommand(sql, conn);

                //Sql data adapter to hold the data from db Temperorily
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                //open db Conn
                conn.Open();

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
        #region Method To Search Product IN Transaction Module
        public productsBLL GetProductsForTransaction(string keyword)
        {
            //Create an OBject of productsBLL and return It
            productsBLL p = new productsBLL();
            //SqlConn
            SqlConnection conn = new SqlConnection(myconnstrng);
            //DT to store data temp
            DataTable dt = new DataTable();

            try
            {
                //Write the Query to Get the Details
                string sql = "SELECT name, rate, qty FROM tbl_products WHERE id LIKE '%"+keyword+"%' OR name LIKE '%"+keyword+"%'";
                //Create Sql DataAdapter to execute the query
                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);

                //Open DB COnn
                conn.Open();

                //Pass the val frm adapter to dt
                adapter.Fill(dt);

                //If we have any val on dt then set the val to productsBLL
                if (dt.Rows.Count > 0)
                {
                    p.name = dt.Rows[0]["name"].ToString();
                    p.rate = decimal.Parse(dt.Rows[0]["rate"].ToString());
                    p.qty = decimal.Parse(dt.Rows[0]["qty"].ToString());
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //Close DB conn
                conn.Close();
            }

            return p;
        }
        #endregion
        #region Method to Get ProductID Based On Product Name
        public productsBLL GetProductIDFromName(string ProductName)
        {
            //1st Create an obj of the Deacust Bll And Return It
            productsBLL p = new productsBLL();


            //Sql Conn here
            SqlConnection conn = new SqlConnection(myconnstrng);
            //DataT to hold the data temp
            DataTable dt = new DataTable();

            try
            {
                //Sql Query to get id based on name
                string sql = "SELECT id FROM tbl_products WHERE name='" + ProductName + "'";
                //create the sql dataadapter to execute the Query
                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);

                conn.Open();

                //Passing the val from adapter to DT
                adapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    //Pass the val from dt to DeaCustBll dc
                    p.id = int.Parse(dt.Rows[0]["id"].ToString());
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return p;
        }
        #endregion
        #region Method to Get Current Qty Frm DB Based on ProductID
        public decimal GetProductQty(int ProductID)
        {
            //Sql Conn 1st
            SqlConnection conn = new SqlConnection(myconnstrng);
            //create a deci var and set its default val to 0
            decimal qty = 0;

            //Crea DT to save The Data Frm Db temp
            DataTable dt = new DataTable();

            try
            {
                //Write sql query to get qty frm db
                string sql = "SELECT qty FROM tbl_products WHERE id = "+ProductID;

                //create a sqlcmd
                SqlCommand cmd = new SqlCommand(sql, conn);

                //Create a sqldataAdapter to Execute the query
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                //open db
                conn.Open();

                //pass the val frm dataadapter to DT
                adapter.Fill(dt);

                //Lets check if the DT has val or not 
                if (dt.Rows.Count > 0)
                {
                    qty = decimal.Parse(dt.Rows[0]["qty"].ToString());
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

            return qty;
        }
        #endregion
        #region Method To Update QTY 
        public bool UpdateQuantity(int ProductID, decimal Qty)
        {
            //Create a boolean var and set its val to false
            bool success = false;

            //sql con to connect Db
            SqlConnection conn = new SqlConnection(myconnstrng);

            try
            {
                //Write the sql query to update qty
                string sql = "UPDATE tbl_products SET qty=@qty WHERE id=@id";

                //create sqlcmd to pass the val into query
                SqlCommand cmd = new SqlCommand(sql, conn);
                //passing val thrg paramtrs
                cmd.Parameters.AddWithValue("@qty", Qty);
                cmd.Parameters.AddWithValue("@id", ProductID);

                conn.Open();

                //Create int var and check wheather the query is executed succesfully or not
                int rows = cmd.ExecuteNonQuery();
                //Lets check if the query is executed successfully or not
                if (rows > 0)
                {
                    success = true;
                }
                else
                {
                    success = false;
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
            return success;
        }
        #endregion
        #region Method to INCREASE Product
        public bool IncreaseProduct(int ProductID, decimal IncreaseQty)
        {

            bool success = false;

            //Create sql connection to conn DB
            SqlConnection conn = new SqlConnection(myconnstrng);

            try
            {
                // Create the current qty to db based on id
                decimal currentQty = GetProductQty(ProductID);

                //Increase the current qty by the Qty purchased from Dealer
                decimal NewQty = currentQty + IncreaseQty;

                //Update the pro qty now
                success = UpdateQuantity(ProductID, NewQty);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return success;

        }
        #endregion
        #region Method to Decrease Product
        public bool DecreaseProduct(int ProductID, decimal Qty)
        {
            bool success = false;

            SqlConnection conn = new SqlConnection(myconnstrng);

            try
            {
                //Get the current product qty
                decimal currentQty = GetProductQty(ProductID);

                //Dec the pro qty based on pro sales
                decimal NewQty = currentQty - Qty;

                //Update pro in db
                success = UpdateQuantity(ProductID, NewQty);

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return success;
        }
        #endregion
        #region DESPLAY PRODUCTS BASED ON CATEGORIES
        public DataTable DisplayProductsByCategory(string category)
        {
            //Sql Connection First
            SqlConnection conn = new SqlConnection(myconnstrng);

            DataTable dt = new DataTable();

            try
            {
                //SQL Query to Display Product Based on CAtegory
                string sql = "SELECT * FROM tbl_products WHERE category='" + category + "'";

                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                //Open Database Connection Here
                conn.Open();

                adapter.Fill(dt);
            }
            catch (Exception ex)
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
