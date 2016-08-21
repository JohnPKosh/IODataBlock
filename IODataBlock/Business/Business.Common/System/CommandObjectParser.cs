using System;
using System.Collections.Generic;
using System.Linq;
using Business.Common.Requests;
using Business.Common.Responses;

namespace Business.Common.System
{
    public class CommandObjectParser : ICommandObjectParser
    {
        #region Class Initialization

        public CommandObjectParser(IEnumerable<ICommandObject> commands)
        {
            _commands = commands;
        }

        #endregion Class Initialization

        #region Fields and Properties

        private readonly IEnumerable<ICommandObject> _commands;

        #endregion Fields and Properties

        #region Parse Methods

        public ICommandObject Parse(IRequestObject requestObject)
        {
            var command = Find(requestObject.CommandName);
            return ((ICommandObjectFactory)command).Create(requestObject);
        }

        public ICommandObject Parse(string commandName, IRequestObject requestObject)
        {
            var command = Find(commandName);
            return ((ICommandObjectFactory)command).Create(requestObject);
        }

        #endregion Parse Methods

        #region Execute Methods

        public IResponseObject Execute(IRequestObject requestObject)
        {
            var command = Find(requestObject.CommandName);
            return ((ICommandObjectFactory)command).Create(requestObject).Execute();
            // TODO: decide if logger functionality should be injected here!
        }

        public IResponseObject Execute(string commandName, object requestData, string correlationId = null)
        {
            var requestObject = new RequestObject
            {
                CommandName = commandName,
                RequestData = requestData,
                CorrelationId = correlationId ?? Guid.NewGuid().ToString()
            };
            return Execute(requestObject);
            // TODO: decide if logger functionality should be injected here!
        }

        #endregion Execute Methods

        #region private Utility methods

        private ICommandObject Find(string commandName)
        {
            return _commands.FirstOrDefault(c => c.CommandName == $@"{commandName}Command");
        }

        #endregion private Utility methods
    }
}