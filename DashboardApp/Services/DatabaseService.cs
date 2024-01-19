using DashboardApp.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboardApp.Services
{
    public class DatabaseService
    {
        public async Task<IEnumerable<System.Data.DataRow>?> SelectQuery(string sql, SqlCommand cmd)
        {
            SettingsService settingsService = new SettingsService();
            Settings? settings = settingsService.Get();

            string connectionString = $"Data Source={settings?.DatabaseConnection?.Server};Database={settings?.DatabaseConnection?.Database};User Id={settings?.DatabaseConnection?.Username};Password={settings?.DatabaseConnection?.Password};TrustServerCertificate=True";
            Config config = new Config();

            await using (SqlConnection connection = new SqlConnection(connectionString))
            {
                cmd.Connection = connection;
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                Trace.WriteLine("Execute Query: " + sql);

                await using (cmd)
                {
                    try
                    {
                        connection.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        
                        try
                        {
                            dt.Load(reader);
                            return dt.AsEnumerable();
                        }
                        catch (SqlException ex)
                        {
                            Trace.WriteLine(ex.Message);
                            return null;
                        }
                    }
                    catch (SqlException ex)
                    {
                        Trace.WriteLine(ex.Message);
                        return null;
                    }
                }
            }

        }

        public async Task<int?> ActionQuery(string sql, SqlCommand cmd)
        {
            SettingsService settingsService = new SettingsService();
            Settings? settings = settingsService.Get();

            string connectionString = $"Data Source={settings?.DatabaseConnection?.Server};Database={settings?.DatabaseConnection?.Database};User Id={settings?.DatabaseConnection?.Username};Password={settings?.DatabaseConnection?.Password};TrustServerCertificate=True";
            Config config = new Config();

            await using (SqlConnection connection = new SqlConnection(connectionString))
            {
                cmd.Connection = connection;
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;

                await using (cmd)
                {
                    try
                    {
                        connection.Open();
                        int writer = cmd.ExecuteNonQuery();

                        return writer;
                    }
                    catch (SqlException ex)
                    {
                        Trace.WriteLine(ex.Message);
                        return 0;
                    }
                }
            }
        }
    }
}
