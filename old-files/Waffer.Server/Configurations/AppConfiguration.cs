using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Waffer.Server.Configurations
{
    public class AppConfiguration
    {

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string Key { get; set; }

        public int Lifetime { get; set; }
    }
}
