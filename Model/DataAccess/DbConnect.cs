using Funq;
using MarioPizzaOriginal.Domain;
using MySql.Data.MySqlClient;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Sqlite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace MarioPizzaOriginal.DataAccess
{
    public class DbConnect
    {
        private MySqlConnection connection;
        private readonly string user = "Z1OcHrFN6W";
        private readonly string password = "vVWjP7XEfW";
        private readonly string database = "Z1OcHrFN6W";
        private readonly string host = "remotemysql.com";

        //Constructor
        public DbConnect()
        {
            connection = new MySqlConnection($"Server={host};Database={database};Uid={user};Pwd={password};");
        }


        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        //Insert statement
        public void Insert(string tableName, List<string> args, List<string> values)
        {
            string argsOneLine = String.Join(",", args.ToArray());
            string valuesOneLine = String.Join(",", values.ToArray());
            string query = $"INSERT INTO {tableName} ({argsOneLine}) VALUES({valuesOneLine})";

            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Execute command
                cmd.ExecuteNonQuery();
                //close connection
                this.CloseConnection();
            }
        }

        //Update statement
        public void Update(string tableName, string condition, List<string> args, List<string> values)
        {
            var editString = new StringBuilder();
            for(int i=0; i<args.Count; i++)
            {
                editString.Append(args[i] + "=" + values[i]);
                if(i != args.Count - 1)
                {
                    editString.Append(",");
                }
            }
            string query = $"UPDATE {tableName} SET {editString.ToString()} WHERE {condition}";

            //Open connection
            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;
                //Execute query
                cmd.ExecuteNonQuery();
                //close connection
                this.CloseConnection();
            }
        }

        //Delete statement
        public void Delete()
        {
            string query = "DELETE FROM tableinfo WHERE name='John Smith'";

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        //Select statement
        public List<string>[] Select()
        {
            string query = "SELECT * FROM ORDER_ELEMENTS";

            //Create a list to store the result
            List<string>[] list = new List<string>[3];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["ORD_ELM_ID"] + "");
                    list[1].Add(dataReader["ORD_ID"] + "");
                    list[2].Add(dataReader["AMNT"] + "");
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }

        //Count statement
        public int Count()
        {
            return 0;
        }

        //Backup
        public void Backup()
        {
        }

        //Restore
        public void Restore()
        {
        }
    }
}
