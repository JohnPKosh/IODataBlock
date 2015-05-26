using System;
using System.Collections.Generic;
using System.Linq;
using Business.Common.Requests;
using Business.Common.Responses;

namespace Business.Common.System
{
    public class CommandCollectionParser : ICommandCollectionParser
    {
        #region Class Initialization

        public CommandCollectionParser(Dictionary<string, IEnumerable<ICommandObject>> commandObjectDictionary)
        {
            _commandObjectDictionary = commandObjectDictionary;
        }

        #endregion Class Initialization

        #region Fields and Properties

        private readonly Dictionary<string, IEnumerable<ICommandObject>> _commandObjectDictionary;

        #endregion Fields and Properties

        #region Parse Methods

        public ICommandObject Parse(string collectionName, IRequestObject requestObject)
        {
            var command = Find(collectionName, requestObject.CommandName);
            return ((ICommandObjectFactory)command).Create(requestObject);
        }

        public ICommandObject Parse(string collectionName, string commandName, IRequestObject requestObject)
        {
            var command = Find(collectionName, commandName);
            return ((ICommandObjectFactory)command).Create(requestObject);
        }

        #endregion Parse Methods

        #region Execute Methods

        public IResponseObject Execute(string collectionName, IRequestObject requestObject)
        {
            var command = Find(collectionName, requestObject.CommandName);
            return ((ICommandObjectFactory)command).Create(requestObject).Execute();
            // TODO: decide if logger functionality should be injected here!
        }

        public IResponseObject Execute(string collectionName, string commandName, object requestData, string correlationId = null)
        {
            // TODO: Suffix commandName with "Command" to move to convention based naming of classes. Add TryExecute and/or Exception for Command not found scenario.
            var requestObject = new RequestObject
            {
                CommandName = commandName,
                RequestData = requestData,
                CorrelationId = correlationId ?? Guid.NewGuid().ToString()
            };
            return Execute(collectionName, requestObject);
            // TODO: decide if logger functionality should be injected here!
        }

        #endregion Execute Methods

        #region private Utility methods

        private ICommandObject Find(string collectionName, string commandName)
        {
            var commands = _commandObjectDictionary[collectionName];
            return commands.FirstOrDefault(c => c.CommandName == commandName);
        }

        #endregion private Utility methods
    }
}