using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace HRSystem.DB
{
    class DBsql
    {
        //Returns the database connection string
        public static SqlConnection CreateSqlConnectionStr()
        {
            SqlConnection dbConStr = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CnStr"].ConnectionString);
            return dbConStr;
        }

        //Returns SqlDataAdapter
        public SqlDataAdapter GetSqlDataAdapt(string strsql)
        {
            SqlConnection con = CreateSqlConnectionStr();
            SqlDataAdapter sda = new SqlDataAdapter(strsql, con);
            return sda;
        }

        //returns datareader from stored procedure
        public DataTable GetDataTableSP(string storedProc,Hashtable htParam)
        {
            SqlConnection con = CreateSqlConnectionStr();
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = new SqlCommand(storedProc,con);
                sda.SelectCommand.CommandType = CommandType.StoredProcedure;
                foreach (DictionaryEntry de in htParam)
                {
                    sda.SelectCommand.Parameters.AddWithValue(de.Key.ToString(), de.Value.ToString());
                }
                con.Open();
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
            catch
            {
                return null;
            }
            finally 
            {
                con.Close(); 
            }
        }

        //returns DataTable
        public DataTable GetDataTable(string strsql, Hashtable htParam)
        {
            SqlConnection con = CreateSqlConnectionStr();
            using (con)
            {
                SqlDataAdapter sda = new SqlDataAdapter(strsql, con);
                if (htParam != null)
                {
                    foreach (DictionaryEntry de in htParam)
                    {
                        sda.SelectCommand.Parameters.AddWithValue(de.Key.ToString(), de.Value.ToString());
                    }
                }
                DataTable dt = new DataTable();
                con.Open();
                sda.Fill(dt);
                con.Close();
                return dt;
            }
        }

        //return sql command with parameters
        public SqlCommand GetSqlCommWithParam(string strsql, Hashtable htParam)
        {
            SqlConnection con = CreateSqlConnectionStr();
            StringBuilder str = new StringBuilder();
            str.Append(strsql);
            using (con)
            {
                SqlCommand sqlcommand = new SqlCommand(str.ToString(), con);
                foreach (DictionaryEntry entry in htParam)
                {
                    sqlcommand.Parameters.AddWithValue(entry.Key.ToString(), entry.Value);
                }

                return sqlcommand;
            }
        }

        //return multiple row result
        public DataTable GetMultipleRowResult(string strsql, Hashtable htParam)//string param, string value
        {
            SqlConnection con = CreateSqlConnectionStr();
            //SqlConnection con = this.CreateSqlConnectionStr();
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.SelectCommand = GetSqlCommWithParam(strsql, htParam);
            DataTable dt = new DataTable();
            con.Open();
            sda.Fill(dt);
            con.Close();
            return dt;
        }

        //insert into database
        public void InsertIntoDB(string tableName, Hashtable htParam)
        {
            SqlConnection con = CreateSqlConnectionStr();
            using (con)
            {
                //create an empty dataset
                SqlDataAdapter sda = new SqlDataAdapter("select TOP 0 * from " + tableName, con);
                DataSet ds = new DataSet();
                con.Open();
                sda.Fill(ds, tableName);
                //DataTable dt = ds.Tables[tableName];

                //create new row
                DataRow newRow = ds.Tables[tableName].NewRow();
                //fill new row
                foreach (DictionaryEntry de in htParam)
                {
                    newRow[de.Key.ToString()] = de.Value;
                }
                //insert new row
                ds.Tables[tableName].Rows.Add(newRow);
                new SqlCommandBuilder(sda).GetInsertCommand();
                sda.Update(ds, tableName);
            }
        }

        //insert into database or update with stored procedures
        public bool InsUpdIntoDBSP(string storedProc, Hashtable htParam)
        {
            SqlConnection con = CreateSqlConnectionStr();
            try
            {
                SqlCommand insCmd = new SqlCommand(storedProc, con);
                insCmd.CommandType = CommandType.StoredProcedure;
                foreach (DictionaryEntry de in htParam)
                {
                    insCmd.Parameters.AddWithValue(de.Key.ToString(), de.Value);
                }
                con.Open();
                insCmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }
     
        //delete from database
        public bool DeleteFromDB(string strsql,Hashtable htParam)
        {
            SqlConnection con = CreateSqlConnectionStr();
            using (con)
            {
                SqlCommand deletecmd = new SqlCommand(strsql, con);
                foreach (DictionaryEntry de in htParam)
                {
                    deletecmd.Parameters.AddWithValue(de.Key.ToString(),de.Value.ToString());
                }
                try
                {
                    con.Open();
                    deletecmd.ExecuteNonQuery();
                    con.Close();
                    return true;
                }
                catch (SqlException ex)
                {
                    return false;
                }
            }
        }

        public void UpdateDB(string strsql, Hashtable htParams)
        {
            SqlConnection con = CreateSqlConnectionStr();
            SqlDataAdapter sda = GetSqlDataAdapt(strsql);
            DataSet ds = new DataSet();
            sda.Fill(ds);

            foreach (DictionaryEntry de in htParams)
            {
                ds.Tables[0].Rows[0][de.Key.ToString()] = de.Value;//column name and column value
            }

            new SqlCommandBuilder(sda);
            sda.Update(ds, ds.Tables[0].TableName);
            ds.Dispose();
        }

       
    }
}
