using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AP.RMA.Frontend.CA.Models
{
    public class LoginModel
    {
        public string Uid { get; set; }
        public string Login { get; set; }
        public string Fullname { get; set; }
        public string Hash { get; set; }
        public string Salt { get; set; }
        public bool IsAdmin { get; set; }
    }
}
