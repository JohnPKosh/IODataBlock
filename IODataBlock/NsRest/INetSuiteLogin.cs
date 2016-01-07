using System;

namespace NetSuite.RESTlet.Integration
{
    public interface INetSuiteLogin
    {
        String Account { set; get; }

        String Email { set; get; }

        String Password { set; get; }

        String Role { set; get; }
    }
}