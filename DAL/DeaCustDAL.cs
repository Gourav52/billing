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
    class DeaCustDAL
    {
        //Static String Method for DB conn
        static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

        #region Select Method for dealer and customer
        public DataTable Select()
        {
            //sql 4 db conn
            SqlConnection conn = new SqlConnection(myconnstrng);

            //DT to hold the value from DB and return it
            DataTable dt = new DataTable();

            try
            {
                //write Query to Select all data from db
                string sql = "SELECT * FROM tbl_dea_cust";

                //Creating Sql Command to execute query
                SqlCommand cmd = new SqlCommand(sql, conn);

                //Creating Sqldataadapter to store data from db Temperorily
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                //open DB conn
                conn.Open();
                //Passing The value from Sql dataAdapter to dt
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
        #region INSERT Method to add Details of Dealer OR Customer
        public bool Insert(DeaCustBLL dc)
        {
            //CreatingSql conn 1st
            SqlConnection conn = new SqlConnection(myconnstrng);

            //Cre a Boolean val and Set its default value to false
            bool isSuccess = false;

            try
            {
                //Write sql query to insert details of cus or dea
                string sql = "INSERT INTO tbl_dea_cust (type, name, email, contact, address, added_date, added_by) VALUES (@type, @name, @email, @contact, @address, @added_date, @added_by)";

                //Sql cmd to pass val to query and execute
                SqlCommand cmd = new SqlCommand(sql, conn);
                //passing the val using parameters
                cmd.Parameters.AddWithValue("@type", dc.type);
                cmd.Parameters.AddWithValue("@name", dc.name);
                cmd.Parameters.AddWithValue("@email",dc.email);
                cmd.Parameters.AddWithValue("@contact", dc.contact);
                cmd.Parameters.AddWithValue("@address", dc.address);
                cmd.Parameters.AddWithValue("@added_date", dc.added_date);
                cmd.Parameters.AddWithValue("@added_by", dc.added_by);

                //opening db CONN
                conn.Open();

                //int var to check wheather the query is executed Successfully or not
                int rows = cmd.ExecuteNonQuery();

                //if the query is executed successfully then the value of rows will be >0 else it will be 0<
                if (rows>0)
                {
                    //Query executed Successfully
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
        #region UPDATE Method For Dealer & Customer MOdule
        public bool Update(DeaCustBLL dc)
        {
            //sql conn 4 db conn
            SqlConnection conn = new SqlConnection(myconnstrng);
            //Create boolean var and set its default val to false
            bool isSuccess = false;

            try
            {
                //Sql query to update data in db
                string sql = "UPDATE tbl_dea_cust SET type=@type, name=@name, email=@email, contact=@contact, address=@address, added_date=@added_date, added_by=@added_by WHERE id=@id";
                //Cre Sql cmd to pass the  val in sql
                SqlCommand cmd = new SqlCommand(sql, conn);

                //passing the val through parameters
                cmd.Parameters.AddWithValue("@type", dc.type);
                cmd.Parameters.AddWithValue("@name", dc.name);
                cmd.Parameters.AddWithValue("@email", dc.email);
                cmd.Parameters.AddWithValue("@contact", dc.contact);
                cmd.Parameters.AddWithValue("@address", dc.address);
                cmd.Parameters.AddWithValue("@added_date", dc.added_date);
                cmd.Parameters.AddWithValue("@added_by", dc.added_by);
                cmd.Parameters.AddWithValue("@id", dc.id);

                //Open the db conn
                conn.Open();

                //Creating int var to check if the query is executed succesfully or not
                int rows = cmd.ExecuteNonQuery();
                if (rows>0)
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
        #region Delete Method For Dealer and Customer Module
        public bool Delete(DeaCustBLL dc)
        {
            //Sql conn 4 db
            SqlConnection conn = new SqlConnection(myconnstrng);

            //Cre Boolean var and set its default val to false
            bool isSuccess = false;

            try
            {
                //Sql query to del data from db
                string sql = "DELETE FROM tbl_dea_cust WHERE id=@id";

                //sql cmd to pass the val
                SqlCommand cmd = new SqlCommand(sql, conn);
                //pass the val
                cmd.Parameters.AddWithValue("@id", dc.id);

                //open DB conn
                conn.Open();
                //int var
                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    //query Executed Successfully
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
        #region Search Method for Dealer and Customer Module

        public DataTable Search(string keyword)
        {

            //Creatre a Sql Connection
            SqlConnection conn = new SqlConnection(myconnstrng);

            //Creating a DT and Returning Its val
            DataTable dt = new DataTable();

            try
            {
                //write the query to Search Dealer or Customer based in id,type and name
                string sql = "SELECT * FROM tbl_dea_cust WHERE id LIKE '%"+keyword+"%' OR type LIKE '%"+keyword+"%' OR name LIKE '%"+keyword+"%'";

                //Sql command to execute the Query
                SqlCommand cmd = new SqlCommand(sql, conn);
                //Sql dataAdapter to hold the data from database Temperorily
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                //open DB conn
                conn.Open();
                //Pass the val from adaptor to dt
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
        #region Method to Search Dealer or Customer for Transaction Module
        public DeaCustBLL SearchDealerCustomerForTransaction(string keyword)
        {
            //Create an object for DeacustBLL class
            DeaCustBLL dc = new DeaCustBLL();

            //Create a Db Conn
            SqlConnection conn = new SqlConnection(myconnstrng);

            //Create a DT to hold the val temp
            DataTable dt = new DataTable();

            try
            {
                //write a sql query to search dealer or cust based on keyword
                string sql = "SELECT name, email, contact, address FROM tbl_dea_cust WHERE id LIKE '%"+keyword+"%' OR name LIKE '%"+keyword+"%'";

                //Create sql Data Adapter to Execute the query
                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);

                //open the DB conn
                conn.Open();

                //Transfer the Data from Sql Data adapter to DT
                adapter.Fill(dt);

                //if we have val on dt we need to save it in dealercustomerBLL
                if (dt.Rows.Count > 0)
                {
                    dc.name = dt.Rows[0]["name"].ToString();
                    dc.email = dt.Rows[0]["email"].ToString();
                    dc.contact = dt.Rows[0]["contact"].ToString();
                    dc.address = dt.Rows[0]["address"].ToString();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //Close DB Conn
                conn.Close();
            }

            return dc;
        }
        #endregion
        #region Method To Get ID of the DEALER OR Customer BASED on Name 
        public DeaCustBLL GetDeaCustIDFromName(string Name)
        {
            //1st Create an obj of the Deacust Bll And Return It
            DeaCustBLL dc = new DeaCustBLL();


            //Sql Conn here
            SqlConnection conn = new SqlConnection(myconnstrng);
            //DataT to hold the data temp
            DataTable dt = new DataTable();

            try
            {
                //Sql Query to get id based on name
                string sql = "SELECT id FROM tbl_dea_cust WHERE name='"+Name+"'";
                //create the sql dataadapter to execute the Query
                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);

                conn.Open();

                //Passing the val from adapter to DT
                adapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    //Pass the val from dt to DeaCustBll dc
                    dc.id=int.Parse(dt.Rows[0]["id"].ToString());
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

            return dc;
        }
        #endregion
    }
}
