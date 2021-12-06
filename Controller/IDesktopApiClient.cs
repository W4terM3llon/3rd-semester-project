using DesktopClient.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.Controller
{
    interface IDesktopApiClient
    {
        public JWTToken Login(string email, string password);
        public int Register(User user);
    }
}
