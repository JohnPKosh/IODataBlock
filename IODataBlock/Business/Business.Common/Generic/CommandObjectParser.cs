using Business.Common.Exceptions;
using Business.Common.GenericRequests;
using Business.Common.GenericResponses;
using Business.Common.Responses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Common.Generic
{
    public class CommandObjectParser<TIn, TOut> : ICommandObjectParser<TIn, TOut>
    {
        #region Class Initialization

        public CommandObjectParser(IEnumerable<ICommandObject<TIn, TOut>> commands)
        {
            _commands = commands;
        }

        #endregion Class Initialization

        #region Fields and Properties

        private readonly IEnumerable<ICommandObject<TIn, TOut>> _commands;

        #endregion Fields and Properties

        #region Parse Methods

        public ICommandObject<TIn, TOut> Parse(IRequestObject<TIn> requestObject)
        {
            var command = Find(requestObject.CommandName);
            return ((ICommandObjectFactory<TIn, TOut>)command).Create(requestObject);
        }

        public ICommandObject<TIn, TOut> Parse(string commandName, IRequestObject<TIn> requestObject)
        {
            var command = Find(commandName);
            return ((ICommandObjectFactory<TIn, TOut>)command).Create(requestObject);
        }

        public bool TryParse(IRequestObject<TIn> requestObject, out ICommandObject<TIn, TOut> commandObject)
        {
            try
            {
                commandObject = Parse(requestObject);
                return true;
            }
            catch (Exception)
            {
                commandObject = null;
                return false;
            }
        }

        public bool TryParse(string commandName, IRequestObject<TIn> requestObject, out ICommandObject<TIn, TOut> commandObject)
        {
            try
            {
                commandObject = Parse(commandName, requestObject);
                return true;
            }
            catch (Exception)
            {
                commandObject = null;
                return false;
            }
        }

        #endregion Parse Methods

        #region Execute Methods

        public IResponseObject<TIn, TOut> Execute(IRequestObject<TIn> requestObject)
        {
            // TODO: decide if logger functionality should be injected here!
            try
            {
                var command = Find(requestObject.CommandName);
                return ((ICommandObjectFactory<TIn, TOut>)command).Create(requestObject).Execute();
            }
            catch (Exception ex)
            {
                return requestObject.RequestData.ToFailedGenericResponse(default(TOut), ExceptionObjectListBase.Create(ex), new ResponseCode(500, @"500 Internal Server Error"), requestObject.CorrelationId);
            }
        }

        public IResponseObject<TIn, TOut> Execute(string commandName, TIn requestData, string correlationId = null)
        {
            // TODO: decide if logger functionality should be injected here!
            var requestObject = new RequestObject<TIn>
            {
                CommandName = commandName,
                RequestData = requestData,
                CorrelationId = correlationId ?? Guid.NewGuid().ToString()
            };
            try
            {
                return Execute(requestObject);
            }
            catch (Exception ex)
            {
                return requestObject.RequestData.ToFailedGenericResponse(default(TOut), ExceptionObjectListBase.Create(ex), new ResponseCode(500, @"500 Internal Server Error"), requestObject.CorrelationId);
            }
        }

        #endregion Execute Methods

        #region private Utility methods

        private ICommandObject<TIn, TOut> Find(string commandName)
        {
            try
            {
                return _commands.First(c => c.CommandName == $@"{commandName}Command");
            }
            catch (Exception)
            {
                throw new CommandNameNotFoundException(@"CommandNameNotFoundException occurred in the command parser. {0} Command Name not found in the current command collection!", commandName);
            }
        }

        #endregion private Utility methods
    }
}