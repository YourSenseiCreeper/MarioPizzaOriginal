using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Text;
using Dapper;
using MarioPizzaOriginal.Domain;
using MarioPizzaOriginal.Domain.Enums;
using Model;
using Model.DataAccess;

namespace MarioPizzaOriginal.DataAccess
{
    public class SqlLiteDatabase
    {
        private static string LoadConnectionString(string id = "SqlLite")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
