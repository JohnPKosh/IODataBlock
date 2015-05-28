using System.Collections.Generic;
using Business.Common.Responses;
using Business.Common.System.Commands;

namespace Business.Common.System
{
    public class SystemCommandParser : CommandCollectionParser
    {
        private static SystemCommandParser _instance = new SystemCommandParser(SystemCommandDictionary.Commands);

        private SystemCommandParser(Dictionary<string, IEnumerable<ICommandObject>> commandObjectDictionary)
            : base(commandObjectDictionary)
        {
        }

        public static SystemCommandParser Instance
        {
            get { return _instance ?? (_instance = new SystemCommandParser(SystemCommandDictionary.Commands)); }
        }

        public static IResponseObject ExecuteCommand(string collectionName, string commandName, object requestData, string correlationId = null)
        {
            return Instance.Execute(collectionName, commandName, requestData, correlationId);
        }
    }

    internal static class SystemCommandDictionary
    {
        public static Dictionary<string, IEnumerable<ICommandObject>> Commands = new Dictionary<string, IEnumerable<ICommandObject>>
        {
            {"System.App.DynamicAppState", new List<ICommandObject>
            {
                new SaveDynamicAppStateCommand()
                , new LoadDynamicAppStateCommand()
                , new GetDynamicAppStateCommand()
                , new SetDynamicAppStateCommand()
            }}
        };
    }
}