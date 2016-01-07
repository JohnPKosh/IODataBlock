using System;
using System.Collections.Generic;
using System.Linq;
//using System.Net.PeerToPeer.Collaboration;
using System.Text;
using System.Threading.Tasks;

namespace NetSuite.RESTlet.Integration
{
    public class NetSuiteLogin: INetSuiteLogin
    {
        public NetSuiteLogin() { }

        public NetSuiteLogin(String account, String email, String password, String role)
        {
            Account = account;
            Email = email;
            Password = password;
            Role = role;
        }

        public static NetSuiteLogin Create(String account, String email, String password, String role)
        {
            return new NetSuiteLogin(account, email, password, role);
        }

        public string Account { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
