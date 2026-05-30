using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace FinalProject.Models
{
    public class DBManager
    {
       SqlConnection connection = new SqlConnection("Data Source=PALLO\\SQLEXPRESS;Initial Catalog=IndeedLearning;Integrated Security=True");

        public int executeInsertUpdateDelete(string query)
        {
            SqlCommand command  = new SqlCommand( query , connection );
          connection.Open();
          int result = command.ExecuteNonQuery();
            connection.Close();
            return result;
        }



        public DataTable executeSelect(String query)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }
    }
} 