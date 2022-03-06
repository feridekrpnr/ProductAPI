using DitenProductAPI.MyEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DitenProductAPI.Interfaces
{
    public interface IJWTAuthenticateManager
    {
        public string createToken(User user);
    }
}
