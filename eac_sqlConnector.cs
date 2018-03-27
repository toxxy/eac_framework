using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;
using System.Text.RegularExpressions;

//SAMPLE

namespace EAC_Framework
{
    class eac_sqlConnector
    {
        public string stringConnection;
        public SqlConnection conn;

        public eac_sqlConnector(string serverName, string dataBaseName, 
            string table = "", string user = "", string password = "")
        {
            conn = new SqlConnection();
            conn.ConnectionString =
            "Data Source=" + serverName + ";" +
            "Initial Catalog=" + dataBaseName + ";" +
            "User id=" + user + ";" +
            "Password=" + password + ";";
        }

        public void Insert(string query)
        {
            //Maybe we can use Update Metod
            conn.Open();
            SqlCommand exec = new SqlCommand(query, conn);
            exec.ExecuteNonQuery();
            conn.Close();
        }

        public void Update(string query)
        {
            conn.Open();
            SqlCommand exec = new SqlCommand(query, conn);
            exec.ExecuteNonQuery();
            conn.Close();
        }

        public void Query(string query)
        {
            conn.Open();
            SqlCommand exec = new SqlCommand(query, conn);
            exec.ExecuteNonQuery();
            conn.Close();
        }
        public DataTable Select(string query)
        {
            DataTable result = new DataTable();
            SqlDataReader res;
            conn.Open();
            SqlCommand exec = new SqlCommand(query, conn);
            try
            {
                res = exec.ExecuteReader();
                result.Load(res);
            }
            catch (Exception exs)
            {
                MessageBox.Show(exs.ToString());
            }
            conn.Close();

            string tablename = GetWordAfter("from", query);
            result.TableName = tablename;
            return result;
        }
        private void Open(SqlConnection conn)
        {
            try
            {
                conn.Open();
            }
            catch (Exception exep)
            {
                MessageBox.Show("" + exep.ToString());
            }
        }
        private void Close(SqlConnection conn)
        {
            try
            {
                conn.Close();
            }
            catch (Exception exep)
            {
                MessageBox.Show("" + exep.ToString());
            }
        }
        static string GetWordAfter(string word, string phrase)
        {
            //https://stackoverflow.com/questions/14395721/regex-to-find-the-word-immediately-after-a-particular-word 
            var pattern = @"\b" + Regex.Escape(word) + @"\s+(\w+)";
            return Regex.Match(phrase, pattern, RegexOptions.IgnoreCase).Groups[1].Value;
        }
    }
}
