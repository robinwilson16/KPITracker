using DashboardApp.Models;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Reflection.PortableExecutable;
using System.Diagnostics;

namespace DashboardApp.Services
{
    public class ConfigService
    {
        public async Task<Config?> Get(string setting)
        {
            DatabaseService db = new DatabaseService();
            SqlCommand cmd = new SqlCommand();
            string sql = $"SELECT * FROM Config";
            cmd.Parameters.AddWithValue("@ConfigID", SqlDbType.NVarChar).Value = setting;
            IEnumerable<System.Data.DataRow>? selectQuery = await db.SelectQuery(sql, cmd);
            Trace.WriteLine("Execute Query");

            Config config = new Config();

            if (selectQuery != null)
            {
                if (selectQuery.Count() == 1)
                {
                    config = new Config()
                    {
                        ConfigID = selectQuery.FirstOrDefault()?["ConfigID"].ToString(),
                        Description = selectQuery.FirstOrDefault()?["Description"].ToString(),
                        Value = selectQuery.FirstOrDefault()?["Value"].ToString()
                    };
                }
                //If many
                //foreach (DataRow row in selectQuery)
                //{
                //    config = new Config()
                //    {
                //        ConfigID = row["ConfigID"].ToString(),
                //        Description = row["Description"].ToString(),
                //        Value = row["Value"].ToString()
                //    };
                //}
            }

            return config;
        }
    }
}