using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Common.System.Commands;

namespace Business.Common.System
{
    public class SystemCommandParser : CommandCollectionParser
    {
        private static SystemCommandParser _instance = new SystemCommandParser(SystemCommandDictionary.Commands);

        private SystemCommandParser(Dictionary<string, IEnumerable<ICommandObject>> commandObjectDictionary) : base(commandObjectDictionary){}

        public static SystemCommandParser Instance
        {
            get { return SystemCommandParser._instance ?? (SystemCommandParser._instance = new SystemCommandParser(SystemCommandDictionary.Commands)); }
        }
    }


    internal static class SystemCommandDictionary
    {
        public static Dictionary<string, IEnumerable<ICommandObject>> Commands = new Dictionary<string, IEnumerable<ICommandObject>>
        {
            {"System.App.AppState", new List<ICommandObject> {new DynamicAppStateSaveCommand()}}
        };
    }
}
