using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AP.RMA.Frontend.CA.Models
{
    public class AdminViewModel
    {
        public List<LoginModel> Users { get; set; }
        public List<CoreModel> Cores { get; set; }
    }
}
