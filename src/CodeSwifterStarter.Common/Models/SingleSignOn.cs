using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSwifterStarter.Common.Models
{
    public class SingleSignOn
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public AuthApi Api { get; set; }
    }
}
