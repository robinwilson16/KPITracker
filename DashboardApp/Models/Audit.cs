using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboardApp.Models
{
    public class Audit
    {
        public int AuditID { get; set; }

        [StringLength(200)]
        public string? Details { get; set; }
    }
}
