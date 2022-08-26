using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seasail
{
    public static class GlobalContext
    {
        public static string DBProvider { get; set; }

        public static string DBConnectionString { get; set; }
        public static string EntityAssembly { get; set; }
        public static string SQLiteConnectionString { get; set; }
    }
}
