using DashboardApp.Models;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DashboardApp.Services
{
    public class SettingsService
    {
        //private const string server = "";
        //private const string database = "";
        //private const string username = "";
        //private const string password = "";

        public Settings? _settings;

        public SettingsService()
        {
            var options = new JsonSerializerOptions
            {
                ReadCommentHandling = JsonCommentHandling.Skip
            };

            var a = Assembly.GetExecutingAssembly();
            using Stream? stream = a.GetManifestResourceStream("DashboardApp.appsettings.json");
            using var reader = new StreamReader(stream);
            _settings = JsonSerializer.Deserialize<Settings?>(reader.ReadToEnd(), options);
            Trace.WriteLine("Loaded settings for: " + _settings?.AdditionalPages?[0].Name);
        }
        
        public Settings? Get()
        {
            return _settings;
        }

        public async Task<Settings>? Set()
        {
            _settings.Version = "1.0.1";
            
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string customSettingsFileName = "customsettings.json";
            string customSettingsFile = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, customSettingsFileName);
            using FileStream outputStream = System.IO.File.OpenWrite(customSettingsFile);
            using StreamWriter streamWriter = new StreamWriter(outputStream);
            await streamWriter.WriteAsync(JsonSerializer.Serialize(_settings, options));
            Trace.WriteLine("Saved settings file to: " + customSettingsFile);

            return _settings;
        }
    }
}
