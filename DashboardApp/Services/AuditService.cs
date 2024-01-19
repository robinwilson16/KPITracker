using DashboardApp.Models;
using DashboardApp.Shared;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboardApp.Services
{
    public class AuditService
    {
        public Audit Audit { get; set; }
        public async Task<int?> Add()
        {
            //DatabaseService db = new DatabaseService();
            //SqlCommand cmd = new SqlCommand();
            //string sql = $"SPR_DSH_AuditAdd";
            //cmd.Parameters.AddWithValue("@Details", SqlDbType.NVarChar).Value = "Logged In";
            //int? actionQuery = await db.ActionQuery(sql, cmd);

            Audit = new Audit
            {
                Details = "Logged In"
            };

            App.Context.Audit.Add(Audit);

            return await App.Context.SaveChangesAsync();
        }
    }
}
