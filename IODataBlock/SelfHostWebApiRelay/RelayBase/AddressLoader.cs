using System.IO;
using Business.Common.System.App;

namespace SelfHostWebApiRelay.RelayBase
{
    public static class AddressLoader
    {

        public static string LoadRelayAddress(string fileName = "RelayAddress.txt")
        {
            var addressFile = Path.Combine(EnvironmentUtilities.GetAssemblyDirectory(), "App_Data", fileName);
            return File.ReadAllText(addressFile);
        }

        public static string LoadBaseAddress(string fileName = "BaseAddress.txt")
        {
            var addressFile = Path.Combine(EnvironmentUtilities.GetAssemblyDirectory(), "App_Data", fileName);
            return File.ReadAllText(addressFile);
        }
    }
}
