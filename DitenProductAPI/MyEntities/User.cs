using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DitenProductAPI.MyEntities
{
    public class User
    {
      
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string ApiToken { get; set; }
    }
}
