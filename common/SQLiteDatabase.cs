using System.Data;
using System.Data.SQLite;

namespace common
{
    public class SQLiteDatabase
    {
        private String dbConnection;
        private SQLiteConnection cnn;

        public SQLiteDatabase(String inputFile)
        {
            dbConnection = String.Format("Data Source={0}", inputFile);
            cnn = new SQLiteConnection(dbConnection);
            // Make things a bit faster
            cnn.Open();
        }

        public void SQLiteDatabase_Close()
        {
            // Clean up after
            cnn.Close();
        }

        public DataTable GetDataTable(string sql)
        {
            DataTable dt = new DataTable();
            try
            {
                SQLiteCommand mycommand = new SQLiteCommand(cnn);
                mycommand.CommandText = sql;
                SQLiteDataReader reader = mycommand.ExecuteReader();
                dt.Load(reader);
                reader.Close();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return dt;
        }

        public int ExecuteNonQuery(string sql)
        {
            SQLiteCommand mycommand = new SQLiteCommand(cnn);
            mycommand.CommandText = sql;
            try
            {
                int rowsUpdated = mycommand.ExecuteNonQuery();
                return rowsUpdated;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public string ExecuteScalar(string sql)
        {
            SQLiteCommand mycommand = new SQLiteCommand(cnn);
            mycommand.CommandText = sql;
            try
            {
                object value = mycommand.ExecuteScalar();
                if (value != null)
                {
                    return value.ToString();
                }
                return "";
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public bool Update(String tableName, Dictionary<String, String> data, String where)
        {
            String vals = "";
            Boolean returnCode = true;
            if (data.Count >= 1)
            {
                foreach (KeyValuePair<String, String> val in data)
                {
                    vals += String.Format(" {0} = '{1}',", val.Key.ToString(), val.Value.ToString());
                }
                vals = vals.Substring(0, vals.Length - 1);
            }
            try
            {
                this.ExecuteNonQuery(String.Format("update {0} set {1} where {2};", tableName, vals, where));
            }
            catch
            {
                returnCode = false;
            }
            return returnCode;
        }

        public bool Delete(String tableName, String where)
        {
            Boolean returnCode = true;
            try
            {
                this.ExecuteNonQuery(String.Format("delete from {0} where {1};", tableName, where));
            }
            catch (Exception fail)
            {
                Console.WriteLine(fail.Message);
                returnCode = false;
            }
            return returnCode;
        }

        public bool Insert(String tableName, Dictionary<String, String> data)
        {
            String columns = "";
            String values = "";
            Boolean returnCode = true;
            foreach (KeyValuePair<String, String> val in data)
            {
                // but don't allow ' in column names
                columns += String.Format(" '{0}',", val.Key.ToString());
                // Work around ' in fields
                values += String.Format(" '{0}',", val.Value.Replace("'", "''"));
            }
            columns = columns.Substring(0, columns.Length - 1);
            values = values.Substring(0, values.Length - 1);
            try
            {
                string expression = String.Format("insert into {0}({1}) values({2});", tableName, columns, values);
                this.ExecuteNonQuery(expression);
            }
            catch (Exception fail)
            {
                Console.WriteLine(fail.Message);
                returnCode = false;
            }
            return returnCode;
        }
    }
}
