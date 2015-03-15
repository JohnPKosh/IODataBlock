using System;
using System.Collections.Generic;
using System.Linq;
using Business.Common.Exceptions;
using Business.Common.Requests;
using Business.Common.Responses;

namespace Business.Common.System
{
    public class CommandObjectParser
    {
        private readonly IEnumerable<ICommandObject> _commands;

        public CommandObjectParser(IEnumerable<ICommandObject> commands)
        {
            _commands = commands;
        }

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

        public IResponseObject ParseAndExecute(IRequestObject requestObject)
        {
            var command = Find(requestObject.CommandName);
            return ((ICommandObjectFactory)command).Create(requestObject).Execute();
        }

        public IResponseObject ParseAndExecute(string commandName, object requestData, string correlationId = null)
        {
            var requestObject = new RequestObject
            {
                CommandName = commandName,
                RequestData = requestData,
                CorrelationId = correlationId ?? Guid.NewGuid().ToString()
            };
            return ParseAndExecute(requestObject);
        }

        private ICommandObject Find(string commandName)
        {
            return _commands.FirstOrDefault(c => c.CommandName == commandName);
        }
    }
}