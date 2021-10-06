using AnyStore7.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyStore7.DAL
{
    class transactionDetailDAL
    {
        //Create conn String
        static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

        #region Insert Method for Transaction Detail
        public bool InsertTransactionDetail(transactionDetailBLL td)
        {
            //Create a boolean val and set its default val to false
            bool isSuccess = false;

            //Create a db Conn here
            SqlConnection conn = new SqlConnection(myconnstrng);

            try
            {

                //Sql query to insert transaction details
                string sql = "INSERT INTO tbl_transaction_detail (product_id, rate, qty, total, dea_cust_id, added_date, added_by) VALUES (@product_id, @rate, @qty, @total, @dea_cust_id, @added_date, @added_by)";

                //Passsing the val to the sql query
                SqlCommand cmd = new SqlCommand(sql, conn);
                //passing the val using cmd
                cmd.Parameters.AddWithValue("@product_id", td.product_id);
                cmd.Parameters.AddWithValue("@rate", td.rate);
                cmd.Parameters.AddWithValue("@qty", td.qty);
                cmd.Parameters.AddWithValue("@total", td.total);
                cmd.Parameters.AddWithValue("@dea_cust_id", td.dea_cust_id);
                cmd.Parameters.AddWithValue("@added_date", td.added_date);
                cmd.Parameters.AddWithValue("@added_by", td.added_by);

                //open db Conn
                conn.Open();

                //declare the int var and execute the query
                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    //Query Exected Successfully
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
                //close db conn
                conn.Close();
            }
            return isSuccess;
        }
        #endregion
    }
}
