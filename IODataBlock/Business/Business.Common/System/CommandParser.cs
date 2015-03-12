using System.Collections.Generic;
using System.Linq;
using Business.Common.Requests;

namespace Business.Common.System
{
    public class CommandParser
    {
        private readonly IEnumerable<ICommand> _commands;

        public CommandParser(IEnumerable<ICommand> commands)
        {
            _commands = commands;
        }

        public ICommand Parse(IRequestObject requestObject)
        {
            var command = Find(requestObject.CommandName);
            return ((ICommandFactory)command).Create(requestObject);
        }

        public ICommand Parse(string commandName, IRequestObject requestObject)
        {
            var command = Find(commandName);
            return ((ICommandFactory)command).Create(requestObject);
        }

        private ICommand Find(string commandName)
        {
            return _commands.FirstOrDefault(c => c.CommandName == commandName);
        }
    }
}