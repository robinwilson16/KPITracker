using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DatabaseMigrations.Models
{
    [NotMapped]
    public class Settings
    {
        public ConnectionString? ConnectionStrings { get; set; }
        public string? Version { get; set; }

        public DatabaseConnection? DatabaseConnection { get; set; }

        public List<Item>? Items { get; set; }
    }

    [NotMapped]
    public class ConnectionString
    {
        public string? DefaultConnection { get; set; }
    }

    [NotMapped]
    public class DatabaseConnection
    {
        public string? Server {  get; set; }
        public string? Database { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

    [NotMapped]
    public class Item
    {
        public string? Name { get; set; }
        public string? Value { get; set; }
    }
}
