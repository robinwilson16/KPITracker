using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboardApp.Shared
{
    public static class GlobalVariables
    {
        public static bool IsInitialised = false;
        public static bool IsDbContextValid = false;
        public static bool IsDbContextErrorShown = false;
        public static bool IsDbContextBusy = false;
    }
}
