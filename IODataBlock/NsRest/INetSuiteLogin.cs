using System;

namespace NsRest
{
    public interface INetSuiteLogin
    {
        String Account { set; get; }

        String Email { set; get; }

        String Password { set; get; }

        String Role { set; get; }
    }
}