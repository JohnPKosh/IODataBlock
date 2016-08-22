using Business.Common.System.App;
using System.IO;

namespace SelfHostWebApi.RelayBase
{
    public static class AddressLoader
    {
        public static string LoadBaseAddress(string fileName = "BaseAddress.txt")
        {
            var addressFile = Path.Combine(EnvironmentUtilities.GetAssemblyDirectory(), "App_Data", fileName);
            return File.ReadAllText(addressFile);
        }
    }
}